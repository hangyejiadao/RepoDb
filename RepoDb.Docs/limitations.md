# RepoDb Limitations

We would like you and the community of .NET to understand the limitations of the library before you decide using it. RepoDb is a micro-ORM that has some advance features built to address the advance use-cases. But, it also has its own limitations that may not work in all use-cases.

**Disclaimer:** This page may not contain all the limitations as the other use-cases is not yet discovered. We will further update this page for any discoveries pertaining to the use-cases that cannot be supported (or impossible to support).

## Known Limitations

- [Composite Keys](#composite-keys)
- [Computed Columns](#computed-columns)
- [JOIN Query (Support)](#join-query)

## Composite Keys

The default support to this will never be implemented as RepoDb tend to sided the other scenario that is forcefully eliminating this use-case. When you do the push operations in RepoDb (i.e.: [Insert](https://repodb.net/operation/insert), [Delete](https://repodb.net/operation/delete), [Update](https://repodb.net/operation/update), [Merge](https://repodb.net/operation/merge) and etc), it uses the PK as the qualifiers.

### Scenario 1 - Insert

```csharp
using (var connection = new SqlConnection(ConnectionString))
{
    var id = connection.Insert<Person>(new Person { Name = "John Doe" });
}
```

Though the call above may insert the data in your database, but the return value will not be the value of the Composite Keys, instead, it will return the ID of that row, whether the ID column is an identity (or not). The value of the Composite Keys cannot be returned on this situation as we only expect a scalar value. This is also true for the other operations, specially for the [Bulk Operations](https://repodb.net/feature/bulkoperations).

### Scenario 2 - Update

Another example is for update operation, we tend to defaultly use the PK as the qualifier. See the code below

```csharp
using (var connection = new SqlConnection(ConnectionString))
{
    var affectedRows = connection.Update<Person>(new Person { Name = "John Doe" }, 10045);
}
```

In which, the 10045 is pointed to a single column in the DB, which is the PK.

If you use the code below, it will behave unexpectedly if you have the Composite Keys in the Name and DateOfBirth columns.

```csharp
using (var connection = new SqlConnection(ConnectionString))
{
    var affectedRows = connection.Update<Person>(new Person { Id = 10045, Name = "John Doe", DateOfBirth = DateTime.Parse("1970/01/01"), Address = "New York" });
}
```

**Alternative Solution**

RepoDb will instead ask you to do it this way, targeting the Composite Keys as the qualifiers.

```csharp
using (var connection = new SqlConnection(ConnectionString))
{
    var person = new Person { Name = "John Doe", DateOfBirth = DateTime.Parse("1970/01/01"), Address = "New York" };
    var affectedRows = connection.Update(person, e => e.Name = person.Name && e.DateOfBirth = person.DateOfBirth);
}
```

### Scenario 3 - Delete

Same goes to the Update scenario, we use the PK as the default qualifier.

```csharp
using (var connection = new SqlConnection(ConnectionString))
{
    var affectedRows = connection.Delete<Person>(10045);
}
```

It is impossible to map the value of 10045 on the Composite Keys.

**Alternative Solution**

Simply do the expression-based or dynamic-based approach by targeting the Composite Keys.

```csharp
using (var connection = new SqlConnection(ConnectionString))
{
    var affectedRows = connection.Delete(e => e.Name == "John Doe" && e.DateOfBirth == DateTime.Parse("1970/01/01"));
}
```

Or like below.

```csharp
using (var connection = new SqlConnection(ConnectionString))
{
    var affectedRows = connection.Delete(ClassMappedNameCache.Get<Person>(), new { Name = "John Doe", DateOfBirth = DateTime.Parse("1970/01/01") });
}
```

**Note**: There may be plenty of undiscovered scenarios that makes RepoDb unusable for the use-cases of having a table with Composite Keys.

## Computed Columns

Though the computed column is supported in all fluent-based GET operations (i.e.: [Query](https://repodb.net/operation/query), [QueryAll](https://repodb.net/operation/queryall), etc), but by default, it is not supported for all fluent-based PUSH operations (i.e.: [Insert](https://repodb.net/operation/insert), [Merge](https://repodb.net/operation/merge), [Update](https://repodb.net/operation/update), etc). For you to understand the computed columns, we recommend that you visit this Microsoft [documentation](https://docs.microsoft.com/en-us/sql/relational-databases/tables/specify-computed-columns-in-a-table?view=sql-server-ver15).

It is important to take note that all the non fluent-based methods like [Query(TableName)](https://repodb.net/operation/query#targetting-a-table) and [Insert(TableName)](https://repodb.net/operation/insert#targetting-a-table) supports the Computed Columns.

RepoDb is dynamic enough on the property projection, but it does not eliminate the computed columns on its projection. Historically, we have the IgnoreAttribute in placed but has been removed in response to the auto-projection capabilities. Of course, up until being pushed by the community to prove the commonality of this use-case, the support to this may not be delivered. To be specific, please see the example below.

Supposed you have a class named Person.

```csharp
public class Person
{
	public long Id { get; set; }
	public string Name { get; set; }
	public DateTime? DateOfBirth { get; set; }
	public int Age { get; set; }
}
```

And you created a table People below.

```csharp
CREATE TABLE [dbo].[Person](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[DateOfBirth] [datetime] NULL,
	[Age] AS (DATEDIFF(YEAR,[DateOfBirth], GETUTCDATE())),
	CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED
	(
		[Id] ASC
	) ON [PRIMARY]
) ON [PRIMARY];
```

Notice, the column Age is a computed column.

As mentioned above, RepoDb will work if you are to use any of the GET operations.

```csharp
using (var connection = new SqlConnection(ConnectionString))
{
	var people = connection.QueryAll<Person>();
}
```

But will fail in the push operation.

```csharp
using (var connection = new SqlConnection(ConnectionString))
{
	var id = connection.Insert<Person, long>(new Person { Name = "John Doe", DateOfBirth = DateTime.Parse("1970/01/01") });
}
```

**Alternative Solution**

We recommend to use the targeted operation when doing a push, see the sample code snippet below.

```csharp
using (var connection = new SqlConnection(ConnectionString))
{
	var id = connection.Insert(ClassMappedNameCache.Get<Person>(), new { Name = "John Doe", DateOfBirth = DateTime.Parse("1970/01/01") });
}
```

Or, create a dedicated model for GET operations that has a computed column in it and a dedicated model for PUSH operations that has no computed column in it.

```csharp
public class Person
{
	public long Id { get; set; }
	public string Name { get; set; }
	public DateTime? DateOfBirth { get; set; }
}

[Table("[dbo].[Person]")]
public class CompletePerson
{
	public long Id { get; set; }
	public string Name { get; set; }
	public DateTime? DateOfBirth { get; set; }
	public int Age { get; set; }
}
```

And do query like below.

```csharp
using (var connection = new SqlConnection(ConnectionString))
{
	var people = connection.QueryAll<CompletePerson>();
}
```

And do insert like below.

```csharp
using (var connection = new SqlConnection(ConnectionString))
{
	var id = connection.Insert<Person, long>(new Person { Name = "John Doe", DateOfBirth = DateTime.Parse("1970/01/01") });
}
```

## JOIN Query (Support)

We understand the reality that without having a support to JOIN Query will somehow eliminate the concepts of ORM in the library. The correct term maybe is Object-Mapper (OM) library, rather than Object/Relational Mapper (ORM) library. Though we consider RepoDb as ORM due to the fact of its flexible features. We tend to leave to the users on how will they implement the JOIN Query, on their own perusal.

We see that majority of the problems of the RDBMS data providers are managing the relationships. These includes the constraints, delegations, cascading and many more. To maintain the robustness of the library and put the control to the users when doing the things, we purposely did not supported this feature (for now), up until we have a much better solution ahead of other ORM libraries.

**Example**

You would like to retrieve the related data of the Supplier record.

Given with these classes.

```csharp
public class Address
{
    public int Id { get; set; }
    ...
}

public class Product
{
    public int Id { get; set; }
    ...
}

public class Warehouse
{
    public int Id { get; set; }
    ...
}

public class Supplier
{
    public int Id { get; set; }
    public IEnumerable<Address> Addresses { get; set; }
    public IEnumerable<Product> Products { get; set; }
    public IEnumerable<Warehouse> Warehouses { get; set; }
}
```

You write the code below.

```csharp
using (var connection = new SqlConnection(ConnectionString))
{
	var supplier = connection
		.Query<Customer>(e => e.Name == "Amazon")
		.Include<Address>()
		.Include<Product>()
		.Include<Warehouse>();
}
```

And have these scripts executed by ORM.

```
> SELECT * FROM [Supplier] WHERE Name = 'Amazon';
> SELECT A.* FROM [Address] A INNER JOIN [Supplier] S ON S.Id = A.SupplierId WHERE A.Name = 'Amazon';
> SELECT P.* FROM [Product] P INNER JOIN [Supplier] S ON S.Id = P.SupplierId WHERE P.Name = 'Amazon';
> SELECT W.* FROM [Warehouse] W INNER JOIN [Warehouse] S ON S.Id = W.SupplierId WHERE A.Name = 'Amazon';
```

We do not want to control the implementation for now, but instead we leave it all to you. We do not know yet whether the solution of multiple execution is acceptable to the community with the use of CTE, LEFT JOIN, OUTER APPLY or whatever techniques.

**SplitQuery**

Though SplitQuery seems to be working in this case, solving the problem beyond N+1. Let us say you write the code below.

```csharp
using (var connection = new SqlConnection(ConnectionString))
{
	var supplier = connection
		.SplitQuery<Customer, Address, Product, Warehouse>(e => e.Name == "Amazon")
		.For<Address>(e => e.Id)
		.For<Product>(e => e.Id)
		.For<Wharehouse>(e => e.Id)
}
```

That may execute this LEFT JOIN query from the database.

```csharp
> SELECT S.*
>	, A.*
>	, P.*
>	, W.*
> FROM [Supplier] S
> LEFT JOIN [Address] A ON A.SupplierId = S.Id
> LEFT JOIN [Product] P ON P.SupplierId = S.Id
> LEFT JOIN [Warehouse] W ON W.SupplierId = S.Id
> WHERE S.Name = 'Amazon';
```

Is still not the most optimal thing to do as it needed a lot of process on the data afterwards. Like grouping the main object and the other N+X values.

**Alternative Solution**

We tend to ask the community to use the [QueryMultiple](https://repodb.net/operation/querymultiple) operation to solve this.

```csharp
using (var connection = new SqlConnection(connectionString).EnsureOpen())
{
	var result = connection.QueryMultiple<Supplier, Address, Product, Warehouse>(s => s.Id == 10045,
		a => a.SupplierId == 10045,
		p => p.SupplierId == 10045,
		w => w.SupplierId == 10045);
	var supplier = result.Item1.FirstOrDefault();
	var addresses = result.Item2.AsList();
	var products = result.Item3.AsList();
	var warehouses = result.Item4.AsList();
	
	// Do the stuffs here
}
```

Or via the [ExecuteMultiple](https://repodb.net/operation/executequerymultiple) operation.

```csharp
using (var connection = new SqlConnection(connectionString).EnsureOpen())
{
	using (var result = connection.ExecuteQueryMultiple(@"SELECT * FROM [dbo].[Supplier] WHERE [Id] = @SupplierId;
		SELECT * FROM [dbo].[Address] WHERE SupplierId = @SupplierId;
		SELECT * FROM [dbo].[Product] WHERE SupplierId = @SupplierId;
		SELECT * FROM [dbo].[Warehouse] WHERE SupplierId = @SupplierId;"))
	{
		var supplier = result.Extract<Person>().FirstOrDefault();
		var addresses = result.Extract<Address>().AsList();
		var products = result.Extract<Product>().AsList();
		var warehouses = result.Extract<Warehouse>().AsList();
		
		// Do the stuffs here
	}
}
```

The good thing to this is controlled by you, and that is a very important case to us.

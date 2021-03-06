﻿using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoDb.Extensions;
using RepoDb.SqLite.IntegrationTests.Models;
using RepoDb.SqLite.IntegrationTests.Setup;
using System.Linq;

namespace RepoDb.SqLite.IntegrationTests.Operations.MDS
{
    [TestClass]
    public class MergeAllTest
    {
        [TestInitialize]
        public void Initialize()
        {
            Database.Initialize();
            Cleanup();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Database.Cleanup();
        }

        #region DataEntity

        #region Sync

        [TestMethod]
        public void TestSqLiteConnectionMergeAllForIdentityForEmptyTable()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Create the tables
                Database.CreateMdsTables(connection);

                // Setup
                var tables = Helper.CreateMdsCompleteTables(10);

                // Act
                var result = connection.MergeAll<MdsCompleteTable>(tables);

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsCompleteTable>();

                // Assert
                Assert.AreEqual(10, queryResult.Count());
                Helper.AssertPropertiesEquality(tables.Last(), queryResult.Last());
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllForIdentityForNonEmptyTable()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection).AsList();

                // Setup
                tables.ForEach(table => Helper.UpdateMdsCompleteTableProperties(table));

                // Act
                var result = connection.MergeAll<MdsCompleteTable>(tables);

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsCompleteTable>();

                // Assert
                tables.ForEach(table => Helper.AssertPropertiesEquality(table, queryResult.First(e => e.Id == table.Id)));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllForIdentityForNonEmptyTableWithQualifiers()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection).AsList();
                var qualifiers = new[]
                {
                    new Field("Id", typeof(long))
                };

                // Setup
                tables.ForEach(table => Helper.UpdateMdsCompleteTableProperties(table));

                // Act
                var result = connection.MergeAll<MdsCompleteTable>(tables,
                    qualifiers);

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsCompleteTable>();

                // Assert
                tables.ForEach(table => Helper.AssertPropertiesEquality(table, queryResult.First(e => e.Id == table.Id)));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllForNonIdentityForEmptyTable()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Create the tables
                Database.CreateMdsTables(connection);

                // Setup
                var tables = Helper.CreateMdsNonIdentityCompleteTables(10);

                // Act
                var result = connection.MergeAll<MdsNonIdentityCompleteTable>(tables);

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsNonIdentityCompleteTable>();

                // Assert
                tables.ForEach(table => Helper.AssertPropertiesEquality(table, queryResult.First(e => e.Id == table.Id)));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllForNonIdentityForNonEmptyTable()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsNonIdentityCompleteTables(10, connection).AsList();

                // Setup
                tables.ForEach(table => Helper.UpdateMdsNonIdentityCompleteTableProperties(table));

                // Act
                var result = connection.MergeAll<MdsNonIdentityCompleteTable>(tables);

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsNonIdentityCompleteTable>();

                // Assert
                tables.ForEach(table => Helper.AssertPropertiesEquality(table, queryResult.First(e => e.Id == table.Id)));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllForNonIdentityForNonEmptyTableWithQualifiers()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsNonIdentityCompleteTables(10, connection).AsList();
                var qualifiers = new[]
                {
                    new Field("Id", typeof(long))
                };

                // Setup
                tables.ForEach(table => Helper.UpdateMdsNonIdentityCompleteTableProperties(table));

                // Act
                var result = connection.MergeAll<MdsNonIdentityCompleteTable>(tables,
                    qualifiers);

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsNonIdentityCompleteTable>();

                // Assert
                tables.ForEach(table => Helper.AssertPropertiesEquality(table, queryResult.First(e => e.Id == table.Id)));
            }
        }

        #endregion

        #region Async

        [TestMethod]
        public void TestSqLiteConnectionMergeAllAsyncForIdentityForEmptyTable()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Create the tables
                Database.CreateMdsTables(connection);

                // Setup
                var tables = Helper.CreateMdsCompleteTables(10);

                // Act
                var result = connection.MergeAllAsync<MdsCompleteTable>(tables).Result;

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsCompleteTable>();

                // Assert
                Assert.AreEqual(10, queryResult.Count());
                Helper.AssertPropertiesEquality(tables.Last(), queryResult.Last());
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllAsyncForIdentityForNonEmptyTable()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection).AsList();

                // Setup
                tables.ForEach(table => Helper.UpdateMdsCompleteTableProperties(table));

                // Act
                var result = connection.MergeAllAsync<MdsCompleteTable>(tables).Result;

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsCompleteTable>();

                // Assert
                tables.ForEach(table => Helper.AssertPropertiesEquality(table, queryResult.First(e => e.Id == table.Id)));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllAsyncForIdentityForNonEmptyTableWithQualifiers()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection).AsList();
                var qualifiers = new[]
                {
                    new Field("Id", typeof(long))
                };

                // Setup
                tables.ForEach(table => Helper.UpdateMdsCompleteTableProperties(table));

                // Act
                var result = connection.MergeAllAsync<MdsCompleteTable>(tables,
                    qualifiers).Result;

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsCompleteTable>();

                // Assert
                tables.ForEach(table => Helper.AssertPropertiesEquality(table, queryResult.First(e => e.Id == table.Id)));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllAsyncForNonIdentityForEmptyTable()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Create the tables
                Database.CreateMdsTables(connection);

                // Setup
                var tables = Helper.CreateMdsNonIdentityCompleteTables(10);

                // Act
                var result = connection.MergeAllAsync<MdsNonIdentityCompleteTable>(tables).Result;

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsNonIdentityCompleteTable>();

                // Assert
                tables.ForEach(table => Helper.AssertPropertiesEquality(table, queryResult.First(e => e.Id == table.Id)));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllAsyncForNonIdentityForNonEmptyTable()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsNonIdentityCompleteTables(10, connection).AsList();

                // Setup
                tables.ForEach(table => Helper.UpdateMdsNonIdentityCompleteTableProperties(table));

                // Act
                var result = connection.MergeAllAsync<MdsNonIdentityCompleteTable>(tables).Result;

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsNonIdentityCompleteTable>();

                // Assert
                tables.ForEach(table => Helper.AssertPropertiesEquality(table, queryResult.First(e => e.Id == table.Id)));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllAsyncForNonIdentityForNonEmptyTableWithQualifiers()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsNonIdentityCompleteTables(10, connection).AsList();
                var qualifiers = new[]
                {
                    new Field("Id", typeof(long))
                };

                // Setup
                tables.ForEach(table => Helper.UpdateMdsNonIdentityCompleteTableProperties(table));

                // Act
                var result = connection.MergeAllAsync<MdsNonIdentityCompleteTable>(tables,
                    qualifiers).Result;

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsNonIdentityCompleteTable>();

                // Assert
                tables.ForEach(table => Helper.AssertPropertiesEquality(table, queryResult.First(e => e.Id == table.Id)));
            }
        }

        #endregion

        #endregion

        #region TableName

        #region Sync

        [TestMethod]
        public void TestSqLiteConnectionMergeAllViaTableNameForIdentityForEmptyTable()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Create the tables
                Database.CreateMdsTables(connection);

                // Setup
                var tables = Helper.CreateMdsCompleteTables(10);

                // Act
                var result = connection.MergeAll(ClassMappedNameCache.Get<MdsCompleteTable>(),
                    tables);

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsCompleteTable>();

                // Assert
                Assert.AreEqual(10, queryResult.Count());
                Helper.AssertMembersEquality(tables.Last(), queryResult.Last());
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllViaTableNameForIdentityForNonEmptyTable()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection).AsList();

                // Setup
                tables.ForEach(table => Helper.UpdateMdsCompleteTableProperties(table));

                // Act
                var result = connection.MergeAll(ClassMappedNameCache.Get<MdsCompleteTable>(),
                    tables);

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsCompleteTable>();

                // Assert
                tables.ForEach(table => Helper.AssertMembersEquality(table, queryResult.ElementAt(tables.IndexOf(table))));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllViaTableNameForIdentityForNonEmptyTableWithQualifiers()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection).AsList();
                var qualifiers = new[]
                {
                    new Field("Id", typeof(long))
                };

                // Setup
                tables.ForEach(table => Helper.UpdateMdsCompleteTableProperties(table));

                // Act
                var result = connection.MergeAll(ClassMappedNameCache.Get<MdsCompleteTable>(),
                    tables,
                    qualifiers);

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsCompleteTable>();

                // Assert
                tables.ForEach(table => Helper.AssertMembersEquality(table, queryResult.ElementAt(tables.IndexOf(table))));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllAsDynamicsViaTableNameForIdentityForEmptyTable()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Create the tables
                Database.CreateMdsTables(connection);

                // Setup
                var tables = Helper.CreateMdsCompleteTablesAsDynamics(10);

                // Act
                var result = connection.MergeAll(ClassMappedNameCache.Get<MdsCompleteTable>(),
                    tables);

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsCompleteTable>();

                // Assert
                tables.ForEach(table => Helper.AssertMembersEquality(table, queryResult.ElementAt((int)tables.IndexOf(table))));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllAsDynamicsViaTableNameForIdentityForNonEmptyTable()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection).AsList();
                var entities = tables.Select(table => new
                {
                    Id = table.Id,
                    ColumnInt = int.MaxValue
                }).AsList();

                // Act
                var result = connection.MergeAll(ClassMappedNameCache.Get<MdsCompleteTable>(),
                    entities);

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsCompleteTable>();

                // Assert
                entities.ForEach(table => Assert.AreEqual(table.ColumnInt, queryResult.ElementAt((int)entities.IndexOf(table)).ColumnInt));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllAsDynamicsViaTableNameForIdentityForNonEmptyTableWithQualifiers()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection).AsList();
                var entities = tables.Select(table => new
                {
                    Id = table.Id,
                    ColumnInt = int.MaxValue
                }).AsList();
                var qualifiers = new[]
                {
                    new Field("Id", typeof(long))
                };

                // Act
                var result = connection.MergeAll(ClassMappedNameCache.Get<MdsCompleteTable>(),
                    entities,
                    qualifiers);

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsCompleteTable>();

                // Assert
                entities.ForEach(table => Assert.AreEqual(table.ColumnInt, queryResult.ElementAt((int)entities.IndexOf(table)).ColumnInt));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllViaTableNameForNonIdentityForEmptyTable()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Create the tables
                Database.CreateMdsTables(connection);

                // Setup
                var tables = Helper.CreateMdsNonIdentityCompleteTables(10);

                // Act
                var result = connection.MergeAll(ClassMappedNameCache.Get<MdsNonIdentityCompleteTable>(),
                    tables);

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsNonIdentityCompleteTable>();

                // Assert
                tables.ForEach(table => Helper.AssertMembersEquality(table, queryResult.ElementAt(tables.IndexOf(table))));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllViaTableNameForNonIdentityForNonEmptyTable()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsNonIdentityCompleteTables(10, connection).AsList();

                // Setup
                tables.ForEach(table => Helper.UpdateMdsNonIdentityCompleteTableProperties(table));

                // Act
                var result = connection.MergeAll(ClassMappedNameCache.Get<MdsNonIdentityCompleteTable>(),
                    tables);

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsNonIdentityCompleteTable>();

                // Assert
                tables.ForEach(table => Helper.AssertMembersEquality(table, queryResult.ElementAt(tables.IndexOf(table))));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllViaTableNameForNonIdentityForNonEmptyTableWithQualifiers()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsNonIdentityCompleteTables(10, connection).AsList();
                var qualifiers = new[]
                {
                    new Field("Id", typeof(long))
                };

                // Setup
                tables.ForEach(table => Helper.UpdateMdsNonIdentityCompleteTableProperties(table));

                // Act
                var result = connection.MergeAll(ClassMappedNameCache.Get<MdsNonIdentityCompleteTable>(),
                    tables,
                    qualifiers);

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsNonIdentityCompleteTable>();

                // Assert
                tables.ForEach(table => Helper.AssertMembersEquality(table, queryResult.ElementAt(tables.IndexOf(table))));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllAsDynamicsViaTableNameForNonIdentityForEmptyTable()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Create the tables
                Database.CreateMdsTables(connection);

                // Setup
                var tables = Helper.CreateMdsNonIdentityCompleteTablesAsDynamics(10);

                // Act
                var result = connection.MergeAll(ClassMappedNameCache.Get<MdsNonIdentityCompleteTable>(),
                    tables);

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsNonIdentityCompleteTable>();

                // Assert
                tables.ForEach(table => Helper.AssertMembersEquality(table, queryResult.ElementAt((int)tables.IndexOf(table))));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllAsDynamicsViaTableNameForNonIdentityForNonEmptyTable()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsNonIdentityCompleteTables(10, connection).AsList();
                var entities = tables.Select(table => new
                {
                    Id = table.Id,
                    ColumnInt = int.MaxValue
                }).AsList();

                // Act
                var result = connection.MergeAll(ClassMappedNameCache.Get<MdsNonIdentityCompleteTable>(),
                    entities);

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsNonIdentityCompleteTable>();

                // Assert
                entities.ForEach(table => Assert.AreEqual(table.ColumnInt, queryResult.ElementAt((int)entities.IndexOf(table)).ColumnInt));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllAsDynamicsViaTableNameForNonIdentityForNonEmptyTableWithQualifiers()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsNonIdentityCompleteTables(10, connection).AsList();
                var entities = tables.Select(table => new
                {
                    Id = table.Id,
                    ColumnInt = int.MaxValue
                }).AsList();
                var qualifiers = new[]
                {
                    new Field("Id", typeof(long))
                };

                // Act
                var result = connection.MergeAll(ClassMappedNameCache.Get<MdsNonIdentityCompleteTable>(),
                    entities,
                    qualifiers);

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsNonIdentityCompleteTable>();

                // Assert
                entities.ForEach(table => Assert.AreEqual(table.ColumnInt, queryResult.ElementAt((int)entities.IndexOf(table)).ColumnInt));
            }
        }

        #endregion

        #region Async

        [TestMethod]
        public void TestSqLiteConnectionMergeAllViaTableNameAsyncForIdentityForEmptyTable()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Create the tables
                Database.CreateMdsTables(connection);

                // Setup
                var tables = Helper.CreateMdsCompleteTables(10);

                // Act
                var result = connection.MergeAllAsync(ClassMappedNameCache.Get<MdsCompleteTable>(),
                    tables).Result;

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsCompleteTable>();

                // Assert
                Assert.AreEqual(10, queryResult.Count());
                Helper.AssertMembersEquality(tables.Last(), queryResult.Last());
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllViaTableNameAsyncForIdentityForNonEmptyTable()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection).AsList();

                // Setup
                tables.ForEach(table => Helper.UpdateMdsCompleteTableProperties(table));

                // Act
                var result = connection.MergeAllAsync(ClassMappedNameCache.Get<MdsCompleteTable>(),
                    tables).Result;

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsCompleteTable>();

                // Assert
                tables.ForEach(table => Helper.AssertMembersEquality(table, queryResult.ElementAt(tables.IndexOf(table))));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllViaTableNameAsyncForIdentityForNonEmptyTableWithQualifiers()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection).AsList();
                var qualifiers = new[]
                {
                    new Field("Id", typeof(long))
                };

                // Setup
                tables.ForEach(table => Helper.UpdateMdsCompleteTableProperties(table));

                // Act
                var result = connection.MergeAllAsync(ClassMappedNameCache.Get<MdsCompleteTable>(),
                    tables,
                    qualifiers).Result;

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsCompleteTable>();

                // Assert
                tables.ForEach(table => Helper.AssertMembersEquality(table, queryResult.ElementAt(tables.IndexOf(table))));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllAsyncAsDynamicsViaTableNameForIdentityForEmptyTable()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Create the tables
                Database.CreateMdsTables(connection);

                // Setup
                var tables = Helper.CreateMdsCompleteTablesAsDynamics(10);

                // Act
                var result = connection.MergeAllAsync(ClassMappedNameCache.Get<MdsCompleteTable>(),
                    tables).Result;

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsCompleteTable>();

                // Assert
                tables.ForEach(table => Helper.AssertMembersEquality(table, queryResult.ElementAt((int)tables.IndexOf(table))));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllAsyncAsDynamicsViaTableNameForIdentityForNonEmptyTable()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection).AsList();
                var entities = tables.Select(table => new
                {
                    Id = table.Id,
                    ColumnInt = int.MaxValue
                }).AsList();

                // Act
                var result = connection.MergeAllAsync(ClassMappedNameCache.Get<MdsCompleteTable>(),
                    entities).Result;

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsCompleteTable>();

                // Assert
                entities.ForEach(table => Assert.AreEqual(table.ColumnInt, queryResult.ElementAt((int)entities.IndexOf(table)).ColumnInt));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllAsyncAsDynamicsViaTableNameForIdentityForNonEmptyTableWithQualifiers()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsCompleteTables(10, connection).AsList();
                var entities = tables.Select(table => new
                {
                    Id = table.Id,
                    ColumnInt = int.MaxValue
                }).AsList();
                var qualifiers = new[]
                {
                    new Field("Id", typeof(long))
                };

                // Act
                var result = connection.MergeAllAsync(ClassMappedNameCache.Get<MdsCompleteTable>(),
                    entities,
                    qualifiers).Result;

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsCompleteTable>();

                // Assert
                entities.ForEach(table => Assert.AreEqual(table.ColumnInt, queryResult.ElementAt((int)entities.IndexOf(table)).ColumnInt));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllAsyncViaTableNameForNonIdentityForEmptyTable()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Create the tables
                Database.CreateMdsTables(connection);

                // Setup
                var tables = Helper.CreateMdsNonIdentityCompleteTables(10);

                // Act
                var result = connection.MergeAllAsync(ClassMappedNameCache.Get<MdsNonIdentityCompleteTable>(),
                    tables).Result;

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsNonIdentityCompleteTable>();

                // Assert
                tables.ForEach(table => Helper.AssertMembersEquality(table, queryResult.ElementAt(tables.IndexOf(table))));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllAsyncViaTableNameForNonIdentityForNonEmptyTable()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsNonIdentityCompleteTables(10, connection).AsList();

                // Setup
                tables.ForEach(table => Helper.UpdateMdsNonIdentityCompleteTableProperties(table));

                // Act
                var result = connection.MergeAllAsync(ClassMappedNameCache.Get<MdsNonIdentityCompleteTable>(),
                    tables).Result;

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsNonIdentityCompleteTable>();

                // Assert
                tables.ForEach(table => Helper.AssertMembersEquality(table, queryResult.ElementAt(tables.IndexOf(table))));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllAsyncViaTableNameForNonIdentityForNonEmptyTableWithQualifiers()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsNonIdentityCompleteTables(10, connection).AsList();
                var qualifiers = new[]
                {
                    new Field("Id", typeof(long))
                };

                // Setup
                tables.ForEach(table => Helper.UpdateMdsNonIdentityCompleteTableProperties(table));

                // Act
                var result = connection.MergeAllAsync(ClassMappedNameCache.Get<MdsNonIdentityCompleteTable>(),
                    tables,
                    qualifiers).Result;

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsNonIdentityCompleteTable>();

                // Assert
                tables.ForEach(table => Helper.AssertMembersEquality(table, queryResult.ElementAt(tables.IndexOf(table))));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllAsyncAsDynamicsViaTableNameForNonIdentityForEmptyTable()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Create the tables
                Database.CreateMdsTables(connection);

                // Setup
                var tables = Helper.CreateMdsNonIdentityCompleteTablesAsDynamics(10);

                // Act
                var result = connection.MergeAllAsync(ClassMappedNameCache.Get<MdsNonIdentityCompleteTable>(),
                    tables).Result;

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsNonIdentityCompleteTable>();

                // Assert
                tables.ForEach(table => Helper.AssertMembersEquality(table, queryResult.ElementAt((int)tables.IndexOf(table))));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllAsyncAsDynamicsViaTableNameForNonIdentityForNonEmptyTable()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsNonIdentityCompleteTables(10, connection).AsList();
                var entities = tables.Select(table => new
                {
                    Id = table.Id,
                    ColumnInt = int.MaxValue
                }).AsList();

                // Act
                var result = connection.MergeAllAsync(ClassMappedNameCache.Get<MdsNonIdentityCompleteTable>(),
                    entities).Result;

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsNonIdentityCompleteTable>();

                // Assert
                entities.ForEach(table => Assert.AreEqual(table.ColumnInt, queryResult.ElementAt((int)entities.IndexOf(table)).ColumnInt));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionMergeAllAsyncAsDynamicsViaTableNameForNonIdentityForNonEmptyTableWithQualifiers()
        {
            using (var connection = new SqliteConnection(Database.ConnectionStringMDS))
            {
                // Setup
                var tables = Database.CreateMdsNonIdentityCompleteTables(10, connection).AsList();
                var entities = tables.Select(table => new
                {
                    Id = table.Id,
                    ColumnInt = int.MaxValue
                }).AsList();
                var qualifiers = new[]
                {
                    new Field("Id", typeof(long))
                };

                // Act
                var result = connection.MergeAllAsync(ClassMappedNameCache.Get<MdsNonIdentityCompleteTable>(),
                    entities,
                    qualifiers).Result;

                // Assert
                Assert.AreEqual(tables.Count(), result);

                // Act
                var queryResult = connection.QueryAll<MdsNonIdentityCompleteTable>();

                // Assert
                entities.ForEach(table => Assert.AreEqual(table.ColumnInt, queryResult.ElementAt((int)entities.IndexOf(table)).ColumnInt));
            }
        }

        #endregion

        #endregion
    }
}

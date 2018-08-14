﻿using RepoDb.Enumerations;
using System;
using System.Linq.Expressions;

namespace RepoDb
{
    /// <summary>
    /// A class used for manipulating class objects via expressions.
    /// </summary>
    public static partial class ClassExpression
    {
        /// <summary>
        /// Gets the mapped-name of the class object.
        /// </summary>
        /// <typeparam name="TEntity">The target type of the class.</typeparam>
        /// <param name="command">The target command for extraction.</param>
        /// <returns>The mapped-name of the class.</returns>
        public static string GetClassMappedName<TEntity>(Command command)
            where TEntity : class
        {
            switch (command)
            {
                case Command.BatchQuery:
                    return ClassMappedNameExtractorForBatchQuery<TEntity>.Extract();
                case Command.BulkInsert:
                    return ClassMappedNameExtractorForBulkInsert<TEntity>.Extract();
                case Command.Count:
                    return ClassMappedNameExtractorForCount<TEntity>.Extract();
                case Command.Delete:
                    return ClassMappedNameExtractorForDelete<TEntity>.Extract();
                case Command.DeleteAll:
                    return ClassMappedNameExtractorForDeleteAll<TEntity>.Extract();
                case Command.InlineInsert:
                    return ClassMappedNameExtractorForInlineInsert<TEntity>.Extract();
                case Command.InlineMerge:
                    return ClassMappedNameExtractorForInlineMerge<TEntity>.Extract();
                case Command.InlineUpdate:
                    return ClassMappedNameExtractorForInlineUpdate<TEntity>.Extract();
                case Command.Insert:
                    return ClassMappedNameExtractorForInsert<TEntity>.Extract();
                case Command.Merge:
                    return ClassMappedNameExtractorForMerge<TEntity>.Extract();
                case Command.Query:
                    return ClassMappedNameExtractorForQuery<TEntity>.Extract();
                case Command.Truncate:
                    return ClassMappedNameExtractorForTruncate<TEntity>.Extract();
                case Command.Update:
                    return ClassMappedNameExtractorForUpdate<TEntity>.Extract();
                default:
                    return ClassMappedNameExtractor<TEntity>.Extract();
            }
        }

        /// <summary>
        /// Gets a function from the defined properties.
        /// </summary>
        /// <param name="command">The command type.</param>
        /// <returns>The mapped-name of the class.</returns>
        private static Func<string> GetCompiledFunctionForClassMappedNameExtractor<T>(Command command)
            where T : class
        {
            // Expressions
            var body = Expression.Constant(ClassMappedNameCache.Get<T>(command));

            // Set the function value
            return Expression
                .Lambda<Func<string>>(body)
                .Compile();
        }

        #region ClassMappedNameExtractor

        private static class ClassMappedNameExtractor<T> where T : class
        {
            private static readonly Func<string> m_func;

            static ClassMappedNameExtractor()
            {
                m_func = GetCompiledFunctionForClassMappedNameExtractor<T>(Command.None);
            }

            public static string Extract()
            {
                return m_func();
            }
        }

        #endregion

        #region ExtractorForBatchQuery

        private static class ClassMappedNameExtractorForBatchQuery<T> where T : class
        {
            private static readonly Func<string> m_func;

            static ClassMappedNameExtractorForBatchQuery()
            {
                m_func = GetCompiledFunctionForClassMappedNameExtractor<T>(Command.BatchQuery);
            }

            public static string Extract()
            {
                return m_func();
            }
        }

        #endregion

        #region ExtractorForBulkInsert

        private static class ClassMappedNameExtractorForBulkInsert<T> where T : class
        {
            private static readonly Func<string> m_func;

            static ClassMappedNameExtractorForBulkInsert()
            {
                m_func = GetCompiledFunctionForClassMappedNameExtractor<T>(Command.BulkInsert);
            }

            public static string Extract()
            {
                return m_func();
            }
        }

        #endregion

        #region ExtractorForCount

        private static class ClassMappedNameExtractorForCount<T> where T : class
        {
            private static readonly Func<string> m_func;

            static ClassMappedNameExtractorForCount()
            {
                m_func = GetCompiledFunctionForClassMappedNameExtractor<T>(Command.Count);
            }

            public static string Extract()
            {
                return m_func();
            }
        }

        #endregion

        #region ExtractorForDelete

        private static class ClassMappedNameExtractorForDelete<T> where T : class
        {
            private static readonly Func<string> m_func;

            static ClassMappedNameExtractorForDelete()
            {
                m_func = GetCompiledFunctionForClassMappedNameExtractor<T>(Command.Delete);
            }

            public static string Extract()
            {
                return m_func();
            }
        }

        #endregion

        #region ExtractorForDeleteAll

        private static class ClassMappedNameExtractorForDeleteAll<T> where T : class
        {
            private static readonly Func<string> m_func;

            static ClassMappedNameExtractorForDeleteAll()
            {
                m_func = GetCompiledFunctionForClassMappedNameExtractor<T>(Command.DeleteAll);
            }

            public static string Extract()
            {
                return m_func();
            }
        }

        #endregion

        #region ExtractorForInlineInsert

        private static class ClassMappedNameExtractorForInlineInsert<T> where T : class
        {
            private static readonly Func<string> m_func;

            static ClassMappedNameExtractorForInlineInsert()
            {
                m_func = GetCompiledFunctionForClassMappedNameExtractor<T>(Command.InlineInsert);
            }

            public static string Extract()
            {
                return m_func();
            }
        }

        #endregion

        #region ExtractorForInlineMerge

        private static class ClassMappedNameExtractorForInlineMerge<T> where T : class
        {
            private static readonly Func<string> m_func;

            static ClassMappedNameExtractorForInlineMerge()
            {
                m_func = GetCompiledFunctionForClassMappedNameExtractor<T>(Command.InlineMerge);
            }

            public static string Extract()
            {
                return m_func();
            }
        }

        #endregion

        #region ExtractorForInlineUpdate

        private static class ClassMappedNameExtractorForInlineUpdate<T> where T : class
        {
            private static readonly Func<string> m_func;

            static ClassMappedNameExtractorForInlineUpdate()
            {
                m_func = GetCompiledFunctionForClassMappedNameExtractor<T>(Command.InlineUpdate);
            }

            public static string Extract()
            {
                return m_func();
            }
        }

        #endregion

        #region ExtractorForInsert

        private static class ClassMappedNameExtractorForInsert<T> where T : class
        {
            private static readonly Func<string> m_func;

            static ClassMappedNameExtractorForInsert()
            {
                m_func = GetCompiledFunctionForClassMappedNameExtractor<T>(Command.Insert);
            }

            public static string Extract()
            {
                return m_func();
            }
        }

        #endregion

        #region ExtractorForMerge

        private static class ClassMappedNameExtractorForMerge<T> where T : class
        {
            private static readonly Func<string> m_func;

            static ClassMappedNameExtractorForMerge()
            {
                m_func = GetCompiledFunctionForClassMappedNameExtractor<T>(Command.Merge);
            }

            public static string Extract()
            {
                return m_func();
            }
        }

        #endregion

        #region ExtractorForQuery

        private static class ClassMappedNameExtractorForQuery<T> where T : class
        {
            private static readonly Func<string> m_func;

            static ClassMappedNameExtractorForQuery()
            {
                m_func = GetCompiledFunctionForClassMappedNameExtractor<T>(Command.Query);
            }

            public static string Extract()
            {
                return m_func();
            }
        }

        #endregion

        #region ExtractorForTruncate

        private static class ClassMappedNameExtractorForTruncate<T> where T : class
        {
            private static readonly Func<string> m_func;

            static ClassMappedNameExtractorForTruncate()
            {
                m_func = GetCompiledFunctionForClassMappedNameExtractor<T>(Command.Truncate);
            }

            public static string Extract()
            {
                return m_func();
            }
        }

        #endregion

        #region ExtractorForUpdate

        private static class ClassMappedNameExtractorForUpdate<T> where T : class
        {
            private static readonly Func<string> m_func;

            static ClassMappedNameExtractorForUpdate()
            {
                m_func = GetCompiledFunctionForClassMappedNameExtractor<T>(Command.Update);
            }

            public static string Extract()
            {
                return m_func();
            }
        }

        #endregion
    }
}
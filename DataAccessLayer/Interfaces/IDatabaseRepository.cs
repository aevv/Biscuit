using System;
using System.Collections.Generic;
using System.Data;
using DapperExtensions;

namespace DataAccessLayer.Interfaces
{
    public interface IDatabaseRepository
    {
        int Count<T>(object predicate, IDbTransaction transaction, int? commandTimeout = null, bool commit = true,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) where T : class;

        bool Delete<T>(object predicate, IDbTransaction transaction, int? commandTimeout = null, bool commit = true,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) where T : class;

        bool Delete<T>(T entity, IDbTransaction transaction, int? commandTimeout = null, bool commit = true,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) where T : class;

        void Delete<T>() where T : class;

        T Get<T>(dynamic id, IDbTransaction transaction, int? commandTimeout = null, bool commit = true,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) where T : class;

        IEnumerable<T> GetList<T>(object predicate, IList<ISort> sort, IDbTransaction transaction,
            int? commandTimeout = null,
            bool commit = true, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, bool buffered = false)
            where T : class;

        IMultipleResultReader GetMultiple(GetMultiplePredicate predicate, IDbTransaction transaction, bool commit = true,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            int? commandTimeout = null);

        IEnumerable<T> GetPage<T>(object predicate, IList<ISort> sort, int page, int resultsPerPage,
            IDbTransaction transaction, bool commit = true, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            int? commandTimeout = null, bool buffered = false) where T : class;

        IEnumerable<T> GetSet<T>(object predicate, IList<ISort> sort, int firstResult, int maxResults,
            IDbTransaction transaction, bool commit = true, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            int? commandTimeout = null, bool buffered = false) where T : class;

        dynamic Insert<T>(T entity, IDbTransaction transaction, bool commit = true,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, int? commandTimeout = null) where T : class;

        void Insert<T>(IEnumerable<T> entities, IDbTransaction transaction, bool commit = true,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, int? commandTimeout = null)
            where T : class;

        bool Update<T>(T entity, IDbTransaction transaction, int? commandTimeout = null, bool commit = true,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) where T : class;

        void Update<T>(IEnumerable<T> entities, IDbTransaction transaction, int? commandTimeOut = null,
            bool commit = true, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
            where T : class;

        bool TestConnection();
        int Count<T>(object predicate, int? commandTimeout = null) where T : class;
        int Count<T>() where T : class;
        bool Delete<T>(object predicate, int? commandTimeout = null) where T : class;
        bool Delete<T>(T entity, int? commandTimeout = null) where T : class;
        T Get<T>(dynamic id, int? commandTimeout = null) where T : class;

        IEnumerable<T> GetList<T>(object predicate = null, IList<ISort> sort = null, int? commandTimeout = null,
            bool buffered = true) where T : class;

        IMultipleResultReader GetMultiple(GetMultiplePredicate predicate, int? commandTimeout = null);

        IEnumerable<T> GetPage<T>(object predicate, IList<ISort> sort, int page, int resultsPerPage,
            int? commandTimeout = null, bool buffered = false) where T : class;

        IEnumerable<T> GetSet<T>(object predicate, IList<ISort> sort, int firstResult, int maxResults,
            int? commandTimeout = null, bool buffered = false) where T : class;

        object Insert<T>(T entity, int? commandTimeout = null) where T : class;
        void Insert<T>(IEnumerable<T> entities, int? commandTimeout = null) where T : class;
        bool Update<T>(T entity, int? commandTimeout = null) where T : class;
        void Update<T>(IEnumerable<T> entities, int? commandTimeOut = null) where T : class;
        void Commit();
        void RollBack();
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        T RunInTransaction<T>(Func<T> func);
        void RunInTransaction(Action action);
        bool HasActiveTransaction { get; }
        IDbConnection Connection { get; }
    }

    
}
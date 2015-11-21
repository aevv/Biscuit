using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using DapperExtensions;
using DapperExtensions.Sql;
using DataAccessLayer.Interfaces;
using Logging;
using System.Data.SqlClient;

namespace DataAccessLayer.Classes
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly IDatabase _database;
        private IDbTransaction _transaction;

        public DatabaseRepository(string connectionString)
        {
            _database = new Database(new SqlConnection(connectionString),
                new SqlGeneratorImpl(new DapperExtensionsConfiguration(typeof (AttributeMapper<>), new List<Assembly>(),
                    new SqlServerDialect())));
        }

  public int Count<T>(object predicate, IDbTransaction transaction, int? commandTimeout = null, bool commit = true,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) where T : class
        {
            if (predicate == null)
            {
                LogHandler.Log(new ArgumentNullException("predicate"));
            }

            if (transaction == null)
            {
                LogHandler.Log(new ArgumentNullException("transaction"));
            }

            int iCount = 0;

            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                BeginTransaction(isolationLevel);
                iCount = _database.Count<T>(predicate, transaction, commandTimeout);
                if (commit)
                {
                    Commit();
                }
            }
            catch (Exception exception)
            {
                LogHandler.Log(exception);
                RollBack();
            }
            finally
            {
                Connection.Close();
            }

            return iCount;
        }

        public bool Delete<T>(object predicate, IDbTransaction transaction, int? commandTimeout = null,
            bool commit = true, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) where T : class
        {
            if (predicate == null)
            {
                LogHandler.Log(new ArgumentNullException("predicate"));
            }

            if (transaction == null)
            {
                LogHandler.Log(new ArgumentNullException("transaction"));
            }

            bool isDeleted = false;

            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                BeginTransaction(isolationLevel);
                isDeleted = _database.Delete<T>(predicate, transaction, commandTimeout);
                if (commit)
                {
                    Commit();
                }
            }
            catch (Exception exception)
            {
                LogHandler.Log(exception);
                RollBack();
            }
            finally
            {
                Connection.Close();
            }

            return isDeleted;
        }

        public bool Delete<T>(T entity, IDbTransaction transaction, int? commandTimeout = null, bool commit = true,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) where T : class
        {
            if (entity == null)
            {
                LogHandler.Log(new ArgumentNullException("entity"));
            }

            if (transaction == null)
            {
                LogHandler.Log(new ArgumentNullException("transaction"));
            }

            bool isDeleted = false;

            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                BeginTransaction(isolationLevel);
                isDeleted = _database.Delete(entity, transaction, commandTimeout);
                if (commit)
                {
                    Commit();
                }
            }
            catch (Exception exception)
            {
                LogHandler.Log(exception);
                RollBack();
            }
            finally
            {
                Connection.Close();
            }

            return isDeleted;
        }

        public void Delete<T>() where T : class
        {
            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                IEnumerable<T> entities = _database.GetList<T>();
                foreach (T entity in entities)
                {
                    Delete(entity);
                }
            }
            catch (Exception exception)
            {
                LogHandler.Log(exception);
            }
            finally
            {
                Connection.Close();
            }
        }

        public T Get<T>(dynamic id, IDbTransaction transaction, int? commandTimeout = null, bool commit = true,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) where T : class
        {
            if (id == null)
            {
                LogHandler.Log(new ArgumentNullException("id"));
            }

            if (transaction == null)
            {
                LogHandler.Log(new ArgumentNullException("transaction"));
            }

            T entity = null;

            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                BeginTransaction(isolationLevel);
                entity = _database.Get<T>(id, transaction, commandTimeout);
                if (commit)
                {
                    Commit();
                }
            }
            catch (Exception exception)
            {
                LogHandler.Log(exception);
                RollBack();
            }
            finally
            {
                Connection.Close();
            }

            return entity;
        }

        public IEnumerable<T> GetList<T>(object predicate, IList<ISort> sort, IDbTransaction transaction,
            int? commandTimeout = null,
            bool commit = true, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            bool buffered = false) where T : class
        {
            if (predicate == null)
            {
                LogHandler.Log(new ArgumentNullException("predicate"));
            }

            if (sort == null)
            {
                LogHandler.Log(new ArgumentNullException("sort"));
            }

            if (transaction == null)
            {
                LogHandler.Log(new ArgumentNullException("transaction"));
            }

            IEnumerable<T> entities = null;
            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                BeginTransaction(isolationLevel);
                entities = _database.GetList<T>(predicate, sort, transaction, commandTimeout, buffered);
                if (commit)
                {
                    Commit();
                }
            }
            catch (Exception exception)
            {
                LogHandler.Log(exception);
                RollBack();
            }
            finally
            {
                Connection.Close();
            }

            return entities;
        }

        public IMultipleResultReader GetMultiple(GetMultiplePredicate predicate, IDbTransaction transaction,
            bool commit = true, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            int? commandTimeout = null)
        {
            if (predicate == null)
            {
                LogHandler.Log(new ArgumentNullException("predicate"));
            }

            if (transaction == null)
            {
                LogHandler.Log(new ArgumentNullException("transaction"));
            }

            IMultipleResultReader multipleResultReader = null;

            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                BeginTransaction(isolationLevel);
                multipleResultReader = _database.GetMultiple(predicate, transaction, commandTimeout);
                if (commit)
                {
                    Commit();
                }
            }
            catch (Exception exception)
            {
                LogHandler.Log(exception);
                RollBack();
            }
            finally
            {
                Connection.Close();
            }

            return multipleResultReader;
        }

        public IEnumerable<T> GetPage<T>(object predicate, IList<ISort> sort, int page, int resultsPerPage,
            IDbTransaction transaction, bool commit = true, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            int? commandTimeout = null, bool buffered = false) where T : class
        {
            if (predicate == null)
            {
                LogHandler.Log(new ArgumentNullException("predicate"));
            }

            if (sort == null)
            {
                LogHandler.Log(new ArgumentNullException("sort"));
            }

            if (page == 0)
            {
                LogHandler.Log("Page is set to 0");
            }

            if (resultsPerPage == 0)
            {
                LogHandler.Log("ResultsPerPage is set to 0");
            }

            if (transaction == null)
            {
                LogHandler.Log(new ArgumentNullException("transaction"));
            }

            IEnumerable<T> entities = null;

            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                BeginTransaction(isolationLevel);
                entities = _database.GetPage<T>(predicate, sort, page, resultsPerPage, transaction, commandTimeout,
                    buffered);
                if (commit)
                {
                    Commit();
                }
            }
            catch (Exception exception)
            {
                LogHandler.Log(exception);
                RollBack();
            }
            finally
            {
                Connection.Close();
            }

            return entities;
        }

        public IEnumerable<T> GetSet<T>(object predicate, IList<ISort> sort, int firstResult, int maxResults,
            IDbTransaction transaction, bool commit = true, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            int? commandTimeout = null, bool buffered = false) where T : class
        {
            if (predicate == null)
            {
                LogHandler.Log(new ArgumentNullException("predicate"));
            }

            if (sort == null)
            {
                LogHandler.Log(new ArgumentNullException("sort"));
            }

            if (firstResult == 0)
            {
                LogHandler.Log("firstResult is 0");
            }

            if (maxResults == 0)
            {
                LogHandler.Log("maxResults is 0");
            }

            if (transaction == null)
            {
                LogHandler.Log(new ArgumentNullException("transaction"));
            }

            IEnumerable<T> entities = null;

            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                BeginTransaction(isolationLevel);
                entities = _database.GetSet<T>(predicate, sort, firstResult, maxResults, transaction, commandTimeout,
                    buffered);
                if (commit)
                {
                    Commit();
                }
            }
            catch (Exception exception)
            {
                LogHandler.Log(exception);
                RollBack();
            }
            finally
            {
                Connection.Close();
            }

            return entities;
        }

        public dynamic Insert<T>(T entity, IDbTransaction transaction, bool commit = true,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, int? commandTimeout = null) where T : class
        {
            if (entity == null)
            {
                LogHandler.Log(new ArgumentNullException("entity"));
            }

            if (transaction == null)
            {
                LogHandler.Log(new ArgumentNullException("transaction"));
            }

            dynamic dym = null;

            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                BeginTransaction(isolationLevel);
                dym = _database.Insert(entity, transaction, commandTimeout);
                if (commit)
                {
                    Commit();
                }
            }
            catch (Exception exception)
            {
                LogHandler.Log(exception);
                RollBack();
            }
            finally
            {
                Connection.Close();
            }

            return dym;
        }

        public void Insert<T>(IEnumerable<T> entities, IDbTransaction transaction, bool commit = true,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, int? commandTimeout = null)
            where T : class
        {
            if (entities == null)
            {
                LogHandler.Log(new ArgumentNullException("entities"));
            }

            if (transaction == null)
            {
                LogHandler.Log(new ArgumentNullException("transaction"));
            }

            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                BeginTransaction(isolationLevel);
                _database.Insert(entities, transaction, commandTimeout);
                if (commit)
                {
                    Commit();
                }
            }
            catch (Exception exception)
            {
                LogHandler.Log(exception);
                RollBack();
            }
            finally
            {
                Connection.Close();
            }
        }

        public bool Update<T>(T entity, IDbTransaction transaction, int? commandTimeout = null, bool commit = true,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) where T : class
        {
            if (entity == null)
            {
                LogHandler.Log(new ArgumentNullException("entity"));
            }

            if (transaction == null)
            {
                LogHandler.Log(new ArgumentNullException("transaction"));
            }

            bool isUpdated = false;

            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                BeginTransaction(isolationLevel);
                isUpdated = _database.Update(entity, transaction, commandTimeout);
                if (commit)
                {
                    Commit();
                }
            }
            catch (Exception exception)
            {
                LogHandler.Log(exception);
                RollBack();
            }
            finally
            {
                Connection.Close();
            }

            return isUpdated;
        }

        public void Update<T>(IEnumerable<T> entities, IDbTransaction transaction, int? commandTimeOut = null,
            bool commit = true, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
            where T : class
        {
            if (entities == null)
            {
                LogHandler.Log(new ArgumentNullException("entities"));
            }

            if (transaction == null)
            {
                LogHandler.Log(new ArgumentNullException("transaction"));
            }

            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                BeginTransaction(isolationLevel);
                foreach (T entity in entities)
                {
                    Update(entity, transaction, commandTimeOut, commit, isolationLevel);
                }
                if (commit)
                {
                    Commit();
                }
            }
            catch (Exception exception)
            {
                LogHandler.Log(exception);
                RollBack();
            }
            finally
            {
                Connection.Close();
            }
        }

        public bool TestConnection()
        {
            if (_database == null)
            {
                LogHandler.Log(new NullReferenceException("database"));
            }

            try
            {
                if (_database.Connection.State == ConnectionState.Open)
                {
                    _database.Connection.Close();
                }

                _database.Connection.Open();

                if (_database.Connection.State == ConnectionState.Open)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exception)
            {
                return false;
            }
            finally
            {
                _database.Connection.Close();
            }
        }

        public int Count<T>(object predicate, int? commandTimeout = null) where T : class
        {
            if (predicate == null)
            {
                LogHandler.Log(new ArgumentNullException("predicate"));
            }

            int iCount = 0;

            try
            {
                iCount = _database.Count<T>(predicate, commandTimeout);
            }
            catch (Exception exception)
            {
                LogHandler.Log(exception);
            }

            return iCount;
        }

        public int Count<T>() where T : class
        {
            int iCount = 0;

            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                IEnumerable<T> entities = _database.GetList<T>();
                iCount = entities.Count();
            }
            catch (Exception exception)
            {
                LogHandler.Log(exception);
            }
            finally
            {
                Connection.Close();
            }

            return iCount;
        }

        public bool Delete<T>(object predicate, int? commandTimeout = null) where T : class
        {
            if (predicate == null)
            {
                LogHandler.Log(new ArgumentNullException("predicate"));
            }

            bool isDeleted = false;

            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                isDeleted = _database.Delete<T>(predicate, commandTimeout);
            }
            catch (Exception exception)
            {
                LogHandler.Log(exception);
            }
            finally
            {
                Connection.Close();
            }

            return isDeleted;
        }

        public bool Delete<T>(T entity, int? commandTimeout = null) where T : class
        {
            if (entity == null)
            {
                LogHandler.Log(new ArgumentNullException("entity"));
            }

            bool isDeleted = false;

            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                isDeleted = _database.Delete(entity, commandTimeout);
            }
            catch (Exception exception)
            {
                LogHandler.Log(exception);
            }
            finally
            {
                Connection.Close();
            }

            return isDeleted;
        }

        public T Get<T>(object predicate, int? commandTimeout = null) where T : class
        {
            if (predicate == null)
            {
                LogHandler.Log(new ArgumentNullException("predicate"));
            }

            T entity = null;

            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                IEnumerable<T> entities = _database.GetList<T>(predicate, buffered: false);
                entity = entities.FirstOrDefault();
            }
            catch (Exception exception)
            {
                LogHandler.Log(exception);
            }
            finally
            {
                Connection.Close();
            }

            return entity;
        }

        public IEnumerable<T> GetList<T>(object predicate = null, IList<ISort> sort = null, int? commandTimeout = null,
            bool buffered = false) where T : class
        {
            IEnumerable<T> entities = null;
            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                entities = _database.GetList<T>(predicate, sort, commandTimeout, buffered);
            }
            catch (Exception exception)
            {
                LogHandler.Log(exception);
            }
            finally
            {
                Connection.Close();
            }
            return entities;
        }

        public IMultipleResultReader GetMultiple(GetMultiplePredicate predicate, int? commandTimeout = null)
        {
            if (predicate == null)
            {
                LogHandler.Log(new ArgumentNullException("predicate"));
            }

            IMultipleResultReader multipleResultReader = null;

            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                multipleResultReader = _database.GetMultiple(predicate, commandTimeout);
            }
            catch (Exception exception)
            {
                LogHandler.Log(exception);
            }

            return multipleResultReader;
        }

        public IEnumerable<T> GetPage<T>(object predicate, IList<ISort> sort, int page, int resultsPerPage,
            int? commandTimeout = null, bool buffered = false) where T : class
        {
            if (predicate == null)
            {
                LogHandler.Log(new ArgumentNullException("predicate"));
            }

            if (sort == null)
            {
                LogHandler.Log(new ArgumentNullException("sort"));
            }

            if (page == 0)
            {
                LogHandler.Log("Page is set to 0");
            }

            if (resultsPerPage == 0)
            {
                LogHandler.Log("ResultsPerPage is set to 0");
            }

            IEnumerable<T> entities = null;

            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                entities = _database.GetPage<T>(predicate, sort, page, resultsPerPage, commandTimeout,
                    buffered);
            }
            catch (Exception exception)
            {
                LogHandler.Log(exception);
            }
            finally
            {
                Connection.Close();
            }

            return entities;
        }

        public IEnumerable<T> GetSet<T>(object predicate, IList<ISort> sort, int firstResult, int maxResults,
            int? commandTimeout = null, bool buffered = false) where T : class
        {
            if (predicate == null)
            {
                LogHandler.Log(new ArgumentNullException("predicate"));
            }

            if (sort == null)
            {
                LogHandler.Log(new ArgumentNullException("sort"));
            }

            if (firstResult == 0)
            {
                LogHandler.Log("firstResult is 0");
            }

            if (maxResults == 0)
            {
                LogHandler.Log("maxResults is 0");
            }

            IEnumerable<T> entities = null;

            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                entities = _database.GetSet<T>(predicate, sort, firstResult, maxResults, commandTimeout,
                    buffered);
            }
            catch (Exception exception)
            {
                LogHandler.Log(exception);
            }
            finally
            {
                Connection.Close();
            }

            return entities;
        }

        public object Insert<T>(T entity, int? commandTimeout = null) where T : class
        {
            if (entity == null)
            {
                LogHandler.Log(new ArgumentNullException("entity"));
            }

            object dym = null;

            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                dym = _database.Insert(entity, commandTimeout);
            }
            catch (Exception exception)
            {
                LogHandler.Log(exception);
            }
            finally
            {
                Connection.Close();
            }

            return dym;
        }

        public void Insert<T>(IEnumerable<T> entities, int? commandTimeout = null) where T : class
        {
            if (entities == null)
            {
                LogHandler.Log(new ArgumentNullException("entities"));
            }

            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }

                _database.Insert(entities, commandTimeout);
            }
            catch (Exception exception)
            {
                LogHandler.Log(exception);
            }
            finally
            {
                Connection.Close();
            }
        }

        public bool Update<T>(T entity, int? commandTimeout = null) where T : class
        {
            if (entity == null)
            {
                LogHandler.Log(new ArgumentNullException("entity"));
            }

            bool isUpdated = false;

            try
            {
                if (Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                }
                isUpdated = _database.Update(entity, commandTimeout);
            }
            catch (Exception exception)
            {
                LogHandler.Log(exception);
            }
            finally
            {
                Connection.Close();
            }

            return isUpdated;
        }

        public void Update<T>(IEnumerable<T> entities, int? commandTimeOut = null) where T : class
        {
            if (entities == null)
            {
                LogHandler.Log(new ArgumentNullException("entities"));
            }

            try
            {
                foreach (T entity in entities)
                {
                    Update(entity, commandTimeOut);
                }
            }
            catch (Exception exception)
            {
                LogHandler.Log(exception);
            }
        }

        public void Commit()
        {
            _transaction.Commit();
            _transaction = null;
        }

        public void RollBack()
        {
            _transaction.Commit();
        }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            _transaction = Connection.BeginTransaction(isolationLevel);
        }

        public T RunInTransaction<T>(Func<T> func)
        {
            T result = default(T);
            BeginTransaction();
            try
            {
                result = func();
                Commit();
                return result;
            }
            catch (Exception exception)
            {
                if (HasActiveTransaction)
                {
                    RollBack();
                }

                LogHandler.Log(exception);
            }

            return result;
        }

        public void RunInTransaction(Action action)
        {
            if (action == null)
            {
                LogHandler.Log(new ArgumentNullException("action"));
            }

            BeginTransaction();
            try
            {
                action();
                Commit();
            }
            catch (Exception exception)
            {
                if (HasActiveTransaction)
                {
                    RollBack();
                }
                LogHandler.Log(exception);
            }
        }

        public bool HasActiveTransaction
        {
            get { return _transaction != null; }
        }

        public IDbConnection Connection
        {
            get { return _database.Connection; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

using Mirabeau.uTransporter.Persistence.UnitOfWork;

using Umbraco.Core.Persistence;

namespace Mirabeau.uTransporter.Persistence.Providers
{
    public class CrudUnitOfWorkProvider : ICrudUnitOfWorkProvider
    {
        [ThreadStatic]
        private static Database _threadSafeDatabaseInstance;

        public CrudUnitOfWorkProvider(IDatabaseUnitOfWork databaseUnitOfWork)
        {
            if (_threadSafeDatabaseInstance == null)
            {
                _threadSafeDatabaseInstance = databaseUnitOfWork.CreateDatabaseUnitOfWork();
            }
        }

        public object Create(object obj)
        {
            return _threadSafeDatabaseInstance.Insert(obj);
        }

        public IEnumerable<T> Read<T>(string query, params object[] args)
        {
            return _threadSafeDatabaseInstance.Query<T>(new Sql(query, args));
        }

        public T ReadSingle<T>(string query, params object[] args)
        {
            IEnumerable<T> enumerable = Read<T>(query, args);

            return enumerable.FirstOrDefault<T>();
        }

        public int Update<T>(string query, params object[] args)
        {
            return _threadSafeDatabaseInstance.Update<T>(new Sql(query, args));
        }

        public int Delete(object obj)
        {
            return _threadSafeDatabaseInstance.Delete(obj);
        }
    }

    public interface ICrudUnitOfWorkProvider
    {
        object Create(object model);

        IEnumerable<T> Read<T>(string query, params object[] args);

        T ReadSingle<T>(string query, params object[] args);

        int Update<T>(string query, params object[] args);

        int Delete(object model);
    }
}

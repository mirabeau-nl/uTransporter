using System;

using Mirabeau.uTransporter.Persistence.UnitOfWork;

using Umbraco.Core.Persistence;

namespace Mirabeau.uTransporter.Persistence.Providers
{
    public class TableUnitOfWorkProvider : ITableUnitOfWorkProvider
    {
        [ThreadStatic]
        private static Database _threadSafeDatabaseInstance;

        public TableUnitOfWorkProvider(IDatabaseUnitOfWork databaseUnitOfWork)
        {
            if (_threadSafeDatabaseInstance == null)
            {
                _threadSafeDatabaseInstance = databaseUnitOfWork.CreateDatabaseUnitOfWork();
            }
        }

        public void CreateTableIfNotExists<T>(string tableName) where T : new()
        {
            if (TableExists(tableName) == false)
            {
                CreateTable<T>();
            }
        }

        public bool TableExists(string tableName)
        {
            return _threadSafeDatabaseInstance.TableExist(tableName);
        }

        public void CreateTable<T>() where T : new()
        {
            _threadSafeDatabaseInstance.CreateTable<T>(false);
        }
    }

    public interface ITableUnitOfWorkProvider
    {
        void CreateTableIfNotExists<T>(string tableName) where T : new();

        bool TableExists(string tableName);

        void CreateTable<T>() where T : new();
    }
}

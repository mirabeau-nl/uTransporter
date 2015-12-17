using System;

using Mirabeau.uTransporter.Persistence.Models;
using Mirabeau.uTransporter.Persistence.UnitOfWork;

using Umbraco.Core.Persistence;

namespace Mirabeau.uTransporter.Persistence.Providers
{
    public class ImportHistoryUnitOfWorkProvider : IImportHistoryUnitOfWorkProvider
    {
        [ThreadStatic]
        private static Database _threadSafeDatabaseInstance = null;

        public ImportHistoryUnitOfWorkProvider(IDatabaseUnitOfWork databaseUnitOfWork)
        {
            if (_threadSafeDatabaseInstance == null)
            {
                _threadSafeDatabaseInstance = databaseUnitOfWork.CreateDatabaseUnitOfWork();
            }
        }

        public ImportHistory Get()
        {
            throw new System.NotImplementedException();
        }

        public ImportHistory Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(ImportHistory model)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(ImportHistory model)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IImportHistoryUnitOfWorkProvider
    {
        ImportHistory Get();

        ImportHistory Get(int id);

        bool Update(ImportHistory model);

        bool Delete(ImportHistory model);
    }
}

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

        public bool Create(ImportHistory model)
        {
            throw new NotImplementedException();
        }

        public ImportHistory Read()
        {
            throw new System.NotImplementedException();
        }

        public ImportHistory Read(int id)
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
        bool Create(ImportHistory model);

        ImportHistory Read();

        ImportHistory Read(int id);

        bool Update(ImportHistory model);

        bool Delete(ImportHistory model);
    }
}

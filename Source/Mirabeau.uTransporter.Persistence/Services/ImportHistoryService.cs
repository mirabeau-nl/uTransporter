using System.Collections.Generic;

using Mirabeau.uTransporter.Persistence.Models;
using Mirabeau.uTransporter.Persistence.Providers;
using Mirabeau.uTransporter.Persistence.Repositories;

namespace Mirabeau.uTransporter.Persistence.Services
{
    public class ImportHistoryService : IImportHistoryService
    {
        private readonly IImportHistoryRepository _repository;

        private readonly ITableUnitOfWorkProvider _tableUnitOfWorkProvider;

        public ImportHistoryService(IImportHistoryRepository repository, ITableUnitOfWorkProvider tableUnitOfWorkProvider)
        {
            _repository = repository;
            _tableUnitOfWorkProvider = tableUnitOfWorkProvider;
            DoesHistoryTableExist();
        }

        public IEnumerable<ImportHistory> GetHistoryData()
        {
            IEnumerable<ImportHistory> importHistories = _repository.GetData();

            return importHistories;
        }

        private void DoesHistoryTableExist()
        {
            _tableUnitOfWorkProvider.CreateTableIfNotExists<ImportHistory>("dbo.uTransporterImportHistory");
        }
    }

    public interface IImportHistoryService
    {
        IEnumerable<ImportHistory> GetHistoryData();
    }
}

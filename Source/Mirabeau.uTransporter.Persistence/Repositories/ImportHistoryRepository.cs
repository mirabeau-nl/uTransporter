using System.Collections.Generic;

using Mirabeau.uTransporter.Persistence.Models;
using Mirabeau.uTransporter.Persistence.Providers;

namespace Mirabeau.uTransporter.Persistence.Repositories
{
    public class ImportHistoryRepository : IImportHistoryRepository
    {
        private readonly ICrudUnitOfWorkProvider _unitOfWorkProvider;

        public ImportHistoryRepository(ICrudUnitOfWorkProvider unitOfWorkProvider)
        {
            _unitOfWorkProvider = unitOfWorkProvider;
        }

        public IEnumerable<ImportHistory> GetData()
        {
            return _unitOfWorkProvider.Read<ImportHistory>("SELECT * FROM dbo.uTransporterImportHistory");
        }
    }

    public interface IImportHistoryRepository
    {
        IEnumerable<ImportHistory> GetData();
    }
}

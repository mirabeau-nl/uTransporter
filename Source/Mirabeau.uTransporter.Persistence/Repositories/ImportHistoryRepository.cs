using Mirabeau.uTransporter.Persistence.Providers;

namespace Mirabeau.uTransporter.Persistence.Repositories
{
    public class ImportHistoryRepository : IImportHistoryRepository
    {
        private readonly IImportHistoryUnitOfWorkProvider _unitOfWorkProvider;

        public ImportHistoryRepository(IImportHistoryUnitOfWorkProvider unitOfWorkProvider)
        {
            _unitOfWorkProvider = unitOfWorkProvider;
        }
    }

    public interface IImportHistoryRepository
    {

    }
}

﻿using System.Collections.Generic;

using Mirabeau.uTransporter.Persistence.Models;
using Mirabeau.uTransporter.Persistence.Providers;

namespace Mirabeau.uTransporter.Persistence.Repositories
{
    public class ImportHistoryRepository : IImportHistoryRepository
    {
        private readonly ICrudHistoryUnitOfWorkProvider _unitOfWorkProvider;

        public ImportHistoryRepository(ICrudHistoryUnitOfWorkProvider unitOfWorkProvider)
        {
            _unitOfWorkProvider = unitOfWorkProvider;
        }

        public IEnumerable<ImportHistory> GetHistory()
        {
            return _unitOfWorkProvider.Read<ImportHistory>("SELECT * FROM uTransporterImportHistory");
        }
    }

    public interface IImportHistoryRepository
    {
        IEnumerable<ImportHistory> GetHistory();
    }
}

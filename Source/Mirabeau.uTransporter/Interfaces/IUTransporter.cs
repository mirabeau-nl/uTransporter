using System.Collections.Generic;

using Mirabeau.uTransporter.Models;
using Mirabeau.uTransporter.Persistence.Models;

namespace Mirabeau.uTransporter.Interfaces
{
    public interface IUTransporter
    {
        ImportHistoryResult GetHistory();
        /// <summary>
        /// Synchronizes the document types.
        /// main application entraince
        /// </summary>
        SyncResult RunImport();

        /// <summary>
        /// 
        /// </summary>
        RemoveResult RemoveDocumentTypes();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        GenerateResult GenerateDocumentTypes(GenerateOptions generateOptions);

        /// <summary>
        /// DryRun! Synchronizes the document types.
        /// main application entraince
        /// </summary>
        SyncResult SynchronizeDocumentTypesDryRun();

        string GetVersion();
    }
}
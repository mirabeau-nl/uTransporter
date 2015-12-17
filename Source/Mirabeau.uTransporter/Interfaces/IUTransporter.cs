using Mirabeau.uTransporter.Models;

namespace Mirabeau.uTransporter.Interfaces
{
    public interface IUTransporter
    {
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
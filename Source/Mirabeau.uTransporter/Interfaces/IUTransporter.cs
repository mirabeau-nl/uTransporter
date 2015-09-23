using Mirabeau.uTransporter.Models;

namespace Mirabeau.uTransporter.Interfaces
{
    public interface IUTransporter
    {
        /// <summary>
        /// Kickoff the sync
        /// </summary>
        void RunSync();

        /// <summary>
        /// Synchronizes the document types.
        /// main application entraince
        /// </summary>
        SyncResult SynchronizeDocumentTypes();

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
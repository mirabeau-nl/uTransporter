using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;

using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.Logging;
using Mirabeau.uTransporter.Models;

namespace Mirabeau.uTransporter
{
    /// <summary>
    ///  uTransporter Main Class- Starts the import process
    ///  during umbraco startup.
    /// </summary>
    public class UTransporter : IUTransporter
    {
        private static ILog4NetWrapper _log;

        private readonly IContentTypeManager _contentTypeManager;

        private readonly ITemplateReadRepository _templateReadRepository;

        private readonly ISqlObjectManager _sqlManagerObject;

        private readonly IContentReadRepository _contentReadRepository;

        private readonly Stopwatch _timer;

        private readonly IDocumentTypeGenerator _documentTypeGenerator;

        private readonly ITemplateGenerator _templateGenerator;

        private readonly string _targetPath = Utils.Util.CombinePaths(AppDomain.CurrentDomain.BaseDirectory, Properties.Settings.Default.SyncBaseDir);

        /// <summary>
        /// Public constructor 
        /// </summary>
        /// <param name="contentTypeManager"></param>
        /// <param name="sqlManagerObject"></param>
        /// <param name="contentReadRepository"></param>
        /// <param name="templateReadRepository"></param>
        /// <param name="documentTypeGenerator"></param>
        /// <param name="templateGenerator"></param>
        /// <param name="tabGenerator"></param>
        public UTransporter(IContentTypeManager contentTypeManager, ISqlObjectManager sqlManagerObject, IContentReadRepository contentReadRepository, ITemplateReadRepository templateReadRepository, IDocumentTypeGenerator documentTypeGenerator, ITemplateGenerator templateGenerator)
        {
            _contentTypeManager = contentTypeManager;
            _templateReadRepository = templateReadRepository;
            _sqlManagerObject = sqlManagerObject;
            _contentReadRepository = contentReadRepository;
            _log = LogManagerWrapper.GetLogger("Mirabeau.uTransporter");
            _timer = new Stopwatch();
            _documentTypeGenerator = documentTypeGenerator;
            _templateGenerator = templateGenerator;
        }

        /// <summary>
        /// Kickoff the sync
        /// </summary>
        public void RunSync()
        {
            SynchronizeDocumentTypes();
        }

        public GenerateResult GenerateDocumentTypes(GenerateOptions generateOptions)
        {
            GenerateResult generateResult = CreateGenerateResult();
            try
            {
                _timer.Restart();

                if (generateOptions.RemoveOutDatedDocumentTypes)
                {
                    _documentTypeGenerator.Delete(_targetPath);
                }

                generateResult.GeneratedClasses = _documentTypeGenerator.Generate(_targetPath);
                generateResult.GeneratedTemplates = _templateGenerator.Generate(_targetPath);
                generateResult.Successful = true;
                generateResult.Message = "Generate successful";
            }
            catch (Exception exception)
            {
                // Gotta catch them all!
                _log.Error(string.Format("Exception: {0}", exception), exception);

                generateResult.Successful = false;
                generateResult.Message = "Generation failed, see the log for the full error message";
            }
            finally
            {
                _timer.Stop();

                generateResult.ElapsedTime = _timer.Elapsed;
                generateResult.FinishedTimestamp = DateTime.Now;
            }

            return generateResult;
        }

        /// <summary>
        /// Removed all document types and templates, use with caution!
        /// </summary>
        /// <returns>RemoveResult instance</returns>
        public RemoveResult RemoveDocumentTypes()
        {
            RemoveResult removeResult = CreateRemoveResult();
            try
            {
                _timer.Restart();

                int numberOfDocumentTypesRemoved = _contentTypeManager.RemoveContentTypes();
                int numberOfTemplatesRemoved = _contentTypeManager.RemoveTemplates();

                removeResult.Successful = true;
                removeResult.Message = "Document Types removal successful";
                removeResult.NumberOfDocumentTypesRemoved = numberOfDocumentTypesRemoved;
                removeResult.NumberOfTemplatesRemoved = numberOfTemplatesRemoved;
            }
            catch (Exception exception)
            {
                // Gotta catch them all!
                _log.Error(string.Format("Exception: {0}", exception.ToString()), exception);

                removeResult.Successful = false;
                removeResult.Message = "Document Types removal failed";
            }
            finally
            {
                _timer.Stop();

                removeResult.ElapsedTime = _timer.Elapsed;
                removeResult.FinishedTimestamp = DateTime.Now;

                if (removeResult.Successful)
                {
                    // Add additional stats
                }
            }

            return removeResult;
        }

        /// <summary>
        /// Synchronizes the document types
        /// </summary>
        /// <returns>SyncResult instance</returns>
        public SyncResult SynchronizeDocumentTypes()
        {
            SyncResult syncResult = CreateSyncResult();

            int numberOfTemplatesBefore = _templateReadRepository.CountAllTemplates();
            int numberOfPropertiesBefore = _contentReadRepository.CountAllPropertiesFromAllContentTypes();
            int numberOfDocumentTypesBefore = _contentReadRepository.GetAllContentTypesCount();

            try
            {
                _timer.Restart();
                _contentTypeManager.SaveContentType();

                syncResult.Successful = true;
                syncResult.Message = "Synchronisation successful";
            }
            catch (Exception exception)
            {
                // Gotta catch them all!
                _log.Error(string.Format("Exception: {0}", exception), exception);

                syncResult.Successful = false;
                syncResult.Message = "Synchronisation failed";
            }
            finally
            {
                _timer.Stop();

                syncResult.ElapsedTime = _timer.Elapsed;
                syncResult.FinishedTimestamp = DateTime.Now;

                if (syncResult.Successful)
                {
                    syncResult.NumberOfDocumentTypesChecked = _contentReadRepository.GetAllContentTypesCount();
                    syncResult.NumberOfTemplatesAdded = _templateReadRepository.CountAllTemplates() - numberOfTemplatesBefore;
                    syncResult.NumberOfPropertiesAdded = _contentReadRepository.CountAllPropertiesFromAllContentTypes() - numberOfPropertiesBefore;
                    syncResult.NumberOfDocumentTypesAdded = _contentReadRepository.GetAllContentTypesCount() - numberOfDocumentTypesBefore;
                }
            }

            return syncResult;
        }

        /// <summary>
        /// Synchronize with dry run
        /// </summary>
        /// <returns>SyncResult instance</returns>
        public SyncResult SynchronizeDocumentTypesDryRun()
        {
            SyncResult syncResult;
            try
            {
                SetupDryRunDatabase();
                UmbracoServices.UmbracoService.EnableDryRunMode();
                syncResult = SynchronizeDocumentTypes();
            }
            catch (SqlException)
            {
                return new SyncResult() { Successful = false, Message = "Failed to setup DryRun database." };
            }
            finally
            {
                UmbracoServices.UmbracoService.DisableDryRunMode();
                CleanupDryRunDatabase();
            }

            return syncResult;
        }

        public string GetVersion()
        {
            string releaseVersionNumber = "0";

            Assembly assembly = Assembly.GetAssembly(typeof(UTransporter));
            if (assembly != null)
            {
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                releaseVersionNumber = fileVersionInfo.FileVersion;
            }

            return releaseVersionNumber;
        }

        private GenerateResult CreateGenerateResult()
        {
            GenerateResult generateResult = new GenerateResult();
            generateResult.StartedTimestamp = DateTime.Now;

            return generateResult;
        }

        /// <summary>
        /// Creates a fresh RemoveResult instance
        /// </summary>
        /// <returns>RemoveResult instance</returns>
        private RemoveResult CreateRemoveResult()
        {
            RemoveResult removeResult = new RemoveResult();
            removeResult.StartedTimestamp = DateTime.Now;

            return removeResult;
        }

        private void SetupDryRunDatabase()
        {
            try
            {
                var sqlConnection = _sqlManagerObject.BuildConnectionString("umbracoDbDSN");
                _sqlManagerObject.CreateDatabase(sqlConnection, "DryRun");
            }
            catch (SqlException exception)
            {
                _log.Error("An error occured during the DryRun database setup", exception);
                throw;
            }
        }

        private void CleanupDryRunDatabase()
        {
            try
            {
                var sqlConnection = _sqlManagerObject.BuildConnectionString("umbracoDbDSN");
                sqlConnection.InitialCatalog += "_DryRun";

                _sqlManagerObject.DeleteDatabase(sqlConnection);
            }
            catch (SqlException exception)
            {
                _log.Error("An error occured during the DryRun database setup", exception);
                throw;
            }
        }

        /// <summary>
        /// Creates a fresh SyncResult instance
        /// </summary>
        /// <returns>SyncResult instance</returns>
        private SyncResult CreateSyncResult()
        {
            SyncResult syncResult = new SyncResult();
            syncResult.StartedTimestamp = DateTime.Now;

            return syncResult;
        }
    }
}
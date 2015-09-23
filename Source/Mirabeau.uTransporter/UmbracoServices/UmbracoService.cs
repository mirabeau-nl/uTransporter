using Mirabeau.uTransporter.Factories;
using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.Logging;

using Umbraco.Core;
using Umbraco.Core.Services;

namespace Mirabeau.uTransporter.UmbracoServices
{
    public class UmbracoService : IUmbracoService
    {
        private static ILog4NetWrapper _log;

        private static readonly object isDryRunMutex = new object();

        private static bool _isDryRun;

        private readonly DryRunServiceFactory _dryRunServiceContextFactory;

        private readonly ServiceContext _serviceContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoService"/> class.
        /// </summary>
        public UmbracoService()
        {
            _log = LogManagerWrapper.GetLogger("Mirabeau.uTransporter");
            _dryRunServiceContextFactory = new DryRunServiceFactory("umbracoDbDSN");
            _serviceContext = this.GetApplicationServiceContext();
        }

        public static bool GetDryRunMode()
        {
            return _isDryRun;
        }

        public static void EnableDryRunMode()
        {
            SetDryRunMode(true);
            _log.Debug("Enabled dry-run mode.");
        }

        public static void DisableDryRunMode()
        {
            SetDryRunMode(false);
            _log.Debug("Disabled dry-run mode.");
        }

        private static void SetDryRunMode(bool enabled)
        {
            lock (isDryRunMutex)
            {
                _isDryRun = enabled;
            }
        }

        /// <summary>
        /// Gets the content type service
        /// </summary>
        /// <returns>The content type service</returns>
        public IContentTypeService GetContentTypeService()
        {
            return _isDryRun ? _dryRunServiceContextFactory.CreateContentTypeService() : _serviceContext.ContentTypeService;
        }

        /// <summary>
        /// Gets the content service
        /// </summary>
        /// <returns>The content service</returns>
        public IContentService GetContentService()
        {
            return _isDryRun ? _dryRunServiceContextFactory.CreateContentService() : _serviceContext.ContentService;
        }

        /// <summary>
        /// Gets the data type service
        /// </summary>
        /// <returns>The data type service</returns>
        public IDataTypeService GetDataTypeService()
        {
            return _isDryRun ? _dryRunServiceContextFactory.CreateDataTypeService() : _serviceContext.DataTypeService;
        }

        /// <summary>
        /// Gets the file service
        /// </summary>
        /// <returns>The file service</returns>
        public IFileService GetFileService()
        {
            return _isDryRun ? _dryRunServiceContextFactory.CreateFileService() : _serviceContext.FileService;
        }

        /// <summary>
        /// Gets the media service
        /// </summary>
        /// <returns>The media service</returns>
        public IMediaService GetMediaService()
        {
            return _isDryRun ? _dryRunServiceContextFactory.CreateMediaService() : _serviceContext.MediaService;
        }

        private ServiceContext GetApplicationServiceContext()
        {
            return ApplicationContext.Current.Services;
        }
    }
}

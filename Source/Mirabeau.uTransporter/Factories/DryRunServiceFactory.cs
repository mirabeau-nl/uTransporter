using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.Providers;
using Mirabeau.uTransporter.Services;

using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.UnitOfWork;
using Umbraco.Core.Services;

namespace Mirabeau.uTransporter.Factories
{
    /// <summary>
    ///  uTransporter UmbracoFactory - Factory class for Umbraco Core API Service
    /// </summary>
    public class DryRunServiceFactory
    {
        private readonly string _connectionString;

        private readonly object _contentTypeServiceMutex;

        private readonly object _contentServiceMutex;

        private readonly object _dataTypeServiceMutex;

        private readonly object _fileServiceMutex;

        private readonly object _mediaServiceMutex;

        private readonly ISqlObjectManager _sqlObjectManager;

        private IContentTypeService _contentTypeService;

        private IContentService _contentService;

        private IDataTypeService _dataTypeService;

        private IFileService _fileService;

        private IMediaService _mediaService;

        private RepositoryFactory _repositoryFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DryRunServiceFactory"/> class.
        /// </summary>
        /// <param name="connectionString">Connection String for the target database</param>
        public DryRunServiceFactory(string connectionString)
        {
            /* mutex objects for locking */
            /* TODO refacting to make use of the C# Mutex class */
            _contentTypeServiceMutex = new object();
            _contentServiceMutex = new object();
            _dataTypeServiceMutex = new object();
            _fileServiceMutex = new object();
            _mediaServiceMutex = new object();

            _sqlObjectManager = new SqlObjectManager();
            _connectionString = connectionString;
        }

        /// <summary>
        ///  Create a Content Type Service
        /// </summary>
        /// <returns>IContentType Service</returns>
        public IContentTypeService CreateContentTypeService()
        {
            if (_contentTypeService == null)
            {
                lock (_contentTypeServiceMutex) // lock this block so only one thread can enter
                {
                    if (_contentTypeService == null)
                    {
                        IDatabaseUnitOfWorkProvider provider = CreateDatabaseUnitOfWorkProvider();
                        RepositoryFactory repositoryFactory = CreateRepositoryFactory();
                        IContentService contentService = CreateContentService();
                        IMediaService mediaService = CreateMediaService();

                        _contentTypeService = new ContentTypeService(provider, repositoryFactory, contentService, mediaService);
                    }
                }
            }

            return _contentTypeService;
        }

        /// <summary>
        ///  Create a Content Type Service
        /// </summary>
        /// <returns>IContentType Service</returns>
        public IContentService CreateContentService()
        {
            if (_contentService == null)
            {
                lock (_contentServiceMutex)
                {
                    if (_contentService == null)
                    {
                        IDatabaseUnitOfWorkProvider provider = CreateDatabaseUnitOfWorkProvider();
                        RepositoryFactory repositoryFactory = CreateRepositoryFactory();

                        _contentService = new ContentService(provider, repositoryFactory);
                    }
                }
            }

            return _contentService;
        }

        /// <summary>
        ///  Create a Data Type Service
        /// </summary>
        /// <returns>IDataType Service</returns>
        public IDataTypeService CreateDataTypeService()
        {
            if (_dataTypeService == null)
            {
                lock (_dataTypeServiceMutex)
                {
                    if (_dataTypeService == null)
                    {
                        IDatabaseUnitOfWorkProvider provider = CreateDatabaseUnitOfWorkProvider();
                        RepositoryFactory repositoryFactory = CreateRepositoryFactory();

                        _dataTypeService = new DataTypeService(provider, repositoryFactory);
                    }
                }
            }

            return _dataTypeService;
        }

        /// <summary>
        /// Create A File Service
        /// </summary>
        /// <returns>IFile Service</returns>
        public IFileService CreateFileService()
        {
            if (_fileService == null)
            {
                lock (_fileServiceMutex)
                {
                    if (_fileService == null)
                    {
                        RepositoryFactory repositoryFactory = CreateRepositoryFactory();

                        _fileService = new FileService(repositoryFactory);
                    }
                }
            }

            return _fileService;
        }

        /// <summary>
        /// Create A File Service
        /// </summary>
        /// <returns>IFile Service</returns>
        public IMediaService CreateMediaService()
        {
            if (_mediaService == null)
            {
                lock (_mediaServiceMutex)
                {
                    if (_mediaService == null)
                    {
                        IDatabaseUnitOfWorkProvider provider = CreateDatabaseUnitOfWorkProvider();
                        RepositoryFactory repositoryFactory = CreateRepositoryFactory();

                        _mediaService = new MediaService(provider, repositoryFactory);
                    }
                }
            }

            return _mediaService;
        }

        private IDatabaseUnitOfWorkProvider CreateDatabaseUnitOfWorkProvider()
        {
            return new DatabaseUnitOfWorkProvider(_connectionString, _sqlObjectManager, "DryRun");
        }

        private RepositoryFactory CreateRepositoryFactory()
        {
            return _repositoryFactory ?? (_repositoryFactory = new RepositoryFactory());
        }
    }
}
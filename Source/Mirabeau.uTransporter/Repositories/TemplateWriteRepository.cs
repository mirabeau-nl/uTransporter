using Mirabeau.uTransporter.Interfaces;

using Umbraco.Core.Services;

namespace Mirabeau.uTransporter.Repositories
{
    public class TemplateWriteRepository : ITemplateWriteRepository
    {
        private readonly ITemplateReadRepository _templateReadRepository;

        private IFileService _fileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateWriteRepository"/> class.
        /// </summary>
        /// <param name="templateReadRepository">The template read repository.</param>
        /// <param name="umbracoFactory">The umbraco factory.</param>
        public TemplateWriteRepository(ITemplateReadRepository templateReadRepository, IUmbracoService umbracoFactory)
        {
            _templateReadRepository = templateReadRepository;
            _fileService = umbracoFactory.GetFileService();
        }
    }
}

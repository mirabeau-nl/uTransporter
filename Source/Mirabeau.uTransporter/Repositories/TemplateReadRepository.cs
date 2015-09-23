using System;
using System.Collections.Generic;
using System.Linq;

using Mirabeau.uTransporter.Interfaces;

using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Mirabeau.uTransporter.Repositories
{
    public class TemplateReadRepository : ITemplateReadRepository
    {
        private readonly IFileService _fileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateReadRepository"/> class.
        /// </summary>
        /// <param name="umbracoFactory">The umbraco factory.</param>
        public TemplateReadRepository(IUmbracoService umbracoFactory)
        {
            _fileService = umbracoFactory.GetFileService();
        }

        /// <summary>
        /// Gets a template.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <returns>Returns an ITemplate objects</returns>
        public ITemplate GetATemplate(string alias)
        {
            return _fileService.GetTemplate(alias);
        }

        /// <summary>
        /// Gets all template.
        /// </summary>
        /// <returns>A List of ITemplate objects</returns>
        public IEnumerable<ITemplate> GetAllTemplates()
        {
            return _fileService.GetTemplates();
        }

        /// <summary>
        /// Counts all templates.
        /// </summary>
        /// <returns>Return the number of all the templates</returns>
        public int CountAllTemplates()
        {
            return _fileService.GetTemplates().Count();
        }

        /// <summary>
        /// Gets the document type attributes.
        /// </summary>
        /// <typeparam name="T">T type</typeparam>
        /// <param name="type">The type.</param>
        /// <returns>T</returns>
        public T GetTemplateAttributes<T>(Type type)
        {
            object[] attributes = type.GetCustomAttributes(typeof(T), true);
            T result = (T)attributes[0];

            return result;
        }
    }
}
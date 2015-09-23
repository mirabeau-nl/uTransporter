using System;
using System.Collections.Generic;
using System.Linq;

using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.Logging;

using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Mirabeau.uTransporter.Managers
{
    public class ContentTypeManager : IContentTypeManager
    {
        private readonly IRetryableContentTypeService _retryableContentTypeService;

        private readonly IFileService _fileService;

        private readonly IDocumentFinder _documentFinder;

        private readonly ILog4NetWrapper _log;

        private readonly IContentWriteRepository _contentWriteRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeManager"/> class.
        /// </summary>
        /// <param name="documentFinder">The document finder.</param>
        /// <param name="umbracoFactory">The umbraco factory.</param>
        /// <param name="contentWriteRepository">The document write repository.</param>
        public ContentTypeManager(IDocumentFinder documentFinder, IRetryableContentTypeService retryableContentTypeService, IContentWriteRepository contentWriteRepository, IUmbracoService umbracoFactory)
        {
            _documentFinder = documentFinder;
            _retryableContentTypeService = retryableContentTypeService;
            _fileService = umbracoFactory.GetFileService();
            _contentWriteRepository = contentWriteRepository;
            _log = LogManagerWrapper.GetLogger("Mirabeau.uTransporter");
        }

        /// <summary>
        /// Saves the type of the document.
        /// </summary>
        public void SaveContentType()
        {
            List<Type> objectList = _documentFinder.GetAllIDocumentTypesBase(true);
            _contentWriteRepository.SaveList(objectList);

            _contentWriteRepository.RemoveContentType(objectList);
        }

        /// <summary>
        /// Doeses the document type exists.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <returns>bool if the doctype exists</returns>
        /// <exception cref="System.ArgumentNullException">Argument Null Exception</exception>
        public bool DoesContentTypeExists(string alias)
        {
            if (string.IsNullOrEmpty(alias))
            {
                throw new ArgumentNullException(string.Format("Can't preform API method with empty {1} argument at: {0}", GetType().Name, alias));
            }

            if (_retryableContentTypeService.GetContentType(alias) != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Remove contenttypes from DB
        /// </summary>
        /// <returns>Number of document types removed</returns>
        public int RemoveContentTypes()
        {
            IEnumerable<IContentType> documentEnumerable = _retryableContentTypeService.GetAllContentTypes().ToList();

            int removedDocumentsCounter = 0;
            foreach (var document in documentEnumerable.OrderByDescending(m => m.Id))
            {
                _retryableContentTypeService.Delete(document);
                removedDocumentsCounter++;
            }

            _log.Info("Removed all document types from the database");

            return removedDocumentsCounter;
        }

        /// <summary>
        /// Removes the templates.
        /// </summary>
        /// <returns>Number of templates removed</returns>
        public int RemoveTemplates()
        {
            IEnumerable<ITemplate> templateEnumerable = _fileService.GetTemplates();

            int removedTemplatesCounter = 0;
            foreach (ITemplate template in templateEnumerable.OrderByDescending(m => m.Id))
            {
                _fileService.DeleteTemplate(template.Alias);
                removedTemplatesCounter++;
            }

            return removedTemplatesCounter;
        }

        /// <summary>
        /// Removes the type of the document.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void RemoveContentType(int id)
        {
            IContentType docType = _retryableContentTypeService.GetContentType(id);
            _retryableContentTypeService.Delete(docType);
        }
    }
}
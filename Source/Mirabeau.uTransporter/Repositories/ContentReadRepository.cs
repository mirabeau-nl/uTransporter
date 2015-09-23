using System;
using System.Collections.Generic;
using System.Linq;

using Mirabeau.uTransporter.Attributes;
using Mirabeau.uTransporter.Extensions;
using Mirabeau.uTransporter.Interfaces;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Repositories
{
    public class ContentReadRepository : IContentReadRepository
    {
        private readonly IRetryableContentTypeService _retryableContentTypeService;

        private readonly IPropertyReadRepository _propertyReadRepository;

        private readonly IAttributeManager _attributeManager;

        public ContentReadRepository(IRetryableContentTypeService contentTypeService, IPropertyReadRepository propertyReadRepository, IAttributeManager attributeManager)
        {
            _retryableContentTypeService = contentTypeService;
            _propertyReadRepository = propertyReadRepository;
            _attributeManager = attributeManager;
        }

        /// <summary>
        /// Gets the content types based on ids.
        /// </summary>
        /// <param name="contentIds">The ids.</param>
        /// <returns>IEnumerable IContentType</returns>
        public IEnumerable<IContentType> GetContentTypesBasedOnIds(int[] contentIds)
        {
            Ensure.ArgumentNotNull(contentIds, "Content Ids");

            return _retryableContentTypeService.GetAllContentTypes(contentIds);
        }

        /// <summary>
        /// Gets the content types based on identifier.
        /// </summary>
        /// <param name="contentId">The identifier.</param>
        /// <returns>IContentType object</returns>
        public IContentType GetContentTypesBasedOnId(int contentId)
        {
            Ensure.ArgumentNotNull(contentId, "Content id");

            return _retryableContentTypeService.GetContentType(contentId);
        }

        /// <summary>
        /// Gets all content types.
        /// </summary>
        /// <returns>IEnumerable IContentType</returns>
        public IEnumerable<IContentType> GetAllContentTypes()
        {
            return _retryableContentTypeService.GetAllContentTypes();
        }

        public List<string> GetAllContentAliases()
        {
            List<IContentType> contentypes = GetAllContentTypes().ToList();
            List<string> aliases = new List<string>();

            foreach (IContentType contentType in contentypes)
            {
                aliases.Add(contentType.Alias);
            }

            return aliases;
        }

        /// <summary>
        /// Gets all content types count.
        /// </summary>
        /// <returns>int number of content types</returns>
        public int GetAllContentTypesCount()
        {
            return _retryableContentTypeService.GetAllContentTypes().Count();
        }

        /// <summary>
        /// Gets the document type alias.
        /// </summary>
        /// <param name="typeDocType">Type of the type document.</param>
        /// <returns>string document type alias</returns>
        public string GetContentTypeAlias(Type typeDocType)
        {
            string alias;

            DocumentTypeAttribute docTypeAttr = _attributeManager.GetContentTypeAttributes<DocumentTypeAttribute>(typeDocType);

            if (!string.IsNullOrEmpty(docTypeAttr.Alias))
            {
                alias = docTypeAttr.Alias;
            }
            else
            {
                alias = typeDocType.Name;
            }

            return alias;
        }

        public bool HasChilderen(int contentId)
        {
            Ensure.ArgumentNotNull(contentId, "Content Id");

            return _retryableContentTypeService.HasChildren(contentId);
        }

        /// <summary>
        /// Gets the document type identifier.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <returns>int id of the document type</returns>
        public int GetContentTypeId(string alias)
        {
            return this.GetContentTypeBasedOnAlias(alias).Id;
        }

        public IContentType GetContentTypeBasedOnAlias(string alias)
        {
            return _retryableContentTypeService.GetContentType(alias);
        }

        public int CountAllPropertiesFromAllContentTypes()
        {
            return _propertyReadRepository.CountPropertiesFromContentTypes(this.GetAllContentTypes());
        }
    }
}
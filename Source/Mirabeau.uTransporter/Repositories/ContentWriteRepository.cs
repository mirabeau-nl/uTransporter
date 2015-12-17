using System;
using System.Collections.Generic;
using System.Linq;

using Mirabeau.uTransporter.Attributes;
using Mirabeau.uTransporter.Exceptions;
using Mirabeau.uTransporter.Extensions;
using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.UmbracoServices;

using Umbraco.Core;
using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class ContentWriteRepository : IContentWriteRepository
    {
        #region Private Fields

        private readonly IAttributeManager _attributeManager;

        private readonly IContentReadRepository _contentReadRepository;

        private readonly IContentTypeFactory _contentTypeFactory;

        private readonly IDocumentFinder _documentFinder;

        private readonly IRetryableContentTypeService _retryableContentTypeService;

        private readonly IContentTypeComparer _contentTypeComparer;

        private readonly IPropertyWriteRepository _propertyWriteRepository;

        private readonly IPropertyReadRepository _propertyReadRepository;

        private readonly ITemplateManager _templateManager;

        private readonly List<Type> _typeList = new List<Type>();

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentWriteRepository"/> class.
        /// </summary>
        /// <param name="retryableContentTypeService">The retryable content type service.</param>
        /// <param name="contentTypeFactory">The document type factory.</param>
        /// <param name="documentFinder">The document finder.</param>
        /// <param name="contentReadRepository">The document read repository.</param>
        /// <param name="attributeManager">The attribute manager</param>
        /// <param name="contentTypeComparer">The document type comparer.</param>
        /// <param name="propertyWriteRepository">The property write repository.</param>
        /// <param name="propertyReadRepository">The property read repository.</param>
        /// <param name="managerFactory">The manager factory.</param>
        public ContentWriteRepository(
            IRetryableContentTypeService retryableContentTypeService,
            IContentReadRepository contentReadRepository,
            IContentTypeFactory contentTypeFactory,
            IDocumentFinder documentFinder,
            IAttributeManager attributeManager,
            IContentTypeComparer contentTypeComparer,
            IPropertyWriteRepository propertyWriteRepository,
            IPropertyReadRepository propertyReadRepository,
            IManagerFactory managerFactory)
        {
            _contentTypeFactory = contentTypeFactory;
            _contentReadRepository = contentReadRepository;
            _documentFinder = documentFinder;
            _attributeManager = attributeManager;
            _contentTypeComparer = contentTypeComparer;
            _retryableContentTypeService = retryableContentTypeService;
            _propertyWriteRepository = propertyWriteRepository;
            _templateManager = managerFactory.CreateTemplateManager();
            _propertyReadRepository = propertyReadRepository;
        }

        /// <summary>
        /// Saves the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="parent">The parent.</param>
        /// <returns>IContentType object</returns>
        public IContentType SaveType(Type type, IContentType parent)
        {
            DocumentTypeAttribute docAttribute = _attributeManager.GetContentTypeAttributes<DocumentTypeAttribute>(type);
            IContentType contentType;
            int parentId = -1;

            if (parent != null && parent.Id != 0)
            {
                parentId = parent.Id;
            }

            try
            {
                contentType = _contentTypeFactory.CreateDocumentType(type, docAttribute, parentId);
                _retryableContentTypeService.Save(contentType);
            }
            catch (AliasNullException exception)
            {
                Logger.WriteErrorLine<ContentWriteRepository>("{0}", exception.Message);
                throw;
            }

            // Add all the types to a list so we can retrive them later without reflection
            _typeList.Add(type);

            return contentType;
        }

        /// <summary>
        /// Saves the list.
        /// </summary>
        /// <param name="objectList">The object list.</param>
        public void SaveList(IEnumerable<Type> objectList)
        {
            this.Save(objectList, null);
            this.SaveAllowedChildeTypes();
        }

        public void RemoveContentType(List<Type> typeList)
        {
            List<string> redundantContentTypes = FindRedundantContentTypes();
            DeleteRedundantContentTypes(redundantContentTypes);
        }

        public List<string> FindRedundantContentTypes()
        {
            List<string> contentTypeAliases = _contentReadRepository.GetAllContentAliases();
            List<string> typeAliasList = new List<string>();

            foreach (Type type in _typeList)
            {
                typeAliasList.Add(_attributeManager.GetContentTypeAttributes<DocumentTypeAttribute>(type).Alias);
            }

            return contentTypeAliases.Except(typeAliasList).ToList();
        }

        public void DeleteRedundantContentTypes(List<string> contentTypeAliases)
        {
            foreach (string contentTypeAlias in contentTypeAliases)
            {
                IContentType contentType = _retryableContentTypeService.GetContentType(contentTypeAlias);
                _retryableContentTypeService.Delete(contentType);
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Saves the allowed childe types.
        /// </summary>
        private void SaveAllowedChildeTypes()
        {
            _typeList.ForEach(docType => SaveAllowedChildeTypesInnerMethod(docType));
        }

        /// <summary>
        /// Updates the type.
        /// </summary>
        /// <param name="type">The type.</param>
        private void UpdateContentType(Type type)
        {
            DocumentTypeAttribute docAttribute = _attributeManager.GetContentTypeAttributes<DocumentTypeAttribute>(type);

            IContentType contentType = GetContentType(type);
            contentType.Name = docAttribute.Name;
            contentType.Icon = docAttribute.Icon;
            contentType.Thumbnail = docAttribute.Thumbnail;
            contentType.Description = docAttribute.Description;
            contentType.SetDefaultTemplate(_templateManager.GetTheDefaultTemplateOrCreateIt(docAttribute));
            contentType.AllowedTemplates = _templateManager.GetAllowedTemplateList(docAttribute);
            contentType.AllowedAsRoot = docAttribute.AllowAtRoot;

            contentType.AllowedContentTypes = _contentTypeFactory.CreateAllowedChildNodeTypeStructure(docAttribute);

            _propertyWriteRepository.UpdateProperties(type, contentType);

            _retryableContentTypeService.Save(contentType);
        }

        private void SaveAllowedChildeTypesInnerMethod(Type docType)
        {
            DocumentTypeAttribute docTypeAttr = _attributeManager.GetContentTypeAttributes<DocumentTypeAttribute>(docType);

            if (docTypeAttr.AllowedChildNodeTypes != null && docTypeAttr.AllowedChildNodeTypes.Length > 0)
            {
                List<ContentTypeSort> contentTypeSort = _contentTypeFactory.CreateAllowedChildNodeTypeStructure(docTypeAttr);
                IContentType contentType = _retryableContentTypeService.GetContentType(docTypeAttr.Alias);

                contentType.AllowedContentTypes = contentTypeSort.ToArray();
                _retryableContentTypeService.Save(contentType);

                Logger.WriteInfoLine<ContentWriteRepository>(
                    "Document type {0} was saved with {1} new type node types",
                    contentType.Name,
                    contentType.AllowedContentTypes.Count());
            }
        }

        private void Save(IEnumerable<Type> objectList, IContentType parent)
        {
            var dryRunMode = UmbracoService.GetDryRunMode();
            Logger.WriteInfoLine<ContentWriteRepository>("DocumentWriteRepository.Save: dryrun: {0}", dryRunMode);

            objectList.ForEach(child => SaveInnerMethod(child, parent));
        }

        private void SaveInnerMethod(Type type, IContentType parent)
        {
            IContentType contentType = GetContentType(type);
            _typeList.Add(type);
            LogBlock(type.Name);

            // does the document type allrighty exists, yes? Don't resave it.
            if (contentType != null)
            {
                Logger.WriteInfoLine<ContentWriteRepository>("Document type {0} already exist, checking for changes", type.Name);

                this.RemoveRedudantProperties(type, contentType);

                if (_contentTypeComparer.Compare(type, contentType))
                {
                    Logger.WriteInfoLine<ContentWriteRepository>("No changes found in {0}", type.Name);
                    NextContentType(type);
                }
                else
                {
                    // some thing has change get the documnent type and update it
                    Logger.WriteInfoLine<ContentWriteRepository>("The document or property atrributes of {0} have changed.", contentType.Name);
                    UpdateContentType(type);
                    NextContentType(type);
                }
            }
            else
            {
                // document doesn't exist, we need to add it. 
                Logger.WriteInfoLine<ContentWriteRepository>("Document type {0} doesn't exists, adding it", type.Name);
                IContentType levelTwoParent = this.SaveType(type, parent);
                SaveChildren(type, levelTwoParent);
            }
        }

        private void SaveChildren(Type type, IContentType documentType)
        {
            List<Type> children = _documentFinder.GetChildTypes(type);

            if (children.Any())
            {
                IContentType parent = _retryableContentTypeService.GetContentType(documentType.Alias);
                Save(children, parent);
            }
        }

        private void RemoveRedudantProperties(Type documentType, IContentType contentType)
        {
            // remove properties that are not needed anymore 
            foreach (PropertyType redundantProperty in _propertyReadRepository.CreateRedundantPropertiesList(documentType, contentType))
            {
                _propertyWriteRepository.RemoveProperty(contentType, redundantProperty);
            }
        }

        private void NextContentType(Type nextDocumentType)
        {
            IContentType levelTwoParent = _retryableContentTypeService.GetContentType(_contentReadRepository.GetContentTypeAlias(nextDocumentType));
            this.SaveChildren(nextDocumentType, levelTwoParent);
        }

        private IContentType GetContentType(Type type)
        {
            string alias = _contentReadRepository.GetContentTypeAlias(type);

            return _retryableContentTypeService.GetContentType(alias);
        }

        private void LogBlock(string contentTypeName)
        {
            Logger.WriteInfoLine<ContentWriteRepository>("Found document type with name: {0}", contentTypeName);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

using Mirabeau.uTransporter.Attributes;
using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.Logging;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Comparers
{
    /// <summary>
    /// Comparer class for document types
    /// </summary>
    /// <remarks></remarks>
    public class ContentTypeComparer : IContentTypeComparer
    {
        #region Private Fields

        private readonly IAttributeManager _attributeManager;

        private readonly ITemplateManager _templateManager;

        private readonly IPropertyComparer _propertyComparer;

        private readonly IContentTypeFactory _contentTypeFactory;

        private readonly ILog4NetWrapper _log = LogManagerWrapper.GetLogger("Mirabeau.UuTransporter");

        #endregion

        #region Public Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="managerFactory">IManagerFactory managerFactory</param>
        /// <param name="propertyComparer">IPropertyComparer propertyComparer</param>
        /// <param name="contentTypeFactory">IDocumentTypeFactory documentTypeFactory</param>
        public ContentTypeComparer(IManagerFactory managerFactory, IPropertyComparer propertyComparer, IContentTypeFactory contentTypeFactory)
        {
            _attributeManager = managerFactory.CreateAttributeManager();
            _templateManager = managerFactory.CreateTemplateManager();
            _propertyComparer = propertyComparer;
            _contentTypeFactory = contentTypeFactory;
        }

        /// <summary>
        /// Compares the specified document type agains an content type.
        /// </summary>
        /// <param name="type">Type of the document.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>Returns bool</returns>
        public bool Compare(Type type, IContentType contentType)
        {

            DocumentTypeAttribute docTypeAttribute = _attributeManager.GetContentTypeAttributes<DocumentTypeAttribute>(type);
            string contentTypeName = contentType.Name;

            if (docTypeAttribute.Name != contentType.Name)
            {
                LogChanges(contentTypeName, contentType.Name, docTypeAttribute.Name);
                return false;
            }

            // ToLower() because the if statement would return false if not
            if (docTypeAttribute.Alias != contentType.Alias)
            {
                LogChanges(contentTypeName, contentType.Alias, docTypeAttribute.Alias);
                return false;
            }

            if (contentType.Icon != docTypeAttribute.Icon)
            {
                LogChanges(contentTypeName, contentType.Icon, docTypeAttribute.Icon);
                return false;
            }

            if (contentType.Thumbnail != docTypeAttribute.Thumbnail)
            {
                LogChanges(contentTypeName, contentType.Thumbnail, docTypeAttribute.Thumbnail);
                return false;
            }

            if (contentType.Description != docTypeAttribute.Description)
            {
                LogChanges(contentTypeName, contentType.Description, docTypeAttribute.Description);
                return false;
            }

            if (!CompareAllowedTemplates(docTypeAttribute, contentType))
            {
                return false;
            }

            if (docTypeAttribute.DefaultTemplate != null && CompaireDefaultTemplates(contentType.DefaultTemplate, GetTheDefaulTemplate(docTypeAttribute.DefaultTemplate)))
            {
                return false;
            }

            if (!CompareAllowedChildNodeTypes(docTypeAttribute, contentType))
            {
                return false;
            }

            // Check if the properties are changed
            if (!_propertyComparer.Compare(type, contentType))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Compares the allowed templates.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>Returns bool</returns>
        public bool CompareAllowedTemplates(DocumentTypeAttribute attributes, IContentType contentType)
        {
            IList<ITemplate> allowedTemplates = _templateManager.GetAllowedTemplateList(attributes);
            IEnumerable<ITemplate> existingTemplates = contentType.AllowedTemplates.ToList();

            if (allowedTemplates == null)
            {
                return true;
            }

            if (allowedTemplates.Count != existingTemplates.Count())
            {
                return false;
            }

            foreach (ITemplate template in allowedTemplates)
            {
                // strip Template. from string before compaire
                if (!existingTemplates.Any(t => t.Alias.Substring(t.Alias.IndexOf(".", StringComparison.Ordinal) + 1).Trim() == template.Alias))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Compaires the default templates.
        /// </summary>
        /// <param name="existingAlias">The existing alias.</param>
        /// <param name="newAlias">The new alias.</param>
        /// <returns>Returns bool</returns>
        public bool CompaireDefaultTemplates(ITemplate existingAlias, TemplateAttribute newAlias)
        {
            if (existingAlias.Alias.Substring(existingAlias.Alias.IndexOf(".", StringComparison.Ordinal) + 1).Trim() != newAlias.Alias.ToLowerInvariant())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Compares the allowed child node types.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>Returns bool</returns>
        public bool CompareAllowedChildNodeTypes(DocumentTypeAttribute attributes, IContentType contentType)
        {
            List<ContentTypeSort> allowedChildNodeTypes = _contentTypeFactory.CreateAllowedChildNodeTypeStructure(attributes);
            IEnumerable<ContentTypeSort> existingAllowedChildNodeTypes = contentType.AllowedContentTypes.ToList();

            if (allowedChildNodeTypes == null)
            {
                return true;
            }

            if (allowedChildNodeTypes.Count != existingAllowedChildNodeTypes.Count())
            {
                return false;
            }

            foreach (ContentTypeSort allowedChildNodeType in allowedChildNodeTypes)
            {
                if (!existingAllowedChildNodeTypes.Any(t => t.Alias.Substring(t.Alias.IndexOf(".", StringComparison.Ordinal) + 1).Trim() == allowedChildNodeType.Alias))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the defaul template.
        /// </summary>
        /// <param name="temaplateType">Type of the temaplate.</param>
        /// <returns>Return the templates attributes</returns>
        private TemplateAttribute GetTheDefaulTemplate(Type temaplateType)
        {
            return _attributeManager.GetTemplateAttributes<TemplateAttribute>(temaplateType);
        }

        private void LogChanges(string contentTypeName, string previous, string updated)
        {
            _log.Info(string.Format("In document type {0} the value of '{1}' has changed to '{2}'", contentTypeName, previous, updated));
        }

        #endregion
    }
}
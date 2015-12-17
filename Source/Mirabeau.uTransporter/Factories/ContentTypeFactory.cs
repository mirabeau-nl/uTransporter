using System;
using System.Collections.Generic;
using System.Linq;

using Mirabeau.uTransporter.Attributes;
using Mirabeau.uTransporter.Builders;
using Mirabeau.uTransporter.Exceptions;
using Mirabeau.uTransporter.Extensions;
using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.Utils;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Factories
{
    /// <summary>
    /// ContentType Factory class
    /// </summary>
    public class ContentTypeFactory : IContentTypeFactory
    {
        private readonly ITemplateManager _templateManager;
        private readonly IPropertyFactory _propertyFactory;
        private readonly IContentReadRepository _contentReadRepository;

        public ContentTypeFactory(IManagerFactory managerFactory, IPropertyFactory propertyFactory, IContentReadRepository contentReadRepository)
        {
            _templateManager = managerFactory.CreateTemplateManager();
            _propertyFactory = propertyFactory;
            _contentReadRepository = contentReadRepository;
        }

        public IContentType CreateDocumentType(Type type, DocumentTypeAttribute docAttribute, int parentId, ContentTypeBuilder contentTypeBuilder = null)
        {
            if (type == null || docAttribute == null)
            {
                throw new ArgumentNullException(string.Format("Can't use ContentTypeBuilder with empty arguments, {0}", GetType().Name));
            }

            if (string.IsNullOrEmpty(docAttribute.Alias))
            {
                throw new AliasNullException(string.Format("Can't create a document type with no alias, please specify an alias in {0}", GetType().Name));
            }

            contentTypeBuilder = contentTypeBuilder ?? new ContentTypeBuilder(parentId);
            contentTypeBuilder.WithName(Util.TrimLength(docAttribute.Name, 255))
                .WithAlias(this.ValidateAlias(type, docAttribute.Alias))
                .WithDescription(docAttribute.Description)
                .IsAllowedAtRoot(docAttribute.AllowAtRoot)
                .AllowedTemplateList(_templateManager.CreateAllowedTemplateList(docAttribute).ToList())
                .WithIcon(docAttribute.Icon)
                .WithThumbnail(docAttribute.Thumbnail)
                .WithDefaultTemplate(_templateManager.GetTheDefaultTemplateOrCreateIt(docAttribute));

            IContentType contentType = contentTypeBuilder.Build();

            _propertyFactory.CreateDocumentTypeProperties(type, contentType);

            return contentType;
        }

        public List<ContentTypeSort> CreateAllowedChildNodeTypeStructure(DocumentTypeAttribute docTypeAttr)
        {
            List<ContentTypeSort> allowedTypes = new List<ContentTypeSort>();

            if (docTypeAttr.AllowedChildNodeTypes != null && docTypeAttr.AllowedChildNodeTypes.Length > 0)
            {
                foreach (Type allowedChildNodeType in docTypeAttr.AllowedChildNodeTypes)
                {
                    string allowedChildNodeTypeAlias = _contentReadRepository.GetContentTypeAlias(allowedChildNodeType);
                    int allowedChildNodeTypeId = _contentReadRepository.GetContentTypeId(allowedChildNodeTypeAlias);

                    allowedTypes.Add(new ContentTypeSort
                    {
                        Alias = allowedChildNodeTypeAlias,
                        Id = new Lazy<int>(() => allowedChildNodeTypeId)
                    });
                }
            }

            return allowedTypes;
        }

        private string ValidateAlias(Type type, string alias)
        {
            string result;

            if (string.IsNullOrEmpty(alias))
            {
                Logger.WriteInfoLine<ContentTypeFactory>("Document type alias attribute for class {0} is empty, using class name", type.Name);
                result = type.Name;
            }
            else
            {
                result = alias;
            }

            return Util.TrimLength(result, 255, type.Name);
        }
    }
}

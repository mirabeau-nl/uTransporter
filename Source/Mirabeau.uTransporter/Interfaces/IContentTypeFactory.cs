using System;
using System.Collections.Generic;

using Mirabeau.uTransporter.Attributes;
using Mirabeau.uTransporter.Builders;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Interfaces
{
    public interface IContentTypeFactory
    {
        IContentType CreateDocumentType(Type type, DocumentTypeAttribute docAttribute, int parentId, ContentTypeBuilder contentTypeBuilder = null);

        List<ContentTypeSort> CreateAllowedChildNodeTypeStructure(DocumentTypeAttribute docTypeAttr);
    }
}

using System;
using System.Collections.Generic;

using Mirabeau.uTransporter.Attributes;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Interfaces
{
    public interface IPropertyFactory
    {
        void CreateDocumentTypeProperties(Type documentType, IContentType contentType);

        void CreateDocumentTypeProperty(DocumentTypePropertyAttribute property, IContentType contentType);

        void CreatePropertyGroup(Type tab, IContentType contentType, PropertyType propertyType);

        List<PropertyType> CreatePropertyTypeList(Type docType, IContentType contentType);
    }
}
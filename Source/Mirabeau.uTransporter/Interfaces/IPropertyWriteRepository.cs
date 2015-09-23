using System;

using Mirabeau.uTransporter.Attributes;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Interfaces
{
    public interface IPropertyWriteRepository
    {
        void UpdateProperties(Type type, IContentType contentType);

        void RemoveProperty(IContentType contentType, PropertyType propertyType);

        void CreateMissingProperties(Type type, IContentType contentType, DocumentTypePropertyAttribute property);
    }
}

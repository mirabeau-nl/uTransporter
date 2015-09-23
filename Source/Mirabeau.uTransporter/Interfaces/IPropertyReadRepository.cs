using System;
using System.Collections.Generic;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Interfaces
{
    public interface IPropertyReadRepository
    {
        IDictionary<string, PropertyGroup> GetTabNamesForDocumentType(IContentType contentType);

        IEnumerable<PropertyType> GetAllPropertiesFromContentType(IContentType contentType);

        List<PropertyType> CreateMissingPropertiesList(
            Type docType,
            IContentType contentType);

        List<PropertyType> CreateRedundantPropertiesList(
            Type docType,
            IContentType contentType);

        int CountPropertiesFromContentType(IContentType contentType);

        int CountPropertiesFromContentTypes(IEnumerable<IContentType> contentTypes);

        object GetValueFromEnum(float id);

        int CountPropertiesFromDocumentTypeAttribute(Type docType);

        IEnumerable<PropertyGroup> GetAllPropertyGroups(IEnumerable<IContentType> contentTypes);
    }
}
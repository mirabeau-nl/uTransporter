using System;
using System.Collections.Generic;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Interfaces
{
    public interface IContentReadRepository
    {
        IEnumerable<IContentType> GetAllContentTypes();

        int GetAllContentTypesCount();

        IEnumerable<IContentType> GetContentTypesBasedOnIds(int[] contentIds);

        IContentType GetContentTypesBasedOnId(int contentId);

        string GetContentTypeAlias(Type typeDocType);

        List<string> GetAllContentAliases();

        int GetContentTypeId(string alias);

        IContentType GetContentTypeBasedOnAlias(string alias);

        int CountAllPropertiesFromAllContentTypes();
    }
}
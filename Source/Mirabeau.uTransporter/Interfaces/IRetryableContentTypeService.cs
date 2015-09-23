using System.Collections.Generic;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Interfaces
{
    public interface IRetryableContentTypeService
    {
        IContentType GetContentType(int id);
        IContentType GetContentType(string alias);
        IEnumerable<IContentType> GetAllContentTypes(params int[] ids);
        IEnumerable<IContentType> GetContentTypeChildren(int id);
        void Save(IContentType contentType, int userId = 0);
        void Save(IEnumerable<IContentType> contentTypes, int userId = 0);
        void Delete(IContentType contentType, int userId = 0);
        void Delete(IEnumerable<IContentType> contentTypes, int userId = 0);
        string GetDtd();
        string GetContentTypesDtd();
        bool HasChildren(int id);
    }
}
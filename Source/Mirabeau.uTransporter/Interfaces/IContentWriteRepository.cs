using System;
using System.Collections.Generic;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Interfaces
{
    public interface IContentWriteRepository
    {
        IContentType SaveType(Type type, IContentType parent);

        void RemoveContentType(List<Type> typeList);

        List<string> FindRedundantContentTypes();

        void DeleteRedundantContentTypes(List<string> contentTypeAliases);

        void SaveList(IEnumerable<Type> objectList);
    }
}
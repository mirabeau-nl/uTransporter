using System;

namespace Mirabeau.uTransporter.Interfaces
{

    public interface IContentTypeValidator 
    {
        string ValidateContentTypeName(Type type, string name);

        string ValidateContentTypeAlias(Type type, string alias);
    }
}

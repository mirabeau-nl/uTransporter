using System.CodeDom;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Interfaces
{
    public interface IDocumentTypeGenerator
    {
        int Generate(string targetPath);

        CodeTypeDeclaration CreateClass(IContentType contentType);

        void Delete(string targetPath);
    }
}
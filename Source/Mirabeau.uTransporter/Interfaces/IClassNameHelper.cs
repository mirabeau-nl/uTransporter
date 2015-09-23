using System.CodeDom;
using System.Text.RegularExpressions;

namespace Mirabeau.uTransporter.Interfaces
{
    public interface IClassNameHelper
    {
        Regex RegEx { get; }

        CodeTypeDeclaration CreateClass(string name);

        string CreateSafeClassName(string unsafeClassName);

        string CheckForTrailingSlash(string targetPath);

        string RemoveInvalidFilenameChars(string text);

        string TrimDigitFromStartOfString(string text);
    }
}
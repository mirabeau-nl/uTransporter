using System.CodeDom;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

using Humanizer;

using Mirabeau.uTransporter.Interfaces;

namespace Mirabeau.uTransporter.Extensions
{
    public class ClassNameHelper : IClassNameHelper
    {
        private readonly Regex rgx = new Regex("[^a-zA-Z0-9 -]");

        public Regex RegEx
        {
            get
            {
                return rgx;
            }
        }

        public CodeTypeDeclaration CreateClass(string name)
        {
            string className = this.CreateSafeClassName(name);
            CodeTypeDeclaration targetClass = new CodeTypeDeclaration(className);
            targetClass.IsClass = true;

            return targetClass;
        }

        public string CreateSafeClassName(string unsafeClassName)
        {
            // remove digit and invalid chars
            string className = ReplaceSymbolsWithEmptyString(unsafeClassName);
            className = ReplaceDigitsWithString(className);
            className = DehumanizeAndTrimClassName(className);
            className = RemoveInvalidFilenameChars(className);
            className = TrimDigitFromStartOfString(className);

            return className;
        }

        public string CheckForTrailingSlash(string targetPath)
        {
            return targetPath.EndsWith(@"\") ? targetPath : targetPath + @"\";
        }

        public string RemoveInvalidFilenameChars(string text)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                text = text.Replace(c, '_');
            }

            return text;
        }

        public string TrimDigitFromStartOfString(string text)
        {
            Regex regex = new Regex(@"^\d+");
            Match match = regex.Match(text);

            if (match.Length > 0)
            {
                text = text.Remove(0, match.Length);
            }

            return text;
        }

        private string ReplaceSymbolsWithEmptyString(string className)
        {
            return RegEx.Replace(className, string.Empty);
        }

        private string ReplaceDigitsWithString(string className)
        {
            string newClassName = string.Empty;

            foreach (char c in className)
            {
                if (char.IsDigit(c))
                {
                    int i = int.Parse(c.ToString(CultureInfo.InvariantCulture));
                    newClassName += i.ToWords();
                    newClassName += " ";
                }
                else
                {
                    newClassName += c;
                }
            }

            return newClassName;
        }

        private string DehumanizeAndTrimClassName(string humanString)
        {
            return humanString.Dehumanize().Replace(" ", string.Empty);
        }
    }
}

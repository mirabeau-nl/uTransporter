using System.CodeDom;

namespace Mirabeau.uTransporter.Interfaces
{
    public interface IFileHelper
    {
        void WriteFile(string filename, CodeCompileUnit codeCompileUnit);

        void WriteToTextFile(string text, string fileName);

        string ReadFirstLineFromFile(string fileName);

        void DeleteFilesInDir(string dirName);
    }
}
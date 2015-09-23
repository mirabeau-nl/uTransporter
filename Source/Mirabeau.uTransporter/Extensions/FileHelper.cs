using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.Logging;

namespace Mirabeau.uTransporter.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class FileHelper : IFileHelper
    {
        private readonly ILog4NetWrapper _log;

        public FileHelper()
        {
            _log = LogManagerWrapper.GetLogger("Mirabeau.uTransporter");
        }
        /// <summary>
        /// Writes the file to disk.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="codeCompileUnit">The code compile unit.</param>
        public void WriteFile(string filename, CodeCompileUnit codeCompileUnit)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";

            CreateDirectory(filename);

            using (StreamWriter sourceWriter = new StreamWriter(filename))
            {
                provider.GenerateCodeFromCompileUnit(codeCompileUnit, sourceWriter, options);

                _log.Info("file with name: {0} written to file system", filename);
            }

            provider.Dispose();
        }

        /// <summary>
        /// Write a string to a text file.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="fileName">Name of the file.</param>
        public void WriteToTextFile(string text, string fileName)
        {
            string rootPath = Environment.CurrentDirectory;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(text);

            if (!this.FileExists(rootPath, fileName))
            {
                this.CreateFile(rootPath, fileName);
            }

            using (StreamWriter outfile = new StreamWriter(Path.Combine(rootPath, fileName) + @".txt", true))
            {
                outfile.WriteAsync(sb.ToString());
            }
        }

        /// <summary>
        /// Reads the first line from file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>the first line of a file</returns>
        public string ReadFirstLineFromFile(string fileName)
        {
            string rootPath = Environment.CurrentDirectory;
            string text;

            if (!this.FileExists(rootPath, fileName))
            {
                _log.Error("Can't find/read from file with name {0}", fileName);
                throw new Exception();
            }

            using (StreamReader sr = new StreamReader(Path.Combine(rootPath, fileName) + @".txt", true))
            {
                text = sr.ReadLine();
            }

            return text;
        }

        /// <summary>
        /// Files the delete.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        public void FileDelete(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (UnauthorizedAccessException e)
            {
                _log.Error("Can't delete file in directory {0}", path);
            }
        }

        /// <summary>
        /// Deletes the files in dir.
        /// </summary>
        /// <param name="dirName">Name of the dir.</param>
        public void DeleteFilesInDir(string dirName)
        {
            IEnumerable<string> files = GetFiles(dirName);

            foreach (var file in files)
            {
                FileDelete(file);
            }
        }

        private IEnumerable<string> GetFiles(string targetDirectory)
        {
            List<string> files = new List<string>();

            try
            {
                Ensure.DirectoryExists(targetDirectory);
                files = Directory.GetFiles(targetDirectory).ToList();
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException(
                    string.Format("Unable to access files in directory {0}, check your permissions", targetDirectory));
            }
            catch (DirectoryNotFoundException)
            {
                _log.Error("Directory not found, no items to delete");
            }


            return files;
        }

        private bool FileExists(string path, string fileName)
        {
            if (!File.Exists(Path.Combine(path, fileName)))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Creates the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <exception cref="System.UnauthorizedAccessException">Unauthorized</exception>
        private void CreateFile(string path, string fileName)
        {
            try
            {
                File.Create(Path.Combine(path, fileName));
            }
            catch (UnauthorizedAccessException)
            {
                _log.Error("Can't create file in directory {0}, check your permissions...", path);
            }
        }

        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <param name="filename">The filename.</param>
        private void CreateDirectory(string filename)
        {
            FileInfo fileInfo = new FileInfo(filename);
            DirectoryInfo directoryInfo = fileInfo.Directory;

            if (directoryInfo != null && !directoryInfo.Exists)
            {
                try
                {
                    directoryInfo.Create();
                }
                catch (IOException e)
                {
                    _log.Error("Can't create, check your permissions...");
                }
            }
        }
    }
}
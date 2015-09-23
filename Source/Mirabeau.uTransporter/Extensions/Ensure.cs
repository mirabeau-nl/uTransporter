using System;
using System.IO;

namespace Mirabeau.uTransporter.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class Ensure
    {
        /// <summary>
        /// Arguments the not null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void ArgumentNotNull([ValidatedNotNull]object value, string name)
        {
            if (value != null)
            {
                return;
            }

            throw new ArgumentNullException(name);
        }

        /// <summary>
        /// Arguments the not null or string empty.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentException">String cannot be empty</exception>
        public static void ArgumentNotNullOrStringEmpty([ValidatedNotNull]string value, string name)
        {
            ArgumentNotNull(value, name);

            if (!string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            throw new ArgumentException("String cannot be empty", name);
        }

        /// <summary>
        /// Directories the exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <exception cref="System.IO.DirectoryNotFoundException">Directory not found</exception>
        public static void DirectoryExists(string path)
        {
            if (Directory.Exists(path))
            {
                return;
            }

            throw new DirectoryNotFoundException("Directory not found");
        }

        /// <summary>
        /// Files the exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <exception cref="System.IO.FileNotFoundException">File Not found</exception>
        public static void FileExists(string path)
        {
            if (System.IO.File.Exists(path))
            {
                return;
            }

            throw new FileNotFoundException("File Not found");
        }
    }

    internal sealed class ValidatedNotNullAttribute : Attribute
    {
    }
}

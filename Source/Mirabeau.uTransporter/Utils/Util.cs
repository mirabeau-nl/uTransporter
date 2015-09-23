using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

using Humanizer;

using Microsoft.SqlServer.Management.Common;

using Umbraco.Core;

namespace Mirabeau.uTransporter.Utils
{
    public static class Util
    {
        /// <summary>Trims a string to specific length</summary>
        /// <param name="input">The input.</param>
        /// <param name="length">The length.</param>
        /// <param name="source">The source.</param>
        /// <returns>The <see cref="string"/></returns>
        public static string TrimLength(string input, int length, string source = "default path")
        {
            if (input.Length > length)
            {
                input = input.Substring(0, length);
            }

            return input;
        }

        /// <summary>Check's if the sychronisation key value pair is true or false</summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsSyncActive()
        {
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["umbracoSyncActive"]) || ConfigurationManager.AppSettings["umbracoSyncActive"] == "false")
            {
                return false;
            }

            return true;

        }

        /// <summary>
        /// Determines whether [is dry run active].
        /// </summary>
        /// <returns>true / false</returns>
        public static bool IsDryRunActive()
        {
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["DryRun"]) || ConfigurationManager.AppSettings["DryRun"] == "false")
            {
                return false;
            }

            return true;
        }

        /// <summary>Check's if there's a connection to the database. If database string is correct the Umbraco API returns null</summary>
        /// <returns><see cref="bool"/></returns>
        public static bool IsDataConfiguredAndAvailable()
        {
            if (ApplicationContext.Current.DatabaseContext.Database.Connection != null)
            {
                throw new SqlServerManagementException("Database is not configured, verify your connection string.");
            }

            return true;
        }

        /// <summary>
        /// Dehumanizes and trim an string.
        /// </summary>
        /// <param name="humanString">The human string.</param>
        /// <returns>An humanized and trimed string</returns>
        public static string DehumanizeAndTrim(string humanString)
        {
            return humanString.Dehumanize().Replace(" ", string.Empty);
        }

        /// <summary>
        /// Combines the paths.
        /// </summary>
        /// <param name="path1">The path1.</param>
        /// <param name="paths">The paths.</param>
        /// <returns>The combined path</returns>
        public static string CombinePaths(string path1, params string[] paths)
        {
            return paths.Aggregate(path1, (acc, p) => Path.Combine(acc, p));
        }

        public static List<string> GetFilesNamesFromDir(string dirPath)
        {
            List<string> result = Directory.GetFiles(dirPath, "*.cs")
                                .Select(Path.GetFileNameWithoutExtension)
                                .ToList();

            return result;
        }

        /// <summary>
        /// Determines if a directory exists
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool DoesDirExists(string path)
        {
            if (!Directory.Exists(path))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines if a file exists
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool DoesFileExists(string file)
        {
            if (!File.Exists(file))
            {
                return false;
            }

            return true;
        }
    }
}
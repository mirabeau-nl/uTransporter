using System;
using System.IO;
using System.Security.Authentication;
using System.Web.Hosting;

using Mirabeau.uTransporter.Models;

namespace Mirabeau.uTransporter.Logging
{
    public class LogFileService
    {
        private string BasePath
        {
            get { return "~/App_Data/Logs/"; }
        }

        public static string UmbracoSyncLogPath
        {
            get { return "~/App_Plugins/umbraco-sync-dashboard/logs/"; }
        }

        private string FileName
        {
            get { return "UmbracoTraceLog.txt"; } 
        }

        public static string SyncFileName
        {
            get { return "UmbracoSyncLog"; }
        }

        public static string BuildFilePathWithHostName()
        {
            DateTime dateTime = new DateTime();
            string fullPluginPath = HostingEnvironment.MapPath(UmbracoSyncLogPath);

            return fullPluginPath + SyncFileName + "-" + dateTime.ToString("yy-MM-dd") + ".txt";
        }

        public static string BuildFilePath()
        {
            DateTime dateTime = new DateTime();
            string substring = UmbracoSyncLogPath.Substring(1);

            return substring + SyncFileName + "-" + dateTime.ToString("yy-MM-dd") + ".txt";
        }

        public LogFile GetLogFile()
        {
            string basePath = HostingEnvironment.MapPath(BasePath);
            string fullPath = Path.Combine(basePath, FileName);

            FileInfo logFile = new FileInfo(fullPath);

            return new LogFile(logFile.LastWriteTime, logFile.FullName);
        }

        public bool CreateLogFile()
        {
            bool result = true;

            string fullFileName = BuildFilePathWithHostName();

            if (!File.Exists(fullFileName))
            {
                try
                {
                    using (StreamWriter streamWriter = File.CreateText(fullFileName))
                    {
                    }
                }
                catch (AuthenticationException e)
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
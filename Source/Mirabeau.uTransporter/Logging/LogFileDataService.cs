using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Hosting;

using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.Models;

namespace Mirabeau.uTransporter.Logging
{
    public class LogFileDataService
    {
        public List<LogFileDataItem> Entries = new List<LogFileDataItem>();

        private string pattern = @"^.*mirabeau\.umbraco\.synctool.*$";

        private ILog4NetWrapper _log = LogManagerWrapper.GetLogger("Mirabeau.uTransporter");

        public List<LogFileDataItem> GetLogData(string path)
        {
            string logData = null;
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            string LogFile = HostingEnvironment.MapPath(path);

            try
            {
                logData = File.ReadAllText(LogFile);
            }
            catch (IOException e)
            {
                _log.Error("Can read from file", e);
            }

            string[] lines = logData.Split('\n');

            foreach (string line in lines)
            {
                if (regex.IsMatch(line))
                {
                    LogFileDataItem item = new LogFileDataItem();
                    item.Date = line.Substring(0, 19);
                    item.Message = line.Substring(59);

                    Entries.Add(item);
                }
            }

            return Entries;
        }

        public string WriteToFile()
        {
            string fullPath = LogFileService.BuildFilePathWithHostName();
            
            using (StreamWriter sw =  new StreamWriter(fullPath))
            {
                IEnumerable<LogFileDataItem> logDataItems = this.GetLogData("~/App_Data/Logs/UmbracoTraceLog.txt");

                foreach (LogFileDataItem logDataitem in logDataItems)
                {
                    sw.WriteLine(logDataitem.Date + " " + logDataitem.Message);
                }
            }

            return LogFileService.BuildFilePath();
        }
    }
}

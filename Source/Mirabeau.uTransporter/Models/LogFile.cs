using System;

namespace Mirabeau.uTransporter.Models
{
    public class LogFile
    {
        public DateTime DateTime { get; set; }
        public string Path { get; set; }

        public LogFile(DateTime dateTime, string path)
        {
            this.DateTime = dateTime;
            this.Path = path;
        }
    }
}

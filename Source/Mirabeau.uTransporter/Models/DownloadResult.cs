using System;

using Newtonsoft.Json;

namespace Mirabeau.uTransporter.Models
{
    public class DownloadResult
    {
        [JsonProperty(PropertyName = "successful")]
        public bool Successful { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "startedTimestamp")]
        public DateTime StartedTimestamp { get; set; }

        [JsonProperty(PropertyName = "finishedTimestamp")]
        public DateTime FinishedTimestamp { get; set; }

        [JsonProperty(PropertyName = "eplasedTime")]
        public TimeSpan EplasedTime { get; set; }

        [JsonProperty(PropertyName = "generatedClasses")]
        public int? GeneratedClasses { get; set; }

        [JsonProperty(PropertyName = "generatedTemplates")]
        public int? GeneratedTemplates { get; set; }

        [JsonProperty(PropertyName = "generatedTabs")]
        public int? GeneratedTabs { get; set; }

        [JsonProperty(PropertyName = "filePath")]
        public string FilePath { get; set; }
    }
}

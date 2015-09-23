using System;

using Newtonsoft.Json;

namespace Mirabeau.uTransporter.Models
{
    public class RemoveResult
    {
        [JsonProperty(PropertyName = "successful")]
        public bool Successful { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "startedTimestamp")]
        public DateTime StartedTimestamp { get; set; }

        [JsonProperty(PropertyName = "finishedTimestamp")]
        public DateTime FinishedTimestamp { get; set; }

        [JsonProperty(PropertyName = "elapsedTime")]
        public TimeSpan ElapsedTime { get; set; }

        [JsonProperty(PropertyName = "numberOfDocumentTypesRemoved")]
        public int NumberOfDocumentTypesRemoved { get; set; }

        [JsonProperty(PropertyName = "numberOfTemplatesRemoved")]
        public int NumberOfTemplatesRemoved { get; set; }
    }
}

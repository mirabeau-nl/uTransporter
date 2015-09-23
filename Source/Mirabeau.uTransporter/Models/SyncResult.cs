using System;

using Newtonsoft.Json;

namespace Mirabeau.uTransporter.Models
{
    public class SyncResult
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

        [JsonProperty(PropertyName = "numberOfDocumentTypesChecked")]
        public int NumberOfDocumentTypesChecked { get; set; }

        [JsonProperty(PropertyName = "numberOfTemplatesAdded")]
        public int NumberOfTemplatesAdded { get; set; }

        [JsonProperty(PropertyName = "numberOfPropertiesAdded")]
        public int NumberOfPropertiesAdded { get; set; }

        [JsonProperty(PropertyName = "numberOfDocumentTypesAdded")]
        public int NumberOfDocumentTypesAdded { get; set; }
    }
}

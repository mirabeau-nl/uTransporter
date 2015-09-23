using System;

using Newtonsoft.Json;

namespace Mirabeau.uTransporter.Models
{
    /// <summary>
    /// GenerateResult object
    /// </summary>
    public class GenerateResult
    {
        /// <summary>
        /// Gets or sets the Successful property
        /// </summary>
        [JsonProperty(PropertyName = "successful")]
        public bool Successful { get; set; }

        /// <summary>
        /// Gets or sets the message property
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the StartedTimestamp property
        /// </summary>
        [JsonProperty(PropertyName = "startedTimestamp")]
        public DateTime StartedTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the FinishedTimestamp property
        /// </summary>
        [JsonProperty(PropertyName = "finishedTimestamp")]
        public DateTime FinishedTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the ElapsedTime property
        /// </summary>
        [JsonProperty(PropertyName = "elapsedTime")]
        public TimeSpan ElapsedTime { get; set; }

        /// <summary>
        /// Gets or sets the GeneratedClasses property
        /// </summary>
        [JsonProperty(PropertyName = "generatedClasses")]
        public int GeneratedClasses { get; set; }

        /// <summary>
        /// Gets or sets the GeneratedTemplates property
        /// </summary>
        [JsonProperty(PropertyName = "generatedTemplates")]
        public int? GeneratedTemplates { get; set; }

        /// <summary>
        /// Gets or sets the GeneratedTabs property
        /// </summary>
        [JsonProperty(PropertyName = "generatedTabs")]
        public int? GeneratedTabs { get; set; }

        [JsonProperty(PropertyName = "elapsedTimeShort")]
        public string ElapsedTimeShort
        {
            get
            {
                return ElapsedTime.ToString("g");
            }
        }
    }

}

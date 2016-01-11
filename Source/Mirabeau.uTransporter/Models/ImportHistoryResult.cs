using System.Collections.Generic;

using Mirabeau.uTransporter.Persistence.Models;

using Newtonsoft.Json;

namespace Mirabeau.uTransporter.Models
{
    public class ImportHistoryResult
    {
        public ImportHistoryResult()
        {
            ImportHistories = new List<ImportHistory>();
        }

        [JsonProperty(PropertyName = "importHistory")]
        public List<ImportHistory> ImportHistories { get; set; }
    }
}

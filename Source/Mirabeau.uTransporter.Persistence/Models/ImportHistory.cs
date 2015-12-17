using System;

using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Mirabeau.uTransporter.Persistence.Models
{
    [TableName("uTransporterImportHistory")]
    [PrimaryKey("Id", autoIncrement = true)]
    [ExplicitColumns]
    public class ImportHistory
    {
        [Column("id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [Column("documenttypes")]
        public int NumberOfDocumentTypes { get; set; }

        [Column("properties")]
        public int NumberOfProperties { get; set; }

        [Column("datetime")]
        public DateTime DateTime { get; set; }

        [Column("duration")]
        public float Duration { get; set; }

        [Column("user")]
        public string User { get; set; }
    }
}

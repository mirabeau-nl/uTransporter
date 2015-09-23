using Mirabeau.uTransporter.Attributes;
using Mirabeau.uTransporter.Enums;
using Mirabeau.uTransporter.Models;

namespace Mirabeau.uTransporter.UnitTests.Stubs {
    [DocumentType(

   )]
    public class TestDocumentTypeBaseMissingAttribute : IDocumentTypeBase {

        [DocumentTypeProperty(UmbracoPropertyType.CheckBoxList)]
        public string PageTitle { get; set; }

    }
}
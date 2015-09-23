using Mirabeau.uTransporter.Attributes;
using Mirabeau.uTransporter.Enums;
using Mirabeau.uTransporter.Models;

namespace Mirabeau.uTransporter.UnitTests.Stubs
{
    [DocumentType(
       Name = "empty",
       Alias = "empty",
       Icon = "folder.gif",
       Thumbnail = "folder.png",
       Description = "A Description",
       DefaultTemplate = null,
       AllowAtRoot = true

   )]
    public class TestDocumentTypeBaseEmpty : IDocumentTypeBase
    {

        [DocumentTypeProperty(
            UmbracoPropertyType.CheckBoxList,
            Name = "",
            Alias = "",
            Description = "A short description",
            Mandatory = true,
            Tab = typeof(Help),
            ValidationRegExp = ""
            )]
        public string PageTitle { get; set; }

        [DocumentTypeProperty(
            UmbracoPropertyType.CheckBoxList,
           Name = "",
           Alias = "",
           Description = "A short description",
           Mandatory = true,
           Tab = typeof(Help),
           ValidationRegExp = ""
           )]
        public string ContentTitle { get; set; }

    }
}
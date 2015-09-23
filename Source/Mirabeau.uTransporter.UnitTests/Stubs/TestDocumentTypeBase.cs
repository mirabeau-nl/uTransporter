using Mirabeau.uTransporter.Attributes;
using Mirabeau.uTransporter.Enums;
using Mirabeau.uTransporter.Models;

namespace Mirabeau.uTransporter.UnitTests.Stubs
{
    [DocumentType(
       Name = "BarPage",
       Alias = "BarPage",
       Icon = "folder.gif",
       Thumbnail = "folder.png",
       Description = "A Description",
       DefaultTemplate = null,
       AllowAtRoot = true)]
    public class TestDocumentTypeBase : IDocumentTypeBase
    {
        [DocumentTypeProperty(
            UmbracoPropertyType.CheckBoxList,
            Name = "Bar Page Title Property",
            Alias = "Bar Page Title Property",
            Description = "A short description",
            Mandatory = true,
            Tab = typeof(Help),
            ValidationRegExp = "")]
        public string PageTitle { get; set; }

        [DocumentTypeProperty(
            UmbracoPropertyType.CheckBoxList,
           Name = "Bar Content Title Property",
           Alias = "Bar Content Title Property",
           Description = "A short description",
           Mandatory = true,
           Tab = typeof(Brand),
           ValidationRegExp = "")]
        public string ContentTitle { get; set; }
    }
}
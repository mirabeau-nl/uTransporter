using Mirabeau.uTransporter.Attributes;
using Mirabeau.uTransporter.Enums;

namespace Mirabeau.uTransporter.UnitTests.Stubs {
    [DocumentType(
       Name = "BarPage",
       Alias = "BarPage",
       Icon = "folder.gif",
       Thumbnail = "folder.png",
       Description = "A Description",
       DefaultTemplate = null,
       AllowAtRoot = true)]
    public class TestDocumentTypeBaseThirdLevel : TestDocumentTypeBaseSecondLevel 
    {

        [DocumentTypeProperty(
            UmbracoPropertyType.CheckBoxList,
            Name = "Bar Page Title Property",
            Alias = "Bar Page Title Property",
            Description = "A short description",
            Mandatory = true,
            Tab = typeof(Brand),
            ValidationRegExp = "")]
        public string PageTitle12134 { get; set; }

        [DocumentTypeProperty(
            UmbracoPropertyType.CheckBoxList,
           Name = "Bar Content Title Property",
           Alias = "Bar Content Title Property",
           Description = "A short description",
           Mandatory = true,
           Tab = typeof(Brand),
           ValidationRegExp = "")]
        public string ContentTitle2134 { get; set; }
    }
}
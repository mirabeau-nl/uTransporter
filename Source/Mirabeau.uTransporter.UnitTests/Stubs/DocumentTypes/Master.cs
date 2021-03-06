//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Mirabeau.uTransporter.Attributes;
using Mirabeau.uTransporter.Enums;
using Mirabeau.uTransporter.Models;

namespace Mirabeau.uTransporter.UnitTests.Stubs.DocumentTypes
{
    [DocumentType(Name="Master", Alias="Master", Icon=".sprTreeDeveloperMacro", Thumbnail="folder.png", Description="", AllowAtRoot=false, AllowedTemplates=new System.Type[0], AllowedChildNodeTypes=new System.Type[0])]
    public class Master : IDocumentTypeBase
    {
        
        private string _Background;
        
        private string _QuotePicker;
        
        private string _PageTitle;
        
        private string _PageDescription;
        
        [DocumentTypeProperty(UmbracoPropertyType.MediaPicker, Name="Background", Alias="background", Description="", Tab=typeof(Tabs.Settings), SortOrder=0, Mandatory=false, ValidationRegExp="")]
        public string Background
        {
            get
            {
                return this._Background;
            }
            set
            {
                this._Background = value;
            }
        }
        
        [DocumentTypeProperty(UmbracoPropertyType.Other, OtherTypeName="MIRA:QuotePicker", Name="QuotePicker", Alias="quotePicker", Description="", Tab=typeof(Tabs.Settings), SortOrder=0, Mandatory=false, ValidationRegExp="")]
        public string QuotePicker
        {
            get
            {
                return this._QuotePicker;
            }
            set
            {
                this._QuotePicker = value;
            }
        }
        
        [DocumentTypeProperty(UmbracoPropertyType.Textstring, Name="Page Title", Alias="pageTitle", Description="", Tab=typeof(Tabs.Seo), SortOrder=0, Mandatory=false, ValidationRegExp="")]
        public string PageTitle
        {
            get
            {
                return this._PageTitle;
            }
            set
            {
                this._PageTitle = value;
            }
        }
        
        [DocumentTypeProperty(UmbracoPropertyType.TextBoxMultiple, Name="Page Description", Alias="pageDescription", Description="", Tab=typeof(Tabs.Seo), SortOrder=0, Mandatory=false, ValidationRegExp="")]
        public string PageDescription
        {
            get
            {
                return this._PageDescription;
            }
            set
            {
                this._PageDescription = value;
            }
        }
    }
}

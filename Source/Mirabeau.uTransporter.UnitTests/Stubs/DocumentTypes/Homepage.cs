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

namespace Mirabeau.uTransporter.UnitTests.Stubs.DocumentTypes
{
    [DocumentType(Name="Homepage", Alias="Homepage", Icon="house.png", Thumbnail="folder.png", Description="", AllowAtRoot=true, DefaultTemplate=typeof(Templates.Homepage), AllowedTemplates=new System.Type[] {
            typeof(Templates.Homepage)}, AllowedChildNodeTypes=new System.Type[] {
            typeof(DocumentTypes.Vestiging),
            typeof(DocumentTypes.Content),
            typeof(DocumentTypes.Contact),
            typeof(DocumentTypes.Portfolio),
            typeof(DocumentTypes.VacaturesOverzicht)})]
    public class Homepage : Master
    {
    }
}
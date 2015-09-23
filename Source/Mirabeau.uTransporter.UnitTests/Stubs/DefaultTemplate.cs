using Mirabeau.uTransporter.Attributes;

namespace Mirabeau.uTransporter.UnitTests.Stubs
{
    [Template(
        Name = "Default Template",
        Alias = "defaulttemplate",
        Content = @"<%@ Master Language='C#' MasterPageFile='~/umbraco/masterpages/default.master' AutoEventWireup='true' %>
                                <asp:Content ContentPlaceHolderID='ContentPlaceHolderDefault' runat='server'></asp:Content>")]
    public class DefaultTemplate
    {
    }
}
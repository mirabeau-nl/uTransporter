namespace Mirabeau.uTransporter.Interfaces
{
    public interface IManagerFactory
    {
        ITemplateManager CreateTemplateManager();

        IDataTypeManager CreateDataTypeManager();

        IPropertyManager CreatePropertyManager();

        IAttributeManager CreateAttributeManager();
    }
}
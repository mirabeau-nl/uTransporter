namespace Mirabeau.uTransporter.Interfaces
{
    public interface IValidatorFactory
    {
        IContentTypeValidator CreateDocumentTypeValidator();

        IPropertyValidator CreatePropertyValidator();
    }
}
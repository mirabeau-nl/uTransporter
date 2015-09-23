using Mirabeau.uTransporter.Interfaces;

namespace Mirabeau.uTransporter.Factories
{
    public class ValidatorFactory : IValidatorFactory
    {
        private readonly IPropertyValidator propertyValidator;

        private readonly IContentTypeValidator _contentTypeValidator;

        public ValidatorFactory(IPropertyValidator propertyValidator, object dummy)
        {
            // TODO: fix this
            this.propertyValidator = propertyValidator;
            this._contentTypeValidator = null;
        }

        public IContentTypeValidator CreateDocumentTypeValidator()
        {
            return _contentTypeValidator;
        }

        public IPropertyValidator CreatePropertyValidator()
        {
            return propertyValidator;
        }
    }
}
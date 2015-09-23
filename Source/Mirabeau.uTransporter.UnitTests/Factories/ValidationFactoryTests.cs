using Mirabeau.uTransporter.Factories;
using Mirabeau.uTransporter.Interfaces;

using NUnit.Framework;

using Rhino.Mocks;

namespace Mirabeau.uTransporter.UnitTests.Factories
{
    [TestFixture]
    public class ValidationFactoryTests
    {
        private IValidatorFactory validatorFactory;
        private IPropertyValidator propertyValidator;
        private IContentTypeValidator _contentTypeValidator;

        [SetUp]
        public void SetUp()
        {
            propertyValidator = MockRepository.GenerateStrictMock<IPropertyValidator>();
            _contentTypeValidator = MockRepository.GenerateStrictMock<IContentTypeValidator>();
        }

        [Test]
        public void CreatePropertyValidator_CreateAndReturn_ReturnPropertyValidator()
        {
            validatorFactory = new ValidatorFactory(propertyValidator, _contentTypeValidator);
            var actual = validatorFactory.CreatePropertyValidator();
            Assert.AreEqual(propertyValidator, actual);            
        }

        [Test]
        public void CreatePropertyValidator_CreateAndReturn_ReturnNotNull()
        {
            validatorFactory = new ValidatorFactory(propertyValidator, _contentTypeValidator);
            var actual = validatorFactory.CreatePropertyValidator();
            Assert.IsNotNull(actual);
        }
    }
}
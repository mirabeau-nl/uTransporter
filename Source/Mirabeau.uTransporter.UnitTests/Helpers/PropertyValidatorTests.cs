using System.Reflection;

using Mirabeau.uTransporter.Attributes;
using Mirabeau.uTransporter.Extensions;
using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.UnitTests.Stubs;

using NUnit.Framework;

using Rhino.Mocks;

namespace Mirabeau.uTransporter.UnitTests.Helpers
{
    [TestFixture]
    public class PropertyValidatorTests
    {
        private DocumentTypePropertyAttribute _documentTypePropertyAttribute;
        private IPropertyValidator _propertyValidator;
        private PropertyInfo _propertyInfo;

        [SetUp]
        public void SetUp()
        {
            _documentTypePropertyAttribute = MockRepository.GenerateStub<DocumentTypePropertyAttribute>();
            _propertyInfo = MockRepository.GenerateStub<PropertyInfo>();
        }

        [Test]
        public void GetPropertyName_ReturnTheNameOfTheProeprty_ReturnName()
        {
            // Arrange
            _propertyValidator = new PropertyValidator();
            _documentTypePropertyAttribute.Name = "Bar";

            // Act
            var actual = _propertyValidator.GetPropertyName(_propertyInfo, _documentTypePropertyAttribute);
            var expected = "Bar";

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPropertyAlias_ReturnTheAliasOfTheProeprty_ReturnAlias()
        {
            // Arrange
            _propertyValidator = new PropertyValidator();
            _documentTypePropertyAttribute.Alias = "Foo";
            _documentTypePropertyAttribute.Name = "TestDocumentTypeBase";

            // Act
            _propertyInfo = typeof(TestDocumentTypeBase).GetProperty("PageTitle");
            var actual = _propertyValidator.GetPropertyAlias(_propertyInfo, _documentTypePropertyAttribute);
            var expected = "Foo";

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPropertyTypeAlias_ReturnDocumentTypeAttributeWhenPropertyNameIsNull_ReturnDocumentTypeAttributeName()
        {
            // Arrange
            _propertyValidator = new PropertyValidator();
            _documentTypePropertyAttribute.Alias = string.Empty;
            _documentTypePropertyAttribute.Name = "TestDocumentTypeBase";

            // Act
            _propertyInfo = typeof(TestDocumentTypeBase).GetProperty("PageTitle");
            var actual = _propertyValidator.GetPropertyAlias(_propertyInfo, _documentTypePropertyAttribute);
            var expected = "PageTitle";

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetPropertyTypeName_ReturnDocumentTypeAttributeWhenPropertyNameIsNull_ReturnDocumentTypeAttributeName()
        {
            // Arrange
            _propertyValidator = new PropertyValidator();
            _documentTypePropertyAttribute.Name = string.Empty;

            // Act
            _propertyInfo = typeof(TestDocumentTypeBase).GetProperty("PageTitle");
            var actual = _propertyValidator.GetPropertyAlias(_propertyInfo, _documentTypePropertyAttribute);
            var expected = "PageTitle";

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
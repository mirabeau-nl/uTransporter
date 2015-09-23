using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.Repositories;
using Mirabeau.uTransporter.UnitTests.Stubs;

using NUnit.Framework;

using Rhino.Mocks;

using Umbraco.Core;
using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.UnitTests.Repositories
{
    [TestFixture]
    public class PropertyReadRepositoryTests
    {
        private IDataTypeDefinition dataTypeDefinition;

        private IPropertyFactory propertyFactory;

        [SetUp]
        public void SetUp()
        {
            dataTypeDefinition = MockRepository.GenerateStub<IDataTypeDefinition>();
            propertyFactory = MockRepository.GenerateStub<IPropertyFactory>();
        }

        [Test]
        public void GetTabsNamesForDocumentType_ShouldNotReturnEmptyDictionary_ReturnFullDictionary()
        {
            // Arrange
            IPropertyReadRepository propertyReadRepository = new PropertyReadRepository(propertyFactory);
            IContentType contentType = this.CreateContentType();

            // Act
            var actual = propertyReadRepository.GetTabNamesForDocumentType(contentType);

            // Assert
            CollectionAssert.AllItemsAreNotNull(actual);
        }

        [Test]
        public void CountPropertiesFromDocumentTypeAttribute_DocumentTypeWith_ShouldReturn()
        {
            // Arrange
            IPropertyReadRepository propertyReadRepository = new PropertyReadRepository(propertyFactory);

            // Act
            var actual = propertyReadRepository.CountPropertiesFromDocumentTypeAttribute(typeof(TestDocumentTypeBase));

            // Assert
            Assert.That(actual, Is.EqualTo(2));
        }

        [Test]
        public void CountPropertiesFromContentType_ContentTypeWithTwoProperties_ReturnTwo()
        {
            // Arrange
            IPropertyReadRepository propertyReadRepository = new PropertyReadRepository(propertyFactory);
            IContentType contentType = this.CreateContentType();

            // Act
            var actual = propertyReadRepository.CountPropertiesFromContentType(contentType);

            // Assert
            Assert.That(actual, Is.EqualTo(3));
        }

        private IContentType CreateContentType()
        {
            ContentType contentType = new ContentType(-1);

            PropertyType propertyTypeOne = new PropertyType(dataTypeDefinition);
            propertyTypeOne.Name = "PropertyOne";
            propertyTypeOne.Alias = string.Format("{0}_propertyOne", Constants.PropertyEditors.InternalGenericPropertiesPrefix);
            contentType.AddPropertyType(propertyTypeOne);

            PropertyType propertyTypeTwo = new PropertyType(dataTypeDefinition);
            propertyTypeTwo.Name = "PropertyTwo";
            propertyTypeTwo.Alias = string.Format("{0}_propertyTwo", Constants.PropertyEditors.InternalGenericPropertiesPrefix);
            contentType.AddPropertyType(propertyTypeTwo);

            PropertyType propertyTypeThree = new PropertyType(dataTypeDefinition);
            propertyTypeThree.Name = "PropertyThree";
            propertyTypeThree.Alias = string.Format("{0}_propertyThree", Constants.PropertyEditors.InternalGenericPropertiesPrefix);
            contentType.AddPropertyType(propertyTypeThree);

            return contentType;
        }
    }
}

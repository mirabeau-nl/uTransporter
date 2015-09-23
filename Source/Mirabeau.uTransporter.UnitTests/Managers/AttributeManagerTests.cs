using Mirabeau.uTransporter.Attributes;
using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.Managers;
using Mirabeau.uTransporter.UnitTests.Stubs;

using NUnit.Framework;

namespace Mirabeau.uTransporter.UnitTests.Managers
{
    [TestFixture]
    public class AttributeManagerTests
    {
        [Test]
        public void GetDocumentTypeAttributes_ShouldGiveName_ReturnName()
        {
            // Arrange
            IAttributeManager attributeManager = new AttributeManager();

            // Act
            var actual = attributeManager.GetContentTypeAttributes<DocumentTypeAttribute>(typeof(TestDocumentTypeBase));

            // Assert
            Assert.That(actual.Name, Is.EqualTo("BarPage"));
        }

        [Test]
        public void GetPropertyValues_ShouldGiveAlias_ReturnAlias()
        {
            // Arrange
            IAttributeManager attributeManager = new AttributeManager();

            // Act
            var actual = attributeManager.GetContentTypeAttributes<DocumentTypeAttribute>(typeof(TestDocumentTypeBase));

            // Assert  
            Assert.AreEqual("BarPage", actual.Alias);
        }
    }
}

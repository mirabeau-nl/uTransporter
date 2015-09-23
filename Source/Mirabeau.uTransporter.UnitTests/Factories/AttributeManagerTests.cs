using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Mirabeau.uTransporter.Attributes;
using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.Managers;
using Mirabeau.uTransporter.UnitTests.Stubs;

using NUnit.Framework;

namespace Mirabeau.uTransporter.UnitTests.Factories
{
    [TestFixture]
    public class AttributeManagerTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void GetPropertyAttributes_ShouldReturnCustomPropertyAttributes_ReturnPropertyAttributes()
        {
            // Arrange
            IAttributeManager attributeManager = new AttributeManager();

            // Act
            IEnumerable<PropertyInfo> propertyInfo =
                typeof(TestDocumentTypeBase).GetProperties().Where(m => m.DeclaringType == typeof(TestDocumentTypeBase));
            var actual = attributeManager.GetPropertyAttributes<DocumentTypePropertyAttribute>(propertyInfo.First());

            // Assert
            Assert.That(actual, Is.Not.Null);
        }

        [Test]
        public void GetPropertyAttributes_ShouldReturnCustomPropertyAttributes_ReturnPropertyAttributeList()
        {
            // Arrange
            IAttributeManager attributeManager = new AttributeManager();

            // Act
            foreach (PropertyInfo propertyInfo in typeof(TestDocumentTypeBase).GetProperties().Where(m => m.DeclaringType == typeof(TestDocumentTypeBase)))
            {
                var actual = attributeManager.GetPropertyAttributes<DocumentTypePropertyAttribute>(propertyInfo);

                // Assert
                Assert.That(actual, Is.Not.Null);
            }
        }
    }
}

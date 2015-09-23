using Mirabeau.uTransporter.Attributes;
using Mirabeau.uTransporter.UnitTests.Stubs;

using NUnit.Framework;

namespace Mirabeau.uTransporter.UnitTests.Attributes
{
    [TestFixture]
    public class DocumentTypePropertyAttributeTests
    {
        [Test]
        public void ShouldReturnEmptyStringWhenAttributeTabIsNull()
        {
            DocumentTypePropertyAttribute attribute = new DocumentTypePropertyAttribute();
            attribute.Tab = typeof(Brand);
            Assert.That(attribute.Tab.Name, Is.EqualTo("Brand"));
        }
    }
}

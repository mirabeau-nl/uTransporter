using Mirabeau.uTransporter.Attributes;
using Mirabeau.uTransporter.UnitTests.Stubs;

using NUnit.Framework;

namespace Mirabeau.uTransporter.UnitTests.Attributes
{
    [TestFixture]
    public class DocumentTypeAttributeTests
    {
        [Test]
        public void DefaultTemplate_ShouldGiveTheNameOfTheTemplateAsString_ReturnTemplateName()
        {
            DocumentTypeAttribute attribute = new DocumentTypeAttribute();
            attribute.DefaultTemplate = typeof(DefaultTemplate);

            var actual = attribute.DefaultTemplateAsString;
            Assert.That(actual, Is.EqualTo("DefaultTemplate"));
        }

        [Test]
        public void DefaultTemplate_ShouldReturnEmptyStringWhenTemplateNameIsNotSet_ReturnEmptyString()
        {
            DocumentTypeAttribute attribute = new DocumentTypeAttribute();

            var actual = attribute.DefaultTemplateAsString;
            Assert.That(actual, Is.EqualTo(string.Empty));
        }
    }
}

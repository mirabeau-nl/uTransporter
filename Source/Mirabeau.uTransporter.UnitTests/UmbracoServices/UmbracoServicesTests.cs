using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.UmbracoServices;

using NUnit.Framework;

namespace Mirabeau.uTransporter.UnitTests.UmbracoServices
{
    [TestFixture]
    public class UmbracoServicesTests
    {
        [Test]
        [Ignore("Need fixing")]
        public void UmbracoServices_ShouldCreateAnUmbracoServicesObject_ReturnUmbracoServices()
        {
            IUmbracoService umbracoService = new UmbracoService();

            Assert.That(umbracoService, !Is.Null);
        }

        [Test]
        public void Methodname_Should_Return()
        {
            
        }
    }
}

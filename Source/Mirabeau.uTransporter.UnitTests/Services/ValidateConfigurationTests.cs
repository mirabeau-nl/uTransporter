using Mirabeau.uTransporter.Services;

using NUnit.Framework;

using Rhino.Mocks;

namespace Mirabeau.uTransporter.UnitTests.Services
{
    [TestFixture]
    public class ValidateConfigurationTests 
    {
        [Test]
        public void ValidateConfiguration()
        {
            var serviceLocator = MockRepository.GenerateStrictMock<ServiceLocator>();
            serviceLocator.Restart();
        }
    }
}
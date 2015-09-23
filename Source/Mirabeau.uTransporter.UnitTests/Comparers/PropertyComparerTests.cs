using Mirabeau.uTransporter.Interfaces;

using NUnit.Framework;

using Rhino.Mocks;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.UnitTests.Comparers
{
    public class PropertyComparerTests
    {
        private IValidatorFactory _valadatorFactory;
        private IManagerFactory _managerFactory;
        private IContentType _contentType;
        private IPropertyReadRepository _propertyReadRepository;
        private IAttributeManager _attributeManager;

        [SetUp]
        public void SetUp()
        {
            _valadatorFactory = MockRepository.GenerateStub<IValidatorFactory>();
            _managerFactory = MockRepository.GenerateMock<IManagerFactory>();
            _contentType = MockRepository.GenerateMock<IContentType>();
            _propertyReadRepository = MockRepository.GenerateStub<IPropertyReadRepository>();
            _attributeManager = MockRepository.GenerateMock<IAttributeManager>();
        }
    }
}

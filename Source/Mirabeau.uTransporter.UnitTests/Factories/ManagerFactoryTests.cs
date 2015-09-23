using Mirabeau.uTransporter.Factories;
using Mirabeau.uTransporter.Interfaces;

using NUnit.Framework;

using Rhino.Mocks;

namespace Mirabeau.uTransporter.UnitTests.Factories
{
    [TestFixture]
    public class ManagerFactoryTests
    {
        private IManagerFactory _managerFactory;
        private ITemplateManager _templateManager;
        private IDataTypeManager _dataTypeManager;
        private IPropertyManager _propertyManager;
        private IAttributeManager _attributeManager;

        [SetUp]
        public void SetUp()
        {
            _templateManager = MockRepository.GenerateStrictMock<ITemplateManager>();
            _dataTypeManager = MockRepository.GenerateStrictMock<IDataTypeManager>();
            _propertyManager = MockRepository.GenerateStub<IPropertyManager>();
            _attributeManager = MockRepository.GenerateStub<IAttributeManager>();
        }

        [Test]
        public void CreateDataTypeManager_CreateAndReturnManager_ReturnDataTypeManager()
        {
            _managerFactory = new ManagerFactory(_templateManager, _dataTypeManager, _propertyManager, _attributeManager);
            var actual = _managerFactory.CreateDataTypeManager();
            Assert.AreEqual(_dataTypeManager, actual);
        }

        [Test]
        public void CreateTemplateManager_CreateAndReturnManager_ReturnTemplateManager()
        {
            _managerFactory = new ManagerFactory(_templateManager, _dataTypeManager, _propertyManager, _attributeManager);
            var actual = _managerFactory.CreateTemplateManager();
            Assert.AreEqual(_templateManager, actual);
        }

        [Test]
        public void CreateDataTypeManager_CreateAndReturn_ReturnNotNull()
        {
            _managerFactory = new ManagerFactory(_templateManager, _dataTypeManager, _propertyManager, _attributeManager);
            var retVal = _managerFactory.CreateDataTypeManager();
            Assert.IsNotNull(retVal);
        }

        [Test]
        public void CreateTemplateManager_CreateAndReturn_ReturnNotNull()
        {
            _managerFactory = new ManagerFactory(_templateManager, _dataTypeManager, _propertyManager, _attributeManager);
            var retVal = _managerFactory.CreateTemplateManager();
            Assert.IsNotNull(retVal);
        }

        [Test]
        public void CreatePropertyManager_CreateAndReturn_ReturnNotNull()
        {
            _managerFactory = new ManagerFactory(_templateManager, _dataTypeManager, _propertyManager, _attributeManager);
            var retVal = _managerFactory.CreatePropertyManager();
            Assert.IsNotNull(retVal);
        }

        [Test]
        public void CreateAttributeManager_ShouldCreateAndReturn_ReturnNotNull()
        {
            _managerFactory = new ManagerFactory(_templateManager, _dataTypeManager, _propertyManager, _attributeManager);
            var retVal = _managerFactory.CreateAttributeManager();
            Assert.IsNotNull(retVal); 
        }
    }
}
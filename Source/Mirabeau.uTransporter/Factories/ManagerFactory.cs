using Mirabeau.uTransporter.Interfaces;

namespace Mirabeau.uTransporter.Factories
{
    public class ManagerFactory : IManagerFactory
    {
        private readonly ITemplateManager _templateManager;
        
        private readonly IDataTypeManager _dataTypeManager;
        
        private readonly IPropertyManager _propertyManager;

        private readonly IAttributeManager _attributeManager;

        public ManagerFactory(ITemplateManager templateManager, IDataTypeManager dataTypeManager, IPropertyManager propertyManager, IAttributeManager attributeManager)
        {
            _templateManager = templateManager;
            _dataTypeManager = dataTypeManager;
            _propertyManager = propertyManager;
            _attributeManager = attributeManager;
        }

        public ITemplateManager CreateTemplateManager()
        {
            return _templateManager;
        }

        public IDataTypeManager CreateDataTypeManager()
        {
            return _dataTypeManager;
        }

        public IPropertyManager CreatePropertyManager()
        {
            return _propertyManager;
        }

        public IAttributeManager CreateAttributeManager()
        {
            return _attributeManager;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Mirabeau.uTransporter.Attributes;
using Mirabeau.uTransporter.Extensions;
using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.Logging;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Repositories
{
    public class PropertyWriteRepository : IPropertyWriteRepository
    {
        private readonly IPropertyFactory _propertyFactory;

        private readonly IDataTypeManager _dataTypeManager;

        private readonly IAttributeManager _attributeManager;

        private readonly IPropertyReadRepository _propertyReadRepository;

        private readonly IRetryableContentTypeService _retryableContentTypeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyWriteRepository"/> class.
        /// </summary>
        /// <param name="propertyFactory">The property factory.</param>
        /// <param name="dataTypeManager">The data type manager.</param>
        /// <param name="propertyReadRepository">The property read repository.</param>
        /// <param name="umbracoFactory">The umbraco factory.</param>
        public PropertyWriteRepository(IPropertyFactory propertyFactory, IManagerFactory managerFactory, IPropertyReadRepository propertyReadRepository, IRetryableContentTypeService retryableContentTypeService)
        {
            _propertyFactory = propertyFactory;
            _dataTypeManager = managerFactory.CreateDataTypeManager();
            _attributeManager = managerFactory.CreateAttributeManager();
            _propertyReadRepository = propertyReadRepository;
            _retryableContentTypeService = retryableContentTypeService;
        }

        /// <summary>
        /// Updates all the properties from a document type.
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="contentType">contentType</param>
        public void UpdateProperties(Type type, IContentType contentType)
        {
            DocumentTypePropertyAttribute property = null;

            // single property from the database
            foreach (PropertyType propertyType in contentType.PropertyTypes.ToList())
            {
                // All the properties from de document type classes
                foreach (PropertyInfo propertyInfo in type.GetProperties())
                {
                    property = _attributeManager.GetPropertyAttributes<DocumentTypePropertyAttribute>(propertyInfo);

                    if (propertyType.Alias == property.Alias && propertyInfo.DeclaringType == type)
                    {
                        propertyType.Name = property.Name;
                        propertyType.Mandatory = property.Mandatory;
                        propertyType.ValidationRegExp = property.ValidationRegExp;
                        propertyType.Description = property.Description;
                        propertyType.SortOrder = property.SortOrder;
                        propertyType.DataTypeDefinitionId = _dataTypeManager.GetDataTypeId(property);

                        contentType.AddPropertyType(propertyType);

                        _propertyFactory.CreatePropertyGroup(property.Tab, contentType, propertyType);
                        break;
                    }
                }
            }

            this.CreateMissingProperties(type, contentType, property);
        }

        public void CreateMissingProperties(Type type, IContentType contentType, DocumentTypePropertyAttribute property)
        {
            if (property == null) // if the document doesn't got any properties at all 
            {
                _propertyFactory.CreateDocumentTypeProperties(type, contentType);
            }
            // if there is a difference between the properties in the database and on file.
            if (_propertyReadRepository.CountPropertiesFromDocumentTypeAttribute(type) != _propertyReadRepository.CountPropertiesFromContentType(contentType))
            {
                List<PropertyType> missingPropertyList = _propertyReadRepository.CreateMissingPropertiesList(type, contentType);

                Parallel.ForEach<PropertyType>(missingPropertyList, propertyType =>
                {
                    contentType.AddPropertyType(propertyType);
                    _propertyFactory.CreatePropertyGroup(property.Tab, contentType, propertyType);
                });
            }
        }

        /// <summary>
        /// Removes the properties.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="propertyType">Type of the property.</param>
        public void RemoveProperty(IContentType contentType, PropertyType propertyType)
        {
            contentType.RemovePropertyType(propertyType.Alias);
            _retryableContentTypeService.Save(contentType);

            Logger.WriteInfoLine<PropertyWriteRepository>(
                "Property with name {0}({1}) in Document Type {2}({3}) removed",
                propertyType.Name,
                propertyType.Id,
                contentType.Name,
                contentType.Id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Mirabeau.uTransporter.Attributes;
using Mirabeau.uTransporter.Builders;
using Mirabeau.uTransporter.Interfaces;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Factories
{
    public class PropertyFactory : IPropertyFactory
    {
        private const string GenericPropertyTabName = "Generic Properties";

        private readonly IDataTypeManager _dataTypeManager;

        private readonly IPropertyValidator _propertyValidator;

        private readonly ILog4NetWrapper _log;

        private readonly IRetryableContentTypeService _retryableContentTypeService;

        private readonly IAttributeManager _attributeManager;

        public PropertyFactory(IManagerFactory managerFactory, IValidatorFactory validatorFactory, IRetryableContentTypeService retryableContentTypeService)
        {
            _dataTypeManager = managerFactory.CreateDataTypeManager();
            _propertyValidator = validatorFactory.CreatePropertyValidator();
            _retryableContentTypeService = retryableContentTypeService;
            _attributeManager = managerFactory.CreateAttributeManager();
        }

        /// <summary>
        /// Creates all the properties for a document type.
        /// </summary>
        /// <param name="documentType">Type</param>
        /// <param name="contentType">Content type</param>
        public void CreateDocumentTypeProperties(Type documentType, IContentType contentType)
        {
            foreach (PropertyInfo propInfo in documentType.GetProperties())
            {
                if (propInfo.DeclaringType == documentType)
                {
                    DocumentTypePropertyAttribute property = _attributeManager.GetPropertyAttributes<DocumentTypePropertyAttribute>(propInfo);

                    IDataTypeDefinition dataType = _dataTypeManager.GetDataTypeDefinition(property);

                    PropertyType propertyType = new PropertyTypeBuilder(dataType)
                        .WithName(_propertyValidator.GetPropertyName(propInfo, property))
                        .WithAlias(_propertyValidator.GetPropertyAlias(propInfo, property))
                        .WithDescription(property.Description).IsMandatory(property.Mandatory)
                        .WithValidationRegEx(property.ValidationRegExp)
                        .WithSortOrder(property.SortOrder);

                    contentType.AddPropertyType(propertyType);

                    // add the tab
                    PropertyType savedPropertyType = this.GetPropertyTypeFromContentType(contentType, propertyType.Alias);
                    CreatePropertyGroup(property.Tab, contentType, savedPropertyType);

                    LogPropertyType(propertyType);
                }
            }
        }

        /// <summary>
        /// Creates the document type property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="contentType">Type of the content.</param>
        public void CreateDocumentTypeProperty(DocumentTypePropertyAttribute property, IContentType contentType)
        {
            IDataTypeDefinition dataType = _dataTypeManager.GetDataTypeDefinition(property);

            PropertyType propertyType = this.BuildProperty(dataType, property);

            contentType.AddPropertyType(propertyType);

            // add the tab
            CreatePropertyGroup(property.Tab, contentType, propertyType);

            LogPropertyType(propertyType);
        }

        /// <summary>
        /// Creates the property type list.
        /// </summary>
        /// <param name="docType">Type of the document.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>List of property objects</returns>
        public List<PropertyType> CreatePropertyTypeList(Type docType, IContentType contentType)
        {
            List<PropertyType> propertyTypeList = new List<PropertyType>();

            foreach (PropertyInfo propertyInfo in docType.GetProperties())
            {
                if (propertyInfo.DeclaringType == docType)
                {
                    DocumentTypePropertyAttribute property = _attributeManager.GetPropertyAttributes<DocumentTypePropertyAttribute>(propertyInfo);
                    IDataTypeDefinition dataType = _dataTypeManager.GetDataTypeDefinition(property);

                    PropertyType propertyType = BuildProperty(dataType, property);

                    propertyTypeList.Add(propertyType);
                }
            }

            return propertyTypeList;
        }

        /// <summary>
        /// Creates the property group.
        /// </summary>
        /// <param name="tab">The tab.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="propertyType">Type of the property.</param>
        public void CreatePropertyGroup(Type tab, IContentType contentType, PropertyType propertyType)
        {
            if (tab != null)
            {
                TabNameAttribute tabAtrributes = _attributeManager.GetTabAttributes<TabNameAttribute>(tab);

                if (!string.IsNullOrEmpty(tabAtrributes.Name))
                {
                    PropertyGroup propertyGroup = contentType.PropertyGroups.FirstOrDefault(x => x.Name == tabAtrributes.Name);
                    if (propertyGroup == null)
                    {
                        contentType.AddPropertyGroup(tabAtrributes.Name);
                        propertyGroup = contentType.PropertyGroups.FirstOrDefault(x => x.Name == tabAtrributes.Name);
                    }

                    if (propertyGroup != null)
                    {
                        propertyGroup.SortOrder = tabAtrributes.SortOrder;
                    }

                    if (propertyGroup != null && propertyGroup.PropertyTypes.All(x => x.Alias != propertyType.Alias))
                    {
                        contentType.MovePropertyType(propertyType.Alias, tabAtrributes.Name);
                    }
                }
                else if ((tabAtrributes.Name == string.Empty) || (tabAtrributes.Name.ToLower() == GenericPropertyTabName))
                {
                    // In case when some property exists and needs to be moved to "Generic Properties" tab
                    contentType.MovePropertyType(propertyType.Alias, null);
                }

                _retryableContentTypeService.Save(contentType);
            }
        }

        private PropertyType BuildProperty(IDataTypeDefinition dataType, DocumentTypePropertyAttribute property)
        {
            return new PropertyTypeBuilder(dataType)
                    .WithName(property.Name)
                    .WithAlias(property.Alias)
                    .WithDescription(property.Description)
                    .IsMandatory(property.Mandatory)
                    .WithValidationRegEx(property.ValidationRegExp)
                    .WithSortOrder(property.SortOrder);
        }

        private PropertyType GetPropertyTypeFromContentType(IContentType contentType, string propertyAlias)
        {
            IEnumerable<PropertyType> propertyTypes = contentType.PropertyTypes;

            return propertyTypes.FirstOrDefault(x => x.Alias == propertyAlias);
        }

        private void LogPropertyType(PropertyType propertyType)
        {
            _log.Indent(3);
            _log.Info("Adding Property with name: {0}", propertyType.Alias);
        }
    }
}
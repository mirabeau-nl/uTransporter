using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

using Mirabeau.uTransporter.Attributes;
using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.Logging;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Comparers
{
    /// <summary>
    /// IPropertyComparer instance
    /// </summary>
    public class PropertyComparer : IPropertyComparer
    {
        private readonly IAttributeManager _attributeManager;

        private readonly IPropertyValidator _propertyValidator;

        private readonly IDataTypeManager _dataTypeManager;

        private readonly IPropertyReadRepository _propertyReadRepository;

        private readonly ILog4NetWrapper _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyComparer"/> class.
        /// </summary>
        /// <param name="validatorFactory">The validator factory.</param>
        /// <param name="managerFactory">The manager factory.</param>
        /// <param name="propertyReadRepository">The property read repository.</param>
        public PropertyComparer(IValidatorFactory validatorFactory, IManagerFactory managerFactory, IPropertyReadRepository propertyReadRepository)
        {
            _attributeManager = managerFactory.CreateAttributeManager();
            _propertyReadRepository = propertyReadRepository;
            _propertyValidator = validatorFactory.CreatePropertyValidator();
            _dataTypeManager = managerFactory.CreateDataTypeManager();
            _log = LogManagerWrapper.GetLogger("Mirabeau.uTransporter");
        }

        /// <summary>
        /// Compares the specified content type against an existing content type.
        /// </summary>
        /// <param name="documentType">Type of the document.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>true or false</returns>
        public bool Compare(Type documentType, IContentType contentType)
        {
            IDictionary<string, PropertyGroup> propertyNameTabDictionary = _propertyReadRepository.GetTabNamesForDocumentType(contentType);

            foreach (PropertyInfo propInfo in documentType.GetProperties().Where(m => m.DeclaringType == documentType))
            {
                DocumentTypePropertyAttribute property = _attributeManager.GetPropertyAttributes<DocumentTypePropertyAttribute>(propInfo);

                if (property == null)
                {
                    continue; // skip this property - not part of a document type
                }

                // Get the name and alias of the property
                string propertyName = _propertyValidator.GetPropertyName(propInfo, property);
                string propertyAlias = _propertyValidator.GetPropertyAlias(propInfo, property);

                IDataTypeDefinition dataTypeDefinition = _dataTypeManager.GetDataTypeDefinition(property);

                PropertyType umbracoProperty = contentType.PropertyTypes.FirstOrDefault(p => p.Alias == propertyAlias);

                if (umbracoProperty == null)
                {
                    // new property
                    return false;
                }

                if (umbracoProperty.DataTypeDefinitionId != dataTypeDefinition.Id)
                {
                    this.LogChange(umbracoProperty.Name, umbracoProperty.DataTypeDefinitionId.ToString(CultureInfo.InvariantCulture), dataTypeDefinition.Id.ToString(CultureInfo.InvariantCulture));
                    return false;
                }

                if (umbracoProperty.Name != propertyName)
                {
                    this.LogChange(umbracoProperty.Name, umbracoProperty.Name, propertyName);
                    return false;
                }

                if (umbracoProperty.Alias != propertyAlias)
                {
                    this.LogChange(umbracoProperty.Name, umbracoProperty.Alias, propertyAlias);
                    return false;
                }

                PropertyGroup tab;
                TabNameAttribute tabNameAttribute = new TabNameAttribute();

                propertyNameTabDictionary.TryGetValue(property.Alias, out tab);
                if (tab == null)
                {
                    tab = new PropertyGroup();
                    tab.Name = string.Empty;
                }
                if (property.Tab != null)
                {
                    tabNameAttribute = _attributeManager.GetTabAttributes<TabNameAttribute>(property.Tab);
                }

                if (property.Tab != null && !string.Equals(tabNameAttribute.Name, tab.Name.Replace(" ", string.Empty), StringComparison.OrdinalIgnoreCase))
                {
                    this.LogTabChange(umbracoProperty.Name, property.Tab.Name, tab.Name == string.Empty ? "Generic Tab" : tab.Name);
                    return false;
                }

                if (umbracoProperty.Mandatory != property.Mandatory)
                {
                    this.LogChange(umbracoProperty.Name, umbracoProperty.Mandatory.ToString(), property.Mandatory.ToString());
                    return false;
                }

                if (umbracoProperty.ValidationRegExp != property.ValidationRegExp)
                {
                    //this.LogChange(umbracoProperty.Name, umbracoProperty.ValidationRegExp, property.ValidationRegExp);
                    return false;
                }

                if (umbracoProperty.Description != property.Description)
                {
                    return false;
                }
            }

            return true;
        }

        private static string EscapeCurlyBraces(string value)
        {
            return value.Replace("{", "{{").Replace("}", "}}");
        }

        private void LogChange(string propertyName, string previous, string updated)
        {
            _log.Info(string.Format("In property {0} the value of '{1}' has changed to '{2}'", propertyName, previous, updated));
        }

        private void LogTabChange(string propertyName, string previous, string updated)
        {
            _log.Info(string.Format("The property {0} has changed from tab'{1}' to tab '{2}'", propertyName, updated, previous));
        }
    }
}
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Mirabeau.uTransporter.Comparers;
using Mirabeau.uTransporter.Enums;
using Mirabeau.uTransporter.Interfaces;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Repositories
{
    public class PropertyReadRepository : IPropertyReadRepository
    {
        private readonly IPropertyFactory _propertyFactory;

        public PropertyReadRepository(IPropertyFactory propertyFactory)
        {
            _propertyFactory = propertyFactory;
        }

        /// <summary>
        /// Gets the type of the tab names for document.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>A dictionary with tab name for a specific content type</returns>
        public IDictionary<string, PropertyGroup> GetTabNamesForDocumentType(IContentType contentType)
        {
            IDictionary<string, PropertyGroup> tabsWithProperties = new ConcurrentDictionary<string, PropertyGroup>();

            Parallel.ForEach(
                contentType.PropertyGroups,
                tabItem =>
                {
                    Parallel.ForEach(tabItem.PropertyTypes, propertyItem => tabsWithProperties.Add(propertyItem.Alias, tabItem));
                });

            return tabsWithProperties;
        }

        /// <summary>
        /// Gets the type of all properties from content.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>A list with all properties for a specific content type</returns>
        public IEnumerable<PropertyType> GetAllPropertiesFromContentType(IContentType contentType)
        {
            return contentType.PropertyTypes;
        }

        /// <summary>
        /// Creates the missing properties list.
        /// </summary>
        /// <param name="docType">Type of the document.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>List of property objects</returns>
        public List<PropertyType> CreateMissingPropertiesList(Type docType, IContentType contentType)
        {
            List<PropertyType> newPropertyTypeList = _propertyFactory.CreatePropertyTypeList(docType, contentType);
            IEnumerable<PropertyType> existingPropertyTypes = contentType.PropertyTypes;

            // return the difference between the two lists
            return newPropertyTypeList.Except(existingPropertyTypes).ToList();
        }

        /// <summary>
        /// Creates the redundant properties list.
        /// </summary>
        /// <param name="docType">Type of the document.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>List of property objects</returns>
        public List<PropertyType> CreateRedundantPropertiesList(Type docType, IContentType contentType)
        {
            List<PropertyType> newPropertyTypeList = _propertyFactory.CreatePropertyTypeList(docType, contentType);
            IEnumerable<PropertyType> existingPropertyTypes = contentType.PropertyTypes;

            // return the difference between the two lists
            // return existingPropertyTypes.Except(newPropertyTypeList).ToList();
            return existingPropertyTypes.Where(x => newPropertyTypeList.All(y => y.Alias != x.Alias)).ToList();
        }

        /// <summary>
        /// Counts the type of the properties from content.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>int number of proeprties</returns>
        public int CountPropertiesFromContentType(IContentType contentType)
        {
            return contentType.PropertyTypes.Count();
        }

        public int CountPropertiesFromContentTypes(IEnumerable<IContentType> contentTypes)
        {
            return contentTypes.Sum(contentType => this.CountPropertiesFromContentType(contentType));
        }

        /// <summary>
        /// Counts the properties from document type attribute.
        /// </summary>
        /// <param name="docType">Type of the document.</param>
        /// <returns>int number of document type attributes</returns>
        public int CountPropertiesFromDocumentTypeAttribute(Type docType)
        {
            return docType.GetProperties().Count();
        }

        /// <summary>
        /// Gets the value from enum.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A object with an Enum value</returns>
        public object GetValueFromEnum(float id)
        {
            return Enum.ToObject(typeof(UmbracoPropertyType), id);
        }

        /// <summary>
        /// Gets all property groups.
        /// </summary>
        /// <param name="contentTypes">The content types.</param>
        /// <returns>IEnumerable of Tabs / PropertyGroups</returns>
        public IEnumerable<PropertyGroup> GetAllPropertyGroups(IEnumerable<IContentType> contentTypes)
        {

            List<PropertyGroup> propertyGroups = new List<PropertyGroup>();
            foreach (IContentType contentType in contentTypes)
            {
                IEnumerable<PropertyGroup> compositionPropertyGroups = contentType.CompositionPropertyGroups;
                foreach (PropertyGroup propertyGroup in compositionPropertyGroups)
                {
                    propertyGroups.Add(propertyGroup);
                }
            }

            return propertyGroups.Distinct(new PropertyGroupEqualityCompairer());

        }
    }
}

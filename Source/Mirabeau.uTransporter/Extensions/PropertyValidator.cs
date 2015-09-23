using System.Reflection;

using Mirabeau.uTransporter.Attributes;
using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.Utils;

namespace Mirabeau.uTransporter.Extensions
{
    public class PropertyValidator : IPropertyValidator
    {
        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <param name="documentTypePropertyAttribute">The document type property attribute.</param>
        /// <returns>The name of a property</returns>
        public string GetPropertyName(PropertyInfo propertyInfo, DocumentTypePropertyAttribute documentTypePropertyAttribute)
        {
            return string.IsNullOrEmpty(documentTypePropertyAttribute.Name) ? propertyInfo.Name : documentTypePropertyAttribute.Name;
        }

        /// <summary>
        /// Gets the property alias.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <param name="documentTypePropertyAttribute">The document type property attribute.</param>
        /// <returns>The alias of a property</returns>
        public string GetPropertyAlias(PropertyInfo propertyInfo, DocumentTypePropertyAttribute documentTypePropertyAttribute)
        {
            var alias = propertyInfo.Name; // default alias
            if (!string.IsNullOrEmpty(documentTypePropertyAttribute.Alias))
            {
                alias = documentTypePropertyAttribute.Alias;
                alias = Util.TrimLength(alias, 255, documentTypePropertyAttribute.Name);
            }

            return alias;
        }
    }
}

using Mirabeau.uTransporter.Interfaces;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Managers
{
    public class PropertyManager : IPropertyManager
    {
        /// <summary>
        /// Removes the type of the property.
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="contentType">Type of the content.</param>
        public void RemovePropertyType(PropertyType propertyType, IContentType contentType)
        {
            contentType.RemoveContentType(propertyType.Alias);
        }
    }
}

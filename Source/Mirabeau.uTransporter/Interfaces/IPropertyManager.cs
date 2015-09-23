using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Interfaces
{
    public interface IPropertyManager
    {
        void RemovePropertyType(PropertyType propertyType, IContentType contentType);
    }
}
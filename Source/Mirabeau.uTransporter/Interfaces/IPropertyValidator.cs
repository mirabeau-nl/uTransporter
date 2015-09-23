using System.Reflection;

using Mirabeau.uTransporter.Attributes;

namespace Mirabeau.uTransporter.Interfaces 
{
    public interface IPropertyValidator 
    {
        string GetPropertyName(PropertyInfo propertyInfo, DocumentTypePropertyAttribute documentTypePropertyAttribute);
        
        string GetPropertyAlias(PropertyInfo propertyInfo, DocumentTypePropertyAttribute documentTypePropertyAttribute);
    }
}
using System;
using System.Reflection;

namespace Mirabeau.uTransporter.Interfaces
{
    public interface IAttributeManager
    {
        T GetContentTypeAttributes<T>(Type type);

        T GetPropertyAttributes<T>(PropertyInfo propertyInfo);

        T GetTabAttributes<T>(Type type);

        T GetTemplateAttributes<T>(Type type);
    }
}

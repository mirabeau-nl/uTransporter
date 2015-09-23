using System;
using System.Reflection;

using Mirabeau.uTransporter.Interfaces;

namespace Mirabeau.uTransporter.Managers
{
    /// <summary>
    /// IAttributeManager instance
    /// </summary>
    public class AttributeManager : IAttributeManager
    {
        /// <summary>
        /// Gets the document type attributes.
        /// </summary>
        /// <typeparam name="T">T type</typeparam>
        /// <param name="type">The type.</param>
        /// <returns>T</returns>
        public T GetContentTypeAttributes<T>(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");    
            }

            object[] attributes = type.GetCustomAttributes(typeof(T), true);
            T result = (T)attributes[0];

            return result;
        }

        /// <summary>
        /// Gets all the custom attributes from a class.
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="propertyInfo">propertInfo</param>
        /// <returns>attributes</returns>
        public T GetPropertyAttributes<T>(PropertyInfo propertyInfo)
        {
            object[] attributes = propertyInfo.GetCustomAttributes(typeof(T), true);
            T result = (T)attributes[0];

            return result;
        }

        /// <summary>
        /// Gets the tab type attributes.
        /// </summary>
        /// <typeparam name="T">T type</typeparam>
        /// <param name="type">The type.</param>
        /// <returns>T</returns>
        public T GetTabAttributes<T>(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            object[] attributes = type.GetCustomAttributes(typeof(T), true);
            T result = (T)attributes[0];

            return result;
        }


        /// <summary>
        /// Gets the template attributes.
        /// </summary>
        /// <typeparam name="T">Generic T</typeparam>
        /// <param name="type">The type.</param>
        /// <returns>Returns T with attributes</returns>
        public T GetTemplateAttributes<T>(Type type)
        {
            object[] attributes = type.GetCustomAttributes(typeof(T), true);

            T result = (T)attributes[0];

            return result;
        }

    }
}

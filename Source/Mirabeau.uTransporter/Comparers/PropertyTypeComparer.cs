using System.Collections.Generic;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Comparers
{
    public class PropertyTypeComparer : IEqualityComparer<PropertyType>
    {
        public bool Equals(PropertyType x, PropertyType y)
        {
            return GetHashCode(x) == GetHashCode(y);
        }

        public int GetHashCode(PropertyType propertyType)
        {
            if (propertyType.Name == null)
            {
                return 0;
            }
            else
            {
                if (propertyType.Alias != null)
                {
                    return propertyType.Name.GetHashCode() ^ propertyType.Alias.GetHashCode();
                }
                else
                {
                    return propertyType.Name.GetHashCode();
                }
            }
        }
    }
}
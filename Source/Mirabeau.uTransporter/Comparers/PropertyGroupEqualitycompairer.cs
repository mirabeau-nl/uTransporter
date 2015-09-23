using System.Collections.Generic;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Comparers
{
    public class PropertyGroupEqualityCompairer : IEqualityComparer<PropertyGroup>
    {
        public bool Equals(PropertyGroup x, PropertyGroup y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }
            if (ReferenceEquals(x, null))
            {
                return false;
            }
            if (ReferenceEquals(y, null))
            {
                return false;
            }
            if (x.GetType() != y.GetType())
            {
                return false;
            }
            return string.Equals(x.Name, y.Name) && x.SortOrder == y.SortOrder;
        }

        public int GetHashCode(PropertyGroup obj)
        {
            unchecked
            {
                return ((obj.Name != null ? obj.Name.GetHashCode() : 0) * 397) ^ obj.SortOrder;
            }
        }
    }
}

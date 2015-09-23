using System;
using System.Reflection;

using Umbraco.Core.Models;

namespace Mirabeau.uTransporter.Extensions
{
    public static class ObjectHelper
    {
        public static int Compaire(IContentType x, IContentType y)
        {

            PropertyInfo[] properties = x.GetType().GetProperties();

            FieldInfo[] fields = x.GetType().GetFields();

            var compareValue = 0;

            foreach (PropertyInfo property in properties)
            {
                IComparable valx = property.GetValue(x, null) as IComparable;

                if (valx == null)
                {
                    continue;
                }

                object valy = property.GetValue(y, null);
                compareValue = valx.CompareTo(valy);

                if (compareValue != 0)
                {
                    return compareValue;
                }
            }
            foreach (FieldInfo field in fields)
            {
                IComparable valx = field.GetValue(x) as IComparable;
                if (valx == null)
                {
                    continue;
                }

                object valy = field.GetValue(y);
                compareValue = valx.CompareTo(valy);

                if (compareValue != 0)
                {
                    return compareValue;
                }
            }

            return compareValue;
        }
    }
}

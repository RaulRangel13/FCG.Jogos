using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTIL.Extensions
{
    public static class GenericExtensions
    {
        public static T getTrimStringProperties<T>(this T obj) where T : class
        {
            if (obj == null)
                return obj;

            var properties = obj.GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(string) &&
                            p.CanRead &&
                            p.CanWrite);

            foreach (var property in properties)
            {
                var value = property.GetValue(obj) as string;
                if (value != null)
                {
                    property.SetValue(obj, value.Trim());
                }
            }

            return obj;
        }

    }
}

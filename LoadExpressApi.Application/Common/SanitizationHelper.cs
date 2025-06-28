using Microsoft.Security.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LoadExpressApi.Application.Common
{
    using Microsoft.Security.Application;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class SanitizationHelper
    {
        public static List<string> SanitizeObject<T>(T obj)
        {
            if (obj == null) return new List<string>();

            List<string> sanitizedFields = new List<string>();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = property.GetValue(obj) as string;
                    if (!string.IsNullOrEmpty(value))
                    {
                        var sanitizedValue = Sanitizer.GetSafeHtmlFragment(value);
                        if (sanitizedValue != value)
                        {
                            property.SetValue(obj, sanitizedValue);
                            sanitizedFields.Add(property.Name);
                        }
                    }
                }
                else if (!property.PropertyType.IsValueType && property.PropertyType != typeof(string))
                {
                    var nestedSanitizedFields = SanitizeObject(property.GetValue(obj));
                    sanitizedFields.AddRange(nestedSanitizedFields.Select(f => $"{property.Name}.{f}"));
                }
            }

            return sanitizedFields;
        }
    }
}

using BanBrick.TypeScript.CodeGenerator.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Extensions
{
    internal static class TypePropertyExtensions
    {
        public static bool IsTypeScriptIgnored(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes(true).OfType<TypeScriptIgnoreAttribute>().Any();
        }

        public static object TryGetValue(this PropertyInfo propertyInfo, object obj)
        {
            try {
                return propertyInfo.GetValue(obj);
            }
            catch {
                return null;
            }
        }
    }
}

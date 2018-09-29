using BanBrick.TypeScript.CodeGenerator.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Helpers
{
    public  class PropertyHelper
    {
        public bool IsTypeScriptIgnored(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes(true).OfType<TypeScriptIgnoreAttribute>().Any();
        }
    }
}

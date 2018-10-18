using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Models
{
    public class PropertyDefinition
    {
        public PropertyInfo PropertyInfo { get; set; }

        public Type Type { get; set; }
        
        public bool IsNullable { get; set; }

        public object DefaultValue { get; set; }
    }
}

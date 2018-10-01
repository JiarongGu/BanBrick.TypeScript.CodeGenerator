using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Models
{
    public class PropertyDefinition
    {
        public Type Type { get; set; }

        public string Name { get; set; }

        public bool IsNullable { get; set; }

        public string Value { get; set; }
    }
}

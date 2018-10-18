using BanBrick.TypeScript.CodeGenerator.Annotations;
using BanBrick.TypeScript.CodeGenerator.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Models
{
    internal class TypeDefinition
    {
        public Type Type { get; set; }

        public Type ActualType { get; set; }

        public ProcessingCategory ProcessingCategory { get; set; }
        
        public ProcessConfig ProcessConfig { get; set; }
        
        public bool IsNullable { get; set; }

        public bool IsNumeric { get; set; }

        public bool IsString { get; set; }

        public bool IsFirstLevel { get; set; }

        public List<PropertyDefinition> Properties { get; set; } = new List<PropertyDefinition>();
    }
}

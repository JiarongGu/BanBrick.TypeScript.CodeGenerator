using BanBrick.TypeScript.CodeGenerator.Enums;
using BanBrick.TypeScript.CodeGenerator.TypeHandlers;
using System;
using System.Collections.Generic;

namespace BanBrick.TypeScript.CodeGenerator.Models
{
    internal class TypeDefinition
    {
        public Type Type { get; set; }

        public ProcessingCategory ProcessingCategory { get; set; }
        
        public ProcessConfig ProcessConfig { get; set; }

        public ITypeHandler TypeHandler { get; set; }
        
        public bool IsFirstLevel { get; set; }

        public bool IsNullable { get; set; }

        public List<PropertyDefinition> Properties { get; set; } = new List<PropertyDefinition>();
    }
}

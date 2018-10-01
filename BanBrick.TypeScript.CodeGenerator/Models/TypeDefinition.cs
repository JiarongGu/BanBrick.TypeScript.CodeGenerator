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

        public ProcessingCategory Category { get; set; }
    }
}

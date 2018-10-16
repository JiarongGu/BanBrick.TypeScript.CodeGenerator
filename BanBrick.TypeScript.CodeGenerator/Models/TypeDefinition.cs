﻿using BanBrick.TypeScript.CodeGenerator.Annotations;
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

        public ProcessingCategory Category { get; set; }

        public TypeScriptObjectType ProcessType { get; set; }

        public string Name { get; set; }

        public bool IsNullable { get; set; }

        public bool IsNumeric { get; set; }

        public bool NoGeneration { get; set; }
    }
}

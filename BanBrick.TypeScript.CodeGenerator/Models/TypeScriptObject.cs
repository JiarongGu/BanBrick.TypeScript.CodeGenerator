using BanBrick.TypeScript.CodeGenerator.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Models
{
    public class TypeScriptObject
    {
        public Type Type { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public TypeCategory TypeCategory { get; set;}
    }
}

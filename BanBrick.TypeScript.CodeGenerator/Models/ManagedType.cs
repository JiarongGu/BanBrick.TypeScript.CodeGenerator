using BanBrick.TypeScript.CodeGenerator.Annotations;
using BanBrick.TypeScript.CodeGenerator.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Models
{
    public class ManagedType
    {
        public Type Type { get; set; }

        public TypeCategory Category { get; set; }
    }
}

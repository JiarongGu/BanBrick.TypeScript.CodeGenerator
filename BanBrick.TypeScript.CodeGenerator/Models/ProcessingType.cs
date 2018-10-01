using BanBrick.TypeScript.CodeGenerator.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Models
{
    internal class ProcessingType
    {
        public Type Type { get; set; }

        public ProcessingCategory? Category { get; set; }

        public bool Verified => Category != null;
    }
}

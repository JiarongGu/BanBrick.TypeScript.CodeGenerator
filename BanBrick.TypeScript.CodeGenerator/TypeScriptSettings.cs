using System;
using System.Collections.Generic;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator
{
    public class TypeScriptSettings
    {
        public TypeScriptSettings() => Indentation = "  ";
        public TypeScriptSettings(string indentation) => Indentation = indentation;

        public string Indentation { get; set; }
    }
}

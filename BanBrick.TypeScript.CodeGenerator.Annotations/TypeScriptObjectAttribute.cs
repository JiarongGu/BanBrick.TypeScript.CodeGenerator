using System;
using System.Collections.Generic;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Annotations
{
    [AttributeUsage(AttributeTargets.Class| AttributeTargets.Enum, AllowMultiple = false)]
    public class TypeScriptObjectAttribute: Attribute
    {
        public TypeScriptObjectAttribute() { }

        public TypeScriptObjectAttribute(string name) 
            => (Name) = name;

        public TypeScriptObjectAttribute(string name, TypeScriptObjectType type)
            => (Type, Name) = (type, name);

        public TypeScriptObjectAttribute(string name, TypeScriptObjectType type, bool inherit)
            => (Type, Name, Inherit) = (type, name, inherit);

        public string Name { get; set; }

        public TypeScriptObjectType Type { get; set; }

        public bool Inherit { get; set; }
    }
}

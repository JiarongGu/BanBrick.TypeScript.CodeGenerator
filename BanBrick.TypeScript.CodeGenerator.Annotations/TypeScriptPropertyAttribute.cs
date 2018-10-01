using System;
using System.Collections.Generic;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Annotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class TypeScriptPropertyAttribute : Attribute
    {
        public TypeScriptPropertyAttribute() { }

        public TypeScriptPropertyAttribute(string name) 
            => (Name) = name;

        public TypeScriptPropertyAttribute(string name, string defaultValue) 
            => (Name, DefaultValue) = (name, defaultValue);

        public TypeScriptPropertyAttribute(string name, string defaultValue, bool? allowedNull)
            => (Name, DefaultValue, AllowedNull) = (name, defaultValue, allowedNull);

        public string Name { get; set; }

        public string DefaultValue { get; set; }

        public bool? AllowedNull { get; set; }
    }
}

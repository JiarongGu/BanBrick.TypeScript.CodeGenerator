using BanBrick.TypeScript.CodeGenerator.Annotations;
using BanBrick.TypeScript.CodeGenerator.Enums;
using BanBrick.TypeScript.CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Convertors
{
    /// <summary>
    /// convert attributes to configs
    /// </summary>
    internal static class ConfigConvertor
    {
        public static IEnumerable<ProcessConfig> GetProcessConfigs(IEnumerable<Type> types)
        {
            return types.Select(x => GetProcessConfig(x));
        }

        public static IEnumerable<ProcessConfig> GetProcessConfigs(IEnumerable<TypeDefinition> typeDefinitions)
        {
            return GetProcessConfigs(typeDefinitions.Select(x => x.Type));
        }

        public static ProcessConfig GetProcessConfig(Type type)
        {
            var attribute = type.GetCustomAttribute<TypeScriptObjectAttribute>();

            if (attribute == null) return null;

            return new ProcessConfig()
            {
                OutputType = Parse(attribute.Type),
                Name = attribute.Name,
                Inherit = attribute.Inherit
            };
        }

        public static ProcessConfig GetProcessConfig(ProcessingCategory category)
        {
            return new ProcessConfig()
            {
                OutputType = Parse(category)
            };
        }

        public static OutputType Parse(TypeScriptObjectType objectType)
        {
            switch (objectType)
            {
                case TypeScriptObjectType.Class:
                    return OutputType.Class;
                case TypeScriptObjectType.Const:
                    return OutputType.Class;
                case TypeScriptObjectType.Enum:
                    return OutputType.Enum;
                case TypeScriptObjectType.Interface:
                    return OutputType.Interface;
                default:
                    return OutputType.Default;
            }
        }

        public static OutputType Parse(ProcessingCategory category)
        {
            switch (category)
            {
                case ProcessingCategory.Enum:
                    return OutputType.Enum;
                case ProcessingCategory.Object:
                    return OutputType.Class;
                case ProcessingCategory.Interface:
                    return OutputType.Interface;
                default:
                    return OutputType.None;
            }
        }
    }
}

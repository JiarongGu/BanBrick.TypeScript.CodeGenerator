using BanBrick.TypeScript.CodeGenerator.Annotations;
using BanBrick.TypeScript.CodeGenerator.Enums;
using BanBrick.TypeScript.CodeGenerator.Models;
using BanBrick.TypeScript.CodeGenerator.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Extensions
{
    internal static class TypeDefinitionExtension
    {
        public static IEnumerable<TypeDefinition> ResolveRelations(this IEnumerable<Type> types)
        {
            return new RelationResolver().Resolve(types);
        }

        public static IEnumerable<TypeDefinition> ResolveNames(this IEnumerable<TypeDefinition> typeDefinitions)
        {
            return new NameResolver().Resolve(typeDefinitions);
        }

        public static IEnumerable<TypeDefinition> ResolveDuplications(this IEnumerable<TypeDefinition> typeDefinitions)
        {
            return new DuplicationResolver().Resolve(typeDefinitions);
        }

        public static IEnumerable<TypeDefinition> ResolveConfigs(this IEnumerable<TypeDefinition> typeDefinitions)
        {
            return new ConfigResolver().Resolve(typeDefinitions);
        }

        public static List<Type> GetProcessingTypes(this IEnumerable<TypeDefinition> typeDefinitions, OutputType outputType)
        {
            return typeDefinitions
                .Where(x => x.ProcessConfig.OutputType == outputType)
                .OrderBy(x => x.ProcessConfig.Name)
                .Select(x => x.Type)
                .ToList();
        }

        public static bool IsInheritable(this TypeDefinition typeDefinition)
        {
            switch (typeDefinition.ProcessingCategory)
            {
                case ProcessingCategory.Interface:
                case ProcessingCategory.Object:
                    return true;
                default: return false;
            }
        }
    }
}

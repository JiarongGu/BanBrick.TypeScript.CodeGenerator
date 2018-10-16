using BanBrick.TypeScript.CodeGenerator.Annotations;
using BanBrick.TypeScript.CodeGenerator.Enums;
using BanBrick.TypeScript.CodeGenerator.Models;
using BanBrick.TypeScript.CodeGenerator.Processers;
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
            return new TypeDefinitionProcesser().Process(types);
        }

        public static IEnumerable<TypeDefinition> ResolveNames(this IEnumerable<TypeDefinition> typeDefinitions)
        {
            return new TypeNameProcesser().Process(typeDefinitions);
        }

        public static IEnumerable<TypeDefinition> ResolveDuplications(this IEnumerable<TypeDefinition> typeDefinitions)
        {
            return new TypeDuplicationProcesser().Process(typeDefinitions);
        }

        public static List<Type> GetProcessingTypes(this IEnumerable<TypeDefinition> typeDefinitions, TypeScriptObjectType processingType)
        {
            return typeDefinitions.Where(x => x.ProcessType == processingType).Select(x => x.Type).ToList();
        }
    }
}

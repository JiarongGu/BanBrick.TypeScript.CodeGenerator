using BanBrick.TypeScript.CodeGenerator.Enums;
using BanBrick.TypeScript.CodeGenerator.Extensions;
using BanBrick.TypeScript.CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Resolvers
{
    internal class NameResolver
    {
        public IEnumerable<TypeDefinition> Resolve(IEnumerable<TypeDefinition> definitions)
        {
            var unprocessedDefinitions = definitions.ToList();
            var processedDefinitions = new List<TypeDefinition>();

            while (unprocessedDefinitions.Any())
            {
                // pop first type
                var processingDefinition = unprocessedDefinitions.First();
                unprocessedDefinitions.RemoveAt(0);

                var type = processingDefinition.Type;
                var processConfig = processingDefinition.ProcessConfig;
                var category = processingDefinition.ProcessingCategory;

                if (category == ProcessingCategory.Primitive)
                    processConfig.Name = GetPrimitiveName(processingDefinition);

                if (category == ProcessingCategory.Enum)
                    processConfig.Name = GetDefaultName(processConfig, type);

                if (category == ProcessingCategory.Dictionary)
                {
                    var (key, value) = type.GetDictionaryTypes();
                    if (TypeDefinitionContains(processedDefinitions, key, value))
                    {
                        var keyName = $"[key: {GetName(processedDefinitions, key)}]";
                        var valueName = $"{GetName(processedDefinitions, value)}";
                        processConfig.Name = $"{{ {keyName} : {valueName} }}";
                    }
                }

                if (category == ProcessingCategory.Collection)
                {
                    var value = type.GetCollectionType();
                    if (TypeDefinitionContains(processedDefinitions, value))
                    {
                        processConfig.Name = $"{GetName(processedDefinitions, value)}[]";
                    }
                }

                if (category == ProcessingCategory.Object)
                {
                    processConfig.Name = GetDefaultName(processConfig, type);
                }

                if (category == ProcessingCategory.Interface)
                {
                    processConfig.Name = GetDefaultName(processConfig, type);
                }

                if (category == ProcessingCategory.Generic)
                {
                    var genericTypes = type.GetGenericArguments();
                    if (TypeDefinitionContains(processedDefinitions, genericTypes))
                    {
                        var genericNames = genericTypes.Select(x => GetName(processedDefinitions, x));
                        processConfig.Name = $"{GetDefaultName(processConfig, type)}<{string.Join(", ", genericNames)}>";
                    }
                }


                // if cannot process add to last of queue else add to completed list
                if (processingDefinition.ProcessConfig.Name == null)
                {
                    unprocessedDefinitions.Add(processingDefinition);
                }
                else
                {
                    processedDefinitions.Add(processingDefinition);
                }
            }

            return processedDefinitions;
        }

        private string GetDefaultName(ProcessConfig processConfig, Type type) {
            var typeName = type.Name;
            var genericCharIndex = typeName.IndexOf('`');

            return processConfig.Name ?? (genericCharIndex > 0 ? typeName.Substring(0, genericCharIndex) : typeName);
        }

        private string GetName(IEnumerable<TypeDefinition> typeDefinitions, Type type)
        {
            return typeDefinitions.First(x => x.Type == type).ProcessConfig.Name;
        }

        private bool TypeDefinitionContains(IEnumerable<TypeDefinition> typeDefinitions, params Type[] types)
        {
            return types.All(t => typeDefinitions.Any(x => x.Type == t));
        }
        
        public string GetPrimitiveName(TypeDefinition typeDefinition)
        {
            var type = typeDefinition.ActualType;

            if (typeDefinition.IsNumeric)
                return "number";

            if (typeDefinition.IsString)
                return "string";

            if (type == typeof(bool))
                return "boolean";

            return "any";
        }
    }
}
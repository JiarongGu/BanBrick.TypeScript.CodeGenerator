using BanBrick.TypeScript.CodeGenerator.Enums;
using BanBrick.TypeScript.CodeGenerator.Extensions;
using BanBrick.TypeScript.CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Processers
{
    internal class TypeNameProcesser
    {
        public IEnumerable<TypeDefinition> Process(IEnumerable<TypeDefinition> definitions)
        {
            var unprocessedDefinition = definitions.ToList();
            var processedDefinitions = new List<TypeDefinition>();

            while (unprocessedDefinition.Any())
            {
                // pop first type
                var processingDefinition = unprocessedDefinition.First();
                unprocessedDefinition.RemoveAt(0);

                var type = processingDefinition.Type;
                var category = processingDefinition.Category;

                if (category == ProcessingCategory.Primitive)
                    processingDefinition.Name = GetPrimitiveName(processingDefinition);

                if (category == ProcessingCategory.Enum)
                    processingDefinition.Name = type.GetTypeScriptName();

                if (category == ProcessingCategory.Dictionary)
                {
                    var (key, value) = type.GetDictionaryTypes();
                    if (TypeDefinitionContains(processedDefinitions, key, value))
                    {
                        var keyName = $"[key: {GetName(processedDefinitions, key)}]";
                        var valueName = $"{GetName(processedDefinitions, value)}";
                        processingDefinition.Name = $"{{ {keyName} : {valueName} }}";
                    }
                }

                if (category == ProcessingCategory.Collection)
                {
                    var value = type.GetCollectionType();
                    if (TypeDefinitionContains(processedDefinitions, value))
                    {
                        processingDefinition.Name = $"{GetName(processedDefinitions, value)}[]";
                    }
                }

                if (category == ProcessingCategory.Object)
                {
                    processingDefinition.Name = type.GetTypeScriptName();
                }

                if (category == ProcessingCategory.Interface)
                {
                    processingDefinition.Name = type.GetTypeScriptName();
                }

                if (category == ProcessingCategory.Generic)
                {
                    var genericTypes = type.GetGenericArguments();
                    if (TypeDefinitionContains(processedDefinitions, genericTypes))
                    {
                        var genericNames = genericTypes.Select(x => GetName(processedDefinitions, x));
                        processingDefinition.Name = $"{type.GetTypeScriptName()}<{string.Join(", ", genericNames)}>";
                    }
                }


                // if cannot process add to last of queue else add to completed list
                if (processingDefinition.Name == null)
                {
                    unprocessedDefinition.Add(processingDefinition);
                }
                else
                {
                    processedDefinitions.Add(processingDefinition);
                }
            }

            return processedDefinitions;
        }

        private string GetName(IEnumerable<TypeDefinition> typeDefinitions, Type type)
        {
            return typeDefinitions.First(x => x.Type == type).Name;
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

            if (type == typeof(string) || type == typeof(DateTime))
                return "string";

            if (type == typeof(bool))
                return "boolean";

            return "any";
        }
    }
}
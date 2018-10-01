using BanBrick.TypeScript.CodeGenerator.Enums;
using BanBrick.TypeScript.CodeGenerator.Extensions;
using BanBrick.TypeScript.CodeGenerator.Helpers;
using BanBrick.TypeScript.CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Processers
{
    internal class TypeDefinitionProcesser
    {
        private readonly TypeHelper _typeHelper;
        private readonly PropertyHelper _propertyHelper;

        public TypeDefinitionProcesser() {
            _typeHelper = new TypeHelper();
            _propertyHelper = new PropertyHelper();
        }

        public ICollection<TypeDefinition> Process(IEnumerable<Type> types) {
            var unProcessedTypes = types.ToList();
            var typeDefinitions = new List<TypeDefinition>();
            
            while (unProcessedTypes.Any())
            {
                // pop last type
                var processingType = unProcessedTypes.Last();
                unProcessedTypes.RemoveAt(unProcessedTypes.Count - 1);

                // already in category type;
                if (typeDefinitions.Any(x => x.Type == processingType))
                    continue;

                // add type to category types
                var processingTypeCategory = _typeHelper.GetProcessingCategory(processingType);
                typeDefinitions.Add(_typeHelper.ToTypeDefinition(processingType));

                // process all properties
                foreach (var property in TypeExtensions.GetProperties(processingType))
                {
                    if (_propertyHelper.IsTypeScriptIgnored(property))
                        continue;

                    var propertyType = property.PropertyType;
                    var category = _typeHelper.GetProcessingCategory(propertyType);

                    unProcessedTypes.Add(propertyType);

                    switch (category)
                    {
                        case ProcessingCategory.Collection:
                            unProcessedTypes.Add(propertyType.GetCollectionType());
                            break;
                        case ProcessingCategory.Dictionary:
                            var (key, value) = propertyType.GetDictionaryTypes();
                            unProcessedTypes.Add(key);
                            unProcessedTypes.Add(value);
                            break;
                        case ProcessingCategory.Generic:
                            unProcessedTypes.AddRange(propertyType.GetGenericArguments());
                            break;
                    }
                }
            }

            return typeDefinitions;
        }
    }
}

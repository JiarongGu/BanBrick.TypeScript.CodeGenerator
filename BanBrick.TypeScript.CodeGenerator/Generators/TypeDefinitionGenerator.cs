using BanBrick.TypeScript.CodeGenerator.Enums;
using BanBrick.TypeScript.CodeGenerator.Helpers;
using BanBrick.TypeScript.CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Generators
{
    internal class TypeDefinitionGenerator
    {
        private readonly TypeHelper _typeHelper;
        private readonly PropertyHelper _propertyHelper;

        public TypeDefinitionGenerator() {
            _typeHelper = new TypeHelper();
            _propertyHelper = new PropertyHelper();
        }

        public ICollection<TypeDefinition> Generate(IEnumerable<Type> types) {
            var unProcessedTypes = types.ToList();
            var managedTypes = new List<TypeDefinition>();
            
            while (unProcessedTypes.Any())
            {
                // pop first type
                var processingType = unProcessedTypes.Last();
                unProcessedTypes.RemoveAt(unProcessedTypes.Count - 1);

                // already in category type;
                if (managedTypes.Any(x => x.Type == processingType))
                    continue;

                // add type to category types
                var processingTypeCategory = _typeHelper.GetProcessingCategory(processingType);
                managedTypes.Add(_typeHelper.ToTypeDefinition(processingType));

                // process all properties
                foreach (var property in TypeExtensions.GetProperties(processingType))
                {
                    if (_propertyHelper.IsTypeScriptIgnored(property))
                        continue;

                    var propertyType = property.PropertyType;
                    
                    unProcessedTypes.Add(propertyType);

                    if (propertyType.IsGenericType)
                    {
                        unProcessedTypes.AddRange(propertyType.GetGenericArguments());
                    }
                }
            }

            return managedTypes;
        }
    }
}

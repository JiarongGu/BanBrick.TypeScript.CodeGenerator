using BanBrick.TypeScript.CodeGenerator.Enums;
using BanBrick.TypeScript.CodeGenerator.Helpers;
using BanBrick.TypeScript.CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Generators
{
    public class ManagedTypeGenerator
    {
        private readonly TypeHelper _typeHelper;
        private readonly PropertyHelper _propertyHelper;

        public ManagedTypeGenerator() {
            _typeHelper = new TypeHelper();
            _propertyHelper = new PropertyHelper();
        }

        public ICollection<ManagedType> Generate(IEnumerable<Type> types) {
            var unProcessedTypes = types.ToList();
            var managedTypes = new List<ManagedType>();

            while (unProcessedTypes.Any())
            {
                // pop first type
                var processingType = unProcessedTypes.Last();
                unProcessedTypes.RemoveAt(unProcessedTypes.Count - 1);

                // already in category type;
                if (managedTypes.Any(x => x.Type == processingType))
                    continue;

                // add type to category types
                var processingTypeCategory = _typeHelper.GetTypeCategory(processingType);
                if (processingTypeCategory == TypeCategory.Enum || processingTypeCategory == TypeCategory.Object)
                    managedTypes.Add(_typeHelper.ToCategoryType(processingType));

                // process all properties
                foreach (var property in TypeExtensions.GetProperties(processingType))
                {
                    if (_propertyHelper.IsTypeScriptIgnored(property))
                        continue;

                    var propertyType = property.PropertyType;
                    
                    var typeCategory = _typeHelper.GetTypeCategory(propertyType);

                    if (typeCategory == TypeCategory.Primitive)
                        continue;

                    if (typeCategory == TypeCategory.Enum || typeCategory == TypeCategory.Object)
                    {
                        unProcessedTypes.Add(propertyType);
                        continue;
                    }

                    if (typeCategory == TypeCategory.Collection) {
                        if (propertyType.IsArray) {
                            unProcessedTypes.Add(propertyType.GetElementType());
                        } else {
                            unProcessedTypes.AddRange(propertyType.GetGenericArguments());
                        }
                        continue;
                    }

                    if (typeCategory == TypeCategory.Dictionary || typeCategory == TypeCategory.Generic)
                    {
                        unProcessedTypes.AddRange(propertyType.GetGenericArguments());
                        continue;
                    }
                }
            }

            return managedTypes;
        }
    }
}

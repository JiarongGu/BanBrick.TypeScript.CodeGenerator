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

namespace BanBrick.TypeScript.CodeGenerator.Resolvers
{
    internal class RelationResolver
    {
        private readonly TypeHelper _typeHelper;

        public RelationResolver() {
            _typeHelper = new TypeHelper();
        }

        public ICollection<TypeDefinition> Resolve(IEnumerable<Type> types)
        {
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
                var currentDefinition = _typeHelper.ToTypeDefinition(processingType);

                object instance = null;

                // create new instance if type contains parameterless constractor
                if (processingType.GetConstructor(Type.EmptyTypes) != null)
                    instance = Activator.CreateInstance(processingType);

                // process all properties
                foreach (var property in TypeExtensions.GetProperties(processingType))
                {
                    if (property.IsTypeScriptIgnored())
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
                    
                    currentDefinition.Properties.Add(new PropertyDefinition()
                    {
                        PropertyInfo = property,
                        Type = propertyType,
                        IsNullable = _typeHelper.IsNullable(propertyType),
                        DefaultValue = property.TryGetValue(instance)
                    });
                }

                if (types.Contains(currentDefinition.Type))
                    currentDefinition.IsFirstLevel = true;

                typeDefinitions.Add(currentDefinition);
            }

            return typeDefinitions;
        }
    }
}

using BanBrick.TypeScript.CodeGenerator.Enums;
using BanBrick.TypeScript.CodeGenerator.Extensions;
using BanBrick.TypeScript.CodeGenerator.Models;
using BanBrick.TypeScript.CodeGenerator.TypeHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BanBrick.TypeScript.CodeGenerator.Resolvers
{
    internal class RelationResolver
    {
        private readonly TypeResolver _typeResolver;
        private readonly IDictionary<Type, ITypeHandler> _typeHandlerMap;

        public RelationResolver(IDictionary<Type, ITypeHandler> typeHandlerMap) {
            _typeHandlerMap = typeHandlerMap;
            _typeResolver = new TypeResolver(typeHandlerMap);
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
                var processingTypeCategory = _typeResolver.GetProcessingCategory(processingType);
                var currentDefinition = _typeResolver.ToTypeDefinition(processingType);

                object instance = null;

                // create new instance if type contains parameterless constructor
                if (processingType.GetConstructor(Type.EmptyTypes) != null)
                    instance = Activator.CreateInstance(processingType);

                // process all properties
                foreach (var property in TypeExtensions.GetProperties(processingType))
                {
                    if (property.IsTypeScriptIgnored())
                        continue;

                    var propertyType = property.PropertyType;
                    var category = _typeResolver.GetProcessingCategory(propertyType);

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
                    
                    currentDefinition.Properties.Add( new PropertyDefinition()
                    {
                        PropertyInfo = property,
                        Type = propertyType,
                        IsNullable = _typeResolver.IsNullable(propertyType),
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

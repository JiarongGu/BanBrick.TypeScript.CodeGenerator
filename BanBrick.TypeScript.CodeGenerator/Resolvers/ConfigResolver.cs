using BanBrick.TypeScript.CodeGenerator.Convertors;
using BanBrick.TypeScript.CodeGenerator.Enums;
using BanBrick.TypeScript.CodeGenerator.Extensions;
using BanBrick.TypeScript.CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Resolvers
{
    internal class ConfigResolver
    {
        public IEnumerable<TypeDefinition> Resolve(IEnumerable<TypeDefinition> definitions)
        {
            var unprocessedDefinitions = definitions.Where(x => x.IsFirstLevel)
                .Select(x => { x.ProcessConfig = ConfigConvertor.GetProcessConfig(x.Type); return x; })
                .OrderBy(x => x.ProcessConfig.OutputType)
                .ToList();

            var processedDictionary = definitions.ToDictionary(x => x.Type, x => x);
            var processedType = new List<Type>();

            while (unprocessedDefinitions.Any())
            {
                var processingDefinition = unprocessedDefinitions.First();
                unprocessedDefinitions.RemoveAt(0);

                processedType.Add(processingDefinition.Type);

                processingDefinition.ProcessConfig =
                    processingDefinition.ProcessConfig ?? ConfigConvertor.GetProcessConfig(processingDefinition.Type);

                processedDictionary[processingDefinition.Type] = processingDefinition;

                var innerTypes = new List<Type>();

                if (processingDefinition.ProcessingCategory == ProcessingCategory.Collection)
                {
                    innerTypes.Add(processingDefinition.Type.GetCollectionType());
                }

                if (processingDefinition.ProcessingCategory == ProcessingCategory.Dictionary)
                {
                    var dicTypes = processingDefinition.Type.GetDictionaryTypes();
                    innerTypes.Add(dicTypes.key);
                    innerTypes.Add(dicTypes.value);
                }

                if (processingDefinition.ProcessingCategory == ProcessingCategory.Generic)
                {
                    innerTypes.AddRange(processingDefinition.Type.GetGenericArguments());
                }

                if (processingDefinition.ProcessingCategory == ProcessingCategory.Object)
                {
                    innerTypes.AddRange(processingDefinition.Properties.Select(x => x.Type));
                }

                foreach (var innerType in innerTypes)
                {
                    var propertyDefinition = processedDictionary[innerType];

                    // ignore property definition that's the same as processing definition
                    if (propertyDefinition == processingDefinition)
                        continue;

                    if (propertyDefinition.ProcessConfig == null)
                    {
                        if (processingDefinition.ProcessConfig?.Inherit ?? false)
                        {
                            propertyDefinition.ProcessConfig = new ProcessConfig()
                            {
                                OutputType = processingDefinition.ProcessConfig.OutputType,
                                Inherit = true
                            };
                        }

                        if (processingDefinition.ProcessConfig?.OutputType == OutputType.Const)
                        {
                            propertyDefinition.ProcessConfig = new ProcessConfig()
                            {
                                OutputType = OutputType.None,
                                Inherit = true
                            };
                        }
                    }

                    if (processedType.Any(x => x == propertyDefinition.Type) == false)
                    {
                        unprocessedDefinitions.Add(propertyDefinition);
                    }
                }
            }

            var processedDefinitions = processedDictionary.Select(x => x.Value);

            // remove config from inherited non heritable types
            processedDefinitions.Where(x => !x.IsInheritable() && (x.ProcessConfig?.Inherit ?? false)).ToList()
                .ForEach(x =>
                    x.ProcessConfig = null
                );

            processedDefinitions.Where(x => x.ProcessConfig == null).ToList().ForEach(x =>
            {
                x.ProcessConfig = ConfigConvertor.GetProcessConfig(x.ProcessingCategory);
            });

            processedDefinitions.Where(x => x.ProcessConfig.OutputType == OutputType.Default).ToList().ForEach(x =>
            {
                x.ProcessConfig.OutputType = ConfigConvertor.Parse(x.ProcessingCategory);
            });

            return processedDefinitions;
        }
    }
}

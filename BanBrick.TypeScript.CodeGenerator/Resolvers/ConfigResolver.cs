using BanBrick.TypeScript.CodeGenerator.Convertors;
using BanBrick.TypeScript.CodeGenerator.Enums;
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
            var typeDefinitions = definitions.ToList();
            var unprocessedDefinitions = definitions.Where(x => x.IsFirstLevel).ToList();
            var processedDictionary = typeDefinitions.ToDictionary(x => x.Type, x => x);

            while (unprocessedDefinitions.Any())
            {
                var processingDefinition = unprocessedDefinitions.Last();
                unprocessedDefinitions.RemoveAt(unprocessedDefinitions.Count - 1);

                processingDefinition.ProcessConfig = 
                    processingDefinition.ProcessConfig ?? ConfigConvertor.GetProcessConfig(processingDefinition.Type);

                processedDictionary[processingDefinition.Type] = processingDefinition;
                
                foreach (var property in processingDefinition.Properties)
                {
                    var propertyDefinition = typeDefinitions.First(x => x.Type == property.Type);

                    if (processingDefinition.ProcessConfig?.Inherit ?? false)
                        propertyDefinition.ProcessConfig = processingDefinition.ProcessConfig;

                    unprocessedDefinitions.Add(propertyDefinition);
                }
            }

            var processedDefinitions = processedDictionary.Select(x => x.Value);

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

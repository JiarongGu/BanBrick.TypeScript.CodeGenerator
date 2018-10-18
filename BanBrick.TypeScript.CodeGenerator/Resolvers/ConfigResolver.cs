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

            while (unprocessedDefinitions.Any())
            {
                var processingDefinition = unprocessedDefinitions.First();
                unprocessedDefinitions.RemoveAt(0);

                processingDefinition.ProcessConfig = 
                    processingDefinition.ProcessConfig ?? ConfigConvertor.GetProcessConfig(processingDefinition.Type);

                processedDictionary[processingDefinition.Type] = processingDefinition;
                
                foreach (var property in processingDefinition.Properties)
                {
                    var propertyDefinition = processedDictionary[property.Type];

                    if (propertyDefinition.ProcessConfig == null)
                    {
                        if ((processingDefinition.ProcessConfig?.Inherit ?? false) && propertyDefinition.IsInheritable())
                            propertyDefinition.ProcessConfig = new ProcessConfig() { OutputType = processingDefinition.ProcessConfig.OutputType, Inherit = true };

                        if (processingDefinition.ProcessConfig?.OutputType == OutputType.Const)
                            propertyDefinition.ProcessConfig = new ProcessConfig() { OutputType = OutputType.None, Inherit = true };
                    }

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

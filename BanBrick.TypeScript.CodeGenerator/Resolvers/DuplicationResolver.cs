using BanBrick.TypeScript.CodeGenerator.Annotations;
using BanBrick.TypeScript.CodeGenerator.Convertors;
using BanBrick.TypeScript.CodeGenerator.Enums;
using BanBrick.TypeScript.CodeGenerator.Generators;
using BanBrick.TypeScript.CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Resolvers
{
    internal class DuplicationResolver
    {
        public IEnumerable<TypeDefinition> Resolve(IEnumerable<TypeDefinition> definitions)
        {
            var nameConvertor = new NameConvertor(definitions);
            var valueConvertor = new ValueConvertor(definitions, nameConvertor);

            var codeGeneratorFactory = new CodeGeneratorFactory(nameConvertor, valueConvertor);

            var typeDefinitions = definitions.ToList();

            var duplicationGroups = typeDefinitions
                .Where(x => x.ProcessConfig.OutputType != OutputType.None)
                .GroupBy(x => x.ProcessConfig.Name)
                .Where(x => x.ToList().Count > 1)
                .ToList();

            if (duplicationGroups.Count > 0)
            {
                foreach (var duplicationGroup in duplicationGroups)
                {
                    var commonName = duplicationGroup.Key;
                    var duplicateDefinitions = duplicationGroup.ToList();

                    var codeGroups = duplicateDefinitions
                        .Select(x => (
                            code: codeGeneratorFactory.GetInstance(x.ProcessConfig.OutputType)?.Generate(x.ActualType), 
                            definition: x)
                        )
                        .GroupBy(x => x.code)
                        .ToList();

                    // keep the same code of first group, flag others to NoGeneration
                    codeGroups.ForEach(x => {
                        var outputType = x.First().definition.ProcessConfig.OutputType;

                        x.ToList().ForEach(y =>
                        {
                            y.definition.ProcessConfig.OutputType = OutputType.None;
                        });

                        x.First().definition.ProcessConfig.OutputType = outputType;
                    });

                    if (codeGroups.Count() > 1)
                    {
                        // rename if multiple version of code, rename need to apply
                        for (int i = 0; i < codeGroups.Count; i++)
                        {
                            codeGroups[i].ToList().ForEach(x =>
                            {
                                var name = x.definition.ProcessConfig.Name;
                                var indexOfMarker = name.IndexOfAny(new char[]{ '[', '<'});
                                x.definition.ProcessConfig.Name = 
                                    indexOfMarker < 0 ? $"{name}_{i}" : $"{name.Substring(0, indexOfMarker)}_{i}{name.Substring(indexOfMarker)}";  
                            });
                        }
                    }
                }
            }

            return typeDefinitions;
        }
    }
}

using BanBrick.TypeScript.CodeGenerator.Annotations;
using BanBrick.TypeScript.CodeGenerator.Convertors;
using BanBrick.TypeScript.CodeGenerator.Enums;
using BanBrick.TypeScript.CodeGenerator.Generators;
using BanBrick.TypeScript.CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Processers
{
    internal class TypeDuplicationProcesser
    {
        public IEnumerable<TypeDefinition> Process(IEnumerable<TypeDefinition> definitions)
        {
            var nameConvertor = new NameConvertor(definitions);
            var valueConvertor = new ValueConvertor(definitions, nameConvertor);

            var codeGeneratorFactory = new CodeGeneratorFactory(nameConvertor, valueConvertor);

            var typeDefinitions = definitions.ToList();

            var duplicationGroups = typeDefinitions
                .Where(IsGenerationType)
                .GroupBy(x => x.Name)
                .Where(x => x.ToList().Count > 1)
                .ToList();

            if (duplicationGroups.Count > 0)
            {
                foreach (var duplicationGroup in duplicationGroups)
                {
                    var commonName = duplicationGroup.Key;
                    var duplicateDefinitions = duplicationGroup.ToList();

                    var codeGroup = duplicateDefinitions
                        .Select(x => (code: codeGeneratorFactory.GetInstance(x.ProcessType).Generate(x.ActualType), definition: x))
                        .GroupBy(x => x.code);

                    // keep the same code of first group, flag others to NoGeneration
                    codeGroup.ToList().ForEach(x => {
                        x.ToList().ForEach(y =>
                        {
                            y.definition.NoGeneration = true;
                        });

                        x.First().definition.NoGeneration = false;
                    });

                    if (codeGroup.Count() > 1)
                    {
                        // rename if multiple version of code, rename need to apply
                        var codeGroupList = codeGroup.ToList();
                        for (int i = 0; i < codeGroupList.Count; i++)
                        {
                            codeGroupList[i].ToList().ForEach(x =>
                            {
                                x.definition.Name = $"{x.definition.Name}_{i}";
                            });
                        }
                    }
                }
            }

            return typeDefinitions;
        }

        public bool IsGenerationType(TypeDefinition typeDefinition)
        {
            return typeDefinition.ProcessType != TypeScriptObjectType.Default;
        }
    }
}

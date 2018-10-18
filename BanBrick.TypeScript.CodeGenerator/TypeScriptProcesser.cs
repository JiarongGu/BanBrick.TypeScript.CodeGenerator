using BanBrick.TypeScript.CodeGenerator.Convertors;
using BanBrick.TypeScript.CodeGenerator.Generators;
using BanBrick.TypeScript.CodeGenerator.Helpers;
using BanBrick.TypeScript.CodeGenerator.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BanBrick.TypeScript.CodeGenerator.Models;
using BanBrick.TypeScript.CodeGenerator.Enums;
using BanBrick.TypeScript.CodeGenerator.Annotations;
using PluralizationService;
using PluralizationService.English;
using System.Globalization;

namespace BanBrick.TypeScript.CodeGenerator
{
    public class TypeScriptProcesser
    {
        private readonly AssemblyHelper _assemblyHelper;

        public TypeScriptProcesser()
        {
            _assemblyHelper = new AssemblyHelper();
        }

        public string GenerateTypeScript(IEnumerable<Type> types) {
            var typeDefinitions = types
                .ResolveRelations()
                .ResolveConfigs()
                .ResolveNames()
                .ResolveDuplications();

            var processDefinitions = typeDefinitions.Where(x => x.ProcessConfig?.OutputType != OutputType.None);

            var nameConvertor = new NameConvertor(typeDefinitions);
            var valueConvertor = new ValueConvertor(typeDefinitions, nameConvertor);

            var codeGeneratorFactory = new CodeGeneratorFactory(nameConvertor, valueConvertor);
            var codeBuilder = new StringBuilder();

            var pluralizationBuilder = new PluralizationApiBuilder();
            pluralizationBuilder.AddEnglishProvider();

            var pluralizationApi = pluralizationBuilder.Build();
            var cultureInfo = new CultureInfo("en-US");

            codeBuilder.Append(_assemblyHelper.GetAssemblyContent());

            Enum.GetValues(typeof(OutputType)).Cast<OutputType>().ToList()
                .ForEach(x =>
                {
                    var codeGenerator = codeGeneratorFactory.GetInstance(x);
                    if (codeGenerator != null)
                    {
                        var sectionName = pluralizationApi.Pluralize(x.ToString(), cultureInfo);
                        codeBuilder.Append(_assemblyHelper.GetSectionSeparator(sectionName));

                        processDefinitions.GetProcessingTypes(x).ForEach(t =>
                            codeBuilder.AppendLine(codeGenerator.Generate(t))
                        );
                    }
                });

            return codeBuilder.ToString();
        }
    }
}

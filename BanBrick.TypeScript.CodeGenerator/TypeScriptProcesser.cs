using BanBrick.TypeScript.CodeGenerator.Convertors;
using BanBrick.TypeScript.CodeGenerator.Generators;
using BanBrick.TypeScript.CodeGenerator.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BanBrick.TypeScript.CodeGenerator.Enums;
using BanBrick.Utils.Extensions;
using PluralizationService;
using PluralizationService.English;
using System.Globalization;
using BanBrick.TypeScript.CodeGenerator.TypeHandlers;
using System.Reflection;
using BanBrick.TypeScript.CodeGenerator.Models;

namespace BanBrick.TypeScript.CodeGenerator
{
    public class TypeScriptProcesser
    {
        private readonly AssemblyCodeGenerator _assemblyHelper;
        private readonly Dictionary<Type, ITypeHandler> _typeHandlers;

        private IEnumerable<TypeDefinition> _typeDefinitions;
        private IEnumerable<Type> _types;

        public TypeScriptProcesser(IEnumerable<Type> types)
        {
            _assemblyHelper = new AssemblyCodeGenerator();

            _types = types;

            _typeHandlers = Assembly.GetAssembly(typeof(TypeScriptProcesser))
                .GetRelatedLoadableTypes(typeof(ITypeHandler))
                .Select(x => (ITypeHandler)Activator.CreateInstance(x))
                .ToDictionary(x => x.Type, x => x);
        }

        public IReadOnlyDictionary<Type, ITypeHandler> TypeHandlers => _typeHandlers;

        public TypeScriptProcesser ResolveRelations() {
            _typeDefinitions = _types
                .ResolveRelations(_typeHandlers)
                .ResolveConfigs()
                .ResolveNames()
                .ResolveDuplications();

            return this;
        }

        public string GenerateTypeScript(IEnumerable<Type> types) {
            if (_typeDefinitions == null)
                throw new InvalidOperationException("require to run ResolveRelations first");
            
            var processDefinitions = _typeDefinitions.Where(x => x.ProcessConfig?.OutputType != OutputType.None);

            var nameConvertor = new NameConvertor(_typeDefinitions);
            var valueConvertor = new ValueConvertor(_typeDefinitions, nameConvertor);

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

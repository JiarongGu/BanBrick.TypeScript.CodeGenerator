using BanBrick.TypeScript.CodeGenerator.Convertors;
using BanBrick.TypeScript.CodeGenerator.Generators;
using BanBrick.TypeScript.CodeGenerator.Helpers;
using BanBrick.TypeScript.CodeGenerator.Processers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator
{
    public class TypeScriptProcesser
    {
        private readonly TypeDefinitionProcesser _typeDefinitionProcesser;
        private readonly TypeNameProcesser _typeNameProcesser;

        private readonly AssemblyHelper _assemblyHelper;

        public TypeScriptProcesser()
        {
            _typeDefinitionProcesser = new TypeDefinitionProcesser();
            _typeNameProcesser = new TypeNameProcesser();

            _assemblyHelper = new AssemblyHelper();
        }

        public string GenerateTypeScript(IEnumerable<Type> types) {
            var typeDefinitions = _typeDefinitionProcesser.Process(types);
            var namedDefinitions = _typeNameProcesser.Process(typeDefinitions);
            var nameConvertor = new NameConvertor(namedDefinitions);
            var classGenerator = new ClassCodeGenerator(nameConvertor);
            var enumGenerator = new EnumCodeGenerator(nameConvertor);

            var codeBuilder = new StringBuilder();

            codeBuilder.Append(_assemblyHelper.GetAssemblyContent());
            codeBuilder.Append(_assemblyHelper.GetSectionSeparator("Enums"));

            typeDefinitions.Where(x => x.Category == Enums.ProcessingCategory.Enum).ToList().ForEach(x =>
                codeBuilder.AppendLine(enumGenerator.Generate(x.Type))
            );

            codeBuilder.Append(_assemblyHelper.GetSectionSeparator("Classes"));

            typeDefinitions.Where(x => x.Category == Enums.ProcessingCategory.Object).ToList().ForEach(x =>
                codeBuilder.AppendLine(classGenerator.Generate(x.Type))
            );

            return codeBuilder.ToString();
        }
    }
}

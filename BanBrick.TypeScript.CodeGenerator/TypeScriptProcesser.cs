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
            var typeDefinitions = types.ResolveRelations().ResolveNames();
            
            var nameConvertor = new NameConvertor(typeDefinitions);
            var valueConvertor = new ValueConvertor(typeDefinitions, nameConvertor);

            var classGenerator = new ClassCodeGenerator(nameConvertor, valueConvertor);
            var constGenerator = new ConstCodeGenerator(nameConvertor, valueConvertor);
            var enumGenerator = new EnumCodeGenerator(nameConvertor);

            var codeBuilder = new StringBuilder();

            codeBuilder.Append(_assemblyHelper.GetAssemblyContent());
            codeBuilder.Append(_assemblyHelper.GetSectionSeparator("Enums"));

            typeDefinitions.GetProcessingTypes(TypeScriptObjectType.Enum).ForEach(x =>
                codeBuilder.AppendLine(enumGenerator.Generate(x))
            );

            codeBuilder.Append(_assemblyHelper.GetSectionSeparator("Classes"));

            typeDefinitions.GetProcessingTypes(TypeScriptObjectType.Class).ForEach(x =>
                codeBuilder.AppendLine(classGenerator.Generate(x))
            );

            codeBuilder.Append(_assemblyHelper.GetSectionSeparator("Consts"));

            typeDefinitions.GetProcessingTypes(TypeScriptObjectType.Const).ForEach(x =>
                codeBuilder.AppendLine(constGenerator.Generate(x))
            );

            return codeBuilder.ToString();
        }
    }
}

using BanBrick.TypeScript.CodeGenerator.Generators;
using BanBrick.TypeScript.CodeGenerator.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator
{
    public class CodeGenerator
    {
        private readonly ManagedTypeGenerator _managedTypeConvertor;
        private readonly ObjectCodeGenerator _objectGenerator;
        private readonly EnumCodeGenerator _enumGenerator;

        private readonly AssemblyHelper _assemblyHelper;
        public CodeGenerator()
        {
            _managedTypeConvertor = new ManagedTypeGenerator();
            _objectGenerator = new ObjectCodeGenerator();
            _enumGenerator = new EnumCodeGenerator();
            _assemblyHelper = new AssemblyHelper();
        }

        public string GenerateTypeScript(IEnumerable<Type> types) {
            var managedTypes = _managedTypeConvertor.Generate(types);
            var codeBuilder = new StringBuilder();

            codeBuilder.Append(_assemblyHelper.GetAssemblyContent());

            codeBuilder.Append(_assemblyHelper.GetSectionSeparator("Enums"));
            managedTypes.Where(x => x.Category == Enums.TypeCategory.Enum).ToList().ForEach(x =>
                codeBuilder.AppendLine(_enumGenerator.Generate(x.Type))
            );

            codeBuilder.Append(_assemblyHelper.GetSectionSeparator("Classes"));
            managedTypes.Where(x => x.Category == Enums.TypeCategory.Object).ToList().ForEach(x =>
                codeBuilder.AppendLine(_objectGenerator.Generate(x.Type))
            );

            return codeBuilder.ToString();
        }
    }
}

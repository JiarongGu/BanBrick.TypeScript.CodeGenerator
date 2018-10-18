using BanBrick.TypeScript.CodeGenerator.Convertors;
using BanBrick.TypeScript.CodeGenerator.Enums;
using BanBrick.TypeScript.CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Generators
{
    internal class CodeGeneratorFactory
    {
        private readonly IValueConvertor _valueConvertor;
        private readonly INameConvertor _nameConvertor;

        private readonly Dictionary<OutputType, ICodeGenerator> _typeCodeGenerators;

        public CodeGeneratorFactory(INameConvertor nameConvertor, IValueConvertor valueConvertor)
        {
            _valueConvertor = valueConvertor;
            _nameConvertor = nameConvertor;

            _typeCodeGenerators = new Dictionary<OutputType, ICodeGenerator>();
            _typeCodeGenerators[OutputType.Enum] = new EnumCodeGenerator(_nameConvertor);
            _typeCodeGenerators[OutputType.Interface] = new InterfaceCodeGenerator(_nameConvertor);
            _typeCodeGenerators[OutputType.Class] = new ClassCodeGenerator(_nameConvertor, _valueConvertor);
            _typeCodeGenerators[OutputType.Const] = new ConstCodeGenerator(_nameConvertor, _valueConvertor);
        }

        public ICodeGenerator GetInstance(OutputType outputType)
        {
            if (outputType == OutputType.None || outputType == OutputType.Default)
                return null;
            return _typeCodeGenerators[outputType];
        }
    }
}

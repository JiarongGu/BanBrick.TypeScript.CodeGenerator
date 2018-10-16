using BanBrick.TypeScript.CodeGenerator.Annotations;
using BanBrick.TypeScript.CodeGenerator.Convertors;
using System;
using System.Collections.Generic;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Generators
{
    internal class CodeGeneratorFactory
    {
        private readonly IValueConvertor _valueConvertor;
        private readonly INameConvertor _nameConvertor;

        private readonly Dictionary<TypeScriptObjectType, ICodeGenerator> _typeCodeGenerators;

        public CodeGeneratorFactory(INameConvertor nameConvertor, IValueConvertor valueConvertor)
        {
            _valueConvertor = valueConvertor;
            _nameConvertor = nameConvertor;

            _typeCodeGenerators = new Dictionary<TypeScriptObjectType, ICodeGenerator>();
            _typeCodeGenerators[TypeScriptObjectType.Enum] = new EnumCodeGenerator(_nameConvertor);
            _typeCodeGenerators[TypeScriptObjectType.Interface] = new InterfaceCodeGenerator(_nameConvertor);
            _typeCodeGenerators[TypeScriptObjectType.Class] = new ClassCodeGenerator(_nameConvertor, _valueConvertor);
            _typeCodeGenerators[TypeScriptObjectType.Const] = new ConstCodeGenerator(_nameConvertor, _valueConvertor);
        }

        public ICodeGenerator GetInstance(TypeScriptObjectType objectType)
        {
            if (objectType == TypeScriptObjectType.Default)
                return null;
            return _typeCodeGenerators[objectType];
        }
    }
}

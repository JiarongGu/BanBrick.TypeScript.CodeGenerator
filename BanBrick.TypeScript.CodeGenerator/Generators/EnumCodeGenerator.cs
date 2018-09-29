using BanBrick.TypeScript.CodeGenerator.Helpers;
using BanBrick.TypeScript.CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Generators
{
    public class EnumCodeGenerator
    {
        private readonly TypeHelper _typeHelper;
        private readonly ValueCodeGenerator _valueCodeGenerator;

        public EnumCodeGenerator() {
            _typeHelper = new TypeHelper();
            _valueCodeGenerator = new ValueCodeGenerator();
        }

        public string Generate(Type enumType)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException("must be enum type");

            var stringBuilder = new StringBuilder();

            var names = Enum.GetNames(enumType);
            var values = Enum.GetValues(enumType).Cast<int>().ToArray();
            var enumName = _typeHelper.GetTypeScriptName(enumType);

            // add typescript enum
            stringBuilder.AppendLine($"export enum {enumName} {{");

            for (int i = 0; i < names.Length; i++)
                stringBuilder.AppendLine($"  {names[i]} = {values[i]}{(i == names.Length - 1 ? string.Empty : ",")}");

            stringBuilder.AppendLine("}");

            // add typescript enum array
            stringBuilder.AppendLine($"export const {enumName}Array = [");

            for (int i = 0; i < names.Length; i++)
                stringBuilder.AppendLine($"  {_valueCodeGenerator.GetStringValueCode(names[i])},");
            stringBuilder.AppendLine("];");

            return stringBuilder.ToString(); ;
        }
    }
}

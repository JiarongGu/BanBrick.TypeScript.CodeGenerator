using BanBrick.TypeScript.CodeGenerator.Convertors;
using BanBrick.TypeScript.CodeGenerator.Enums;
using BanBrick.TypeScript.CodeGenerator.Helpers;
using BanBrick.TypeScript.CodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Generators
{
    internal sealed class EnumCodeGenerator
    {
        private readonly ValueConvertor _valueConvertor;
        private readonly INameConvertor _nameConvertor;

        public EnumCodeGenerator(INameConvertor nameConvertor) {
            _valueConvertor = new ValueConvertor(nameConvertor);
            _nameConvertor = nameConvertor;
        }

        public string Generate(Type type)
        {
            var stringBuilder = new StringBuilder();

            var names = Enum.GetNames(type);
            var values = Enum.GetValues(type).Cast<int>().ToArray();

            // add typescript enum
            stringBuilder.AppendLine($"export enum {_nameConvertor.GetName(type)} {{");

            for (int i = 0; i < names.Length; i++)
                stringBuilder.AppendLine($"  {names[i]} = {values[i]}{(i == names.Length - 1 ? string.Empty : ",")}");

            stringBuilder.AppendLine("}");

            // add typescript enum array
            stringBuilder.AppendLine($"export const {_nameConvertor.GetName(type)}Array = [");

            for (int i = 0; i < names.Length; i++)
                stringBuilder.AppendLine($"  {_valueConvertor.GetStringValueCode(names[i])},");
            stringBuilder.AppendLine("];");

            return stringBuilder.ToString(); ;
        }
    }
}

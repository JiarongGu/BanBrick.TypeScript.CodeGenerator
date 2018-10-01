using BanBrick.TypeScript.CodeGenerator.Convertors;
using BanBrick.TypeScript.CodeGenerator.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Generators
{
    public class ObjectCodeGenerator
    {
        private readonly TypeHelper _typeHelper;
        private readonly StringHelper _stringHelper;
        private readonly PropertyHelper _propertyHelper;

        private readonly ValueConvertor _valueConvertor;
        private readonly NameConvertor _nameConvertor;

        public ObjectCodeGenerator()
        {
            _typeHelper = new TypeHelper();
            _stringHelper = new StringHelper();
            _propertyHelper = new PropertyHelper();

            _valueConvertor = new ValueConvertor();
            _nameConvertor = new NameConvertor();
        }

        public string Generate(Type objectType)
        {
            if (objectType.IsEnum || objectType.IsPrimitive || objectType.IsGenericType)
                throw new ArgumentException("must be object type");

            if (objectType.IsInterface)
                throw new ArgumentException("currently not support interface");


            var stringBuilder = new StringBuilder();

            var typeScriptType = _nameConvertor.GetTypeScriptName(objectType);
            
            stringBuilder.AppendLine($"export class {typeScriptType} {{");
            stringBuilder.AppendLine("  constructor(");

            object instance = null;
            
            // create new instance if type contains parameterless constractor
            if (objectType.GetConstructor(Type.EmptyTypes) != null)
            {
                instance = Activator.CreateInstance(objectType);
            }

            var properties = TypeExtensions.GetProperties(objectType);

            foreach (var property in properties)
            {
                if (_propertyHelper.IsTypeScriptIgnored(property))
                    continue;

                var propertyType = property.PropertyType;
                var propertyName = _nameConvertor.GetTypeScriptName(propertyType);
                var propertyValue = instance == null ? null : property.GetValue(instance);

                var valueCode = _valueConvertor.GenerateValueCode(propertyType, propertyValue);
                
                var noValueCode = string.IsNullOrEmpty(valueCode);

                var nullableCode =  _typeHelper.IsNullable(propertyType) && noValueCode ? "?" : "";
                stringBuilder.Append($"  public {_stringHelper.ToCamelCase(property.Name)}{nullableCode}: ");
                
                if (!noValueCode)
                {
                    stringBuilder.AppendLine($"{ propertyName} = {valueCode},");
                }
                else
                {
                    stringBuilder.AppendLine($"{ propertyName},");
                }
            }

            stringBuilder.AppendLine("  ) { }");
            stringBuilder.AppendLine("}");

            return stringBuilder.ToString();
        }
    }
}

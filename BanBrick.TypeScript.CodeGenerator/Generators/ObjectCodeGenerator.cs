using BanBrick.TypeScript.CodeGenerator.Enums;
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
        private readonly PropertyHelper _propertyHelper;
        private readonly ValueCodeGenerator _valueCodeGenerator;
        private readonly StringHelper _stringHelper;

        public ObjectCodeGenerator()
        {
            _typeHelper = new TypeHelper();
            _propertyHelper = new PropertyHelper();
            _valueCodeGenerator = new ValueCodeGenerator();
            _stringHelper = new StringHelper();
        }

        public string Generate(Type objectType)
        {
            if (objectType.IsEnum || objectType.IsPrimitive || objectType.IsGenericType)
                throw new ArgumentException("must be object type");

            if (objectType.IsInterface)
                throw new ArgumentException("currently not support interface");


            var stringBuilder = new StringBuilder();

            var typeScriptType = _typeHelper.GetTypeScriptName(objectType);
            
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
                var propertyName = _typeHelper.GetTypeScriptName(propertyType);
                var propertyValue = instance == null ? null : property.GetValue(instance);

                var valueCode = _valueCodeGenerator.GenerateValueCode(propertyType, propertyValue);
                
                var noValueCode = string.IsNullOrEmpty(valueCode);

                var nullableCode =  _typeHelper.IsNullableType(propertyType) && noValueCode ? "?" : "";
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

﻿using BanBrick.TypeScript.CodeGenerator.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Extensions
{
    internal static class AddtionalTypeExtensions
    {
        public static (Type key, Type value) GetDictionaryTypes(this Type type)
        {
            var keyValuetypes = type.GenericTypeArguments;
            return (key: keyValuetypes[0], value: keyValuetypes[1]);
        }

        public static Type GetCollectionType(this Type type)
        {
            return type.IsArray ? type.GetElementType() : type.GenericTypeArguments.First();
        }
    }
}

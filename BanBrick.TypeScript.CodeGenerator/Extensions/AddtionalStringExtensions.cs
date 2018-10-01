using System;
using System.Collections.Generic;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Extensions
{
    internal static class AddtionalStringExtensions
    {
        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return value[0].ToString().ToLower() + value.Substring(1);
        }
    }
}

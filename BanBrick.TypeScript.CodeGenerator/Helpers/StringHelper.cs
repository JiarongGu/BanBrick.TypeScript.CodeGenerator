using System;
using System.Collections.Generic;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Helpers
{
    public class StringHelper
    {
        public string ToCamelCase(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return value[0].ToString().ToLower() + value.Substring(1);
        }
    }
}

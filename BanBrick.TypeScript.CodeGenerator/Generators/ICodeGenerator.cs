using System;
using System.Collections.Generic;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.Generators
{
    interface ICodeGenerator
    {
        string Generate(Type type);
    }
}

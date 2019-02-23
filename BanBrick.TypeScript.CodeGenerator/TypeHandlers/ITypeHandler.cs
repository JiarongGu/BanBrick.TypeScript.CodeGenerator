using BanBrick.TypeScript.CodeGenerator.Convertors;
using BanBrick.TypeScript.CodeGenerator.Enums;
using System;

namespace BanBrick.TypeScript.CodeGenerator.TypeHandlers
{
    public interface ITypeHandler
    {
        Type Type { get; }
        string DefaultValue { get; }
        string GetValue(object value, IValueConvertor valueConvertor);
        string GetName(INameConvertor nameConvertor);
        ProcessingCategory ProcessingCategory { get; }
    }
}

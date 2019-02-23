using BanBrick.TypeScript.CodeGenerator.Convertors;
using BanBrick.TypeScript.CodeGenerator.Enums;
using System;

namespace BanBrick.TypeScript.CodeGenerator.TypeHandlers
{
    public abstract class NotNullStringTypeHandler<TType> : ITypeHandler
    {
        public ProcessingCategory ProcessingCategory => ProcessingCategory.Primitive;
        public Type Type => typeof(TType);
        public virtual string DefaultValue => "''";

        public virtual string GetName(INameConvertor nameConvertor) => "string";
        public virtual string GetValue(object value, IValueConvertor valueConvertor)
        {
            return $"'{value.ToString().Replace("'", "\'")}'";
        }
        public virtual string GetValue(TType value, IValueConvertor valueConvertor)
        {
            return GetValue((object)value, valueConvertor);
        }
    }
    public abstract class NullableStringTypeHandler<TType> : ITypeHandler
    {
        public ProcessingCategory ProcessingCategory => ProcessingCategory.Primitive;
        public Type Type => typeof(TType);
        public string DefaultValue => null;

        public virtual string GetName(INameConvertor nameConvertor) => "string";

        public virtual string GetValue(object value, IValueConvertor valueConvertor)
        {
            if (value == null)
                return null;
            return $"'{value.ToString().Replace("'", "\'")}'";
        }
        public virtual string GetValue(TType value, IValueConvertor valueConvertor)
        {
            return GetValue((object)value, valueConvertor);
        }
    }

    // not null string handlers
    public class DateTimeTypeHandler : NotNullStringTypeHandler<DateTime> { }
    public class DateTimeOffsetTypeHandler : NotNullStringTypeHandler<DateTimeOffset> { }
    public class CharTypeHandler : NullableStringTypeHandler<char> { }
    public class GuidTypeHandler : NullableStringTypeHandler<Guid> { }

    // nullable string handlers
    public class StringTypeHandler : NullableStringTypeHandler<string> { }
    public class NullableDateTimeOffsetTypeHandler : NullableStringTypeHandler<DateTimeOffset?> { }
    public class NullableCharTypeHandler : NullableStringTypeHandler<char?> { }
    public class NullableGuidTypeHandler : NullableStringTypeHandler<Guid?> { }
}

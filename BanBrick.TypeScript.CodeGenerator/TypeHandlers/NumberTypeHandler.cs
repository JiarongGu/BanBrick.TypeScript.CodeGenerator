using BanBrick.TypeScript.CodeGenerator.Convertors;
using BanBrick.TypeScript.CodeGenerator.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BanBrick.TypeScript.CodeGenerator.TypeHandlers
{
    public abstract class NotNullNumberTypeHandler<TType> : ITypeHandler
    {
        public string DefaultValue => "0";
        public ProcessingCategory ProcessingCategory => ProcessingCategory.Primitive;
        public Type Type => typeof(TType);

        public virtual string GetName(INameConvertor nameConvertor) => "number";

        public virtual string GetValue(object value, IValueConvertor valueConvertor)
        {
            return value.ToString();
        }

        public virtual string GetValue(TType value, IValueConvertor valueConvertor)
        {
            return GetValue((object)value, valueConvertor);
        }
    }

    public abstract class NullableNumberTypeHandler<TType> : ITypeHandler
    {
        public string TypeName => "number";
        public string DefaultValue => null;
        public ProcessingCategory ProcessingCategory => ProcessingCategory.Primitive;
        public Type Type => typeof(TType);

        public virtual string GetName(INameConvertor nameConvertor) => "number";

        public virtual string GetValue(object value, IValueConvertor valueConvertor)
        {
            if (value == null)
                return null;
            return value.ToString();
        }

        public virtual string GetValue(TType value, IValueConvertor valueConvertor)
        {
            return GetValue((object)value, valueConvertor);
        }
    }

    // not null types

    public class ByteTypeHandler : NotNullNumberTypeHandler<byte> { };

    public class SByteTypeHandler : NotNullNumberTypeHandler<sbyte> { };

    public class UShortTypeHandler : NotNullNumberTypeHandler<ushort> { };

    public class UIntTypeHandler : NotNullNumberTypeHandler<uint> { };

    public class ULongTypeHandler : NotNullNumberTypeHandler<ulong> { };

    public class ShortTypeHandler : NotNullNumberTypeHandler<short> { };

    public class IntTypeHandler : NotNullNumberTypeHandler<int> { };

    public class LongTypeHandler : NotNullNumberTypeHandler<long> { };

    public class DecimalTypeHandler : NotNullNumberTypeHandler<decimal> { };

    public class DoubleTypeHandler : NotNullNumberTypeHandler<double> { };

    public class FloatTypeHandler : NotNullNumberTypeHandler<float> { };

    public class TimeSpanTypeHandler : NotNullNumberTypeHandler<TimeSpan>
    {
        public override string GetValue(TimeSpan value, IValueConvertor valueConvertor)
        {
            return value.TotalMilliseconds.ToString();
        }
    };

    // nullable types

    public class NullableByteTypeHandler : NullableNumberTypeHandler<byte?> { };

    public class NullableSByteTypeHandler : NullableNumberTypeHandler<sbyte?> { };

    public class NullableUShortTypeHandler : NullableNumberTypeHandler<ushort?> { };

    public class NullableUIntTypeHandler : NullableNumberTypeHandler<uint?> { };

    public class NullableULongTypeHandler : NullableNumberTypeHandler<ulong?> { };

    public class NullableShortTypeHandler : NullableNumberTypeHandler<short?> { };

    public class NullableIntTypeHandler : NullableNumberTypeHandler<int?> { };

    public class NullableLongTypeHandler : NullableNumberTypeHandler<long?> { };

    public class NullableDecimalTypeHandler : NullableNumberTypeHandler<decimal?> { };

    public class NullableDoubleTypeHandler : NullableNumberTypeHandler<double?> { };

    public class NullableFloatTypeHandler : NullableNumberTypeHandler<float?> { };

    public class NullableTimeSpanTypeHandler : NullableNumberTypeHandler<TimeSpan?>
    {
        public override string GetValue(TimeSpan? value, IValueConvertor valueConvertor)
        {
            if (value == null)
                return "null";

            return ((TimeSpan)value).TotalMilliseconds.ToString();
        }
    };
}

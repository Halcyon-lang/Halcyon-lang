using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon
{
    public enum DateTimeZoneHandling
    {
        Local,
        Utc,
        Unspecified,
        RoundtripKind
    }
    public enum DateFormatHandling
    {
        IsoDateFormat,
        MicrosoftDateFormat
    }
    public enum DateParseHandling
    {
        None,
        DateTime,
        DateTimeOffset
    }
    public enum ParserTimeZone
    {
        Unspecified,
        Utc,
        LocalWestOfUtc,
        LocalEastOfUtc
    }
    public enum ParseResult
    {
        None,
        Success,
        Overflow,
        Invalid
    }
    public enum PrimitiveTypeCode
    {
        Empty,
        Object,
        Char,
        CharNullable,
        Boolean,
        BooleanNullable,
        SByte,
        SByteNullable,
        Int16,
        Int16Nullable,
        UInt16,
        UInt16Nullable,
        Int32,
        Int32Nullable,
        Byte,
        ByteNullable,
        UInt32,
        UInt32Nullable,
        Int64,
        Int64Nullable,
        UInt64,
        UInt64Nullable,
        Single,
        SingleNullable,
        Double,
        DoubleNullable,
        DateTime,
        DateTimeNullable,
        DateTimeOffset,
        DateTimeOffsetNullable,
        Decimal,
        DecimalNullable,
        Guid,
        GuidNullable,
        TimeSpan,
        TimeSpanNullable,
        BigInteger,
        BigIntegerNullable,
        Uri,
        String,
        Bytes,
        DBNull
    }
    public enum ConvertResult
    {
        Success,
        CannotConvertNull,
        NotInstantiableType,
        NoValidConversion
    }
    public enum TryUpdateKeyResult
    {
        Success,
        NewKey,
        Failed,
        Unchanged
    }
    public enum TryGetValueResult
    {
        Success,
        Default,
        Failed
    }
}

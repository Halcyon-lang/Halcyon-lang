﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon.Utils
{
    public static class ConvertUtils
    {
        internal struct TypeConvertKey : IEquatable<ConvertUtils.TypeConvertKey>
        {
            private readonly Type _initialType;
            private readonly Type _targetType;
            public Type InitialType
            {
                get
                {
                    return this._initialType;
                }
            }
            public Type TargetType
            {
                get
                {
                    return this._targetType;
                }
            }
            public TypeConvertKey(Type initialType, Type targetType)
            {
                this._initialType = initialType;
                this._targetType = targetType;
            }
            public override int GetHashCode()
            {
                return this._initialType.GetHashCode() ^ this._targetType.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                return obj is ConvertUtils.TypeConvertKey && this.Equals((ConvertUtils.TypeConvertKey)obj);
            }
            public bool Equals(ConvertUtils.TypeConvertKey other)
            {
                return this._initialType == other._initialType && this._targetType == other._targetType;
            }
        }
        #region TypeCodeMap & PrimitiveTypeCode & CastConverters
        private static readonly Dictionary<Type, PrimitiveTypeCode> TypeCodeMap = new Dictionary<Type, PrimitiveTypeCode>
		{
			
			{
				typeof(char),
				PrimitiveTypeCode.Char
			},
			
			{
				typeof(char?),
				PrimitiveTypeCode.CharNullable
			},
			
			{
				typeof(bool),
				PrimitiveTypeCode.Boolean
			},
			
			{
				typeof(bool?),
				PrimitiveTypeCode.BooleanNullable
			},
			
			{
				typeof(sbyte),
				PrimitiveTypeCode.SByte
			},
			
			{
				typeof(sbyte?),
				PrimitiveTypeCode.SByteNullable
			},
			
			{
				typeof(short),
				PrimitiveTypeCode.Int16
			},
			
			{
				typeof(short?),
				PrimitiveTypeCode.Int16Nullable
			},
			
			{
				typeof(ushort),
				PrimitiveTypeCode.UInt16
			},
			
			{
				typeof(ushort?),
				PrimitiveTypeCode.UInt16Nullable
			},
			
			{
				typeof(int),
				PrimitiveTypeCode.Int32
			},
			
			{
				typeof(int?),
				PrimitiveTypeCode.Int32Nullable
			},
			
			{
				typeof(byte),
				PrimitiveTypeCode.Byte
			},
			
			{
				typeof(byte?),
				PrimitiveTypeCode.ByteNullable
			},
			
			{
				typeof(uint),
				PrimitiveTypeCode.UInt32
			},
			
			{
				typeof(uint?),
				PrimitiveTypeCode.UInt32Nullable
			},
			
			{
				typeof(long),
				PrimitiveTypeCode.Int64
			},
			
			{
				typeof(long?),
				PrimitiveTypeCode.Int64Nullable
			},
			
			{
				typeof(ulong),
				PrimitiveTypeCode.UInt64
			},
			
			{
				typeof(ulong?),
				PrimitiveTypeCode.UInt64Nullable
			},
			
			{
				typeof(float),
				PrimitiveTypeCode.Single
			},
			
			{
				typeof(float?),
				PrimitiveTypeCode.SingleNullable
			},
			
			{
				typeof(double),
				PrimitiveTypeCode.Double
			},
			
			{
				typeof(double?),
				PrimitiveTypeCode.DoubleNullable
			},
			
			{
				typeof(DateTime),
				PrimitiveTypeCode.DateTime
			},
			
			{
				typeof(DateTime?),
				PrimitiveTypeCode.DateTimeNullable
			},
			
			{
				typeof(DateTimeOffset),
				PrimitiveTypeCode.DateTimeOffset
			},
			
			{
				typeof(DateTimeOffset?),
				PrimitiveTypeCode.DateTimeOffsetNullable
			},
			
			{
				typeof(decimal),
				PrimitiveTypeCode.Decimal
			},
			
			{
				typeof(decimal?),
				PrimitiveTypeCode.DecimalNullable
			},
			
			{
				typeof(Guid),
				PrimitiveTypeCode.Guid
			},
			
			{
				typeof(Guid?),
				PrimitiveTypeCode.GuidNullable
			},
			
			{
				typeof(TimeSpan),
				PrimitiveTypeCode.TimeSpan
			},
			
			{
				typeof(TimeSpan?),
				PrimitiveTypeCode.TimeSpanNullable
			},
			
			{
				typeof(BigInteger),
				PrimitiveTypeCode.BigInteger
			},
			
			{
				typeof(BigInteger?),
				PrimitiveTypeCode.BigIntegerNullable
			},
			
			{
				typeof(Uri),
				PrimitiveTypeCode.Uri
			},
			
			{
				typeof(string),
				PrimitiveTypeCode.String
			},
			
			{
				typeof(byte[]),
				PrimitiveTypeCode.Bytes
			},
			
			{
				typeof(DBNull),
				PrimitiveTypeCode.DBNull
			}
		};
        private static readonly ThreadSafeStore<ConvertUtils.TypeConvertKey, Func<object, object>> CastConverters = new ThreadSafeStore<ConvertUtils.TypeConvertKey, Func<object, object>>(new Func<ConvertUtils.TypeConvertKey, Func<object, object>>(ConvertUtils.CreateCastConverter));
        private static readonly TypeInformation[] PrimitiveTypeCodes = new TypeInformation[]
		{
			new TypeInformation
			{
				Type = typeof(object),
				TypeCode = PrimitiveTypeCode.Empty
			},
			new TypeInformation
			{
				Type = typeof(object),
				TypeCode = PrimitiveTypeCode.Object
			},
			new TypeInformation
			{
				Type = typeof(object),
				TypeCode = PrimitiveTypeCode.DBNull
			},
			new TypeInformation
			{
				Type = typeof(bool),
				TypeCode = PrimitiveTypeCode.Boolean
			},
			new TypeInformation
			{
				Type = typeof(char),
				TypeCode = PrimitiveTypeCode.Char
			},
			new TypeInformation
			{
				Type = typeof(sbyte),
				TypeCode = PrimitiveTypeCode.SByte
			},
			new TypeInformation
			{
				Type = typeof(byte),
				TypeCode = PrimitiveTypeCode.Byte
			},
			new TypeInformation
			{
				Type = typeof(short),
				TypeCode = PrimitiveTypeCode.Int16
			},
			new TypeInformation
			{
				Type = typeof(ushort),
				TypeCode = PrimitiveTypeCode.UInt16
			},
			new TypeInformation
			{
				Type = typeof(int),
				TypeCode = PrimitiveTypeCode.Int32
			},
			new TypeInformation
			{
				Type = typeof(uint),
				TypeCode = PrimitiveTypeCode.UInt32
			},
			new TypeInformation
			{
				Type = typeof(long),
				TypeCode = PrimitiveTypeCode.Int64
			},
			new TypeInformation
			{
				Type = typeof(ulong),
				TypeCode = PrimitiveTypeCode.UInt64
			},
			new TypeInformation
			{
				Type = typeof(float),
				TypeCode = PrimitiveTypeCode.Single
			},
			new TypeInformation
			{
				Type = typeof(double),
				TypeCode = PrimitiveTypeCode.Double
			},
			new TypeInformation
			{
				Type = typeof(decimal),
				TypeCode = PrimitiveTypeCode.Decimal
			},
			new TypeInformation
			{
				Type = typeof(DateTime),
				TypeCode = PrimitiveTypeCode.DateTime
			},
			new TypeInformation
			{
				Type = typeof(object),
				TypeCode = PrimitiveTypeCode.Empty
			},
			new TypeInformation
			{
				Type = typeof(string),
				TypeCode = PrimitiveTypeCode.String
			}
		};
        #endregion
        public static PrimitiveTypeCode GetTypeCode(Type t)
        {
            bool flag;
            return ConvertUtils.GetTypeCode(t, out flag);
        }
        public static PrimitiveTypeCode GetTypeCode(Type t, out bool isEnum)
        {
            PrimitiveTypeCode result;
            if (ConvertUtils.TypeCodeMap.TryGetValue(t, out result))
            {
                isEnum = false;
                return result;
            }
            if (t.IsEnum == true)
            {
                isEnum = true;
                return ConvertUtils.GetTypeCode(Enum.GetUnderlyingType(t));
            }
            if (ReflectionUtils.IsNullableType(t))
            {
                Type underlyingType = Nullable.GetUnderlyingType(t);
                if (underlyingType.IsEnum)
                {
                    Type t2 = typeof(Nullable<>).MakeGenericType(new Type[]
					{
						Enum.GetUnderlyingType(underlyingType)
					});
                    isEnum = true;
                    return ConvertUtils.GetTypeCode(t2);
                }
            }
            isEnum = false;
            return PrimitiveTypeCode.Object;
        }
        public static TypeInformation GetTypeInformation(IConvertible convertable)
        {
            return ConvertUtils.PrimitiveTypeCodes[(int)convertable.GetTypeCode()];
        }
        public static bool IsConvertible(Type t)
        {
            return typeof(IConvertible).IsAssignableFrom(t);
        }
        public static TimeSpan ParseTimeSpan(string input)
        {
            return TimeSpan.Parse(input, CultureInfo.InvariantCulture);
        }
        internal static BigInteger ToBigInteger(object value)
        {
            if (value is BigInteger)
            {
                return (BigInteger)value;
            }
            if (value is string)
            {
                return BigInteger.Parse((string)value, CultureInfo.InvariantCulture);
            }
            if (value is float)
            {
                return new BigInteger((float)value);
            }
            if (value is double)
            {
                return new BigInteger((double)value);
            }
            if (value is decimal)
            {
                return new BigInteger((decimal)value);
            }
            if (value is int)
            {
                return new BigInteger((int)value);
            }
            if (value is long)
            {
                return new BigInteger((long)value);
            }
            if (value is uint)
            {
                return new BigInteger((uint)value);
            }
            if (value is ulong)
            {
                return new BigInteger((ulong)value);
            }
            if (value is byte[])
            {
                return new BigInteger((byte[])value);
            }
            throw new InvalidCastException("Cannot convert {0} to BigInteger.".FormatWith(CultureInfo.InvariantCulture, value.GetType()));
        }
        public static object FromBigInteger(BigInteger i, Type targetType)
        {
            if (targetType == typeof(decimal))
            {
                return (decimal)i;
            }
            if (targetType == typeof(double))
            {
                return (double)i;
            }
            if (targetType == typeof(float))
            {
                return (float)i;
            }
            if (targetType == typeof(ulong))
            {
                return (ulong)i;
            }
            object result;
            try
            {
                result = System.Convert.ChangeType((long)i, targetType, CultureInfo.InvariantCulture);
            }
            catch (Exception innerException)
            {
                throw new InvalidOperationException("Can not convert from BigInteger to {0}.".FormatWith(CultureInfo.InvariantCulture, targetType), innerException);
            }
            return result;
        }
        public static object Convert(object initialValue, CultureInfo culture, Type targetType)
        {
            object result;
            switch (ConvertUtils.TryConvertInternal(initialValue, culture, targetType, out result))
            {
                case ConvertResult.Success:
                    return result;
                case ConvertResult.CannotConvertNull:
                    throw new Exception("Can not convert null {0} into non-nullable {1}.".FormatWith(CultureInfo.InvariantCulture, initialValue.GetType(), targetType));
                case ConvertResult.NotInstantiableType:
                    throw new ArgumentException("Target type {0} is not a value type or a non-abstract class.".FormatWith(CultureInfo.InvariantCulture, targetType), "targetType");
                case ConvertResult.NoValidConversion:
                    throw new InvalidOperationException("Can not convert from {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, initialValue.GetType(), targetType));
                default:
                    throw new InvalidOperationException("Unexpected conversion result.");
            }
        }
        private static bool TryConvert(object initialValue, CultureInfo culture, Type targetType, out object value)
        {
            bool result;
            try
            {
                if (ConvertUtils.TryConvertInternal(initialValue, culture, targetType, out value) == ConvertResult.Success)
                {
                    result = true;
                }
                else
                {
                    value = null;
                    result = false;
                }
            }
            catch
            {
                value = null;
                result = false;
            }
            return result;
        }
        /// <summary>
        /// Not working.
        /// </summary>
        /// <param name="initialValue"></param>
        /// <param name="culture"></param>
        /// <param name="targetType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static ConvertResult TryConvertInternal(object initialValue, CultureInfo culture, Type targetType, out object value)
        {
            if (initialValue == null)
            {
                throw new ArgumentNullException("initialValue");
            }
            if (ReflectionUtils.IsNullableType(targetType))
            {
                targetType = Nullable.GetUnderlyingType(targetType);
            }
            Type type = initialValue.GetType();
            if (targetType == type)
            {
                value = initialValue;
                return ConvertResult.Success;
            }
            if (ConvertUtils.IsConvertible(initialValue.GetType()) && ConvertUtils.IsConvertible(targetType))
            {
                if (targetType.IsEnum)
                {
                    if (initialValue is string)
                    {
                        value = Enum.Parse(targetType, initialValue.ToString(), true);
                        return ConvertResult.Success;
                    }
                    if (ConvertUtils.IsInteger(initialValue))
                    {
                        value = Enum.ToObject(targetType, initialValue);
                        return ConvertResult.Success;
                    }
                }
                value = System.Convert.ChangeType(initialValue, targetType, culture);
                return ConvertResult.Success;
            }
            if (initialValue is DateTime && targetType == typeof(DateTimeOffset))
            {
                value = new DateTimeOffset((DateTime)initialValue);
                return ConvertResult.Success;
            }
            if (initialValue is byte[] && targetType == typeof(Guid))
            {
                value = new Guid((byte[])initialValue);
                return ConvertResult.Success;
            }
            if (initialValue is Guid && targetType == typeof(byte[]))
            {
                value = ((Guid)initialValue).ToByteArray();
                return ConvertResult.Success;
            }
            if (initialValue is string)
            {
                if (targetType == typeof(Guid))
                {
                    value = new Guid((string)initialValue);
                    return ConvertResult.Success;
                }
                if (targetType == typeof(Uri))
                {
                    value = new Uri((string)initialValue, UriKind.RelativeOrAbsolute);
                    return ConvertResult.Success;
                }
                if (targetType == typeof(TimeSpan))
                {
                    value = ConvertUtils.ParseTimeSpan((string)initialValue);
                    return ConvertResult.Success;
                }
                if (targetType == typeof(byte[]))
                {
                    value = System.Convert.FromBase64String((string)initialValue);
                    return ConvertResult.Success;
                }
                if (typeof(Type).IsAssignableFrom(targetType))
                {
                    value = Type.GetType((string)initialValue, true);
                    return ConvertResult.Success;
                }
            }
            if (targetType == typeof(BigInteger))
            {
                value = ConvertUtils.ToBigInteger(initialValue);
                return ConvertResult.Success;
            }
            if (initialValue is BigInteger)
            {
                value = ConvertUtils.FromBigInteger((BigInteger)initialValue, targetType);
                return ConvertResult.Success;
            }
            TypeConverter converter = ConvertUtils.GetConverter(type);
            if (converter != null && converter.CanConvertTo(targetType))
            {
                value = converter.ConvertTo(null, culture, initialValue, targetType);
                return ConvertResult.Success;
            }
            TypeConverter converter2 = ConvertUtils.GetConverter(targetType);
            if (converter2 != null && converter2.CanConvertFrom(type))
            {
                value = converter2.ConvertFrom(null, culture, initialValue);
                return ConvertResult.Success;
            }
            if (initialValue == DBNull.Value)
            {
                if (ReflectionUtils.IsNullable(targetType))
                {
                    value = ConvertUtils.EnsureTypeAssignable(null, type, targetType);
                    return ConvertResult.Success;
                }
                value = null;
                return ConvertResult.CannotConvertNull;
            }
            else
            {
                if (initialValue is INullable)
                {
                    value = ConvertUtils.EnsureTypeAssignable(ConvertUtils.ToValue((INullable)initialValue), type, targetType);
                    return ConvertResult.Success;
                }
                if (targetType.IsInterface || targetType.IsGenericTypeDefinition || targetType.IsAbstract)
                {
                    value = null;
                    return ConvertResult.NotInstantiableType;
                }
                value = null;
                return ConvertResult.NoValidConversion;
            }
        }
        public static object ConvertOrCast(object initialValue, CultureInfo culture, Type targetType)
        {
            if (targetType == typeof(object))
            {
                return initialValue;
            }
            if (initialValue == null && ReflectionUtils.IsNullable(targetType))
            {
                return null;
            }
            object result;
            if (ConvertUtils.TryConvert(initialValue, culture, targetType, out result))
            {
                return result;
            }
            return ConvertUtils.EnsureTypeAssignable(initialValue, ReflectionUtils.GetObjectType(initialValue), targetType);
        }
        private static object EnsureTypeAssignable(object value, Type initialType, Type targetType)
        {
            Type type = (value != null) ? value.GetType() : null;
            if (value != null)
            {
                if (targetType.IsAssignableFrom(type))
                {
                    return value;
                }
                Func<object, object> func = ConvertUtils.CastConverters.Get(new ConvertUtils.TypeConvertKey(type, targetType));
                if (func != null)
                {
                    return func(value);
                }
            }
            else if (ReflectionUtils.IsNullable(targetType))
            {
                return null;
            }
            throw new ArgumentException("Could not cast or convert from {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, (initialType != null) ? initialType.ToString() : "{null}", targetType));
        }
        public static object ToValue(INullable nullableValue)
        {
            if (nullableValue == null)
            {
                return null;
            }
            if (nullableValue is SqlInt32)
            {
                return ConvertUtils.ToValue((SqlInt32)nullableValue);
            }
            if (nullableValue is SqlInt64)
            {
                return ConvertUtils.ToValue((SqlInt64)nullableValue);
            }
            if (nullableValue is SqlBoolean)
            {
                return ConvertUtils.ToValue((SqlBoolean)nullableValue);
            }
            if (nullableValue is SqlString)
            {
                return ConvertUtils.ToValue((SqlString)nullableValue);
            }
            if (nullableValue is SqlDateTime)
            {
                return ConvertUtils.ToValue((SqlDateTime)nullableValue);
            }
            throw new ArgumentException("Unsupported INullable type: {0}".FormatWith(CultureInfo.InvariantCulture, nullableValue.GetType()));
        }
        /// <summary>
        /// Not working, don't use!
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        internal static TypeConverter GetConverter(Type t)
        {
            return null;
        }
        public static bool IsInteger(object value)
        {
            switch (ConvertUtils.GetTypeCode(value.GetType()))
            {
                case PrimitiveTypeCode.SByte:
                case PrimitiveTypeCode.Int16:
                case PrimitiveTypeCode.UInt16:
                case PrimitiveTypeCode.Int32:
                case PrimitiveTypeCode.Byte:
                case PrimitiveTypeCode.UInt32:
                case PrimitiveTypeCode.Int64:
                case PrimitiveTypeCode.UInt64:
                    return true;
            }
            return false;
        }
        public static ParseResult Int32TryParse(char[] chars, int start, int length, out int value)
        {
            value = 0;
            if (length == 0)
            {
                return ParseResult.Invalid;
            }
            bool flag = chars[start] == '-';
            if (flag)
            {
                if (length == 1)
                {
                    return ParseResult.Invalid;
                }
                start++;
                length--;
            }
            int num = start + length;
            for (int i = start; i < num; i++)
            {
                int num2 = (int)(chars[i] - '0');
                if (num2 < 0 || num2 > 9)
                {
                    return ParseResult.Invalid;
                }
                int num3 = 10 * value - num2;
                if (num3 > value)
                {
                    for (i++; i < num; i++)
                    {
                        num2 = (int)(chars[i] - '0');
                        if (num2 < 0 || num2 > 9)
                        {
                            return ParseResult.Invalid;
                        }
                    }
                    return ParseResult.Overflow;
                }
                value = num3;
            }
            if (!flag)
            {
                if (value == -2147483648)
                {
                    return ParseResult.Overflow;
                }
                value = -value;
            }
            return ParseResult.Success;
        }
        public static ParseResult Int64TryParse(char[] chars, int start, int length, out long value)
        {
            value = 0L;
            if (length == 0)
            {
                return ParseResult.Invalid;
            }
            bool flag = chars[start] == '-';
            if (flag)
            {
                if (length == 1)
                {
                    return ParseResult.Invalid;
                }
                start++;
                length--;
            }
            int num = start + length;
            for (int i = start; i < num; i++)
            {
                int num2 = (int)(chars[i] - '0');
                if (num2 < 0 || num2 > 9)
                {
                    return ParseResult.Invalid;
                }
                long num3 = 10L * value - (long)num2;
                if (num3 > value)
                {
                    for (i++; i < num; i++)
                    {
                        num2 = (int)(chars[i] - '0');
                        if (num2 < 0 || num2 > 9)
                        {
                            return ParseResult.Invalid;
                        }
                    }
                    return ParseResult.Overflow;
                }
                value = num3;
            }
            if (!flag)
            {
                if (value == -9223372036854775808L)
                {
                    return ParseResult.Overflow;
                }
                value = -value;
            }
            return ParseResult.Success;
        }
        public static bool TryConvertGuid(string s, out Guid g)
        {
            return Guid.TryParse(s, out g);
        }
        /// <summary>
        /// Not working, dont use!
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static Func<object, object> CreateCastConverter(ConvertUtils.TypeConvertKey t)
        {
            MethodInfo method = t.TargetType.GetMethod("op_Implicit", new Type[]
			{
				t.InitialType
			});
            if (method == null)
            {
                method = t.TargetType.GetMethod("op_Explicit", new Type[]
				{
					t.InitialType
				});
            }
            if (method == null)
            {
                return null;
            }
            return null;
        }

        public static void ToBool(string unprocessedValue)
        {
            throw new NotImplementedException();
        }
    }
}

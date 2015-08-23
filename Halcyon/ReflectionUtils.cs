﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon
{
    public static class ReflectionUtils
    {
        public static readonly Type[] EmptyTypes;
        static ReflectionUtils()
        {
            ReflectionUtils.EmptyTypes = Type.EmptyTypes;
        }
        public static bool IsVirtual(this PropertyInfo propertyInfo)
        {
            ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
            MethodInfo methodInfo = propertyInfo.GetGetMethod();
            if (methodInfo != null && methodInfo.IsVirtual)
            {
                return true;
            }
            methodInfo = propertyInfo.GetSetMethod();
            return methodInfo != null && methodInfo.IsVirtual;
        }
        public static MethodInfo GetBaseDefinition(this PropertyInfo propertyInfo)
        {
            ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
            MethodInfo methodInfo = propertyInfo.GetGetMethod();
            if (methodInfo != null)
            {
                return methodInfo.GetBaseDefinition();
            }
            methodInfo = propertyInfo.GetSetMethod();
            if (methodInfo != null)
            {
                return methodInfo.GetBaseDefinition();
            }
            return null;
        }
        public static bool IsPublic(PropertyInfo property)
        {
            return (property.GetGetMethod() != null && property.GetGetMethod().IsPublic) || (property.GetSetMethod() != null && property.GetSetMethod().IsPublic);
        }
        public static Type GetObjectType(object v)
        {
            if (v == null)
            {
                return null;
            }
            return v.GetType();
        }
        public static string GetTypeName(Type t, FormatterAssemblyStyle assemblyFormat, SerializationBinder binder)
        {
            string text2;
            if (binder != null)
            {
                string text;
                string str;
                binder.BindToName(t, out text, out str);
                text2 = str + ((text == null) ? "" : (", " + text));
            }
            else
            {
                text2 = t.AssemblyQualifiedName;
            }
            switch (assemblyFormat)
            {
                case FormatterAssemblyStyle.Simple:
                    return ReflectionUtils.RemoveAssemblyDetails(text2);
                case FormatterAssemblyStyle.Full:
                    return text2;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private static string RemoveAssemblyDetails(string fullyQualifiedTypeName)
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool flag = false;
            bool flag2 = false;
            for (int i = 0; i < fullyQualifiedTypeName.Length; i++)
            {
                char c = fullyQualifiedTypeName[i];
                char c2 = c;
                if (c2 != ',')
                {
                    switch (c2)
                    {
                        case '[':
                            flag = false;
                            flag2 = false;
                            stringBuilder.Append(c);
                            goto IL_77;
                        case ']':
                            flag = false;
                            flag2 = false;
                            stringBuilder.Append(c);
                            goto IL_77;
                    }
                    if (!flag2)
                    {
                        stringBuilder.Append(c);
                    }
                }
                else if (!flag)
                {
                    flag = true;
                    stringBuilder.Append(c);
                }
                else
                {
                    flag2 = true;
                }
            IL_77: ;
            }
            return stringBuilder.ToString();
        }
        public static bool HasDefaultConstructor(Type t, bool nonPublic)
        {
            ValidationUtils.ArgumentNotNull(t, "t");
            return t.IsValueType || ReflectionUtils.GetDefaultConstructor(t, nonPublic) != null;
        }
        public static ConstructorInfo GetDefaultConstructor(Type t)
        {
            return ReflectionUtils.GetDefaultConstructor(t, false);
        }
        public static ConstructorInfo GetDefaultConstructor(Type t, bool nonPublic)
        {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;
            if (nonPublic)
            {
                bindingFlags |= BindingFlags.NonPublic;
            }
            return t.GetConstructors(bindingFlags).SingleOrDefault((ConstructorInfo c) => !c.GetParameters().Any<ParameterInfo>());
        }
        public static bool IsNullable(Type t)
        {
            ValidationUtils.ArgumentNotNull(t, "t");
            return !t.IsValueType || ReflectionUtils.IsNullableType(t);
        }
        public static bool IsNullableType(Type t)
        {
            ValidationUtils.ArgumentNotNull(t, "t");
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
        public static Type EnsureNotNullableType(Type t)
        {
            if (!ReflectionUtils.IsNullableType(t))
            {
                return t;
            }
            return Nullable.GetUnderlyingType(t);
        }
        public static bool IsGenericDefinition(Type type, Type genericInterfaceDefinition)
        {
            if (!type.IsGenericType)
            {
                return false;
            }
            Type genericTypeDefinition = type.GetGenericTypeDefinition();
            return genericTypeDefinition == genericInterfaceDefinition;
        }
        public static bool ImplementsGenericDefinition(Type type, Type genericInterfaceDefinition)
        {
            Type type2;
            return ReflectionUtils.ImplementsGenericDefinition(type, genericInterfaceDefinition, out type2);
        }
        public static bool ImplementsGenericDefinition(Type type, Type genericInterfaceDefinition, out Type implementingType)
        {
            ValidationUtils.ArgumentNotNull(type, "type");
            ValidationUtils.ArgumentNotNull(genericInterfaceDefinition, "genericInterfaceDefinition");
            if (!genericInterfaceDefinition.IsInterface || !genericInterfaceDefinition.IsGenericTypeDefinition)
            {
                throw new ArgumentNullException("'{0}' is not a generic interface definition.".FormatWith(CultureInfo.InvariantCulture, genericInterfaceDefinition));
            }
            if (type.IsInterface && type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericInterfaceDefinition == genericTypeDefinition)
                {
                    implementingType = type;
                    return true;
                }
            }
            Type[] interfaces = type.GetInterfaces();
            for (int i = 0; i < interfaces.Length; i++)
            {
                Type type2 = interfaces[i];
                if (type2.IsGenericType)
                {
                    Type genericTypeDefinition2 = type2.GetGenericTypeDefinition();
                    if (genericInterfaceDefinition == genericTypeDefinition2)
                    {
                        implementingType = type2;
                        return true;
                    }
                }
            }
            implementingType = null;
            return false;
        }
        public static bool InheritsGenericDefinition(Type type, Type genericClassDefinition)
        {
            Type type2;
            return ReflectionUtils.InheritsGenericDefinition(type, genericClassDefinition, out type2);
        }
        public static bool InheritsGenericDefinition(Type type, Type genericClassDefinition, out Type implementingType)
        {
            ValidationUtils.ArgumentNotNull(type, "type");
            ValidationUtils.ArgumentNotNull(genericClassDefinition, "genericClassDefinition");
            if (!genericClassDefinition.IsClass || !genericClassDefinition.IsGenericTypeDefinition)
            {
                throw new ArgumentNullException("'{0}' is not a generic class definition.".FormatWith(CultureInfo.InvariantCulture, genericClassDefinition));
            }
            return ReflectionUtils.InheritsGenericDefinitionInternal(type, genericClassDefinition, out implementingType);
        }
        private static bool InheritsGenericDefinitionInternal(Type currentType, Type genericClassDefinition, out Type implementingType)
        {
            if (currentType.IsGenericType)
            {
                Type genericTypeDefinition = currentType.GetGenericTypeDefinition();
                if (genericClassDefinition == genericTypeDefinition)
                {
                    implementingType = currentType;
                    return true;
                }
            }
            if (currentType.BaseType == null)
            {
                implementingType = null;
                return false;
            }
            return ReflectionUtils.InheritsGenericDefinitionInternal(currentType.BaseType, genericClassDefinition, out implementingType);
        }
        public static Type GetCollectionItemType(Type type)
        {
            ValidationUtils.ArgumentNotNull(type, "type");
            if (type.IsArray)
            {
                return type.GetElementType();
            }
            Type type2;
            if (ReflectionUtils.ImplementsGenericDefinition(type, typeof(IEnumerable<>), out type2))
            {
                if (type2.IsGenericTypeDefinition)
                {
                    throw new Exception("Type {0} is not a collection.".FormatWith(CultureInfo.InvariantCulture, type));
                }
                return type2.GetGenericArguments()[0];
            }
            else
            {
                if (typeof(IEnumerable).IsAssignableFrom(type))
                {
                    return null;
                }
                throw new Exception("Type {0} is not a collection.".FormatWith(CultureInfo.InvariantCulture, type));
            }
        }
        public static void GetDictionaryKeyValueTypes(Type dictionaryType, out Type keyType, out Type valueType)
        {
            ValidationUtils.ArgumentNotNull(dictionaryType, "type");
            Type type;
            if (ReflectionUtils.ImplementsGenericDefinition(dictionaryType, typeof(IDictionary<,>), out type))
            {
                if (type.IsGenericTypeDefinition)
                {
                    throw new Exception("Type {0} is not a dictionary.".FormatWith(CultureInfo.InvariantCulture, dictionaryType));
                }
                Type[] genericArguments = type.GetGenericArguments();
                keyType = genericArguments[0];
                valueType = genericArguments[1];
                return;
            }
            else
            {
                if (typeof(IDictionary).IsAssignableFrom(dictionaryType))
                {
                    keyType = null;
                    valueType = null;
                    return;
                }
                throw new Exception("Type {0} is not a dictionary.".FormatWith(CultureInfo.InvariantCulture, dictionaryType));
            }
        }
        public static Type GetMemberUnderlyingType(MemberInfo member)
        {
            ValidationUtils.ArgumentNotNull(member, "member");
            MemberTypes memberTypes = member.MemberType;
            switch (memberTypes)
            {
                case MemberTypes.Event:
                    return ((EventInfo)member).EventHandlerType;
                case MemberTypes.Constructor | MemberTypes.Event:
                    break;
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                default:
                    if (memberTypes == MemberTypes.Method)
                    {
                        return ((MethodInfo)member).ReturnType;
                    }
                    if (memberTypes == MemberTypes.Property)
                    {
                        return ((PropertyInfo)member).PropertyType;
                    }
                    break;
            }
            throw new ArgumentException("MemberInfo must be of type FieldInfo, PropertyInfo, EventInfo or MethodInfo", "member");
        }
        public static bool IsIndexedProperty(MemberInfo member)
        {
            ValidationUtils.ArgumentNotNull(member, "member");
            PropertyInfo propertyInfo = member as PropertyInfo;
            return propertyInfo != null && ReflectionUtils.IsIndexedProperty(propertyInfo);
        }
        public static bool IsIndexedProperty(PropertyInfo property)
        {
            ValidationUtils.ArgumentNotNull(property, "property");
            return property.GetIndexParameters().Length > 0;
        }
        public static object GetMemberValue(MemberInfo member, object target)
        {
            ValidationUtils.ArgumentNotNull(member, "member");
            ValidationUtils.ArgumentNotNull(target, "target");
            MemberTypes memberTypes = member.MemberType;
            if (memberTypes != MemberTypes.Field)
            {
                if (memberTypes == MemberTypes.Property)
                {
                    try
                    {
                        return ((PropertyInfo)member).GetValue(target, null);
                    }
                    catch (TargetParameterCountException innerException)
                    {
                        throw new ArgumentException("MemberInfo '{0}' has index parameters".FormatWith(CultureInfo.InvariantCulture, member.Name), innerException);
                    }
                }
                throw new ArgumentException("MemberInfo '{0}' is not of type FieldInfo or PropertyInfo".FormatWith(CultureInfo.InvariantCulture, CultureInfo.InvariantCulture, member.Name), "member");
            }
            return ((FieldInfo)member).GetValue(target);
        }
        public static void SetMemberValue(MemberInfo member, object target, object value)
        {
            ValidationUtils.ArgumentNotNull(member, "member");
            ValidationUtils.ArgumentNotNull(target, "target");
            MemberTypes memberTypes = member.MemberType;
            if (memberTypes == MemberTypes.Field)
            {
                ((FieldInfo)member).SetValue(target, value);
                return;
            }
            if (memberTypes != MemberTypes.Property)
            {
                throw new ArgumentException("MemberInfo '{0}' must be of type FieldInfo or PropertyInfo".FormatWith(CultureInfo.InvariantCulture, member.Name), "member");
            }
            ((PropertyInfo)member).SetValue(target, value, null);
        }
        public static bool CanReadMemberValue(MemberInfo member, bool nonPublic)
        {
            MemberTypes memberTypes = member.MemberType;
            if (memberTypes == MemberTypes.Field)
            {
                FieldInfo fieldInfo = (FieldInfo)member;
                return nonPublic || fieldInfo.IsPublic;
            }
            if (memberTypes != MemberTypes.Property)
            {
                return false;
            }
            PropertyInfo propertyInfo = (PropertyInfo)member;
            return propertyInfo.CanRead && (nonPublic || propertyInfo.GetGetMethod(nonPublic) != null);
        }
        public static bool CanSetMemberValue(MemberInfo member, bool nonPublic, bool canSetReadOnly)
        {
            MemberTypes memberTypes = member.MemberType;
            if (memberTypes == MemberTypes.Field)
            {
                FieldInfo fieldInfo = (FieldInfo)member;
                return !fieldInfo.IsLiteral && (!fieldInfo.IsInitOnly || canSetReadOnly) && (nonPublic || fieldInfo.IsPublic);
            }
            if (memberTypes != MemberTypes.Property)
            {
                return false;
            }
            PropertyInfo propertyInfo = (PropertyInfo)member;
            return propertyInfo.CanWrite && (nonPublic || propertyInfo.GetSetMethod(nonPublic) != null);
        }
        private static bool IsOverridenGenericMember(MemberInfo memberInfo, BindingFlags bindingAttr)
        {
            if (memberInfo.MemberType != MemberTypes.Property)
            {
                return false;
            }
            PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
            if (!propertyInfo.IsVirtual())
            {
                return false;
            }
            Type declaringType = propertyInfo.DeclaringType;
            if (!declaringType.IsGenericType)
            {
                return false;
            }
            Type genericTypeDefinition = declaringType.GetGenericTypeDefinition();
            if (genericTypeDefinition == null)
            {
                return false;
            }
            MemberInfo[] member = genericTypeDefinition.GetMember(propertyInfo.Name, bindingAttr);
            if (member.Length == 0)
            {
                return false;
            }
            Type memberUnderlyingType = ReflectionUtils.GetMemberUnderlyingType(member[0]);
            return memberUnderlyingType.IsGenericParameter;
        }
        public static T GetAttribute<T>(object attributeProvider) where T : Attribute
        {
            return ReflectionUtils.GetAttribute<T>(attributeProvider, true);
        }
        public static T GetAttribute<T>(object attributeProvider, bool inherit) where T : Attribute
        {
            T[] attributes = ReflectionUtils.GetAttributes<T>(attributeProvider, inherit);
            if (attributes == null)
            {
                return default(T);
            }
            return attributes.FirstOrDefault<T>();
        }
        public static T[] GetAttributes<T>(object attributeProvider, bool inherit) where T : Attribute
        {
            Attribute[] attributes = ReflectionUtils.GetAttributes(attributeProvider, typeof(T), inherit);
            T[] array = attributes as T[];
            if (array != null)
            {
                return array;
            }
            return attributes.Cast<T>().ToArray<T>();
        }
        public static Attribute[] GetAttributes(object attributeProvider, Type attributeType, bool inherit)
        {
            ValidationUtils.ArgumentNotNull(attributeProvider, "attributeProvider");
            if (attributeProvider is Type)
            {
                Type type = (Type)attributeProvider;
                object[] source = (attributeType != null) ? type.GetCustomAttributes(attributeType, inherit) : type.GetCustomAttributes(inherit);
                return source.Cast<Attribute>().ToArray<Attribute>();
            }
            if (attributeProvider is Assembly)
            {
                Assembly element = (Assembly)attributeProvider;
                if (!(attributeType != null))
                {
                    return Attribute.GetCustomAttributes(element);
                }
                return Attribute.GetCustomAttributes(element, attributeType);
            }
            else if (attributeProvider is MemberInfo)
            {
                MemberInfo element2 = (MemberInfo)attributeProvider;
                if (!(attributeType != null))
                {
                    return Attribute.GetCustomAttributes(element2, inherit);
                }
                return Attribute.GetCustomAttributes(element2, attributeType, inherit);
            }
            else if (attributeProvider is Module)
            {
                Module element3 = (Module)attributeProvider;
                if (!(attributeType != null))
                {
                    return Attribute.GetCustomAttributes(element3, inherit);
                }
                return Attribute.GetCustomAttributes(element3, attributeType, inherit);
            }
            else
            {
                if (!(attributeProvider is ParameterInfo))
                {
                    ICustomAttributeProvider customAttributeProvider = (ICustomAttributeProvider)attributeProvider;
                    object[] array = (attributeType != null) ? customAttributeProvider.GetCustomAttributes(attributeType, inherit) : customAttributeProvider.GetCustomAttributes(inherit);
                    return (Attribute[])array;
                }
                ParameterInfo element4 = (ParameterInfo)attributeProvider;
                if (!(attributeType != null))
                {
                    return Attribute.GetCustomAttributes(element4, inherit);
                }
                return Attribute.GetCustomAttributes(element4, attributeType, inherit);
            }
        }
        public static void SplitFullyQualifiedTypeName(string fullyQualifiedTypeName, out string typeName, out string assemblyName)
        {
            int? assemblyDelimiterIndex = ReflectionUtils.GetAssemblyDelimiterIndex(fullyQualifiedTypeName);
            if (assemblyDelimiterIndex.HasValue)
            {
                typeName = fullyQualifiedTypeName.Substring(0, assemblyDelimiterIndex.Value).Trim();
                assemblyName = fullyQualifiedTypeName.Substring(assemblyDelimiterIndex.Value + 1, fullyQualifiedTypeName.Length - assemblyDelimiterIndex.Value - 1).Trim();
                return;
            }
            typeName = fullyQualifiedTypeName;
            assemblyName = null;
        }
        private static int? GetAssemblyDelimiterIndex(string fullyQualifiedTypeName)
        {
            int num = 0;
            for (int i = 0; i < fullyQualifiedTypeName.Length; i++)
            {
                char c = fullyQualifiedTypeName[i];
                char c2 = c;
                if (c2 != ',')
                {
                    switch (c2)
                    {
                        case '[':
                            num++;
                            break;
                        case ']':
                            num--;
                            break;
                    }
                }
                else if (num == 0)
                {
                    return new int?(i);
                }
            }
            return null;
        }
        public static MemberInfo GetMemberInfoFromType(Type targetType, MemberInfo memberInfo)
        {
            MemberTypes memberTypes = memberInfo.MemberType;
            if (memberTypes == MemberTypes.Property)
            {
                PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
                Type[] types = (
                    from p in propertyInfo.GetIndexParameters()
                    select p.ParameterType).ToArray<Type>();
                return targetType.GetProperty(propertyInfo.Name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, propertyInfo.PropertyType, types, null);
            }
            return targetType.GetMember(memberInfo.Name, memberInfo.MemberType, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).SingleOrDefault<MemberInfo>();
        }
        public static IEnumerable<FieldInfo> GetFields(Type targetType, BindingFlags bindingAttr)
        {
            ValidationUtils.ArgumentNotNull(targetType, "targetType");
            List<MemberInfo> list = new List<MemberInfo>(targetType.GetFields(bindingAttr));
            ReflectionUtils.GetChildPrivateFields(list, targetType, bindingAttr);
            return list.Cast<FieldInfo>();
        }
        private static void GetChildPrivateFields(List<MemberInfo> initialFields, Type targetType, BindingFlags bindingAttr)
        {
            if ((bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default)
            {
                BindingFlags bindingAttr2 = bindingAttr.RemoveFlag(BindingFlags.Public);
                while ((targetType = targetType.BaseType) != null)
                {
                    IEnumerable<MemberInfo> collection = (
                        from f in targetType.GetFields(bindingAttr2)
                        where f.IsPrivate
                        select f).Cast<MemberInfo>();
                    initialFields.AddRange(collection);
                }
            }
        }
        
        public static BindingFlags RemoveFlag(this BindingFlags bindingAttr, BindingFlags flag)
        {
            if ((bindingAttr & flag) != flag)
            {
                return bindingAttr;
            }
            return bindingAttr ^ flag;
        }
        public static bool IsMethodOverridden(Type currentType, Type methodDeclaringType, string method)
        {
            return currentType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Any((MethodInfo info) => info.Name == method && info.DeclaringType != methodDeclaringType && info.GetBaseDefinition().DeclaringType == methodDeclaringType);
        }
        public static object GetDefaultValue(Type type)
        {
            if (!type.IsValueType)
            {
                return null;
            }
            PrimitiveTypeCode typeCode = ConvertUtils.GetTypeCode(type);
            switch (typeCode)
            {
                case PrimitiveTypeCode.Char:
                case PrimitiveTypeCode.SByte:
                case PrimitiveTypeCode.Int16:
                case PrimitiveTypeCode.UInt16:
                case PrimitiveTypeCode.Int32:
                case PrimitiveTypeCode.Byte:
                case PrimitiveTypeCode.UInt32:
                    return 0;
                case PrimitiveTypeCode.CharNullable:
                case PrimitiveTypeCode.BooleanNullable:
                case PrimitiveTypeCode.SByteNullable:
                case PrimitiveTypeCode.Int16Nullable:
                case PrimitiveTypeCode.UInt16Nullable:
                case PrimitiveTypeCode.Int32Nullable:
                case PrimitiveTypeCode.ByteNullable:
                case PrimitiveTypeCode.UInt32Nullable:
                case PrimitiveTypeCode.Int64Nullable:
                case PrimitiveTypeCode.UInt64Nullable:
                case PrimitiveTypeCode.SingleNullable:
                case PrimitiveTypeCode.DoubleNullable:
                case PrimitiveTypeCode.DateTimeNullable:
                case PrimitiveTypeCode.DateTimeOffsetNullable:
                case PrimitiveTypeCode.DecimalNullable:
                    break;
                case PrimitiveTypeCode.Boolean:
                    return false;
                case PrimitiveTypeCode.Int64:
                case PrimitiveTypeCode.UInt64:
                    return 0L;
                case PrimitiveTypeCode.Single:
                    return 0f;
                case PrimitiveTypeCode.Double:
                    return 0.0;
                case PrimitiveTypeCode.DateTime:
                    return default(DateTime);
                case PrimitiveTypeCode.DateTimeOffset:
                    return default(DateTimeOffset);
                case PrimitiveTypeCode.Decimal:
                    return 0m;
                case PrimitiveTypeCode.Guid:
                    return default(Guid);
                default:
                    if (typeCode == PrimitiveTypeCode.BigInteger)
                    {
                        return default(BigInteger);
                    }
                    break;
            }
            if (ReflectionUtils.IsNullable(type))
            {
                return null;
            }
            return Activator.CreateInstance(type);
        }
    }
}

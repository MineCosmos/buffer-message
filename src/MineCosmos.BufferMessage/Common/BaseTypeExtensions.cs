using System.Collections;
using MineCosmos.BufferMessage.Enums;

namespace MineCosmos.BufferMessage.Common;

public static class BaseTypeExtensions
{
    public static bool IsValue(this Type type)
    {
        return BufferTypeMapper.IsBasicType(type);
    }

    public static bool IsArray(this Type type, out Type? innerType)
    {
        if (IsValue(type))
        {
            innerType = null;
            return false;
        }

        if (type.IsArray)
        {
            innerType = type.GetElementType();
            return innerType is not null;
        }

        if (typeof(IEnumerable).IsAssignableFrom(type) ||
            typeof(IList).IsAssignableFrom(type) ||
            typeof(ICollection).IsAssignableFrom(type))
        {
            var types = type.GetGenericArguments();
            innerType = types.Any() ? types[0] : null;

            return innerType != null;
        }

        innerType = null;
        return false;
    }

    public static bool IsString(this Type type)
    {
        return BufferTypeMapper.IsString(type);
    }

    private static Type? FormatType(this Type type)
    {
        return null;
    }
    private static BufferTypeMapper BufferTypeMapper = new();

    public static BufferType GetBufferType(this Type type)
    {
        return BufferTypeMapper.GetBufferType(type);
    }

    public static int GetBufferTypeSize(this BufferType bufferType)
    {
        return bufferType switch
        {
            BufferType.Byte => 1,
            BufferType.Short or
            BufferType.UShort or
            BufferType.Int16 or
            BufferType.UInt16 => 2,
            BufferType.Int or
            BufferType.UInt or
            BufferType.Int32 or
            BufferType.UInt32 => 4,
            BufferType.Long or
            BufferType.ULong or
            BufferType.Int64 or
            BufferType.UInt64 => 8,
            BufferType.String => -1,
            BufferType.Array => -2,
            BufferType.Object or
            _ => -3,
        };
    }
}

using System.Collections;
using MineCosmos.BufferMessage.Enums;

namespace MineCosmos.BufferMessage.Common;

public static class BaseTypeExtensions
{
    public static bool IsNullable(this Type type, out Type innerType)
    {
        var nullableType = Nullable.GetUnderlyingType(type);

        innerType = nullableType ?? type;

        return nullableType is null;
    }

    public static bool IsValue(this Type objectType)
    {
        return objectType.IsValueType &&
            objectType != typeof(DateTime) &&
            objectType != typeof(decimal) &&
            objectType != typeof(float) &&
            objectType != typeof(double);
    }

    public static bool IsString(this Type objectType)
    {
        return objectType == typeof(string) ||
                objectType == typeof(DateTime) ||
                objectType == typeof(decimal) ||
                objectType == typeof(float) ||
                objectType == typeof(double);
    }

    public static bool IsArray(this Type type, out Type innerType)
    {
        if (type.IsValueType)
        {
            innerType = type;
            return false;
        }

        if (type.IsArray)
        {
            innerType = type.GetElementType()!;
            return innerType is not null;
        }

        if (typeof(IEnumerable).IsAssignableFrom(type) ||
            typeof(IList).IsAssignableFrom(type) ||
            typeof(ICollection).IsAssignableFrom(type))
        {
            var types = type.GetGenericArguments();
            innerType = types.Any() ? types[0] : null!;

            return innerType != null;
        }

        innerType = type;
        return false;
    }

    public static T? ConvertTo<T>(this object value, T? defaultValue = default)
    {
        if (value is null)
            return defaultValue;

        try
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
        catch
        {
            if (typeof(T) == typeof(Guid))
                return (T)Convert.ChangeType(Guid.Parse(value.ToString() ?? ""), typeof(T));

            if (typeof(T) == typeof(DateTime))
                return (T)Convert.ChangeType(DateTime.Parse(value.ToString() ?? ""), typeof(T));

            if (typeof(T).IsEnum)
                return (T)Convert.ChangeType(Enum.Parse(typeof(T), value.ToString() ?? ""), typeof(T));

            return defaultValue;
        }
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
            BufferType.Number => 1,
            BufferType.String => -1,
            BufferType.Array => -2,
            BufferType.Object or
            _ => -3,
        };
    }
}

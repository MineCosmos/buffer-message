using System.Collections;
using MineCosmos.BufferMessage.Enums;

namespace MineCosmos.BufferMessage.Common;

public static class BaseTypeExtensions
{
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
            BufferType.Number => 1,
            BufferType.String => -1,
            BufferType.Array => -2,
            BufferType.Object or
            _ => -3,
        };
    }
}

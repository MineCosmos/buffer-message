using System.Reflection;
using MineCosmos.BufferMessage.Attributes;
using MineCosmos.BufferMessage.Common;

namespace MineCosmos.BufferMessage;

public static class MineCosmosConvert
{
    public static T? Deserialize<T>(byte[] data) where T : new()
    {
        return default;
    }

    public static byte[] Serialize<T>(T obj, int minBufferSize = 4096)
    {
        var props = obj?.GetType().GetProperties().Where(x => !x.GetCustomAttributes<BufferIgnoreAttribute>().Any()) ?? Enumerable.Empty<PropertyInfo>();

        foreach (var prop in props)
        {
            if (prop.PropertyType.IsValue())
            {

            }

            if (prop.PropertyType.IsString())
            {

            }

            if (prop.PropertyType.IsArray(out var innerType))
            {

            }
        }

        return Array.Empty<byte>();
    }
}

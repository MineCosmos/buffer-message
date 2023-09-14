using System.Collections;
using Microsoft.Extensions.DependencyInjection;
using MineCosmos.BufferMessage.Common;
using MineCosmos.BufferMessage.Enums;

namespace MineCosmos.BufferMessage;

internal interface IBufferMessageFactory
{

}

internal class BufferMessageFactory
{

}

public class BufferTypeFactory
{

}


public class BufferTypeMapper
{
    public readonly Dictionary<Type, BufferType> BasicBufferTypeMap;

    public BufferTypeMapper()
    {
        BasicBufferTypeMap = new()
        {
            [typeof(bool)] = BufferType.Byte,
            [typeof(byte)] = BufferType.Byte,
            [typeof(short)] = BufferType.Short,
            [typeof(ushort)] = BufferType.UShort,
            [typeof(Int16)] = BufferType.Int16,
            [typeof(UInt16)] = BufferType.UInt16,
            [typeof(int)] = BufferType.Int,
            [typeof(Int32)] = BufferType.Int32,
            [typeof(UInt32)] = BufferType.UInt32,
            [typeof(long)] = BufferType.Long,
            [typeof(ulong)] = BufferType.ULong,
            [typeof(Int64)] = BufferType.Int64,
            [typeof(UInt64)] = BufferType.UInt64,
        };
    }

    public BufferType GetBufferType(Type type)
    {
        if (BasicBufferTypeMap.TryGetValue(type, out var value))
        {
            return value;
        }

        return IsArray(type, out _) ? BufferType.Array : BufferType.Object;
    }

    public bool IsBasicType(Type type)
    {
        return BasicBufferTypeMap.ContainsKey(type);
    }

    public bool IsString(Type type)
    {
        return type == typeof(string) || type == typeof(DateTime) || type == typeof(decimal);
    }

    public bool IsArray(Type type, out Type? innerType)
    {
        if (IsBasicType(type))
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
}

internal interface IBufferMessageBuilder
{
    public IServiceProvider? Instance { get; set; }
    public IServiceCollection Services { get; }
    public void Build();
}

internal class BufferMessageBuilder : IBufferMessageBuilder
{
    public BufferMessageBuilder()
    {
        Services = new ServiceCollection();
    }
    public IServiceCollection Services { get; }
    public IServiceProvider? Instance { get; set; }

    public void Build()
    {
        Instance = Services.BuildServiceProvider();
    }
}
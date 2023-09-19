using System.Collections;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using MineCosmos.BufferMessage.Common;
using MineCosmos.BufferMessage.Enums;
using MineCosmos.Buffers;

namespace MineCosmos.BufferMessage;

internal interface IBufferMessageFactory
{

}

internal class BufferMessageFactory
{
    public static object ReadNumber(ref MineCosmosReader reader, Type objectType)
    {
        if (objectType == typeof(byte) || objectType == typeof(sbyte)) return 0 | reader.ReadByte();
        if (objectType == typeof(short)) return (short)(0 | reader.ReadUInt16());
        if (objectType == typeof(int)) return 0 | reader.ReadInt32();
        if (objectType == typeof(long)) return 0 | reader.ReadInt64();

        if (objectType == typeof(ushort)) return 0 | reader.ReadUInt16();
        if (objectType == typeof(uint)) return 0 | reader.ReadUInt32();
        if (objectType == typeof(ulong)) return 0 | reader.ReadUInt64();

        return 0;
    }

    public static object? ReadString(ref MineCosmosReader reader, Type objectType)
    {
        var length = reader.ReadInt32();
        if (objectType == typeof(string)) return reader.ReadUnicode(length);
        if (objectType == typeof(DateTime)) return reader.ReadUnicode(length).ConvertTo<DateTime>();
        if (objectType == typeof(decimal)) return reader.ReadUnicode(length).ConvertTo<decimal>(0);
        if (objectType == typeof(float)) return reader.ReadUnicode(length).ConvertTo<float>(0);
        if (objectType == typeof(double)) return reader.ReadUnicode(length).ConvertTo<double>(0);

        return null;
    }

    public static object ReadArray(ref MineCosmosReader reader, Type objectType)
    {
        var length = reader.ReadInt32();
        object?[] result = new object[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = ReadObject(ref reader, objectType);
        }

        return result;
    }

    public static object? ReadObject(ref MineCosmosReader reader, Type objectType)
    {
        var result = Activator.CreateInstance(objectType);

        foreach (var item in objectType.GetProperties())
        {
            var type = (BufferType)reader.ReadByte();
            item.PropertyType.IsNullable(out var innerType);

            if ((type & BufferType.Number) > 0)
            {
                item.SetValue(result, ReadNumber(ref reader, innerType));
            }

            if ((type & BufferType.String) > 0)
            {
                var length = reader.ReadInt32();

                item.SetValue(result, ReadString(ref reader, innerType));
            }

            if ((type & BufferType.Array) > 0)
            {
                var length = reader.ReadInt32();
                innerType.IsArray(out var arrayType);

                item.SetValue(result, ReadArray(ref reader, arrayType!));
            }

            if ((type & BufferType.Object) > 0)
            {
                item.SetValue(result, ReadObject(ref reader, innerType));
            }
        }

        return result;
    }

    public static void WriteNumber(ref MineCosmosWriter writer, Type objectType, object? value)
    {
        if (!objectType.IsValueType) return;

        if (objectType == typeof(byte) || objectType == typeof(sbyte)) writer.WriteByte(((byte)(value ?? 0)));
        if (objectType == typeof(short)) writer.WriteInt16((short)(value ?? 0));
        if (objectType == typeof(ushort)) writer.WriteUInt16((ushort)(value ?? 0));
        if (objectType == typeof(int)) writer.WriteInt32((int)(value ?? 0));
        if (objectType == typeof(uint)) writer.WriteUInt32((uint)(value ?? 0));
        if (objectType == typeof(long)) writer.WriteInt64((long)(value ?? 0));
        if (objectType == typeof(ulong)) writer.WriteUInt64((ulong)(value ?? 0));

    }

    public static void WriteString(ref MineCosmosWriter writer, Type objectType, object? value)
    {
        var val = value?.ToString() ?? "";
        var length = (byte)Encoding.BigEndianUnicode.GetBytes(val).Length;
        writer.WriteByte(length);
        if (objectType == typeof(string)) writer.WriteUniCode(val);
        if (objectType == typeof(DateTime)) writer.WriteUniCode(val);
        if (objectType == typeof(decimal)) writer.WriteUniCode(val);
        if (objectType == typeof(float)) writer.WriteUniCode(val);
        if (objectType == typeof(double)) writer.WriteUniCode(val);
    }

    public static void WriteArray(ref MineCosmosWriter writer, object? value)
    {
        if (value is Array array)
        {
            writer.WriteByte((byte)array.Length);

            foreach (var item in array)
            {
                WriteObject(ref writer, item.GetType(), item);
            }
        }
    }

    public static void WriteObject(ref MineCosmosWriter writer, Type objectType, object? value)
    {
        foreach (var item in objectType.GetProperties())
        {
            var bufferType = GetBufferType(item.PropertyType);
            item.PropertyType.IsNullable(out var innerType);

            writer.WriteByte((byte)bufferType);

            switch (bufferType)
            {
                case BufferType.Number:
                    WriteNumber(ref writer, innerType, item.GetValue(value));
                    break;
                case BufferType.String:
                    WriteString(ref writer, innerType, item.GetValue(value));
                    break;
                case BufferType.Array:
                    WriteString(ref writer, innerType, item.GetValue(value));
                    break;
                default:
                    WriteObject(ref writer, innerType, item.GetValue(value));
                    break;
            }
        }
    }

    private static BufferType GetBufferType(Type objectType)
    {
        if (objectType.IsValue()) return BufferType.Number;
        if (objectType.IsString()) return BufferType.String;
        if (objectType.IsArray(out _)) return BufferType.Array;

        return BufferType.Object;
    }
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
            // [typeof(bool)] = BufferType.Byte,
            // [typeof(byte)] = BufferType.Byte,
            // [typeof(short)] = BufferType.Short,
            // [typeof(ushort)] = BufferType.UShort,
            // [typeof(Int16)] = BufferType.Int16,
            // [typeof(UInt16)] = BufferType.UInt16,
            // [typeof(int)] = BufferType.Int,
            // [typeof(Int32)] = BufferType.Int32,
            // [typeof(UInt32)] = BufferType.UInt32,
            // [typeof(long)] = BufferType.Long,
            // [typeof(ulong)] = BufferType.ULong,
            // [typeof(Int64)] = BufferType.Int64,
            // [typeof(UInt64)] = BufferType.UInt64,
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
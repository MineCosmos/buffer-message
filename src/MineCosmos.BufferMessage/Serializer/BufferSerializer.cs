using MineCosmos.BufferMessage.Common;
using MineCosmos.BufferMessage.Enums;
using MineCosmos.Buffers;

namespace MineCosmos.BufferMessage;

public class BufferSerializer
{
    private DefaultBufferConverter _converter = new();

    public T? Deserialize<T>(byte[] buffer)
    {
        return (T?)Deserialize(typeof(T), buffer);
    }

    private object? Deserialize(Type objectType, byte[] buffer)
    {
        var _reader = new MineCosmosReader(buffer);

        var result = Activator.CreateInstance(objectType);

        return _converter.ReadBuffer(ref _reader, objectType, result, this);

        // return result;
    }

    public byte[] Serialize(object value)
    {
        // Writer
        var buffer = MineCosmosArrayPool.Rent(4096);
        try
        {
            var writer = new MineCosmosWriter(buffer);

            _converter.WriteBuffer(ref writer, value, this);

            return writer.FlushAndGetEncodingArray();
        }
        finally
        {
            MineCosmosArrayPool.Return(buffer);
        }

    }
}

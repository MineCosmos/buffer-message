using MineCosmos.BufferMessage.Common;
using MineCosmos.BufferMessage.Enums;
using MineCosmos.Buffers;

namespace MineCosmos.BufferMessage;

public class BufferSerializer
{
    private DefaultBufferConverter _converter = new();

    public object? Deserialize<T>(byte[] buffer)
    {
        return Deserialize(typeof(T), buffer);
    }

    private object? Deserialize(Type objectType, byte[] buffer)
    {
        var _reader = new MineCosmosReader(buffer);

        var result = Activator.CreateInstance(objectType);

        return _converter.ReadBuffer(ref _reader, objectType, result, this);

        // return result;
    }

    public void Serialize(object value)
    {

    }
}

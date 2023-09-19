using MineCosmos.Buffers;
using MineCosmos.BufferMessage.Common;
using MineCosmos.BufferMessage.Enums;

namespace MineCosmos.BufferMessage;

public class DefaultBufferConverter : IBufferConverter
{
    public virtual bool CanConvert(Type objectType)
    {
        return true;
    }

    public virtual object? ReadBuffer(ref MineCosmosReader reader, Type objectType, object? existingValue, BufferSerializer serializer)
    {
        return BufferMessageFactory.ReadObject(ref reader, objectType);
    }

    public virtual void WriteBuffer(ref MineCosmosWriter writer, object value, BufferSerializer serializer)
    {
        BufferMessageFactory.WriteObject(ref writer, value.GetType(), value);
    }
}

public interface IBufferConverter
{
    bool CanConvert(Type objectType);

    object? ReadBuffer(ref MineCosmosReader reader, Type objectType, object? existingValue, BufferSerializer serializer);
    void WriteBuffer(ref MineCosmosWriter writer, object value, BufferSerializer serializer);
}
using MineCosmos.Buffers;

namespace MineCosmos.BufferMessage;

public class DefaultBufferConverter : IBufferConverter
{
    public bool CanConvert(Type objectType)
    {
        return true;
    }

    public object ReadBuffer(MineCosmosReader reader, Type objectType, BufferSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public void WriteBuffer(MineCosmosWriter writer, object value, BufferSerializer serializer)
    {
        throw new NotImplementedException();
    }
}

public interface IBufferConverter
{
    bool CanConvert(Type objectType);

    object ReadBuffer(MineCosmosReader reader, Type objectType, BufferSerializer serializer);
    void WriteBuffer(MineCosmosWriter writer, object value, BufferSerializer serializer);
}
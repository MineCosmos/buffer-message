using MineCosmos.Buffers;

namespace MineCosmos.BufferMessage;

public class BufferSerializer : IDisposable
{
    public object? Deserialize<T>(MineCosmosReader reader)
    {
        return null;
    }

    public void Serialize(MineCosmosWriter writer, object value)
    {

    }

    public void Dispose()
    {

    }
}

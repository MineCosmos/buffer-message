using MineCosmos.BufferMessage.Enums;

namespace MineCosmos.BufferMessage.Core;

internal class BasicBufferPackage<T> : IBufferPackage<T>
{
    public BufferType Type { get; set; }
    public T Data { get; set; } = default!;
}

using MineCosmos.BufferMessage.Enums;

namespace MineCosmos.BufferMessage.Core;

internal interface IBufferPackage
{
    public BufferType Type { get; set; }
}

internal interface IBufferPackage<T>
{
    public BufferType Type { get; set; }
    public T Data { get; set; }
}

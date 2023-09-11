using MineCosmos.BufferMessage.Enums;

namespace MineCosmos.BufferMessage.Core;

internal class OtherBufferPackage<T> : IBufferPackage<T>
{
    public BufferType Type { get; set; }
    public byte Length { get; set; }
    public T Data { get; set; } = default!;
}
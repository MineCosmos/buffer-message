using System;
namespace MineCosmos.BufferMessage.Enums;

[Flags]
public enum BufferType
{
    Nullable = 0,
    Number = 1,
    String = 2,
    Array = 4,
    Object = 8
}

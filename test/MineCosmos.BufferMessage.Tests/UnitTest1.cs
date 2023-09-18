using MineCosmos.Buffers;

namespace MineCosmos.BufferMessage.Tests;

public class ValueTypesTests
{
    [Fact]
    public void Test1()
    {
        Assert.True(typeof(DateTime).IsValueType);
        Assert.False(typeof(string).IsValueType);
    }

    [Fact]
    public void Test2()
    {
        Assert.True(typeof(string).IsClass);
    }

    class Data
    {
        public int Arg1 { get; set; }
    }

    [Fact]
    public void Test3()
    {
        var serializer = new BufferSerializer();

        var buffer = "01 00 00 00 01".Replace(" ", "").ToHexBytes();

        var res = serializer.Deserialize<Data>(buffer);
    }
}
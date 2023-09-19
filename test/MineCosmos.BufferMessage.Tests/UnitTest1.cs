using System.Diagnostics;
using System.Text;
using MineCosmos.Buffers;
using Newtonsoft.Json;

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
        public int TestProp1 { get; set; } = 1;
    }

    [Fact]
    public void Test3()
    {
        var serializer = new BufferSerializer();

        var buffer = "01 00 00 00 01".Replace(" ", "").ToHexBytes();

        var res = serializer.Deserialize<Data>(buffer);
    }

    [Fact]
    public void Test4()
    {
        var json = JsonConvert.SerializeObject(new Data());
        var buffer = "0100000001".ToHexBytes();

        Trace.WriteLine($"json: {Encoding.Default.GetByteCount(json)} || buffer: {buffer.Length}");
    }

    [Fact]
    public void Test5()
    {
        var serializer = new BufferSerializer();
        var buffer = serializer.Serialize(new Data());

        Trace.WriteLine($"buffer: {buffer.ToHexString()}");
    }
}
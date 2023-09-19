using System.Diagnostics;
using System.Text;
using MineCosmos.Buffers;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace MineCosmos.BufferMessage.Tests;

public class ValueTypesTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ValueTypesTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

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

    class Student
    {
        public string Name { get; set; } = "Jane";
        public int Age { get; set; } = 25;
        public DateTime Born { get; set; } = DateTime.Now;
        public string Address { get; set; } = "八百标兵奔北坡，炮兵并排北边跑。炮兵怕把标兵碰，标兵怕碰炮兵炮。八了百了标了兵了奔了北了坡，炮了兵了并了排了北了边了跑。炮了兵了怕了把了标了兵了碰，标了兵了怕了碰了炮了兵了炮。";
        public Gender Gender { get; set; } = Gender.男;
    }

    enum Gender { 男, 女 }

    [Fact]
    public void Test5()
    {
        var json = JsonConvert.SerializeObject(new Student());

        var serializer = new BufferSerializer();
        var buffer = serializer.Serialize(new Student());

        Trace.WriteLine($"buffer-test: json: {Encoding.Unicode.GetByteCount(json)} || buffer: {buffer.Length} || 压缩比：{Encoding.Unicode.GetByteCount(json) / (float)buffer.Length:0.00}:1");
        Trace.WriteLine($"buffer-test: json: {json}");
        Trace.WriteLine($"buffer-test: buffer: {buffer.ToHexString()}");

        var student = serializer.Deserialize<Student>(buffer);
        Trace.WriteLine($"buffer-test: Name: {student?.Name}");
        Trace.WriteLine($"buffer-test: Age: {student?.Age}");
        Trace.WriteLine($"buffer-test: Age: {student?.Born}");
        Trace.WriteLine($"buffer-test: Address: {student?.Address}");
        Trace.WriteLine($"buffer-test: Address: {student?.Gender.ToString()}");
    }
}
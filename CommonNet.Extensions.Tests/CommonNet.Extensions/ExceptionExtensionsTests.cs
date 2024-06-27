using FluentAssertions;
using Xunit;

namespace CommonNet.Extensions.Tests;

public class ExceptionExtensionsTests
{
    [Fact]
    public void AddData_ShouldAddSingleValue_WhenCalledWithOneValue()
    {
        var exception = new Exception();
        var key = "TestKey";
        var value = "TestValue";

        exception.AddData(key, value);

        var checkDict = new CastedStringKeyDictionary(exception.Data);
        checkDict.Should().ContainKey(key).WhoseValue.Should().Be(value);
    }

    [Fact]
    public void AddData_ShouldAddArrayOfValues_WhenCalledWithMultipleValues()
    {
        var exception = new Exception();
        var key = "TestKey";
        var values = new object[] { "Value1", "Value2" };

        exception.AddData(key, values);

        var checkDict = new CastedStringKeyDictionary(exception.Data);
        checkDict.Should().ContainKey(key).WhoseValue.Should().BeEquivalentTo(values);
    }

    [Fact]
    public void AddData_ShouldAddNull_WhenCalledWithNoValues()
    {
        var exception = new Exception();
        var key = "TestKey";

        exception.AddData(key);

        var checkDict = new CastedStringKeyDictionary(exception.Data);
        checkDict.Should().ContainKey(key).WhoseValue.Should().BeNull();
    }

    [Fact]
    public void AddData_ShouldThrowArgumentNullException_WhenExceptionIsNull()
    {
        Exception exception = null!;
        var key = "TestKey";

        Action act = () => exception!.AddData(key);

        act.Should().Throw<ArgumentNullException>().WithParameterName("exception");
    }

    [Fact]
    public void AddData_ShouldThrowArgumentException_WhenKeyIsEmpty()
    {
        var exception = new Exception();
        var key = "";

        Action act = () => exception.AddData(key);

        act.Should().Throw<ArgumentException>().WithParameterName("key");
    }
}

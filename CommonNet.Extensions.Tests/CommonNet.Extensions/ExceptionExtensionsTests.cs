using System.Linq;

namespace CommonNet.Extensions.Tests;

public class ExceptionExtensionsTests
{
    [Test]
    public async Task AddData_ShouldAddSingleValue_WhenCalledWithOneValue()
    {
        var exception = new Exception();
        var key = "TestKey";
        var value = "TestValue";

        exception.AddData(key, value);

        await Assert.That(exception.Data.Contains(key))
            .IsTrue();
        await Assert.That(exception.Data[key])
            .IsEqualTo(value);
    }

    [Test]
    public async Task AddData_ShouldAddArrayOfValues_WhenCalledWithMultipleValues()
    {
        var exception = new Exception();
        var key = "TestKey";
        var values = new object[] { "Value1", "Value2" };

        exception.AddData(key, values);

        await Assert.That(exception.Data.Contains(key))
            .IsTrue();
        await Assert.That(exception.Data[key])
            .IsEquivalentTo(values);
    }

    [Test]
    public async Task AddData_ShouldAddNull_WhenCalledWithNoValues()
    {
        var exception = new Exception();
        var key = "TestKey";

        exception.AddData(key);

        await Assert.That(exception.Data.Keys.OfType<object>())
            .Contains(obj => obj is string str && str == key);
        await Assert.That(exception.Data[key])
            .IsNull();
    }

    [Test]
    public async Task AddData_ShouldThrowArgumentNullException_WhenExceptionIsNull()
    {
        Exception exception = null!;
        var key = "TestKey";

        await Assert.That(() => exception!.AddData(key))
            .ThrowsExactly<ArgumentNullException>()
            .WithParameterName("exception");
    }

    [Test]
    public async Task AddData_ShouldThrowArgumentException_WhenKeyIsEmpty()
    {
        var exception = new Exception();
        var key = "";

        await Assert.That(() => exception!.AddData(key))
            .ThrowsExactly<ArgumentException>()
            .WithParameterName("key");
    }
}

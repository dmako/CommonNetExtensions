using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using FluentAssertions;
using Moq;
using Xunit;

namespace CommonNet.Extensions.Tests;

public class EmbeddedResourceStreamExtensionsTests
{

    private readonly IReadOnlyDictionary<string, string> TestData = new Dictionary<string, string>()
    {
        ["TestNamespace.TestTextResource.txt"] = "Some Test Data in UTF-8!",
        ["OtherTestNamespace.OtherEmptyResource.txt"] = "",
        ["TestNamespace.TestTextLinesResource.txt"] = "line1\nline2\nline3"
    };

    private readonly Assembly _mockAssembly;

    public EmbeddedResourceStreamExtensionsTests()
    {
        _mockAssembly = PrepareTestAssembly();
    }

#if NET6_0_OR_GREATER

    private Assembly PrepareTestAssembly()
    {
        var mockAssembly = new Mock<Assembly>();
        mockAssembly.Setup(a => a.GetManifestResourceNames()).Returns(TestData.Keys.ToArray());
        foreach (var key in TestData.Keys)
        {
            mockAssembly.Setup(a => a.GetManifestResourceStream(key)).Returns(new MemoryStream(Encoding.UTF8.GetBytes(TestData[key])));
        }
        return mockAssembly.Object;
    }

#endif

#if NET48

    private Assembly PrepareTestAssembly()
    {
        // .Net Framework 4.8 hack
        // Moq cannot be used, creating a dynamic assembly with the streams
        // Note that the generated dlls remain in output directory

        var guid = Guid.NewGuid();
        var assemblyName = new AssemblyName($"TestDynamicAssembly_{guid}");

        // Create the dynamic assembly in memory
        var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(
            assemblyName,
            AssemblyBuilderAccess.RunAndSave
        );

        var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name, $"{assemblyName.Name}.dll");

        foreach (var key in TestData.Keys)
        {
            moduleBuilder.DefineManifestResource(key, new MemoryStream(Encoding.UTF8.GetBytes(TestData[key])), ResourceAttributes.Public);
        }

        assemblyBuilder.Save($"{assemblyName.Name}.dll");
        return Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, $"{assemblyName.Name}.dll"));
    }

#endif


    [Fact]
    public void GetEmbeddedResourceStream_ShouldReturnStream_WhenResourceExists()
    {
        var stream = _mockAssembly.GetEmbeddedResourceStream("TestTextResource.txt");
        stream.Should().NotBeNull();
    }

    [Fact]
    public void GetEmbeddedResourceStream_ShouldThrowException_WhenResourceDoesNotExist()
    {
        var fnc = () => _mockAssembly.GetEmbeddedResourceStream("NonExistentResource.txt");
        fnc.Should().ThrowExactly<InvalidOperationException>();
    }

    [Fact]
    public void ReadEmbeddedResourceData_ShouldReturnData_WhenResourceExists()
    {
        var testData = Encoding.UTF8.GetBytes(TestData["TestNamespace.TestTextResource.txt"]);
        var result = _mockAssembly.ReadEmbeddedResourceData("TestTextResource.txt");

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(testData);
    }

    [Fact]
    public void ReadEmbeddedResourceLines_ShouldReturnLines_WhenResourceExists()
    {
        var expectedLines = TestData["TestNamespace.TestTextLinesResource.txt"].Split("\n".ToCharArray());
        var result = _mockAssembly.ReadEmbeddedResourceLines("TestTextLinesResource.txt").ToArray();

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedLines);
    }

    [Fact]
    public void ReadEmbeddedResourceLinesAsync_ShouldReturnLines_WhenResourceExists()
    {
        var expectedLines = TestData["TestNamespace.TestTextLinesResource.txt"].Split("\n".ToCharArray());
        var result = _mockAssembly.ReadEmbeddedResourceLinesAsync("TestTextLinesResource.txt").ToBlockingEnumerable().ToArray();

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedLines);
    }
}

using System;
using FluentAssertions;
using FsCheck.Xunit;
using Xunit;

namespace CommonNet.Extensions.Tests;

public class TextReaderExtensions
{
    [Fact]
    public void TextReader_BasicTests()
    {
        const StreamReader? nullReader = null;
        Action action = () => nullReader!.ForEachLine(l => { });
        action.Should().ThrowExactly<ArgumentNullException>();

        using var reader = new StreamReader(new MemoryStream());
        action = () => reader.ForEachLine(null!);
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Property(MaxTest = 100, Arbitrary = new[] { typeof(NonNullNoCrLfStringArbitrary) }, DisplayName = nameof(TextReader_PropertyEnumLines), QuietOnSuccess = true)]
    public void TextReader_PropertyEnumLines(string[] data)
    {
        var i = 0;
        var len = data.Length > 0 && data[data.Length - 1].Length == 0 ? data.Length - 1 : data.Length;
        using var sr = new StringReader(string.Join("\n", data));
        foreach (var line in sr.EnumLines())
        {
            line.Should().Be(data[i]);
            i++;
        }
        i.Should().Be(len);
    }

    [Property(MaxTest = 100, Arbitrary = new[] { typeof(NonNullNoCrLfStringArbitrary) }, DisplayName = nameof(TextReader_ForEachLine), QuietOnSuccess = true)]
    public void TextReader_ForEachLine(string[] data)
    {
        var i = 0;
        var idx = data.Length - 1;
        var len = data.Length > 0 && data[idx].Length == 0 ? data.Length - 1 : data.Length;
        using var sr = new StringReader(string.Join("\n", data));
        sr.ForEachLine(line =>
        {
            line.Should().Be(data[i]);
            i++;
        });
        i.Should().Be(len);
    }
}


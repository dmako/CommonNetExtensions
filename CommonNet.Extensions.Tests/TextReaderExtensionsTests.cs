using FluentAssertions;
using FsCheck.Xunit;
using Xunit;

namespace CommonNet.Extensions.Tests;

public class TextReaderExtensionsTests
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

    [Fact]
    public async Task EnumLinesAsync_ShouldThrow_WhenNullTextReaderIsGiven()
    {
        TextReader reader = null!;
        var enumerator = reader.EnumLinesAsync().GetAsyncEnumerator();
        var func = async () => await enumerator.MoveNextAsync(); ;
        await func.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task EnumLinesAsync_ShouldEnumerateLines()
    {
        var textToRead = "Line 1\nLine 2\nLine 3\n";
        using var reader = new StringReader(textToRead);

        var result = new List<string>();
        await foreach (var line in reader.EnumLinesAsync())
        {
            result.Add(line);
        }
        var expectedLines = textToRead.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        result.Should().BeEquivalentTo(expectedLines);
    }

    [Fact]
    public async Task EnumLinesAsync_ShouldReturnEmptyEnumerableForEmptyReader()
    {
        var reader = new StringReader(string.Empty);
        var result = new List<string>();
        await foreach (var line in reader.EnumLinesAsync())
        {
            result.Add(line);
        }
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task EnumLinesAsync_ShouldStopEnumerationOnCancellation()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var textToRead = "Line 1\nLine 2\nLine 3\n";
        using var reader = new StringReader(textToRead);

        var enumerator = reader.EnumLinesAsync(cancellationTokenSource.Token).GetAsyncEnumerator();

        var moveNextResult = await enumerator.MoveNextAsync();
        moveNextResult.Should().BeTrue();

        cancellationTokenSource.Cancel();

        var func = async () => await enumerator.MoveNextAsync();
        await func.Should().ThrowAsync<OperationCanceledException>();
    }
}


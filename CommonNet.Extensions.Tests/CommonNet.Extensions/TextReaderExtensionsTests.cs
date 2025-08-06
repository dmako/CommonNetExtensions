using TUnit.Assertions.AssertConditions.Throws;

namespace CommonNet.Extensions.Tests;

public class TextReaderExtensionsTests
{
    [Test]
    public async Task TextReader_BasicTests()
    {
        const StreamReader? nullReader = null;
        await Assert.That(() => nullReader!.ForEachLine(l => { }))
            .ThrowsExactly<ArgumentNullException>();

        using var reader = new StreamReader(new MemoryStream());
        await Assert.That(() => reader.ForEachLine(null!))
            .ThrowsExactly<ArgumentNullException>();
    }

    [Test]
    [TextBlockGenerator(100)]
    [ArgumentDisplayFormatter<TextBlockFormatter>]
    public async Task EnumLines_ShouldEnumerateLines_PropertyBased(string[] data)
    {
        var i = 0;
        var len = data.Length > 0 && data[^1].Length == 0 ? data.Length - 1 : data.Length;
        using var sr = new StringReader(string.Join("\n", data));
        foreach (var line in sr.EnumLines())
        {
            await Assert.That(line)
                .IsEqualTo(data[i]);
            i++;
        }
        await Assert.That(i)
            .IsEqualTo(len);
    }

    [Test]
    public async Task EnumLinesAsync_ShouldThrow_WhenNullTextReaderIsGiven()
    {
        TextReader reader = null!;
        var enumerator = reader.EnumLinesAsync().GetAsyncEnumerator();
        await Assert.That(async () => await enumerator.MoveNextAsync())
            .ThrowsExactly<ArgumentNullException>();
    }

    [Test]
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
        await Assert.That(result).IsEquivalentTo(expectedLines);
    }

    [Test]
    [TextBlockGenerator(100)]
    [ArgumentDisplayFormatter<TextBlockFormatter>]
    public async Task EnumLinesAsync_ShouldEnumerateLines_PropertyBased(string[] data)
    {
        var i = 0;
        var len = data.Length > 0 && data[^1].Length == 0 ? data.Length - 1 : data.Length;
        using var reader = new StringReader(string.Join("\n", data));
        foreach (var line in reader.EnumLinesAsync().ToBlockingEnumerable())
        {
            await Assert.That(line)
                .IsEqualTo(data[i]);
            i++;
        }
        await Assert.That(i)
            .IsEqualTo(len);
    }

    [Test]
    public async Task EnumLinesAsync_ShouldReturnEmptyEnumerableForEmptyReader()
    {
        using var reader = new StringReader(string.Empty);
        var result = new List<string>();
        await foreach (var line in reader.EnumLinesAsync())
        {
            result.Add(line);
        }
        await Assert.That(result)
            .IsEmpty();
    }

    [Test]
    public async Task EnumLinesAsync_ShouldStopEnumerationOnCancellation()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var textToRead = "Line 1\nLine 2\nLine 3\n";
        using var reader = new StringReader(textToRead);

        var enumerator = reader.EnumLinesAsync(cancellationTokenSource.Token).GetAsyncEnumerator();

        var moveNextResult = await enumerator.MoveNextAsync();
        await Assert.That(moveNextResult)
            .IsTrue();

        cancellationTokenSource.Cancel();

        await Assert.That(async () => await enumerator.MoveNextAsync())
            .Throws<OperationCanceledException>();
    }

    [Test]
    public async Task ForeachLine_ShouldPerformActionForEachLine()
    {
        var textToRead = "Line 1\nLine 2\nLine 3\n";
        using var reader = new StringReader(textToRead);

        var result = new List<string>();
        reader.ForEachLine(result.Add);
        var expectedLines = textToRead.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        await Assert.That(result)
            .IsEquivalentTo(expectedLines);
    }
}


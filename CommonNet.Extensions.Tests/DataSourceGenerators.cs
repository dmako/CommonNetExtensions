namespace CommonNet.Extensions.Tests;

public class StringGeneratorAttribute
    : DataSourceGeneratorAttribute<string>
{
    private const string Alphabet = "abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789     ";
    private readonly int _itemsCount;
    private readonly int _minLineLen;
    private readonly int _maxLineLen;

    public StringGeneratorAttribute(int ItemsCount = 100, bool AllowEmpty = true, int MaxLen = 100)
        : base()
    {
        _itemsCount = Math.Max(1, ItemsCount);
        _minLineLen = AllowEmpty ? 0 : 1;
        _maxLineLen = Math.Max(_minLineLen, MaxLen);
    }

    protected override IEnumerable<Func<string>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        var rng = new Random(dataGeneratorMetadata.TestClassInstance?.GetHashCode() ?? GetHashCode());

        for (var i = 0; i < _itemsCount; ++i)
        {
            yield return () => GenerateString(  rng);
        }
    }

    public string GenerateString(Random rng)
    {
        var size = rng.Next(_minLineLen, _maxLineLen);

        var chars = new char[size];
        for (var i = 0; i < size; i++)
        {
            chars[i] = Alphabet[rng.Next(Alphabet.Length)];
        }
        return new string(chars);
    }
}

public class TextBlockGeneratorAttribute
    : DataSourceGeneratorAttribute<string[]>
{
    private readonly int _itemsCount;
    private readonly int _minLines;
    private readonly int _maxLines;
    private readonly StringGeneratorAttribute _stringGenerator;

    public TextBlockGeneratorAttribute(int ItemsCount = 100, bool AllowEmptyLines = true, int MinLines = 1, int MaxLines = 100, int MaxLineLen = 100)
    {
        _itemsCount = Math.Max(1, ItemsCount);
        _minLines = Math.Max(0, MinLines);
        _maxLines = Math.Max(_minLines, MaxLines);
        _stringGenerator = new StringGeneratorAttribute(1, AllowEmptyLines, MaxLineLen);
    }

    protected override IEnumerable<Func<string[]>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        var rng = new Random(dataGeneratorMetadata.TestClassInstance?.GetHashCode() ?? GetHashCode());

        for (var i = 0; i < _itemsCount; ++i)
        {
            yield return () => GenerateTextArray(rng);
        }
    }

    private string[] GenerateTextArray(Random rng)
    {
        var linesCount = rng.Next(_minLines, _maxLines);
        var lines = new string[linesCount];

        for (var l = 0; l < linesCount; ++l)
        {
            lines[l] = _stringGenerator.GenerateString(rng);
        }
        return lines;
    }
}

public abstract class ArrayGeneratorAttribute<T>
    : DataSourceGeneratorAttribute<T[]>
{
    protected readonly int _itemsCount;
    protected readonly int _minLen;
    protected readonly int _maxLen;

    public ArrayGeneratorAttribute(int ItemsCount, bool AllowEmpty, int MaxLen)
        : base()
    {
        _itemsCount = Math.Max(1, ItemsCount);
        _minLen = AllowEmpty ? 0 : 1;
        _maxLen = Math.Max(_minLen, MaxLen);
    }

    protected override IEnumerable<Func<T[]>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        var rng = new Random(dataGeneratorMetadata.TestClassInstance?.GetHashCode() ?? GetHashCode());

        for (var i = 0; i < _itemsCount; ++i)
        {
            yield return () => GenerateArray(rng);
        }
    }

    public abstract T[] GenerateArray(Random rng);
}

public class IntArrayGeneratorAttribute
    : ArrayGeneratorAttribute<int>
{
    public IntArrayGeneratorAttribute(int ItemsCount = 100, bool AllowEmpty = true, int MaxLen = 100)
        : base(ItemsCount, AllowEmpty, MaxLen)
    {
    }

    public override int[] GenerateArray(Random rng)
    {
        var size = rng.Next(_minLen, _maxLen);
        var array = new int[size];
        for (var i = 0; i < size; ++i)
        {
            array[i] = rng.Next();
        }
        return array;
    }
}

public class ByteArrayGeneratorAttribute
    : ArrayGeneratorAttribute<byte>
{
    public ByteArrayGeneratorAttribute(int ItemsCount = 100, bool AllowEmpty = true, int MaxLen = 100)
        : base(ItemsCount, AllowEmpty, MaxLen)
    {
    }

    public override byte[] GenerateArray(Random rng)
    {
        var size = rng.Next(_minLen, _maxLen);
        var array = new byte[size];
        rng.NextBytes(array);
        return array;
    }
}


public record ByteArrayPair(byte[] First, byte[] Second);

public class NonEmptyByteArrayPairGeneratorAttribute
    : DataSourceGeneratorAttribute<ByteArrayPair>
{
    protected readonly int _itemsCount;
    private readonly ByteArrayGeneratorAttribute _byteArrayGenerator;

    public NonEmptyByteArrayPairGeneratorAttribute(int ItemsCount = 100, int MaxLen = 100)
        : base()
    {
        _itemsCount = Math.Max(1, ItemsCount);
        _byteArrayGenerator = new ByteArrayGeneratorAttribute(1, false, MaxLen);
    }

    protected override IEnumerable<Func<ByteArrayPair>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        var rng = new Random(dataGeneratorMetadata.TestClassInstance?.GetHashCode() ?? GetHashCode());

        for (var i = 0; i < _itemsCount; ++i)
        {
            yield return () => new(_byteArrayGenerator.GenerateArray(rng), _byteArrayGenerator.GenerateArray(rng));
        }
    }
}




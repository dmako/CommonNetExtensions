namespace CommonNet.Extensions.Tests;

public class TextBlockFormatter : ArgumentDisplayFormatter
{
    public override bool CanHandle(object? value)
    {
        return value is string[];
    }

    public override string FormatValue(object? value)
    {
        if (value is string[] textArray)
        {
            return $"\"{string.Join(Environment.NewLine, textArray)}\"";
        }
        return "Invalid input type";
    }
}

public class EnumerableFormatter : ArgumentDisplayFormatter
{
    public override bool CanHandle(object? value)
    {
        return value is System.Collections.IEnumerable;
    }

    public override string FormatValue(object? value)
    {
        if (value is System.Collections.IEnumerable enumerable)
        {
            return $"[{string.Join(",", enumerable.Cast<object>().Select(o => o?.ToString() ?? "null"))}]";
        }
        return "Invalid input type";
    }
}

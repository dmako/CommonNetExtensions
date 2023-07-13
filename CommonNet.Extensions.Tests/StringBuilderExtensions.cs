using System.Text;
using FluentAssertions;
using Xunit;

namespace CommonNet.Extensions.Tests;

public class StringBuilderExtensions
{
    [Fact]
    public void StringBuilde_AppendIf()
    {
        const StringBuilder? nullSb = null;

        Action action = () => nullSb!.AppendIf(1 > 0, "null");
        action.Should().ThrowExactly<ArgumentNullException>();

        var sb = new StringBuilder();
        var type = typeof(StringBuilderExtensions);

        sb.AppendIf(type.IsNotPublic, "internal")
            .AppendIf(type.IsPublic, "public")
            .Append(' ')
            .AppendIf(type.IsClass, "class")
            .AppendIf(type.IsByRef, "struct")
            .Append(' ')
            .Append(type.Name);

        Assert.Equal($"public class {nameof(StringBuilderExtensions)}", sb.ToString());
    }
}

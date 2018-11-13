namespace CommonNet.Extensions.Tests
{
    using System;
    using System.Text;
    using Xunit;

    public class StringBuilderExtensions
    {
        [Fact]
        public void StringBuilde_AppendIf()
        {
            const StringBuilder nullSb = null;
            Assert.Throws<ArgumentNullException>(() => nullSb.AppendIf(1 > 0, "null"));

            var sb = new StringBuilder();
            var type = typeof(StringBuilderExtensions);

            sb.AppendIf(type.IsNotPublic, "private")
                .AppendIf(type.IsPublic, "public")
                .Append(' ')
                .AppendIf(type.IsClass, "class")
                .AppendIf(type.IsByRef, "struct")
                .Append(' ')
                .Append(type.Name);

            Assert.Equal($"public class {nameof(StringBuilderExtensions)}", sb.ToString());
        }
    }
}

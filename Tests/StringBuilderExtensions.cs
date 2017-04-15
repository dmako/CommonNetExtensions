namespace Tests
{
    using NUnit.Framework;
    using System.Text;
    using System;

    [TestFixture]
    public class StringBuilderExtensions
    {
        [Test]
        public void StringBuilde_AppendIf()
        {
            StringBuilder nullSb = null;
            Assert.Catch<ArgumentException>(() => nullSb.AppendIf(1 > 0, "null"));

            var sb = new StringBuilder();
            var type = typeof(StringBuilderExtensions);

            sb.AppendIf(type.IsNotPublic, "private")
                .AppendIf(type.IsPublic, "public")
                .Append(' ')
                .AppendIf(type.IsClass, "class")
                .AppendIf(type.IsByRef, "struct")
                .Append(' ')
                .Append(type.Name);

            Assert.AreEqual(sb.ToString(), $"public class {nameof(StringBuilderExtensions)}");
        }

    }
}

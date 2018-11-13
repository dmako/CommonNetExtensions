namespace CommonNet.Extensions.Tests
{
    using FsCheck;

    public static class NonNullNoCrlLfStringArbitrary
    {
        public static Arbitrary<string> Strings()
        {
            return Arb.Default.String().Filter(x =>
                    x != null &&
                    !x.Contains("\n") &&
                    !x.Contains("\r")
                );
        }
    }

    public static class NonEmptyArrayArbitrary
    {
        public static Arbitrary<T[]> Array<T>()
        {
            return Arb.Default.Array<T>().Filter(x => x != null && x.Length > 0);
        }
    }
}

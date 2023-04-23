using FsCheck;

namespace CommonNet.Extensions.Tests;

public static class NonNullNoCrLfStringArbitrary
{
    public static Arbitrary<string> Strings()
    {
        return Arb.Default.String().Filter(x =>
                x != null &&
                !x.Contains('\n') &&
                !x.Contains('\r')
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

using FsCheck;

namespace Tests
{
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
}

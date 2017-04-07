namespace System.IO
{
    using Collections.Generic;
    using CommonNet.Extensions;
    using ComponentModel;

    /// <summary>
    /// Commonly used extension methods on <see cref="TextReader"/>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class CommonNetTextReaderExtensions
    {
        /// <summary>
        /// Iterates over lines by using <see cref="TextReader"/> instance without writing while cycle code.
        /// </summary>
        /// <param name="self"><see cref="TextReader"/> instance to use for retrieving lines.</param>
        /// <returns><see cref="IEnumerable{String}"/> yielded lines.</returns>
        public static IEnumerable<string> EnumLines(
            this TextReader self
        )
        {
            Check.Self(self);

            string line = null;
            while ((line = self.ReadLine()) != null)
                yield return line;
        }

        /// <summary>
        /// Performs <see cref="Action{String}"/> on every line while iterating over lines by using <see cref="TextReader"/> instance.
        /// </summary>
        /// <param name="self"><see cref="TextReader"/> instance to use for retrieving lines.</param>
        /// <param name="action"><see cref="Action{String}"/> to be performed.</param>
        public static void ForEachLine(
            this TextReader self,
            Action<string> action
        )
        {
            Check.Self(self);
            Check.Argument(action, nameof(action));

            self.EnumLines().ForEach(line => action(line));
        }

    }
}

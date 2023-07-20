using FluentAssertions;
using Moq;
using Xunit;

namespace CommonNet.Extensions.Tests;

public class AsyncEnumerableExtensionsTests
{

#if !NET7_0_OR_GREATER

    // Helper class for creating a mock
    private class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private IEnumerator<T> _enumerator;

        public TestAsyncEnumerator(IEnumerator<T> enumerator)
        {
            _enumerator = enumerator;
        }

        public T Current => _enumerator.Current;

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(_enumerator.MoveNext());
        }

        public ValueTask DisposeAsync()
        {
            _enumerator.Dispose();
            return new ValueTask();
        }
    }

    [Fact]
    public void ToBlockingEnumerable_ShouldEnumerateAsyncEnumerable()
    {
        var sourceList = new List<int> { 1, 2, 3, 4 };
        var asyncEnumMock = new Mock<IAsyncEnumerable<int>>();
        asyncEnumMock
            .Setup(e => e.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(() => new TestAsyncEnumerator<int>(sourceList.GetEnumerator()));

        var blockingEnumerable = asyncEnumMock.Object.ToBlockingEnumerable().ToList();
        blockingEnumerable.Should().BeEquivalentTo(sourceList);
    }

#endif

}

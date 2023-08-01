using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CommonNet.Logging.Tests;

public class LoggerSuppressingExtensionsTests
{
    [Fact]
    public void Swallow_Action_NoException_LogsNothing()
    {
        // Arrange
        var loggerMock = new Mock<ILogger>();
        var actionExecuted = false;

        // Act
        loggerMock.Object.Swallow(() => actionExecuted = true);

        // Assert
        actionExecuted.Should().BeTrue();
        loggerMock.Verify(
            x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()
            ),
            Times.Never
        );
    }

    [Fact]
    public void Swallow_Action_Exception_LogsError()
    {
        // Arrange
        var loggerMock = new Mock<ILogger>();
        var actionThrows = true;

        // Act
        loggerMock.Object.Swallow(() => LoggerSuppressingExtensionsTests.ThrowTestException(actionThrows));

        // Assert
        loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()
            ),
            Times.Once
        );
    }

    [Fact]
    public async Task SwallowAsync_Action_NoException_LogsNothing()
    {
        // Arrange
        var loggerMock = new Mock<ILogger>();
        var actionExecuted = false;

        // Act
        await loggerMock.Object.SwallowAsync(async () => await Task.Run(() => actionExecuted = true));

        // Assert
        actionExecuted.Should().BeTrue();
        loggerMock.Verify(
            x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()
            ),
            Times.Never
        );
    }

    [Fact]
    public async Task SwallowAsync_Action_Exception_LogsError()
    {
        // Arrange
        var loggerMock = new Mock<ILogger>();
        var actionThrows = true;

        // Act
        await loggerMock.Object.SwallowAsync(async () => await Task.Run(() => LoggerSuppressingExtensionsTests.ThrowTestException(actionThrows)));

        // Assert
        loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()
            ),
            Times.Once
        );
    }

    private static void ThrowTestException(bool throwException)
    {
        if (throwException)
        {
            throw new InvalidOperationException("Test exception");
        }
    }
}

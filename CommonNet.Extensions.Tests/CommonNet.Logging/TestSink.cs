using CommunityToolkit.Diagnostics;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace CommonNet.Logging.Tests;

public class TestSink : ILogEventSink
{
    readonly Action<LogEvent> _write;

    public TestSink(Action<LogEvent> write)
    {
        Guard.IsNotNull(write);
        _write = write;
    }

    public void Emit(LogEvent logEvent)
    {
        _write(logEvent);
    }

    public static LogEvent GetLogEvent(Action<ILogger> writeAction)
    {
        LogEvent? logEvent = null;
        var l = new LoggerConfiguration()
            .WriteTo.Sink(new TestSink(le => logEvent = le))
            .CreateLogger();

        writeAction(l);
        return logEvent!;
    }
}

using CommunityToolkit.Diagnostics;

namespace Microsoft.Extensions.Logging;

/// <summary>
/// Provides an interface to execute System.Actions without surfacing any exceptions raised for that action.
/// </summary>
public static class LoggerSuppressingExtensions
{
    /// <summary>
    /// Runs the provided action. If the action throws, the exception is logged
    /// at Error level. The exception is not propagated outside of this method.
    /// </summary>
    /// <param name="logger">Logger context</param>
    /// <param name="action">Action to execute.</param>
    /// <param name="message">Log message template.</param>
    /// <param name="logLevel">Desired log level to log in case of error. Default value is Error.</param>
    public static void Swallow(this ILogger logger, Action action, string message = "", LogLevel logLevel = LogLevel.Error)
    {
        Guard.IsNotNull(logger);
        Guard.IsNotNull(action);

        try
        {
            action();
        }
        catch (Exception e)
        {
            logger.Log(logLevel, e, message);
        }
    }


    /// <summary>
    /// Runs the provided function and returns its result. If an exception is thrown,
    /// it is logged at Error level. The exception is not propagated outside of this
    /// method; a default value is returned instead.
    /// </summary>
    /// <typeparam name="TResult">Return type of the provided function.</typeparam>
    /// <param name="logger">Logger context</param>
    /// <param name="func">Function to run.</param>
    /// <param name="message">Log message template.</param>
    /// <param name="logLevel">Desired log level to log in case of error. Default value is Error.</param>
    /// <returns>Result returned by the provided function or the default value of type T in case of exception.</returns>
    public static TResult? Swallow<TResult>(this ILogger logger, Func<TResult> func, string message = "", LogLevel logLevel = LogLevel.Error)
    {
        return Swallow(logger, func, default, message, logLevel);
    }

    /// <summary>
    /// Runs the provided function and returns its result. If an exception is thrown,
    /// it is logged at Error level. The exception is not propagated outside of this
    /// method; a fallback value is returned instead.
    /// </summary>
    /// <typeparam name="TResult">Return type of the provided function.</typeparam>
    /// <param name="logger">Logger context</param>
    /// <param name="func">Function to run.</param>
    /// <param name="fallback">Fallback value to return in case of exception.</param>
    /// <param name="message">Log message template.</param>
    /// <param name="logLevel">Desired log level to log in case of error. Default value is Error.</param>
    /// <returns>Result returned by the provided function or fallback value in case of exception.</returns>
    public static TResult Swallow<TResult>(this ILogger logger, Func<TResult> func, TResult fallback, string message = "", LogLevel logLevel = LogLevel.Error)
    {
        Guard.IsNotNull(logger);
        Guard.IsNotNull(func);

        try
        {
            return func();
        }
        catch (Exception e)
        {
            logger.Log(logLevel, e, message);
        }
        return fallback;
    }

    /// <summary>
    /// Logs an exception is logged at Error level if the provided task does not run to completion.
    /// </summary>
    /// <param name="logger">Logger context</param>
    /// <param name="task">The task for which to log an error if it does not run to completion.</param>
    /// <param name="message">Log message template.</param>
    /// <param name="logLevel">Desired log level to log in case of error. Default value is Error.</param>
    /// <remarks>
    /// This method is useful in fire-and-forget situations, where application logic
    /// does not depend on completion of task. This method is avoids C# warning CS4014
    /// in such situations.
    /// </remarks>
    public static async void Swallow(this ILogger logger, Task task, string message = "", LogLevel logLevel = LogLevel.Error)
    {
        Guard.IsNotNull(logger);
        Guard.IsNotNull(task);

        try
        {
            await task;
        }
        catch (Exception e)
        {
            logger.Log(logLevel, e, message);
        }
    }

    /// <summary>
    /// Returns a task that completes when a specified task to completes. If the task
    /// does not run to completion, an exception is logged at Error level. The returned
    /// task always runs to completion.
    /// </summary>
    /// <param name="logger">Logger context</param>
    /// <param name="task">The task for which to log an error if it does not run to completion.</param>
    /// <param name="message">Log message template.</param>
    /// <param name="logLevel">Desired log level to log in case of error. Default value is Error.</param>
    /// <returns>
    /// A task that completes in the System.Threading.Tasks.TaskStatus.RanToCompletion
    ///  state when task completes.
    /// </returns>
    public static async Task SwallowAsync(this ILogger logger, Task task, string message = "", LogLevel logLevel = LogLevel.Error)
    {
        Guard.IsNotNull(logger);
        Guard.IsNotNull(task);

        try
        {
            await task;
        }
        catch (Exception e)
        {
            logger.Log(logLevel, e, message);
        }
    }

    /// <summary>
    /// Runs async action. If the action throws, the exception is logged at Error level.
    /// The exception is not propagated outside of this method.
    /// </summary>
    /// <param name="logger">Logger context</param>
    /// <param name="asyncAction">Async action to execute.</param>
    /// <param name="message">Log message template.</param>
    /// <param name="logLevel">Desired log level to log in case of error. Default value is Error.</param>
    /// <returns>
    /// A task that completes in the System.Threading.Tasks.TaskStatus.RanToCompletion
    /// state when asyncAction completes.
    /// </returns>
    public static async Task SwallowAsync(this ILogger logger, Func<Task> asyncAction, string message = "", LogLevel logLevel = LogLevel.Error)
    {
        Guard.IsNotNull(logger);
        Guard.IsNotNull(asyncAction);

        try
        {
            await asyncAction();
        }
        catch (Exception e)
        {
            logger.Log(logLevel, e, message);
        }
    }

    /// <summary>
    /// Runs the provided async function and returns its result. If the task does not
    /// run to completion, an exception is logged at Error level. The exception is not
    /// propagated outside of this method; a default value is returned instead.
    /// </summary>
    /// <typeparam name="TResult">Return type of the provided function.</typeparam>
    /// <param name="logger">Logger context</param>
    /// <param name="message">Log message template.</param>
    /// <param name="asyncFunc">Async function to run.</param>
    /// <param name="logLevel">Desired log level to log in case of error. Default value is Error.</param>
    /// <returns>
    /// A task that represents the completion of the supplied task. If the supplied task
    /// ends in the System.Threading.Tasks.TaskStatus.RanToCompletion state, the result
    /// of the new task will be the result of the supplied task; otherwise, the result
    /// of the new task will be the default value of type TResult.
    /// </returns>
    public static async Task<TResult?> SwallowAsync<TResult>(this ILogger logger, Func<Task<TResult?>> asyncFunc, string message = "", LogLevel logLevel = LogLevel.Error)
    {
        return await SwallowAsync(logger, asyncFunc, default, message, logLevel);
    }

    /// <summary>
    /// Runs the provided async function and returns its result. If the task does not
    /// run to completion, an exception is logged at Error level. The exception is not
    /// propagated outside of this method; a fallback value is returned instead.
    /// </summary>
    /// <typeparam name="TResult">Return type of the provided function.</typeparam>
    /// <param name="logger">Logger context</param>
    /// <param name="asyncFunc">Async function to run.</param>
    /// <param name="fallback">Fallback value to return if the task does not end in the System.Threading.Tasks.TaskStatus.RanToCompletion state.</param>
    /// <param name="message">Log message template.</param>
    /// <param name="logLevel">Desired log level to log in case of error. Default value is Error.</param>
    /// <returns>
    /// A task that represents the completion of the supplied task. If the supplied task
    /// ends in the System.Threading.Tasks.TaskStatus.RanToCompletion state, the result
    /// of the new task will be the result of the supplied task; otherwise, the result
    /// of the new task will be the fallback value.
    /// </returns>
    public static async Task<TResult> SwallowAsync<TResult>(this ILogger logger, Func<Task<TResult>> asyncFunc, TResult fallback, string message = "", LogLevel logLevel = LogLevel.Error)
    {
        Guard.IsNotNull(logger);
        Guard.IsNotNull(asyncFunc);

        try
        {
            return await asyncFunc();
        }
        catch (Exception e)
        {
            logger.Log(logLevel, e, message);
        }
        return fallback;
    }
}

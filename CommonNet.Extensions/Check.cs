using System.Runtime.CompilerServices;

namespace CommonNet.Extensions;

internal static class Check
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Self(object? selfArg)
    {
        if (selfArg is null)
            throw new ArgumentNullException(nameof(selfArg));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Argument(object? arg, string argName)
    {
        if (arg is null)
            throw new ArgumentNullException(argName);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ArgumentWithMsg(object? arg, string argName, string msg)
    {
        if (arg is null)
            throw new ArgumentNullException($"{argName}: {msg}");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ArgumentIsRootedPath(string arg, string argName)
    {
        Argument(arg, argName);

        if (!Path.IsPathRooted(arg))
            throw new ArgumentException($"{argName} is not rooted path.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void VerifyArgument(string arg, bool result, string message)
    {
        if (!result)
            throw new ArgumentException(message, arg);
    }
}

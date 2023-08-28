namespace CommonNet.Extensions.DependencyInjection.Tests;

public interface IBaseInterface1
{
    string Name1 { get; }
}

public interface IBaseInterface2
{
    string Name2 { get; }
}

public interface IBaseInterface3
{
    string Name3 { get; }
}

public interface IBaseInterface4
{
    string Name4 { get; }
}

public interface IBaseInterface5
{
    string Name5 { get; }
}

public interface IInheritedIface2 : IBaseInterface1
{
}

public interface IInheritedIface3 : IInheritedIface2
{
}

public interface IInheritedIface4 : IInheritedIface3
{
}





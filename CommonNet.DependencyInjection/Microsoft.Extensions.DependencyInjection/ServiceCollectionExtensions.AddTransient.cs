namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a transient service of the type specified in
    /// <typeparamref name="TService1"/> and <typeparamref name="TService2"/>
    /// with an implementation type specified in <typeparamref name="TImplementation"/> to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TService1">The type of the service to add.</typeparam>
    /// <typeparam name="TService2">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <returns>A reference to specified <see cref="IServiceCollection"/> instance after the operation has completed.</returns>
    public static IServiceCollection AddTransient<TService1, TService2, TImplementation>(this IServiceCollection services)
        where TImplementation : class, TService1, TService2
        where TService1 : class
        where TService2 : class
    {
        return services
            .AddTransient<TImplementation>()
            .AddTransient<TService1, TImplementation>()
            .AddTransient<TService2, TImplementation>();
    }

    /// <summary>
    /// Adds a transient service of the type specified in
    /// <typeparamref name="TService1"/>, <typeparamref name="TService2"/> and <typeparamref name="TService3"/>
    /// with an implementation type specified in <typeparamref name="TImplementation"/> to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TService1">The type of the service to add.</typeparam>
    /// <typeparam name="TService2">The type of the service to add.</typeparam>
    /// <typeparam name="TService3">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <returns>A reference to specified <see cref="IServiceCollection"/> instance after the operation has completed.</returns>
    public static IServiceCollection AddTransient<TService1, TService2, TService3, TImplementation>(this IServiceCollection services)
        where TImplementation : class, TService1, TService2, TService3
        where TService1 : class
        where TService2 : class
        where TService3 : class
    {
        return services
            .AddTransient<TService1, TService2, TImplementation>()
            .AddTransient<TService3, TImplementation>();
    }

    /// <summary>
    /// Adds a transient service of the type specified in
    /// <typeparamref name="TService1"/>, <typeparamref name="TService2"/>, <typeparamref name="TService3"/> and <typeparamref name="TService4"/>
    /// with an implementation type specified in <typeparamref name="TImplementation"/> to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TService1">The type of the service to add.</typeparam>
    /// <typeparam name="TService2">The type of the service to add.</typeparam>
    /// <typeparam name="TService3">The type of the service to add.</typeparam>
    /// <typeparam name="TService4">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <returns>A reference to specified <see cref="IServiceCollection"/> instance after the operation has completed.</returns>
    public static IServiceCollection AddTransient<TService1, TService2, TService3, TService4, TImplementation>(this IServiceCollection services)
        where TImplementation : class, TService1, TService2, TService3, TService4
        where TService1 : class
        where TService2 : class
        where TService3 : class
        where TService4 : class
    {
        return services
            .AddTransient<TService1, TService2, TService3, TImplementation>()
            .AddTransient<TService4, TImplementation>();
    }

    /// <summary>
    /// Adds a transient service of the type specified in
    /// <typeparamref name="TService1"/> and <typeparamref name="TService2"/>
    /// with an implementation type specified in <typeparamref name="TImplementation"/> using the factory specified in <paramref name="implementationFactory"/> to the specified to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TService1">The type of the service to add.</typeparam>
    /// <typeparam name="TService2">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="implementationFactory">The <see cref="Func{IServiceProvider, TImplementation}"/> factory that creates the service.</param>
    /// <returns>A reference to specified <see cref="IServiceCollection"/> instance after the operation has completed.</returns>
    public static IServiceCollection AddTransient<TService1, TService2, TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory)
        where TImplementation : class, TService1, TService2
        where TService1 : class
        where TService2 : class
    {
        return services
            .AddTransient(implementationFactory)
            .AddTransient<TService1, TImplementation>(implementationFactory)
            .AddTransient<TService2, TImplementation>(implementationFactory);
    }

    /// <summary>
    /// Adds a transient service of the type specified in
    /// <typeparamref name="TService1"/>, <typeparamref name="TService2"/> and <typeparamref name="TService3"/>
    /// with an implementation type specified in <typeparamref name="TImplementation"/> using the factory specified in <paramref name="implementationFactory"/> to the specified to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TService1">The type of the service to add.</typeparam>
    /// <typeparam name="TService2">The type of the service to add.</typeparam>
    /// <typeparam name="TService3">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="implementationFactory">The <see cref="Func{IServiceProvider, TImplementation}"/> factory that creates the service.</param>
    /// <returns>A reference to specified <see cref="IServiceCollection"/> instance after the operation has completed.</returns>
    public static IServiceCollection AddTransient<TService1, TService2, TService3, TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory)
        where TImplementation : class, TService1, TService2, TService3
        where TService1 : class
        where TService2 : class
        where TService3 : class
    {
        return services
            .AddTransient<TService1, TService2, TImplementation>(implementationFactory)
            .AddTransient<TService3, TImplementation>(implementationFactory);
    }

    /// <summary>
    /// Adds a transient service of the type specified in
    /// <typeparamref name="TService1"/>, <typeparamref name="TService2"/>, <typeparamref name="TService3"/> and <typeparamref name="TService4"/>
    /// with an implementation type specified in <typeparamref name="TImplementation"/> using the factory specified in <paramref name="implementationFactory"/> to the specified to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TService1">The type of the service to add.</typeparam>
    /// <typeparam name="TService2">The type of the service to add.</typeparam>
    /// <typeparam name="TService3">The type of the service to add.</typeparam>
    /// <typeparam name="TService4">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="implementationFactory">The <see cref="Func{IServiceProvider, TImplementation}"/> factory that creates the service.</param>
    /// <returns>A reference to specified <see cref="IServiceCollection"/> instance after the operation has completed.</returns>
    public static IServiceCollection AddTransient<TService1, TService2, TService3, TService4, TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory)
        where TImplementation : class, TService1, TService2, TService3, TService4
        where TService1 : class
        where TService2 : class
        where TService3 : class
        where TService4 : class
    {
        return services
            .AddTransient<TService1, TService2, TService3, TImplementation>(implementationFactory)
            .AddTransient<TService4, TImplementation>(implementationFactory);
    }
}

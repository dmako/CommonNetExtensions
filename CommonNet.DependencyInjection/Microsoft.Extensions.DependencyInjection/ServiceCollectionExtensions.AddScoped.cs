#pragma warning disable S2436 // Reduce the number of generic parameters in method to no more than the 3 authorized

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a scoped service of the type specified in
    /// <typeparamref name="TService"/>
    /// with an implementation type specified in <typeparamref name="TImplementation"/> to the specified <see cref="IServiceCollection"/>
    /// if <paramref name="condition"/> is met.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="condition">Condition for adding to the specified <see cref="IServiceCollection"/>.</param>
    /// <returns>A reference to specified <see cref="IServiceCollection"/> instance after the operation has completed.</returns>
    public static IServiceCollection AddScopedIf<TService, TImplementation>(this IServiceCollection services, bool condition)
        where TImplementation : class, TService
        where TService : class
    {
        if (condition)
        {
            return services.AddScoped<TService, TImplementation>();
        }
        return services;
    }

    /// <summary>
    /// Adds a scoped service of the type specified in
    /// <typeparamref name="TService1"/> and <typeparamref name="TService2"/>
    /// with an implementation type specified in <typeparamref name="TImplementation"/> to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TService1">The type of the service to add.</typeparam>
    /// <typeparam name="TService2">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <returns>A reference to specified <see cref="IServiceCollection"/> instance after the operation has completed.</returns>
    public static IServiceCollection AddScoped<TService1, TService2, TImplementation>(this IServiceCollection services)
        where TImplementation : class, TService1, TService2
        where TService1 : class
        where TService2 : class
    {
        return services
            .AddScoped<TService1, TImplementation>()
            .AddScoped(sp => (sp.GetRequiredService<TService1>() as TService2)!);
    }

    /// <summary>
    /// Adds a scoped service of the type specified in
    /// <typeparamref name="TService1"/> and <typeparamref name="TService2"/>
    /// with an implementation type specified in <typeparamref name="TImplementation"/> to the specified <see cref="IServiceCollection"/>
    /// if <paramref name="condition"/> is met.
    /// </summary>
    /// <typeparam name="TService1">The type of the service to add.</typeparam>
    /// <typeparam name="TService2">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="condition">Condition for adding to the specified <see cref="IServiceCollection"/>.</param>
    /// <returns>A reference to specified <see cref="IServiceCollection"/> instance after the operation has completed.</returns>
    public static IServiceCollection AddScopedIf<TService1, TService2, TImplementation>(this IServiceCollection services, bool condition)
        where TImplementation : class, TService1, TService2
        where TService1 : class
        where TService2 : class
    {
        if (condition)
        {
            return services.AddScoped<TService1, TService2, TImplementation>();
        }
        return services;
    }

    /// <summary>
    /// Adds a scoped service of the type specified in
    /// <typeparamref name="TService1"/>, <typeparamref name="TService2"/> and <typeparamref name="TService3"/>
    /// with an implementation type specified in <typeparamref name="TImplementation"/> to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TService1">The type of the service to add.</typeparam>
    /// <typeparam name="TService2">The type of the service to add.</typeparam>
    /// <typeparam name="TService3">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <returns>A reference to specified <see cref="IServiceCollection"/> instance after the operation has completed.</returns>
    public static IServiceCollection AddScoped<TService1, TService2, TService3, TImplementation>(this IServiceCollection services)
        where TImplementation : class, TService1, TService2, TService3
        where TService1 : class
        where TService2 : class
        where TService3 : class
    {
        return services
            .AddScoped<TService1, TService2, TImplementation>()
            .AddScoped(sp => (sp.GetRequiredService<TService1>() as TService3)!);
    }

    /// <summary>
    /// Adds a scoped service of the type specified in
    /// <typeparamref name="TService1"/>, <typeparamref name="TService2"/> and <typeparamref name="TService3"/>
    /// with an implementation type specified in <typeparamref name="TImplementation"/> to the specified <see cref="IServiceCollection"/>
    /// if <paramref name="condition"/> is met.
    /// </summary>
    /// <typeparam name="TService1">The type of the service to add.</typeparam>
    /// <typeparam name="TService2">The type of the service to add.</typeparam>
    /// <typeparam name="TService3">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="condition">Condition for adding to the specified <see cref="IServiceCollection"/>.</param>
    /// <returns>A reference to specified <see cref="IServiceCollection"/> instance after the operation has completed.</returns>
    public static IServiceCollection AddScopedIf<TService1, TService2, TService3, TImplementation>(this IServiceCollection services, bool condition)
        where TImplementation : class, TService1, TService2, TService3
        where TService1 : class
        where TService2 : class
        where TService3 : class
    {
        if (condition)
        {
            return services.AddScoped<TService1, TService2, TService3, TImplementation>();
        }
        return services;
    }

    /// <summary>
    /// Adds a scoped service of the type specified in
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
    public static IServiceCollection AddScoped<TService1, TService2, TService3, TService4, TImplementation>(this IServiceCollection services)
        where TImplementation : class, TService1, TService2, TService3, TService4
        where TService1 : class
        where TService2 : class
        where TService3 : class
        where TService4 : class
    {
        return services
            .AddScoped<TService1, TService2, TService3, TImplementation>()
            .AddScoped(sp => (sp.GetRequiredService<TService1>() as TService4)!);
    }

    /// <summary>
    /// Adds a scoped service of the type specified in
    /// <typeparamref name="TService1"/>, <typeparamref name="TService2"/>, <typeparamref name="TService3"/> and <typeparamref name="TService4"/>
    /// with an implementation type specified in <typeparamref name="TImplementation"/> to the specified <see cref="IServiceCollection"/>
    /// if <paramref name="condition"/> is met.
    /// </summary>
    /// <typeparam name="TService1">The type of the service to add.</typeparam>
    /// <typeparam name="TService2">The type of the service to add.</typeparam>
    /// <typeparam name="TService3">The type of the service to add.</typeparam>
    /// <typeparam name="TService4">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="condition">Condition for adding to the specified <see cref="IServiceCollection"/>.</param>
    /// <returns>A reference to specified <see cref="IServiceCollection"/> instance after the operation has completed.</returns>
    public static IServiceCollection AddScopedIf<TService1, TService2, TService3, TService4, TImplementation>(this IServiceCollection services, bool condition)
        where TImplementation : class, TService1, TService2, TService3, TService4
        where TService1 : class
        where TService2 : class
        where TService3 : class
        where TService4 : class
    {
        if (condition)
        {
            return services.AddScoped<TService1, TService2, TService3, TService4, TImplementation>();
        }
        return services;
    }

    /// <summary>
    /// Adds a scoped service of the type specified in
    /// <typeparamref name="TService"/>
    /// with an implementation type specified in <typeparamref name="TImplementation"/> using the factory specified in <paramref name="implementationFactory"/> to the specified to the specified <see cref="IServiceCollection"/>
    /// if <paramref name="condition"/> is met.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="condition">Condition for adding to the specified <see cref="IServiceCollection"/>.</param>
    /// <param name="implementationFactory">The <see cref="Func{IServiceProvider, TImplementation}"/> factory that creates the service.</param>
    /// <returns>A reference to specified <see cref="IServiceCollection"/> instance after the operation has completed.</returns>
    public static IServiceCollection AddScopedIf<TService, TImplementation>(this IServiceCollection services, bool condition, Func<IServiceProvider, TImplementation> implementationFactory)
        where TImplementation : class, TService
        where TService : class
    {
        if (condition)
        {
            return services.AddScoped<TService, TImplementation>(implementationFactory);
        }
        return services;
    }

    /// <summary>
    /// Adds a scoped service of the type specified in
    /// <typeparamref name="TService1"/> and <typeparamref name="TService2"/>
    /// with an implementation type specified in <typeparamref name="TImplementation"/> using the factory specified in <paramref name="implementationFactory"/> to the specified to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TService1">The type of the service to add.</typeparam>
    /// <typeparam name="TService2">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="implementationFactory">The <see cref="Func{IServiceProvider, TImplementation}"/> factory that creates the service.</param>
    /// <returns>A reference to specified <see cref="IServiceCollection"/> instance after the operation has completed.</returns>
    public static IServiceCollection AddScoped<TService1, TService2, TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory)
        where TImplementation : class, TService1, TService2
        where TService1 : class
        where TService2 : class
    {
        return services
            .AddScoped<TService1, TImplementation>(implementationFactory)
            .AddScoped(sp => (sp.GetRequiredService<TService1>() as TService2)!);
    }

    /// <summary>
    /// Adds a scoped service of the type specified in
    /// <typeparamref name="TService1"/> and <typeparamref name="TService2"/>
    /// with an implementation type specified in <typeparamref name="TImplementation"/> using the factory specified in <paramref name="implementationFactory"/> to the specified to the specified <see cref="IServiceCollection"/>
    /// if <paramref name="condition"/> is met.
    /// </summary>
    /// <typeparam name="TService1">The type of the service to add.</typeparam>
    /// <typeparam name="TService2">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="condition">Condition for adding to the specified <see cref="IServiceCollection"/>.</param>
    /// <param name="implementationFactory">The <see cref="Func{IServiceProvider, TImplementation}"/> factory that creates the service.</param>
    /// <returns>A reference to specified <see cref="IServiceCollection"/> instance after the operation has completed.</returns>
    public static IServiceCollection AddScopedIf<TService1, TService2, TImplementation>(this IServiceCollection services, bool condition, Func<IServiceProvider, TImplementation> implementationFactory)
        where TImplementation : class, TService1, TService2
        where TService1 : class
        where TService2 : class
    {
        if (condition)
        {
            return services.AddScoped<TService2, TService1, TImplementation>(implementationFactory);
        }
        return services;
    }

    /// <summary>
    /// Adds a scoped service of the type specified in
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
    public static IServiceCollection AddScoped<TService1, TService2, TService3, TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory)
        where TImplementation : class, TService1, TService2, TService3
        where TService1 : class
        where TService2 : class
        where TService3 : class
    {
        return services
            .AddScoped<TService1, TService2, TImplementation>(implementationFactory)
            .AddScoped(sp => (sp.GetRequiredService<TService1>() as TService3)!);
    }

    /// <summary>
    /// Adds a scoped service of the type specified in
    /// <typeparamref name="TService1"/>, <typeparamref name="TService2"/> and <typeparamref name="TService3"/>
    /// with an implementation type specified in <typeparamref name="TImplementation"/> using the factory specified in <paramref name="implementationFactory"/> to the specified to the specified <see cref="IServiceCollection"/>
    /// if <paramref name="condition"/> is met.
    /// </summary>
    /// <typeparam name="TService1">The type of the service to add.</typeparam>
    /// <typeparam name="TService2">The type of the service to add.</typeparam>
    /// <typeparam name="TService3">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="condition">Condition for adding to the specified <see cref="IServiceCollection"/>.</param>
    /// <param name="implementationFactory">The <see cref="Func{IServiceProvider, TImplementation}"/> factory that creates the service.</param>
    /// <returns>A reference to specified <see cref="IServiceCollection"/> instance after the operation has completed.</returns>
    public static IServiceCollection AddScopedIf<TService1, TService2, TService3, TImplementation>(this IServiceCollection services, bool condition, Func<IServiceProvider, TImplementation> implementationFactory)
        where TImplementation : class, TService1, TService2, TService3
        where TService1 : class
        where TService2 : class
        where TService3 : class
    {
        if (condition)
        {
            return services.AddScoped<TService1, TService2, TService3, TImplementation>(implementationFactory);
        }
        return services;
    }

    /// <summary>
    /// Adds a scoped service of the type specified in
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
    public static IServiceCollection AddScoped<TService1, TService2, TService3, TService4, TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory)
        where TImplementation : class, TService1, TService2, TService3, TService4
        where TService1 : class
        where TService2 : class
        where TService3 : class
        where TService4 : class
    {
        return services
            .AddScoped<TService1, TService2, TService3, TImplementation>(implementationFactory)
            .AddScoped(sp => (sp.GetRequiredService<TService1>() as TService4)!);
    }

    /// <summary>
    /// Adds a scoped service of the type specified in
    /// <typeparamref name="TService1"/>, <typeparamref name="TService2"/>, <typeparamref name="TService3"/> and <typeparamref name="TService4"/>
    /// with an implementation type specified in <typeparamref name="TImplementation"/> using the factory specified in <paramref name="implementationFactory"/> to the specified to the specified <see cref="IServiceCollection"/>
    /// if <paramref name="condition"/> is met.
    /// </summary>
    /// <typeparam name="TService1">The type of the service to add.</typeparam>
    /// <typeparam name="TService2">The type of the service to add.</typeparam>
    /// <typeparam name="TService3">The type of the service to add.</typeparam>
    /// <typeparam name="TService4">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="condition">Condition for adding to the specified <see cref="IServiceCollection"/>.</param>
    /// <param name="implementationFactory">The <see cref="Func{IServiceProvider, TImplementation}"/> factory that creates the service.</param>
    /// <returns>A reference to specified <see cref="IServiceCollection"/> instance after the operation has completed.</returns>
    public static IServiceCollection AddScopedIf<TService1, TService2, TService3, TService4, TImplementation>(this IServiceCollection services, bool condition, Func<IServiceProvider, TImplementation> implementationFactory)
        where TImplementation : class, TService1, TService2, TService3, TService4
        where TService1 : class
        where TService2 : class
        where TService3 : class
        where TService4 : class
    {
        if (condition)
        {
            return services.AddScoped<TService4, TService3, TService2, TService1, TImplementation>(implementationFactory);
        }
        return services;
    }
}

#pragma warning restore S2436 // Reduce the number of generic parameters in method to no more than the 3 authorized

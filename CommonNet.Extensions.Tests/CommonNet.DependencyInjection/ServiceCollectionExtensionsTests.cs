using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CommonNet.Extensions.DependencyInjection.Tests;

public class ServiceCollectionExtensionsTests
{

    class A4_1 : IInheritedIface4
    {
        public string Name1 => nameof(A4_1);
    } 

    class B4_1 : IBaseInterface1, IBaseInterface2, IBaseInterface3, IBaseInterface4
    {
        public string Name1 => nameof(B4_1);
        public string Name2 => nameof(Name2);
        public string Name3 => nameof(Name3);
        public string Name4 => nameof(Name4);
    }

    class B4_2 : IBaseInterface2, IBaseInterface3, IBaseInterface4, IBaseInterface5
    {
        public string Name2 => nameof(Name2);
        public string Name3 => nameof(Name3);
        public string Name4 => nameof(Name4);
        public string Name5 => nameof(Name5);
    }

    [Fact]
    public void AddSingleton_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services
            .AddSingletonIf<IInheritedIface4, IInheritedIface3, IInheritedIface2, IBaseInterface1, A4_1>(true)
            .AddSingletonIf<IBaseInterface2, IBaseInterface3, IBaseInterface4, IBaseInterface5, B4_2>(true);

        using var sp = services.BuildServiceProvider();

        var iif4 = sp.GetRequiredService<IInheritedIface4>();
        iif4.Should().NotBeNull();
        var iif3 = sp.GetRequiredService<IInheritedIface3>();
        iif3.Should().NotBeNull();
        var iif2 = sp.GetRequiredService<IInheritedIface2>();
        iif2.Should().NotBeNull();
        var bif1 = sp.GetRequiredService<IBaseInterface1>();
        bif1.Should().NotBeNull();

        iif4.Should().Be(iif3);
        iif3.Should().Be(iif2);
        iif2.Should().Be(bif1);


        var bif2 = sp.GetRequiredService<IBaseInterface2>();
        bif2.Should().NotBeNull();
        var bif3 = sp.GetRequiredService<IBaseInterface3>();
        bif3.Should().NotBeNull();
        var bif4 = sp.GetRequiredService<IBaseInterface4>();
        bif4.Should().NotBeNull();
        var bif5 = sp.GetRequiredService<IBaseInterface5>();
        bif5.Should().NotBeNull();

        bif2.Should().Be(bif3);
        bif3.Should().Be(bif4);
        bif4.Should().Be(bif5);

        bif2.Should().NotBe(bif1);
    }

    [Fact]
    public void AddSingletonFact_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services
            .AddSingletonIf<IInheritedIface4, IInheritedIface3, IInheritedIface2, IBaseInterface1, A4_1>(true, sp => new A4_1())
            .AddSingletonIf<IBaseInterface2, IBaseInterface3, IBaseInterface4, IBaseInterface5, B4_2>(true, sp => new B4_2());

        using var sp = services.BuildServiceProvider();

        var iif4 = sp.GetRequiredService<IInheritedIface4>();
        iif4.Should().NotBeNull();
        var iif3 = sp.GetRequiredService<IInheritedIface3>();
        iif3.Should().NotBeNull();
        var iif2 = sp.GetRequiredService<IInheritedIface2>();
        iif2.Should().NotBeNull();
        var bif1 = sp.GetRequiredService<IBaseInterface1>();
        bif1.Should().NotBeNull();

        iif4.Should().Be(iif3);
        iif3.Should().Be(iif2);
        iif2.Should().Be(bif1);


        var bif2 = sp.GetRequiredService<IBaseInterface2>();
        bif2.Should().NotBeNull();
        var bif3 = sp.GetRequiredService<IBaseInterface3>();
        bif3.Should().NotBeNull();
        var bif4 = sp.GetRequiredService<IBaseInterface4>();
        bif4.Should().NotBeNull();
        var bif5 = sp.GetRequiredService<IBaseInterface5>();
        bif5.Should().NotBeNull();

        bif2.Should().Be(bif3);
        bif3.Should().Be(bif4);
        bif4.Should().Be(bif5);

        bif2.Should().NotBe(bif1);
    }

    [Fact]
    public void AddTransient_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services.AddTransientIf<IInheritedIface4, IInheritedIface3, IInheritedIface2, IBaseInterface1, A4_1>(true);

        using var sp = services.BuildServiceProvider();

        var iif4 = sp.GetRequiredService<IInheritedIface4>();
        iif4.Should().NotBeNull();
        var iif3 = sp.GetRequiredService<IInheritedIface3>();
        iif3.Should().NotBeNull();
        var iif2 = sp.GetRequiredService<IInheritedIface2>();
        iif2.Should().NotBeNull();
        var bif1_1 = sp.GetRequiredService<IBaseInterface1>();
        bif1_1.Should().NotBeNull();

        iif4.Should().NotBe(iif3);
        iif3.Should().NotBe(iif2);
        iif2.Should().NotBe(bif1_1);

        var bif1_2 = sp.GetRequiredService<IBaseInterface1>();
        bif1_2.Should().NotBeNull();
        bif1_2.Should().NotBe(bif1_1);
    }

    [Fact]
    public void AddTransientFact_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services.AddTransientIf<IInheritedIface4, IInheritedIface3, IInheritedIface2, IBaseInterface1, A4_1>(true, sp => new A4_1());

        using var sp = services.BuildServiceProvider();

        var iif4 = sp.GetRequiredService<IInheritedIface4>();
        iif4.Should().NotBeNull();
        var iif3 = sp.GetRequiredService<IInheritedIface3>();
        iif3.Should().NotBeNull();
        var iif2 = sp.GetRequiredService<IInheritedIface2>();
        iif2.Should().NotBeNull();
        var bif1_1 = sp.GetRequiredService<IBaseInterface1>();
        bif1_1.Should().NotBeNull();

        iif4.Should().NotBe(iif3);
        iif3.Should().NotBe(iif2);
        iif2.Should().NotBe(bif1_1);

        var bif1_2 = sp.GetRequiredService<IBaseInterface1>();
        bif1_2.Should().NotBeNull();
        bif1_2.Should().NotBe(bif1_1);
    }

    [Fact]
    public void AddScoped_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services.AddScopedIf<IInheritedIface4, IInheritedIface3, IInheritedIface2, IBaseInterface1, A4_1>(true);

        using var sp = services.BuildServiceProvider();

        using var scope1 = sp.CreateScope();

        var s1_iif4 = scope1.ServiceProvider.GetRequiredService<IInheritedIface4>();
        s1_iif4.Should().NotBeNull();
        var s1_iif3 = scope1.ServiceProvider.GetRequiredService<IInheritedIface3>();
        s1_iif3.Should().NotBeNull();
        var s1_iif2 = scope1.ServiceProvider.GetRequiredService<IInheritedIface2>();
        s1_iif2.Should().NotBeNull();
        var s1_bif1_1 = scope1.ServiceProvider.GetRequiredService<IBaseInterface1>();
        s1_bif1_1.Should().NotBeNull();

        s1_iif4.Should().Be(s1_iif3);
        s1_iif3.Should().Be(s1_iif2);
        s1_iif2.Should().Be(s1_bif1_1);

        var s1_bif1_2 = scope1.ServiceProvider.GetRequiredService<IBaseInterface1>();
        s1_bif1_2.Should().NotBeNull();
        s1_bif1_2.Should().Be(s1_bif1_1);

        using var scope2 = sp.CreateScope();

        var s2_bif1_1 = scope2.ServiceProvider.GetRequiredService<IBaseInterface1>();
        s1_bif1_1.Should().NotBeNull();

        var s2_bif1_2 = scope2.ServiceProvider.GetRequiredService<IBaseInterface1>();
        s2_bif1_2.Should().NotBeNull();
        s2_bif1_2.Should().Be(s2_bif1_1);

        s2_bif1_1.Should().NotBe(s1_bif1_1);
    }

    [Fact]
    public void AddScopedFact_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services.AddScopedIf<IInheritedIface4, IInheritedIface3, IInheritedIface2, IBaseInterface1, A4_1>(true, sp => new A4_1());

        using var sp = services.BuildServiceProvider();

        using var scope1 = sp.CreateScope();

        var s1_iif4 = scope1.ServiceProvider.GetRequiredService<IInheritedIface4>();
        s1_iif4.Should().NotBeNull();
        var s1_iif3 = scope1.ServiceProvider.GetRequiredService<IInheritedIface3>();
        s1_iif3.Should().NotBeNull();
        var s1_iif2 = scope1.ServiceProvider.GetRequiredService<IInheritedIface2>();
        s1_iif2.Should().NotBeNull();
        var s1_bif1_1 = scope1.ServiceProvider.GetRequiredService<IBaseInterface1>();
        s1_bif1_1.Should().NotBeNull();

        s1_iif4.Should().Be(s1_iif3);
        s1_iif3.Should().Be(s1_iif2);
        s1_iif2.Should().Be(s1_bif1_1);

        var s1_bif1_2 = scope1.ServiceProvider.GetRequiredService<IBaseInterface1>();
        s1_bif1_2.Should().NotBeNull();
        s1_bif1_2.Should().Be(s1_bif1_1);

        using var scope2 = sp.CreateScope();

        var s2_bif1_1 = scope2.ServiceProvider.GetRequiredService<IBaseInterface1>();
        s1_bif1_1.Should().NotBeNull();

        var s2_bif1_2 = scope2.ServiceProvider.GetRequiredService<IBaseInterface1>();
        s2_bif1_2.Should().NotBeNull();
        s2_bif1_2.Should().Be(s2_bif1_1);

        s2_bif1_1.Should().NotBe(s1_bif1_1);
    }

    [Fact]
    public void AddIf_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services
            .AddSingletonIf<IBaseInterface1, A4_1>(true)
            .AddSingletonIf<IInheritedIface2, A4_1>(false)
            .AddTransientIf<IBaseInterface2, B4_1>(true)
            .AddTransientIf<IBaseInterface3, B4_1>(false)
            .AddScopedIf<IBaseInterface4, B4_2>(true)
            .AddScopedIf<IBaseInterface5, B4_2>(false);

        using var sp = services.BuildServiceProvider();
        using var scope1 = sp.CreateScope();

        var a41_1 = sp.GetService<IBaseInterface1>();
        a41_1.Should().NotBeNull();
        var a41_2 = sp.GetService<IInheritedIface2>();
        a41_2.Should().BeNull();


        var b41_1 = sp.GetService<IBaseInterface2>();
        b41_1.Should().NotBeNull();
        var b41_2 = sp.GetService<IBaseInterface3>();
        b41_2.Should().BeNull();

        var b42_1 = scope1.ServiceProvider.GetService<IBaseInterface4>();
        b42_1.Should().NotBeNull();
        var b42_2 = scope1.ServiceProvider.GetService<IBaseInterface5>();
        b42_2.Should().BeNull();
    }

    [Fact]
    public void AddIfFact_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services
            .AddSingletonIf<IBaseInterface1, A4_1>(true, sp => new A4_1())
            .AddSingletonIf<IInheritedIface2, A4_1>(false, sp => new A4_1())
            .AddTransientIf<IBaseInterface2, B4_1>(true, sp => new B4_1())
            .AddTransientIf<IBaseInterface3, B4_1>(false, sp => new B4_1())
            .AddScopedIf<IBaseInterface4, B4_2>(true, sp => new B4_2())
            .AddScopedIf<IBaseInterface5, B4_2>(false, sp => new B4_2());

        using var sp = services.BuildServiceProvider();
        using var scope1 = sp.CreateScope();

        var a41_1 = sp.GetService<IBaseInterface1>();
        a41_1.Should().NotBeNull();
        var a41_2 = sp.GetService<IInheritedIface2>();
        a41_2.Should().BeNull();


        var b41_1 = sp.GetService<IBaseInterface2>();
        b41_1.Should().NotBeNull();
        var b41_2 = sp.GetService<IBaseInterface3>();
        b41_2.Should().BeNull();

        var b42_1 = scope1.ServiceProvider.GetService<IBaseInterface4>();
        b42_1.Should().NotBeNull();
        var b42_2 = scope1.ServiceProvider.GetService<IBaseInterface5>();
        b42_2.Should().BeNull();
    }
}


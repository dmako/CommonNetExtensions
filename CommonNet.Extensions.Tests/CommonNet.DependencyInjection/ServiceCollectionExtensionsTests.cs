using Microsoft.Extensions.DependencyInjection;

namespace CommonNet.Extensions.DependencyInjection.Tests;

public class ServiceCollectionExtensionsTests
{
    class A4_1 : IInheritedInterface4
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

    [Test]
    public async Task AddSingleton_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services
            .AddSingletonIf<IInheritedInterface4, IInheritedInterface3, IInheritedInterface2, IBaseInterface1, A4_1>(true)
            .AddSingletonIf<IBaseInterface2, IBaseInterface3, IBaseInterface4, IBaseInterface5, B4_2>(true);

        using var sp = services.BuildServiceProvider();

        var iif4 = sp.GetRequiredService<IInheritedInterface4>();
        await Assert.That(iif4)
            .IsNotNull();
        var iif3 = sp.GetRequiredService<IInheritedInterface3>();
        await Assert.That(iif3)
            .IsNotNull();
        var iif2 = sp.GetRequiredService<IInheritedInterface2>();
        await Assert.That(iif2)
            .IsNotNull();
        var bif1 = sp.GetRequiredService<IBaseInterface1>();
        await Assert.That(bif1)
            .IsNotNull();

        await Assert.That(iif4)
            .IsSameReferenceAs(iif3);
        await Assert.That(iif3)
            .IsSameReferenceAs(iif2);
        await Assert.That(iif2)
            .IsSameReferenceAs(bif1);

        var bif2 = sp.GetRequiredService<IBaseInterface2>();
        await Assert.That(bif2)
            .IsNotNull();
        var bif3 = sp.GetRequiredService<IBaseInterface3>();
        await Assert.That(bif3)
            .IsNotNull();
        var bif4 = sp.GetRequiredService<IBaseInterface4>();
        await Assert.That(bif4)
            .IsNotNull();
        var bif5 = sp.GetRequiredService<IBaseInterface5>();
        await Assert.That(bif5)
            .IsNotNull();

        await Assert.That((object)bif2)
            .IsEqualTo(bif3);
        await Assert.That((object)bif3)
            .IsEqualTo(bif4);
        await Assert.That((object)bif4)
            .IsEqualTo(bif5);
        await Assert.That((object)bif2)
            .IsNotEqualTo(bif1);
    }

    [Test]
    public async Task AddSingletonWithFactory_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services
            .AddSingletonIf<IInheritedInterface4, IInheritedInterface3, IInheritedInterface2, IBaseInterface1, A4_1>(true, sp => new A4_1())
            .AddSingletonIf<IBaseInterface2, IBaseInterface3, IBaseInterface4, IBaseInterface5, B4_2>(true, sp => new B4_2());

        using var sp = services.BuildServiceProvider();

        var iif4 = sp.GetRequiredService<IInheritedInterface4>();
        await Assert.That(iif4)
            .IsNotNull();
        var iif3 = sp.GetRequiredService<IInheritedInterface3>();
        await Assert.That(iif3)
            .IsNotNull();
        var iif2 = sp.GetRequiredService<IInheritedInterface2>();
        await Assert.That(iif2)
            .IsNotNull();
        var bif1 = sp.GetRequiredService<IBaseInterface1>();
        await Assert.That(bif1)
            .IsNotNull();

        await Assert.That(iif4)
            .IsSameReferenceAs(iif3);
        await Assert.That(iif3)
            .IsSameReferenceAs(iif2);
        await Assert.That(iif2)
            .IsSameReferenceAs(bif1);

        var bif2 = sp.GetRequiredService<IBaseInterface2>();
        await Assert.That(bif2)
            .IsNotNull();
        var bif3 = sp.GetRequiredService<IBaseInterface3>();
        await Assert.That(bif3)
            .IsNotNull();
        var bif4 = sp.GetRequiredService<IBaseInterface4>();
        await Assert.That(bif4)
            .IsNotNull();
        var bif5 = sp.GetRequiredService<IBaseInterface5>();
        await Assert.That(bif5)
            .IsNotNull();

        await Assert.That((object)bif2)
            .IsEqualTo(bif3);
        await Assert.That((object)bif3)
            .IsEqualTo(bif4);
        await Assert.That((object)bif4)
            .IsEqualTo(bif5);

        await Assert.That((object)bif2)
            .IsNotEqualTo(bif1);
    }

    [Test]
    public async Task AddSingletonFact_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services
            .AddSingletonIf<IInheritedInterface4, IInheritedInterface3, IInheritedInterface2, IBaseInterface1, A4_1>(true, sp => new A4_1())
            .AddSingletonIf<IBaseInterface2, IBaseInterface3, IBaseInterface4, IBaseInterface5, B4_2>(true, sp => new B4_2());

        using var sp = services.BuildServiceProvider();

        var iif4 = sp.GetRequiredService<IInheritedInterface4>();
        await Assert.That(iif4)
            .IsNotNull();
        var iif3 = sp.GetRequiredService<IInheritedInterface3>();
        await Assert.That(iif3)
            .IsNotNull();
        var iif2 = sp.GetRequiredService<IInheritedInterface2>();
        await Assert.That(iif2)
            .IsNotNull();
        var bif1 = sp.GetRequiredService<IBaseInterface1>();
        await Assert.That(bif1)
            .IsNotNull();

        await Assert.That(iif4)
            .IsSameReferenceAs(iif3);
        await Assert.That(iif3)
            .IsSameReferenceAs(iif2);
        await Assert.That(iif2)
            .IsSameReferenceAs(bif1);

        var bif2 = sp.GetRequiredService<IBaseInterface2>();
        await Assert.That(bif2)
            .IsNotNull();
        var bif3 = sp.GetRequiredService<IBaseInterface3>();
        await Assert.That(bif3)
            .IsNotNull();
        var bif4 = sp.GetRequiredService<IBaseInterface4>();
        await Assert.That(bif4)
            .IsNotNull();
        var bif5 = sp.GetRequiredService<IBaseInterface5>();
        await Assert.That(bif5)
            .IsNotNull();

        await Assert.That((object)bif2)
            .IsEqualTo(bif3);
        await Assert.That((object)bif3)
            .IsEqualTo(bif4);
        await Assert.That((object)bif4)
            .IsEqualTo(bif5);
        await Assert.That((object)bif2)
            .IsNotEqualTo(bif1);
    }

    [Test]
    public async Task AddTransient_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services.AddTransientIf<IInheritedInterface4, IInheritedInterface3, IInheritedInterface2, IBaseInterface1, A4_1>(true);

        using var sp = services.BuildServiceProvider();

        var iif4 = sp.GetRequiredService<IInheritedInterface4>();
        await Assert.That(iif4)
            .IsNotNull();
        var iif3 = sp.GetRequiredService<IInheritedInterface3>();
        await Assert.That(iif3)
            .IsNotNull();
        var iif2 = sp.GetRequiredService<IInheritedInterface2>();
        await Assert.That(iif2)
            .IsNotNull();
        var bif1_1 = sp.GetRequiredService<IBaseInterface1>();
        await Assert.That(bif1_1)
            .IsNotNull();

        await Assert.That(iif4)
            .IsNotSameReferenceAs(iif3);
        await Assert.That(iif3)
            .IsNotSameReferenceAs(iif2);
        await Assert.That(iif2)
            .IsNotSameReferenceAs(bif1_1);

        var bif1_2 = sp.GetRequiredService<IBaseInterface1>();
        await Assert.That(bif1_2)
            .IsNotNull();
        await Assert.That(bif1_2)
            .IsNotSameReferenceAs(bif1_1);
    }

    [Test]
    public async Task AddTransientFact_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services.AddTransientIf<IInheritedInterface4, IInheritedInterface3, IInheritedInterface2, IBaseInterface1, A4_1>(true, sp => new A4_1());

        using var sp = services.BuildServiceProvider();

        var iif4 = sp.GetRequiredService<IInheritedInterface4>();
        await Assert.That(iif4)
            .IsNotNull();
        var iif3 = sp.GetRequiredService<IInheritedInterface3>();
        await Assert.That(iif3)
            .IsNotNull();
        var iif2 = sp.GetRequiredService<IInheritedInterface2>();
        await Assert.That(iif2)
            .IsNotNull();
        var bif1_1 = sp.GetRequiredService<IBaseInterface1>();
        await Assert.That(bif1_1)
            .IsNotNull();

        await Assert.That(iif4)
            .IsNotSameReferenceAs(iif3);
        await Assert.That(iif3)
            .IsNotSameReferenceAs(iif2);
        await Assert.That(iif2)
            .IsNotSameReferenceAs(bif1_1);

        var bif1_2 = sp.GetRequiredService<IBaseInterface1>();
        await Assert.That(bif1_2)
            .IsNotNull();
        await Assert.That(bif1_2)
            .IsNotSameReferenceAs(bif1_1);
    }

    [Test]
    public async Task AddScoped_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services.AddScopedIf<IInheritedInterface4, IInheritedInterface3, IInheritedInterface2, IBaseInterface1, A4_1>(true);

        using var sp = services.BuildServiceProvider();

        using var scope1 = sp.CreateScope();

        var s1_iif4 = scope1.ServiceProvider.GetRequiredService<IInheritedInterface4>();
        await Assert.That(s1_iif4)
            .IsNotNull();
        var s1_iif3 = scope1.ServiceProvider.GetRequiredService<IInheritedInterface3>();
        await Assert.That(s1_iif3)
            .IsNotNull();
        var s1_iif2 = scope1.ServiceProvider.GetRequiredService<IInheritedInterface2>();
        await Assert.That(s1_iif2)
            .IsNotNull();
        var s1_bif1_1 = scope1.ServiceProvider.GetRequiredService<IBaseInterface1>();
        await Assert.That(s1_bif1_1)
            .IsNotNull();

        await Assert.That(s1_iif4)
            .IsSameReferenceAs(s1_iif3);
        await Assert.That(s1_iif3)
            .IsSameReferenceAs(s1_iif2);
        await Assert.That(s1_iif2)
            .IsSameReferenceAs(s1_bif1_1);

        var s1_bif1_2 = scope1.ServiceProvider.GetRequiredService<IBaseInterface1>();
        await Assert.That(s1_bif1_2)
            .IsNotNull();
        await Assert.That(s1_bif1_2)
            .IsEqualTo(s1_bif1_1);

        using var scope2 = sp.CreateScope();

        var s2_bif1_1 = scope2.ServiceProvider.GetRequiredService<IBaseInterface1>();
        await Assert.That(s2_bif1_1)
            .IsNotNull();

        var s2_bif1_2 = scope2.ServiceProvider.GetRequiredService<IBaseInterface1>();
        await Assert.That(s2_bif1_2)
            .IsNotNull();
        await Assert.That(s2_bif1_2)
            .IsEqualTo(s2_bif1_1);

        await Assert.That(s2_bif1_1)
            .IsNotEqualTo(s1_bif1_1);
    }

    [Test]
    public async Task AddScopedFact_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services.AddScopedIf<IInheritedInterface4, IInheritedInterface3, IInheritedInterface2, IBaseInterface1, A4_1>(true, sp => new A4_1());

        using var sp = services.BuildServiceProvider();

        using var scope1 = sp.CreateScope();

        var s1_iif4 = scope1.ServiceProvider.GetRequiredService<IInheritedInterface4>();
        await Assert.That(s1_iif4)
            .IsNotNull();
        var s1_iif3 = scope1.ServiceProvider.GetRequiredService<IInheritedInterface3>();
        await Assert.That(s1_iif3)
            .IsNotNull();
        var s1_iif2 = scope1.ServiceProvider.GetRequiredService<IInheritedInterface2>();
        await Assert.That(s1_iif2)
            .IsNotNull();
        var s1_bif1_1 = scope1.ServiceProvider.GetRequiredService<IBaseInterface1>();
        await Assert.That(s1_bif1_1)
            .IsNotNull();

        await Assert.That(s1_iif4)
            .IsSameReferenceAs(s1_iif3);
        await Assert.That(s1_iif3)
            .IsSameReferenceAs(s1_iif2);
        await Assert.That(s1_iif2)
            .IsSameReferenceAs(s1_bif1_1);

        var s1_bif1_2 = scope1.ServiceProvider.GetRequiredService<IBaseInterface1>();
        await Assert.That(s1_bif1_2)
            .IsNotNull();
        await Assert.That(s1_bif1_2)
            .IsEqualTo(s1_bif1_1);

        using var scope2 = sp.CreateScope();

        var s2_bif1_1 = scope2.ServiceProvider.GetRequiredService<IBaseInterface1>();
        await Assert.That(s2_bif1_1)
            .IsNotNull();

        var s2_bif1_2 = scope2.ServiceProvider.GetRequiredService<IBaseInterface1>();
        await Assert.That(s2_bif1_2)
            .IsNotNull();
        await Assert.That(s2_bif1_2)
            .IsEqualTo(s2_bif1_1);

        await Assert.That(s2_bif1_1)
            .IsNotEqualTo(s1_bif1_1);
    }

    [Test]
    public async Task AddIf_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services
            .AddSingletonIf<IBaseInterface1, A4_1>(true)
            .AddSingletonIf<IInheritedInterface2, A4_1>(false)
            .AddTransientIf<IBaseInterface2, B4_1>(true)
            .AddTransientIf<IBaseInterface3, B4_1>(false)
            .AddScopedIf<IBaseInterface4, B4_2>(true)
            .AddScopedIf<IBaseInterface5, B4_2>(false);

        using var sp = services.BuildServiceProvider();
        using var scope1 = sp.CreateScope();

        var a41_1 = sp.GetService<IBaseInterface1>();
        await Assert.That(a41_1)
            .IsNotNull();
        var a41_2 = sp.GetService<IInheritedInterface2>();
        await Assert.That(a41_2)
            .IsNull();

        var b41_1 = sp.GetService<IBaseInterface2>();
        await Assert.That(b41_1)
            .IsNotNull();
        var b41_2 = sp.GetService<IBaseInterface3>();
        await Assert.That(b41_2)
            .IsNull();

        var b42_1 = scope1.ServiceProvider.GetService<IBaseInterface4>();
        await Assert.That(b42_1)
            .IsNotNull();
        var b42_2 = scope1.ServiceProvider.GetService<IBaseInterface5>();
        await Assert.That(b42_2)
            .IsNull();
    }

    [Test]
    public async Task AddIfFact_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services
            .AddSingletonIf<IBaseInterface1, A4_1>(true, sp => new A4_1())
            .AddSingletonIf<IInheritedInterface2, A4_1>(false, sp => new A4_1())
            .AddTransientIf<IBaseInterface2, B4_1>(true, sp => new B4_1())
            .AddTransientIf<IBaseInterface3, B4_1>(false, sp => new B4_1())
            .AddScopedIf<IBaseInterface4, B4_2>(true, sp => new B4_2())
            .AddScopedIf<IBaseInterface5, B4_2>(false, sp => new B4_2());

        using var sp = services.BuildServiceProvider();
        using var scope1 = sp.CreateScope();

        var a41_1 = sp.GetService<IBaseInterface1>();
        await Assert.That(a41_1)
            .IsNotNull();
        var a41_2 = sp.GetService<IInheritedInterface2>();
        await Assert.That(a41_2)
            .IsNull();

        var b41_1 = sp.GetService<IBaseInterface2>();
        await Assert.That(b41_1)
            .IsNotNull();
        var b41_2 = sp.GetService<IBaseInterface3>();
        await Assert.That(b41_2)
            .IsNull();

        var b42_1 = scope1.ServiceProvider.GetService<IBaseInterface4>();
        await Assert.That(b42_1)
            .IsNotNull();
        var b42_2 = scope1.ServiceProvider.GetService<IBaseInterface5>();
        await Assert.That(b42_2)
            .IsNull();
    }

    // ===== New tests for 2-service registrations =====

    [Test]
    public async Task AddSingleton_TwoServices_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services.AddSingletonIf<IInheritedInterface2, IBaseInterface1, A4_1>(true);

        using var sp = services.BuildServiceProvider();

        var svc1 = sp.GetRequiredService<IInheritedInterface2>();
        await Assert.That(svc1)
            .IsNotNull();
        var svc2 = sp.GetRequiredService<IBaseInterface1>();
        await Assert.That(svc2)
            .IsNotNull();

        await Assert.That(svc1)
            .IsSameReferenceAs(svc2);
    }

    [Test]
    public async Task AddSingletonWithFactory_TwoServices_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services.AddSingletonIf<IInheritedInterface2, IBaseInterface1, A4_1>(true, sp => new A4_1());

        using var sp = services.BuildServiceProvider();

        var svc1 = sp.GetRequiredService<IInheritedInterface2>();
        await Assert.That(svc1)
            .IsNotNull();
        var svc2 = sp.GetRequiredService<IBaseInterface1>();
        await Assert.That(svc2)
            .IsNotNull();

        await Assert.That(svc1)
            .IsSameReferenceAs(svc2);
    }

    [Test]
    public async Task AddTransient_TwoServices_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services.AddTransientIf<IBaseInterface1, IBaseInterface2, B4_1>(true);

        using var sp = services.BuildServiceProvider();

        var svc1 = sp.GetRequiredService<IBaseInterface1>();
        await Assert.That(svc1)
            .IsNotNull();
        var svc2 = sp.GetRequiredService<IBaseInterface2>();
        await Assert.That(svc2)
            .IsNotNull();

        var svc1_2 = sp.GetRequiredService<IBaseInterface1>();
        await Assert.That(svc1_2)
            .IsNotNull();
        await Assert.That(svc1_2)
            .IsNotSameReferenceAs(svc1);
    }

    [Test]
    public async Task AddTransientWithFactory_TwoServices_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services.AddTransientIf<IBaseInterface1, IBaseInterface2, B4_1>(true, sp => new B4_1());

        using var sp = services.BuildServiceProvider();

        var svc1 = sp.GetRequiredService<IBaseInterface1>();
        await Assert.That(svc1)
            .IsNotNull();
        var svc2 = sp.GetRequiredService<IBaseInterface2>();
        await Assert.That(svc2)
            .IsNotNull();

        var svc1_2 = sp.GetRequiredService<IBaseInterface1>();
        await Assert.That(svc1_2)
            .IsNotNull();
        await Assert.That(svc1_2)
            .IsNotSameReferenceAs(svc1);
    }

    [Test]
    public async Task AddScoped_TwoServices_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services.AddScopedIf<IInheritedInterface2, IBaseInterface1, A4_1>(true);

        using var sp = services.BuildServiceProvider();

        using var scope1 = sp.CreateScope();

        var s1_svc1 = scope1.ServiceProvider.GetRequiredService<IInheritedInterface2>();
        await Assert.That(s1_svc1)
            .IsNotNull();
        var s1_svc2 = scope1.ServiceProvider.GetRequiredService<IBaseInterface1>();
        await Assert.That(s1_svc2)
            .IsNotNull();

        await Assert.That(s1_svc1)
            .IsSameReferenceAs(s1_svc2);

        using var scope2 = sp.CreateScope();

        var s2_svc1 = scope2.ServiceProvider.GetRequiredService<IInheritedInterface2>();
        await Assert.That(s2_svc1)
            .IsNotNull();
        await Assert.That(s2_svc1)
            .IsNotSameReferenceAs(s1_svc1);
    }

    [Test]
    public async Task AddScopedWithFactory_TwoServices_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services.AddScopedIf<IInheritedInterface2, IBaseInterface1, A4_1>(true, sp => new A4_1());

        using var sp = services.BuildServiceProvider();

        using var scope1 = sp.CreateScope();

        var s1_svc1 = scope1.ServiceProvider.GetRequiredService<IInheritedInterface2>();
        await Assert.That(s1_svc1)
            .IsNotNull();
        var s1_svc2 = scope1.ServiceProvider.GetRequiredService<IBaseInterface1>();
        await Assert.That(s1_svc2)
            .IsNotNull();

        await Assert.That(s1_svc1)
            .IsSameReferenceAs(s1_svc2);

        using var scope2 = sp.CreateScope();

        var s2_svc1 = scope2.ServiceProvider.GetRequiredService<IInheritedInterface2>();
        await Assert.That(s2_svc1)
            .IsNotNull();
        await Assert.That(s2_svc1)
            .IsNotSameReferenceAs(s1_svc1);
    }

    // ===== New tests for 3-service registrations =====

    [Test]
    public async Task AddSingleton_ThreeServices_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services.AddSingletonIf<IInheritedInterface3, IInheritedInterface2, IBaseInterface1, A4_1>(true);

        using var sp = services.BuildServiceProvider();

        var svc1 = sp.GetRequiredService<IInheritedInterface3>();
        await Assert.That(svc1)
            .IsNotNull();
        var svc2 = sp.GetRequiredService<IInheritedInterface2>();
        await Assert.That(svc2)
            .IsNotNull();
        var svc3 = sp.GetRequiredService<IBaseInterface1>();
        await Assert.That(svc3)
            .IsNotNull();

        await Assert.That(svc1)
            .IsSameReferenceAs(svc2);
        await Assert.That(svc2)
            .IsSameReferenceAs(svc3);
    }

    [Test]
    public async Task AddSingletonWithFactory_ThreeServices_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services.AddSingletonIf<IInheritedInterface3, IInheritedInterface2, IBaseInterface1, A4_1>(true, sp => new A4_1());

        using var sp = services.BuildServiceProvider();

        var svc1 = sp.GetRequiredService<IInheritedInterface3>();
        await Assert.That(svc1)
            .IsNotNull();
        var svc2 = sp.GetRequiredService<IInheritedInterface2>();
        await Assert.That(svc2)
            .IsNotNull();
        var svc3 = sp.GetRequiredService<IBaseInterface1>();
        await Assert.That(svc3)
            .IsNotNull();

        await Assert.That(svc1)
            .IsSameReferenceAs(svc2);
        await Assert.That(svc2)
            .IsSameReferenceAs(svc3);
    }

    [Test]
    public async Task AddTransient_ThreeServices_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services.AddTransientIf<IBaseInterface1, IBaseInterface2, IBaseInterface3, B4_1>(true);

        using var sp = services.BuildServiceProvider();

        var svc1 = sp.GetRequiredService<IBaseInterface1>();
        await Assert.That(svc1)
            .IsNotNull();
        var svc2 = sp.GetRequiredService<IBaseInterface2>();
        await Assert.That(svc2)
            .IsNotNull();
        var svc3 = sp.GetRequiredService<IBaseInterface3>();
        await Assert.That(svc3)
            .IsNotNull();

        var svc1_2 = sp.GetRequiredService<IBaseInterface1>();
        await Assert.That(svc1_2)
            .IsNotNull();
        await Assert.That(svc1_2)
            .IsNotSameReferenceAs(svc1);
    }

    [Test]
    public async Task AddTransientWithFactory_ThreeServices_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services.AddTransientIf<IBaseInterface1, IBaseInterface2, IBaseInterface3, B4_1>(true, sp => new B4_1());

        using var sp = services.BuildServiceProvider();

        var svc1 = sp.GetRequiredService<IBaseInterface1>();
        await Assert.That(svc1)
            .IsNotNull();
        var svc2 = sp.GetRequiredService<IBaseInterface2>();
        await Assert.That(svc2)
            .IsNotNull();
        var svc3 = sp.GetRequiredService<IBaseInterface3>();
        await Assert.That(svc3)
            .IsNotNull();

        var svc1_2 = sp.GetRequiredService<IBaseInterface1>();
        await Assert.That(svc1_2)
            .IsNotNull();
        await Assert.That(svc1_2)
            .IsNotSameReferenceAs(svc1);
    }

    [Test]
    public async Task AddScoped_ThreeServices_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services.AddScopedIf<IInheritedInterface3, IInheritedInterface2, IBaseInterface1, A4_1>(true);

        using var sp = services.BuildServiceProvider();

        using var scope1 = sp.CreateScope();

        var s1_svc1 = scope1.ServiceProvider.GetRequiredService<IInheritedInterface3>();
        await Assert.That(s1_svc1)
            .IsNotNull();
        var s1_svc2 = scope1.ServiceProvider.GetRequiredService<IInheritedInterface2>();
        await Assert.That(s1_svc2)
            .IsNotNull();
        var s1_svc3 = scope1.ServiceProvider.GetRequiredService<IBaseInterface1>();
        await Assert.That(s1_svc3)
            .IsNotNull();

        await Assert.That(s1_svc1)
            .IsSameReferenceAs(s1_svc2);
        await Assert.That(s1_svc2)
            .IsSameReferenceAs(s1_svc3);

        using var scope2 = sp.CreateScope();

        var s2_svc1 = scope2.ServiceProvider.GetRequiredService<IInheritedInterface3>();
        await Assert.That(s2_svc1)
            .IsNotNull();
        await Assert.That(s2_svc1)
            .IsNotSameReferenceAs(s1_svc1);
    }

    [Test]
    public async Task AddScopedWithFactory_ThreeServices_ShouldSucceed()
    {
        var services = new ServiceCollection();

        services.AddScopedIf<IInheritedInterface3, IInheritedInterface2, IBaseInterface1, A4_1>(true, sp => new A4_1());

        using var sp = services.BuildServiceProvider();

        using var scope1 = sp.CreateScope();

        var s1_svc1 = scope1.ServiceProvider.GetRequiredService<IInheritedInterface3>();
        await Assert.That(s1_svc1)
            .IsNotNull();
        var s1_svc2 = scope1.ServiceProvider.GetRequiredService<IInheritedInterface2>();
        await Assert.That(s1_svc2)
            .IsNotNull();
        var s1_svc3 = scope1.ServiceProvider.GetRequiredService<IBaseInterface1>();
        await Assert.That(s1_svc3)
            .IsNotNull();

        await Assert.That(s1_svc1)
            .IsSameReferenceAs(s1_svc2);
        await Assert.That(s1_svc2)
            .IsSameReferenceAs(s1_svc3);

        using var scope2 = sp.CreateScope();

        var s2_svc1 = scope2.ServiceProvider.GetRequiredService<IInheritedInterface3>();
        await Assert.That(s2_svc1)
            .IsNotNull();
        await Assert.That(s2_svc1)
            .IsNotSameReferenceAs(s1_svc1);
    }

    // ===== New tests for false condition on multi-service registrations =====

    [Test]
    public async Task AddIf_MultiService_FalseCondition_ShouldNotRegister()
    {
        var services = new ServiceCollection();

        // Singleton: 2, 3, 4 services with false condition
        services
            .AddSingletonIf<IInheritedInterface2, IBaseInterface1, A4_1>(false)
            .AddSingletonIf<IInheritedInterface3, IInheritedInterface2, IBaseInterface1, A4_1>(false)
            .AddSingletonIf<IInheritedInterface4, IInheritedInterface3, IInheritedInterface2, IBaseInterface1, A4_1>(false);

        // Transient: 2, 3, 4 services with false condition
        services
            .AddTransientIf<IBaseInterface1, IBaseInterface2, B4_1>(false)
            .AddTransientIf<IBaseInterface1, IBaseInterface2, IBaseInterface3, B4_1>(false)
            .AddTransientIf<IBaseInterface1, IBaseInterface2, IBaseInterface3, IBaseInterface4, B4_1>(false);

        // Scoped: 2, 3, 4 services with false condition
        services
            .AddScopedIf<IBaseInterface2, IBaseInterface3, B4_1>(false)
            .AddScopedIf<IBaseInterface2, IBaseInterface3, IBaseInterface4, B4_1>(false)
            .AddScopedIf<IBaseInterface2, IBaseInterface3, IBaseInterface4, IBaseInterface5, B4_2>(false);

        using var sp = services.BuildServiceProvider();
        using var scope = sp.CreateScope();

        await Assert.That(sp.GetService<IInheritedInterface4>())
            .IsNull();
        await Assert.That(sp.GetService<IInheritedInterface3>())
            .IsNull();
        await Assert.That(sp.GetService<IInheritedInterface2>())
            .IsNull();
        await Assert.That(sp.GetService<IBaseInterface1>())
            .IsNull();
        await Assert.That(sp.GetService<IBaseInterface2>())
            .IsNull();
        await Assert.That(scope.ServiceProvider.GetService<IBaseInterface3>())
            .IsNull();
        await Assert.That(scope.ServiceProvider.GetService<IBaseInterface4>())
            .IsNull();
        await Assert.That(scope.ServiceProvider.GetService<IBaseInterface5>())
            .IsNull();
    }

    [Test]
    public async Task AddIfWithFactory_MultiService_FalseCondition_ShouldNotRegister()
    {
        var services = new ServiceCollection();

        // Singleton: 2, 3, 4 services with false condition + factory
        services
            .AddSingletonIf<IInheritedInterface2, IBaseInterface1, A4_1>(false, sp => new A4_1())
            .AddSingletonIf<IInheritedInterface3, IInheritedInterface2, IBaseInterface1, A4_1>(false, sp => new A4_1())
            .AddSingletonIf<IInheritedInterface4, IInheritedInterface3, IInheritedInterface2, IBaseInterface1, A4_1>(false, sp => new A4_1());

        // Transient: 2, 3, 4 services with false condition + factory
        services
            .AddTransientIf<IBaseInterface1, IBaseInterface2, B4_1>(false, sp => new B4_1())
            .AddTransientIf<IBaseInterface1, IBaseInterface2, IBaseInterface3, B4_1>(false, sp => new B4_1())
            .AddTransientIf<IBaseInterface1, IBaseInterface2, IBaseInterface3, IBaseInterface4, B4_1>(false, sp => new B4_1());

        // Scoped: 2, 3, 4 services with false condition + factory
        services
            .AddScopedIf<IBaseInterface2, IBaseInterface3, B4_1>(false, sp => new B4_1())
            .AddScopedIf<IBaseInterface2, IBaseInterface3, IBaseInterface4, B4_1>(false, sp => new B4_1())
            .AddScopedIf<IBaseInterface2, IBaseInterface3, IBaseInterface4, IBaseInterface5, B4_2>(false, sp => new B4_2());

        using var sp = services.BuildServiceProvider();
        using var scope = sp.CreateScope();

        await Assert.That(sp.GetService<IInheritedInterface4>())
            .IsNull();
        await Assert.That(sp.GetService<IInheritedInterface3>())
            .IsNull();
        await Assert.That(sp.GetService<IInheritedInterface2>())
            .IsNull();
        await Assert.That(sp.GetService<IBaseInterface1>())
            .IsNull();
        await Assert.That(sp.GetService<IBaseInterface2>())
            .IsNull();
        await Assert.That(scope.ServiceProvider.GetService<IBaseInterface3>())
            .IsNull();
        await Assert.That(scope.ServiceProvider.GetService<IBaseInterface4>())
            .IsNull();
        await Assert.That(scope.ServiceProvider.GetService<IBaseInterface5>())
            .IsNull();
    }
}

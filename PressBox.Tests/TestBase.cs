using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace PressBox.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<PressBoxApi.Program>
{
    private readonly Action<IServiceCollection>? _overrideDependencies;

    public CustomWebApplicationFactory(Action<IServiceCollection>? overrideDependencies = null)
    {
        _overrideDependencies = overrideDependencies;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services => _overrideDependencies?.Invoke(services));
    }
}
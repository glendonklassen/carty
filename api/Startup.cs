using GK.Carty;
using GK.Carty.Api;
using GK.Carty.Rules;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace GK.Carty;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services
            .AddTransient<IMapRule<TerrainType>, RoadRule>()
            .AddScoped<IRandomProvider, BaseRandomProvider>();
    }
}
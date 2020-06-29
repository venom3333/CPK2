using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CPK.Contracts;
using CPK.Spa.Client.Core.HttpContext;
using CPK.Spa.Client.Core.Repositories;
using CPK.Spa.Client.Core.Services;
using CPK.Spa.Client.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CPK.Spa.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("START MAIN");
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            await ConfigureServices(builder.Services);
            builder.Logging.SetMinimumLevel(LogLevel.Trace);
            await builder.Build().RunAsync();
            Console.WriteLine("END MAIN");
        }

        private static async Task<ConfigModel> GetConfigAsync(IServiceCollection services)
        {
            using (var provider = services.BuildServiceProvider())
            {
                var nm = provider.GetRequiredService<NavigationManager>();
                var uri = nm.BaseUri;
                Console.WriteLine($"BASE URI: {uri}");
                var url = $"{(uri.EndsWith('/') ? uri : uri + "/")}api/v1/config";
                using var client = new HttpClient();
                return await client.GetFromJsonAsync<ConfigModel>(url);
            }
        }

        private static async Task ConfigureServices(IServiceCollection services)
        {
            var cfg = await GetConfigAsync(services);
            services.AddScoped<ConfigModel>(s => cfg);
            Console.WriteLine($"API URI IN STARTUP: {cfg?.ApiUri}");
            services.AddHttpClient("AuthClient", client => 
                    client.BaseAddress = new Uri(cfg.ApiUri))
                .AddHttpMessageHandler(sp =>
                    sp.GetRequiredService<AuthorizationMessageHandler>()
                        .ConfigureHandler(new[] { cfg.ApiUri },scopes: new[] { "api" }));

            services.AddHttpClient<PublicClient>("PublicClient", client =>
                    client.BaseAddress = new Uri(cfg.ApiUri));

            services.AddTransient(sp => 
                sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthClient"));
            Console.WriteLine($"SSO URI IN STARTUP: {cfg?.SsoUri}");
            services.AddOidcAuthentication(x =>
            {
                x.ProviderOptions.Authority = cfg.SsoUri;
                x.ProviderOptions.ClientId = "spaBlazorClient";
                x.ProviderOptions.ResponseType = "code";
                x.ProviderOptions.DefaultScopes.Add("api");
                x.UserOptions.RoleClaim = "role";
            });
            services.AddScoped<IHttpContext, HttpContext>();
            services.AddScoped<IApiRepository, ApiRepository>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ProductsViewModel>();
            services.AddScoped<BasketViewModel>();
            services.AddScoped<OrdersViewModel>();
        }
    }
}

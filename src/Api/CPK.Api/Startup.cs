using System;
using System.Collections.Generic;
using System.IO;
using CPK.Api.Helpers;
using CPK.Api.SecondaryAdapters;
using CPK.Api.SecondaryAdapters.Dto;
using CPK.Api.SecondaryAdapters.Repositories;
using CPK.Api.SecondaryAdapters.UnitsOfWork;
using CPK.BasketModule.PrimaryAdapters;
using CPK.BasketModule.PrimaryPorts;
using CPK.BasketModule.SecondaryPorts;
using CPK.FilesModule.PrimaryAdapters;
using CPK.FilesModule.PrimaryPorts;
using CPK.FilesModule.SecondaryPorts;
using CPK.OrdersModule.PrimaryAdapters;
using CPK.OrdersModule.PrimaryPorts;
using CPK.OrdersModule.SecondaryPorts;
using CPK.ProductsModule.Entities;
using CPK.ProductsModule.PrimaryAdapters;
using CPK.ProductsModule.PrimaryPorts;
using CPK.ProductsModule.SecondaryPorts;
using CPK.SharedConfiguration;
using FluentValidationGuard;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using File = CPK.FilesModule.Entities.File;

namespace CPK.Api
{
    internal sealed class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var config = new ConfigDto();
            Configuration.Bind(config);
            config.FilesDir = Path.Combine(Environment.WebRootPath, "files");
            services.AddSingleton<IConfig>(p => config);
            services.AddControllers();
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = config.Authority;
                    options.Audience = config.ApiName;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters.ValidateIssuer = false;
                });

            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CPK API", Version = "v1" });
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri($"{config.IdentityUrlExternal}/connect/authorize"),
                            TokenUrl = new Uri($"{config.IdentityUrlExternal}/connect/token"),
                            Scopes = new Dictionary<string, string>()
                            {
                                { "api", "CPK API" }
                            }
                        }
                    }
                });

                c.OperationFilter<AuthorizeCheckOperationFilter>();

            });

            services.AddDbContextPool<CpkContext>(builder =>
                builder.UseNpgsql(config.ConnectionString));
            services.AddHttpContextAccessor();
            services.AddTransient<IOrdersUow, OrdersUow>();
            services.AddTransient<IBasketUow, BasketUow>();
            services.AddTransient<IProductsUow, ProductsUow>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IBasketRepository, BasketRepository>();
            services.AddTransient<IProductsRepository, ProductRepository>();
            services.AddTransient<IProductsService, ProductsService>();
            services.AddTransient<IBasketService, BasketService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IErrorConverter, ErrorConverter>();
            services.AddTransient<IFilesUow, FilesUow>();
            services.AddTransient<IFilesRepository, FilesRepository>();
            services.AddTransient<IFilesService, FilesService>();
            services.AddTransient<IConfigRepository, ConfigRepository>();
            IdentityModelEventSource.ShowPII = true;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider sp)
        {
            using (var scope = sp.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<CpkContext>();
                //db.Database.EnsureDeleted();
                //db.Database.EnsureCreated();
                if (SharedConfig.IsProduction)
                {
                    db.Database.Migrate();
                }
                var files = scope.ServiceProvider.GetRequiredService<IFilesService>();
                var items = scope.ServiceProvider.GetRequiredService<IProductsService>();
                for (int i = 1; i < 10; i++)
                {
                    using (var stream = System.IO.File.OpenRead(Path.Combine(Environment.WebRootPath, $"{i}.png")))
                    {
                        var imageId = files.Upload(new File($"image{i}.png", stream, "image/png")).Result;
                        var _ = items.Add(new Product(new Title($"image{i}"), new Money(i), new Image(imageId))).Result;
                    }
                }
            }
            app.UseForwardedHeaders();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CPK API V1");
                c.RoutePrefix = string.Empty;
                c.OAuthClientId("apiSwaggerUi");
                c.OAuthAppName("CPK Swagger UI");
            });
            app.UseExceptionHandler("/api/v1/error");
            app.UseRouting();
            app.UseCors("default");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

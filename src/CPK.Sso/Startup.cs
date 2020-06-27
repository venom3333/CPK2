using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using CPK.Sso.Configuration.AutoMapper;
using CPK.Sso.Devspaces;
using CPK.Sso.Data;
using CPK.Sso.Models;
using CPK.Sso.Services;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CPK.Sso
{
    public class Startup
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
            services.AddControllers();
            services.AddControllersWithViews();
            services.AddRazorPages();

            // configures IIS out-of-proc settings (see https://github.com/aspnet/AspNetCore/issues/14882)
            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            // configures IIS in-proc settings
            services.Configure<IISServerOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
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

            // Add framework services.
            //services.AddDbContextPool<ApplicationDbContext>(builder =>
            //    builder.UseNpgsql(Configuration["ConnectionString"]));
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(Configuration["ConnectionString"],
                    npgsqlOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                        //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorCodesToAdd: null);
                    }));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<ILoginService<ApplicationUser>, EFLoginService>();
            services.AddTransient<IRedirectService, RedirectService>();

            var connectionString = Configuration["ConnectionString"];
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer(x =>
                {
                    x.IssuerUri = null;
                    x.Discovery.ShowApiScopes = true;
                    x.Discovery.ShowClaims = true;
                    x.Discovery.ShowEndpoints = true;
                    x.Discovery.ShowExtensionGrantTypes = true;
                    x.Discovery.ShowGrantTypes = true;
                    x.Discovery.ShowIdentityScopes = true;
                    x.Discovery.ShowKeySet = true;
                    x.Discovery.ShowResponseModes = true;
                    x.Discovery.ShowResponseTypes = true;
                    x.Discovery.ShowTokenEndpointAuthenticationMethods = true;
                    x.Endpoints.EnableAuthorizeEndpoint = true;
                    x.Endpoints.EnableJwtRequestUri = true;
                    x.Authentication.CookieLifetime = TimeSpan.FromHours(2);
                    x.MutualTls.Enabled = false;
                    x.Events.RaiseErrorEvents = true;
                    x.Events.RaiseInformationEvents = true;
                    x.Events.RaiseFailureEvents = true;
                    x.Events.RaiseSuccessEvents = true;
                })
                .AddAspNetIdentity<ApplicationUser>()
                .AddDefaultEndpoints()
                    .AddConfigurationStore(options =>
                    {
                        options.ConfigureDbContext = builder => builder.UseNpgsql(connectionString,
                            npgsqlOptionsAction: sqlOptions =>
                            {
                                sqlOptions.MigrationsAssembly(migrationsAssembly);
                                //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                                sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30),
                                    errorCodesToAdd: null);
                            });
                    })
                    .AddOperationalStore(options =>
                    {
                        options.ConfigureDbContext = builder => builder.UseNpgsql(connectionString,
                            npgsqlOptionsAction: sqlOptions =>
                            {
                                sqlOptions.MigrationsAssembly(migrationsAssembly);
                                //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                                sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30),
                                    errorCodesToAdd: null);
                            });
                    })
                    .AddProfileService<ProfileService>()
                    .AddDevspacesIfNeeded(Configuration.GetValue("EnableDevspaces", true))
                    .AddDeveloperSigningCredential();
            
            services.AddAutoMapper(typeof(UserManagementAutoMapperProfile));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("default");
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Models;
using GrantTypes = IdentityServer4.Models.GrantTypes;

namespace CPK.Sso.Configuration
{
    public static class Config
    {
        public static readonly string EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        public static bool IsProduction => EnvironmentName == "Production";

        public static EnvironmentNameEnum EnvironmentNameEnum
        {
            get
            {
                return EnvironmentName switch
                {
                    "Production" => EnvironmentNameEnum.Production,
                    "Staging" => EnvironmentNameEnum.Staging,
                    "Development" => EnvironmentNameEnum.Development,
                    _ => EnvironmentNameEnum.Development
                };
            }
        }

        public const int UserMailConfirmTimeHours = 24;

        // ApiResources define the apis in your system
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("api", "CPK API"),
            };
        }

        // Identity resources are data like user ID, name, or email address of a user
        // see: http://docs.identityserver.io/en/release/configuration/resources.html
        public static IEnumerable<IdentityResource> GetResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource("role", new []{ ClaimTypes.Role, JwtClaimTypes.Role})
            };
        }

        // client want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients(Dictionary<string, string> clientsUrl)
        {
            if (clientsUrl == null)
            {
                return new List<Client>();
            }
            return new List<Client>
            {
                    new Client
                {
                    ClientId = "spaBlazorClient",
                    ClientName = "SPA Blazor Client",
                    RequireClientSecret = false,
                    RequireConsent = false,
                    RedirectUris = new List<string>
                    {
                        $"{clientsUrl["SpaBlazor"]}/oidc/callbacks/authentication-redirect",
                        $"{clientsUrl["SpaBlazor"]}/authentication/login-callback",
                        $"{clientsUrl["SpaBlazor"]}/authentication/login-failed",
                        $"{clientsUrl["SpaBlazor"]}",
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        $"{clientsUrl["SpaBlazor"]}/oidc/callbacks/logout-redirect",
                        $"{clientsUrl["SpaBlazor"]}",
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                        $"{clientsUrl["SpaBlazor"]}",
                    },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = { "openid", "profile", "email", "api", "role" },
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse
                },
                    new Client
                {
                    ClientId = "apiSwaggerUi",
                    ClientName = "Api Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{clientsUrl["ApiSwagger"]}/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{clientsUrl["ApiSwagger"]}/" },
                    AllowedScopes =
                    {
                        "api",
                        "role"
                    }
                },
            };
        }
    }

    public enum EnvironmentNameEnum
    {
        Production,
        Staging,
        Development
    }
}
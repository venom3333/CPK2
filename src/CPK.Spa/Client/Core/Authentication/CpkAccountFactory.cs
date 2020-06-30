using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;

namespace CPK.Spa.Client.Core.Authentication
{
    public class CpkAccountFactory : AccountClaimsPrincipalFactory<CpkAccount>
    {
        public CpkAccountFactory(NavigationManager navigationManager, IAccessTokenProviderAccessor accessor)
            : base(accessor)
        {
        }

        public override async ValueTask<ClaimsPrincipal> CreateUserAsync(
            CpkAccount account,
            RemoteAuthenticationUserOptions options)
        {
            var initialUser = await base.CreateUserAsync(account, options);

            if (initialUser.Identity.IsAuthenticated)
            {
                foreach (var value in account.AuthenticationMethod)
                {
                    ((ClaimsIdentity) initialUser.Identity).AddClaim(new Claim("amr", value));
                }

                foreach (var value in account.Roles.Split(',',StringSplitOptions.RemoveEmptyEntries))
                {
                    ((ClaimsIdentity) initialUser.Identity).AddClaim(new Claim("role", value));
                }
            }

            return initialUser;
        }
    }
}
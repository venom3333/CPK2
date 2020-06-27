using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CPK.Sso.Extensions;
using CPK.Sso.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CPK.Sso.Data
{


    public class ApplicationDbContextSeed
    {
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher = new PasswordHasher<ApplicationUser>();

        public async Task SeedAsync(ApplicationDbContext context, IWebHostEnvironment env,
            ILogger<ApplicationDbContextSeed> logger, int? retry = 0)
        {
            int retryForAvaiability = retry.Value;

            try
            {
                var contentRootPath = env.ContentRootPath;
                var webroot = env.WebRootPath;

                if (!context.Users.Any())
                {
                    context.Users.AddRange(GetDefaultUser());
                    await context.SaveChangesAsync();
                    context.Roles.AddRange(
                        new IdentityRole("admin")
                        {
                            ConcurrencyStamp = Guid.NewGuid().ToString("D"),
                            NormalizedName = "ADMIN"
                        },
                        new IdentityRole("cpkadmin")
                        {
                            ConcurrencyStamp = Guid.NewGuid().ToString("D"),
                            NormalizedName = "CPKADMIN"
                        },
                        new IdentityRole("user")
                        {
                            ConcurrencyStamp = Guid.NewGuid().ToString("D"),
                            NormalizedName = "USER"
                        }
                    );
                    await context.SaveChangesAsync();
                    var user = await context.Users.FirstAsync(u => u.NormalizedUserName == "DEMOUSER@MICROSOFT.COM");
                    var admin = await context.Users.FirstAsync(u => u.NormalizedUserName == "DEMOADMIN@MICROSOFT.COM");
                    var userRole = await context.Roles.FirstAsync(x => x.Name == "user");
                    var adminRole = await context.Roles.FirstAsync(x => x.Name == "admin");
                    context.UserRoles.AddRange(
                        new IdentityUserRole<string>() { UserId = user.Id, RoleId = userRole.Id },
                        new IdentityUserRole<string>() { UserId = admin.Id, RoleId = userRole.Id },
                        new IdentityUserRole<string>() { UserId = admin.Id, RoleId = adminRole.Id });
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvaiability < 10)
                {
                    retryForAvaiability++;

                    logger.LogError(ex, "EXCEPTION ERROR while migrating {DbContextName}", nameof(ApplicationDbContext));

                    await SeedAsync(context, env, logger, retryForAvaiability);
                }
            }
        }

        private IEnumerable<ApplicationUser> GetDefaultUser()
        {
            var user =
            new ApplicationUser()
            {
                //CardHolderName = "DemoUser",
                //CardNumber = "4012888888881881",
                //CardType = 1,
                //City = "Redmond",
                //Country = "U.S.",
                Email = "demouser@microsoft.com",
                //Expiration = "12/20",
                Id = Guid.NewGuid().ToString(),
                //LastName = "DemoLastName",
                //Name = "DemoUser",
                PhoneNumber = "1234567890",
                UserName = "demouser@microsoft.com",
                //ZipCode = "98052",
                //State = "WA",
                //Street = "15703 NE 61st Ct",
                //SecurityNumber = "535",
                NormalizedEmail = "DEMOUSER@MICROSOFT.COM",
                NormalizedUserName = "DEMOUSER@MICROSOFT.COM",
                SecurityStamp = Guid.NewGuid().ToString("D"),
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, "Pass@word1");

            var admin =
                new ApplicationUser()
                {
                    //CardHolderName = "DemoAdmin",
                    //CardNumber = "4012888888881882",
                    //CardType = 1,
                    //City = "Redmond",
                    //Country = "U.S.",
                    Email = "demoadmin@microsoft.com",
                    //Expiration = "12/20",
                    Id = Guid.NewGuid().ToString(),
                    //LastName = "AdminLastName",
                    //Name = "DemoAdmin",
                    PhoneNumber = "1234567892",
                    UserName = "demoadmin@microsoft.com",
                    //ZipCode = "98053",
                    //State = "WA",
                    //Street = "15703 NE 61st Ct",
                    //SecurityNumber = "535",
                    NormalizedEmail = "DEMOADMIN@MICROSOFT.COM",
                    NormalizedUserName = "DEMOADMIN@MICROSOFT.COM",
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                };

            admin.PasswordHash = _passwordHasher.HashPassword(user, "Pass@word2");

            return new List<ApplicationUser>()
            {
                user,
                admin
            };
        }
    }
}

using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CPK.Sso.Configuration;
using CPK.Sso.Models;
using CPK.Sso.Models.AccountViewModels;
using CPK.Sso.Models.ManageViewModels;
using CPK.Sso.Services;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace CPK.Sso.Controllers
{
    /// <summary>
    /// This sample controller implements a typical login/logout/provision workflow for local accounts.
    /// The login service encapsulates the interactions with the user data store. This data store is in-memory only and cannot be used for production!
    /// The interaction service provides a way for the UI to communicate with identityserver for validation and context retrieval
    /// </summary>
    public class AccountController : BaseController
    {
        //private readonly InMemoryUserLoginService _loginService;
        private readonly ILoginService<ApplicationUser> _loginService;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AccountController(
            //InMemoryUserLoginService loginService,
            ILoginService<ApplicationUser> loginService,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            ILogger<AccountController> logger,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _loginService = loginService;
            _interaction = interaction;
            _clientStore = clientStore;
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Show login page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl).ConfigureAwait(false);
            if (context?.IdP != null)
            {
                throw new NotImplementedException("External login is not implemented!");
            }

            var vm = await BuildLoginViewModelAsync(returnUrl, context);

            ViewData["ReturnUrl"] = returnUrl;

            return View(vm);
        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _loginService.FindByUsername(model.Email);

                if (await _loginService.ValidateCredentials(user, model.Password))
                {
                    if (user.EmailConfirmed)
                    {
                        await SigninAsync(user, model.ReturnUrl, model.RememberMe).ConfigureAwait(false);

                        // make sure the returnUrl is still valid, and if yes - redirect back to authorize endpoint
                        if (_interaction.IsValidReturnUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }

                        return Redirect("~/");
                    }
                    else
                    {
                        ModelState.AddModelError("", ConstantMessages.RegisterConfirmationMessage(model.Email));
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Не верные логин или пароль");
                }
            }

            // something went wrong, show form with error
            var vm = await BuildLoginViewModelAsync(model);

            ViewData["ReturnUrl"] = model.ReturnUrl;

            return View(vm);
        }

        private async Task SigninAsync(ApplicationUser user, string returnUrl, bool rememberMe)
        {
            var tokenLifetime = _configuration.GetValue("TokenLifetimeMinutes", 120);

            var props = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(tokenLifetime),
                AllowRefresh = true,
                RedirectUri = returnUrl
            };

            if (rememberMe)
            {
                var permanentTokenLifetime = _configuration.GetValue("PermanentTokenLifetimeDays", 365);

                props.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(permanentTokenLifetime);
                props.IsPersistent = true;
            }

            await _loginService.SignInAsync(user, props);
        }

        /// <summary>
        /// Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                // if the user is not authenticated, then just show logged out page
                return await Logout(new LogoutViewModel {LogoutId = logoutId});
            }

            //Test for Xamarin. 
            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                //it's safe to automatically sign-out
                return await Logout(new LogoutViewModel {LogoutId = logoutId});
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            var vm = new LogoutViewModel
            {
                LogoutId = logoutId
            };
            return View(vm);
        }

        /// <summary>
        /// Handle logout page postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutViewModel model)
        {
            var idp = User?.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

            if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
            {
                if (model.LogoutId == null)
                {
                    // if there's no current logout context, we need to create one
                    // this captures necessary info from the current logged in user
                    // before we signout and redirect away to the external IdP for signout
                    model.LogoutId = await _interaction.CreateLogoutContextAsync();
                }

                string url = "/Account/Logout?logoutId=" + model.LogoutId;

                try
                {
                    // hack: try/catch to handle social providers that throw
                    await HttpContext.SignOutAsync(idp, new AuthenticationProperties
                    {
                        RedirectUri = url
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "LOGOUT ERROR: {ExceptionMessage}", ex.Message);
                }
            }

            // delete authentication cookie
            await HttpContext.SignOutAsync();

            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            // set this so UI rendering sees an anonymous user
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(model.LogoutId);

            return Redirect(logout?.PostLogoutRedirectUri ?? "/");
        }

        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    //CardHolderName = model.User.CardHolderName,
                    //CardNumber = model.User.CardNumber,
                    //CardType = model.User.CardType,
                    //City = model.User.City,
                    //Country = model.User.Country,
                    //Expiration = model.User.Expiration,
                    //LastName = model.User.LastName,
                    //Name = model.User.Name,
                    //Street = model.User.Street,
                    //State = model.User.State,
                    //ZipCode = model.User.ZipCode,
                    //PhoneNumber = model.User.PhoneNumber,
                    //SecurityNumber = model.User.SecurityNumber
                };

                var userDto = await _userManager.FindByNameAsync(user.UserName).ConfigureAwait(false);
                if (userDto != null
                    && !userDto.EmailConfirmed // не подтвержден
                    && userDto.Created < DateTime.Now - TimeSpan.FromHours(Config.UserMailConfirmTimeHours)
                ) // и висит больше суток
                {
                    var deleteResult = await _userManager.DeleteAsync(userDto).ConfigureAwait(false);
                    if (deleteResult.Errors.Any())
                    {
                        AddErrors(deleteResult);
                        // If we got this far, something failed, redisplay form
                        return View(model);
                    }
                }

                var result = await _userManager.CreateAsync(user, model.Password).ConfigureAwait(false);
                if (result.Errors.Any())
                {
                    AddErrors(result);
                    // If we got this far, something failed, redisplay form
                    return View(model);
                }

                userDto = await _userManager.FindByNameAsync(user.UserName).ConfigureAwait(false);
                await _userManager.AddToRoleAsync(userDto, "user").ConfigureAwait(false);

                // отправка емейла для подтверждения (в линк закладываем id юзера, hashCheck - хэш пароля, returnUrl - урл магазина, куда редиректнем после логина)
                // ...
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(userDto).ConfigureAwait(false);
                var link = GetConfirmationLink(userDto.Id, token, returnUrl);
                
                // TODO: Отправка email
                // await _emailSender.SendAsync(user.Id,
                //     "Подтверждение регистрации",
                //     $"Пожалуйста подтвердите регистрацию кликнув по ссылке: <a href=\"{link}\">Подтвердить</a>");
            }

            
            ViewBag.Message = ConstantMessages.RegisterConfirmationMessage(model.Email);
            return View("Success");
        }

        private string GetConfirmationLink(string id, string token, string returnUrl = null)
        {
            var confirmationUrl = Url.Action(
                "ConfirmEmail",
                "Account",
                new {id, token, returnUrl},
                protocol: HttpScheme.Https.ToString()
            );

            Log.Information($"Confirmation URL: {confirmationUrl}");
            return confirmationUrl;
        }

        [HttpGet]
        public IActionResult Redirecting()
        {
            return View();
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl, AuthorizationRequest context)
        {
            var allowLocal = true;
            if (context?.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;
                }
            }

            return new LoginViewModel
            {
                ReturnUrl = returnUrl,
                Email = context?.LoginHint,
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginViewModel model)
        {
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl).ConfigureAwait(false);
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl, context).ConfigureAwait(false);
            vm.Email = model.Email;
            vm.RememberMe = model.RememberMe;
            return vm;
        }

        // GET: /Account/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null)
                {
                    AddError(description: "Пользователь не найден");
                    return View(model);
                }

                var changePasswordResult =
                    await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (changePasswordResult.Errors.Any())
                {
                    AddErrors(changePasswordResult);
                    return View(model);
                }
            }

            if (returnUrl != null)
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                    return Redirect(returnUrl);
                else if (ModelState.IsValid)
                    return RedirectToAction("login", "Account", new {returnUrl = returnUrl});
                else
                    return View(model);
            }

            return RedirectToAction("index", "Home");
        }

        // GET: /Account/ForgotPassword
        [HttpGet]
        public IActionResult ForgotPassword(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email).ConfigureAwait(false);
                var isConfirmed = await _userManager.IsEmailConfirmedAsync(user).ConfigureAwait(false);
                model.EmailSent = true; // В любом случае показываем сообщщение что отправили на емейл ссылку
                if (user == null || !isConfirmed)
                {
                    return View(model);
                }

                // Формируем ссылку и посылаем емейл
                var token = await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
                var link = GetResetPasswordLink(user.Id, token, returnUrl);
                
                // TODO: Отправка email
                // await _emailSender.SendAsync(user.Id,
                //     "Сброс пароля",
                //     $"Пожалуйста сбросьте свой пароль кликнув по ссылке: <a href=\"{link}\">Подтвердить</a>");

                return View(model);
            }

            if (returnUrl != null)
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                    return Redirect(returnUrl);
                else if (ModelState.IsValid)
                    return RedirectToAction("login", "Account", new {returnUrl = returnUrl});
                else
                    return View(model);
            }

            return RedirectToAction("index", "Home");
        }

        private string GetResetPasswordLink(string userId, string token, string returnUrl)
        {
            var link = Url.Action("ResetPassword", "Account",
                new {id = userId, token = token, returnUrl = returnUrl}, protocol: HttpScheme.Https.ToString());
            Log.Information($"Reset password URL: {link}");
            return link;
        }

        /// <summary>
        /// Подтверждение email
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string id, string token, string returnUrl = null)
        {
            if (!string.IsNullOrWhiteSpace(id) && !string.IsNullOrWhiteSpace(token))
            {
                // Проверяем хэш у юзера по id
                var user = await _userManager.FindByIdAsync(id).ConfigureAwait(false);
                // если все ок, логиним и редиректим по returnUrl
                var confirmEmailResult = await _userManager.ConfirmEmailAsync(user, token).ConfigureAwait(false);
                if (confirmEmailResult.Errors.Any())
                {
                    AddErrors(confirmEmailResult);
                    return RedirectToAction("login", "Account", new {returnUrl = returnUrl});
                }

                await SigninAsync(user, returnUrl, false).ConfigureAwait(false);

                // make sure the returnUrl is still valid, and if yes - redirect back to authorize endpoint
                if (_interaction.IsValidReturnUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return Redirect("~/");
            }

            // если нет, редиректим на страницу логина
            Log.Error($"Ошибка подтверждения email: userID {id} hashCheck {token}");
            return View("Error");
        }

        public IActionResult ResetPassword(string id, string token, string returnurl = null)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(token))
            {
                return View("Error");
            }

            var model = new ResetPasswordViewModel
            {
                UserId = id,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.FindByIdAsync(model.UserId).Result;

                var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password)
                    .ConfigureAwait(false);
                if (result.Succeeded)
                {
                    ViewBag.Message = "Пароль успешно сброшен!";
                    return View("Success");
                }
                else
                {
                    AddError("Ошибка при сбросе пароля! (1)");
                    return View("Error");
                }
            }

            AddError("Ошибка при сбросе пароля! (2)");
            return View("Error");
        }
    }
}
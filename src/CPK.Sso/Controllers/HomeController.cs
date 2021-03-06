﻿using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CPK.Sso.Models;
using CPK.Sso.Services;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CPK.Sso.Controllers
{
    public class HomeController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IRedirectService _redirectSvc;

        public HomeController(IIdentityServerInteractionService interaction, IRedirectService redirectSvc)
        {
            _interaction = interaction;
            _redirectSvc = redirectSvc;
        }

        public IActionResult Index(string returnUrl)
        {
            return View(User?.Claims ?? new List<Claim>());
        }

        public IActionResult ReturnToOriginalApplication(string returnUrl)
        {
            if (returnUrl != null)
            {
                var extractedRedirect = _redirectSvc.ExtractRedirectUriFromReturnUrl(returnUrl);
                if (extractedRedirect == "/") extractedRedirect = returnUrl;
                return Redirect(extractedRedirect);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Shows the error page
        /// </summary>
        public async Task<IActionResult> Error(string errorId)
        {
            var vm = new ErrorViewModel();

            // retrieve error details from identityserver
            var message = await _interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                vm.Error = message;
            }

            return View("Error", vm);
        }
    }
}
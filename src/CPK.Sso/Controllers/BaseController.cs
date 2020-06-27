using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CPK.Sso.Controllers
{
    public class BaseController : Controller
    {
        protected void AddErrors(IdentityResult result)
        {
            if (result == null)
            {
                return;
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        protected void AddError(string description, string key = "")
        {
            ModelState.AddModelError(key, description);
        }
    }
}
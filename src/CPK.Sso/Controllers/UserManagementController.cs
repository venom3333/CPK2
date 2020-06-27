using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CPK.Sso.Models;
using CPK.Sso.Models.ManageViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CPK.Sso.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserManagementController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UserManagementController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        // GET
        // localhost:5001/UserManagement?returnurl=%2F
        public async Task<IActionResult> Index(string returnurl)
        {
            var identityUsers = _userManager.Users;

            var rolesDic = new Dictionary<string, IEnumerable<string>>();
            foreach (var user in identityUsers)
            {
                var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                rolesDic.TryAdd(user.Id, roles);
            }

            var users = await _mapper.ProjectTo<UserViewModel>(identityUsers).ToListAsync().ConfigureAwait(false);
            users.ForEach(u => { u.Roles = rolesDic[u.UserId]; });
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                Log.Error("Error in EditUser, parameter id is null or empty");
                return RedirectToAction("Index", "UserManagement");
            }

            var user = await _userManager.FindByIdAsync(id).ConfigureAwait(false);
            if (user == null)
            {
                Log.Error($"Error in EditUser, user with id {id} not found");
                return RedirectToAction("Index", "UserManagement");
            }

            var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            var userViewModel = _mapper.Map<UserViewModel>(user);
            userViewModel.Roles = roles;
            userViewModel.AvailableRoles = 
                (await _roleManager.Roles.ToListAsync().ConfigureAwait(false))
                .Select(r => r.Name);
            return View(userViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(UserViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(userModel.UserName).ConfigureAwait(false);
                user.EmailConfirmed = userModel.EmailConfirmed;
                var updateUserResult = await _userManager
                    .UpdateAsync(user)
                    .ConfigureAwait(false);

                if (updateUserResult.Errors.Any())
                {
                    AddErrors(updateUserResult);
                    return View(userModel);
                }

                var currentRoles = 
                    await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                var removeFromRolesResult = await _userManager
                    .RemoveFromRolesAsync(user, currentRoles)
                    .ConfigureAwait(false);
                
                if (removeFromRolesResult.Errors.Any())
                {
                    AddErrors(removeFromRolesResult);
                    return View(userModel);
                }
                
                var addToRolesResult = await _userManager
                    .AddToRolesAsync(user, userModel.Roles)
                    .ConfigureAwait(false);

                if (addToRolesResult.Errors.Any())
                {
                    AddErrors(addToRolesResult);
                    return View(userModel);
                }
            }

            return RedirectToAction("Index", "UserManagement");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                Log.Error("Error in DeleteUser, parameter id is null or empty");
                return RedirectToAction("Index", "UserManagement");
            }

            var user = await _userManager.FindByIdAsync(id).ConfigureAwait(false);
            if (user == null)
            {
                Log.Error($"Error in DeleteUser, user with id {id} not found");
                return RedirectToAction("Index", "UserManagement");
            }

            var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            var userViewModel = _mapper.Map<UserViewModel>(user);
            userViewModel.Roles = roles;
            return View(userViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUserConfirmed(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                Log.Error("Error in DeleteUserConfirmed, parameter id is null or empty");
                return RedirectToAction("Index", "UserManagement");
            }

            var user = await _userManager.FindByIdAsync(id).ConfigureAwait(false);
            if (user == null)
            {
                Log.Error($"Error in DeleteUser, user with id {id} not found");
                return RedirectToAction("Index", "UserManagement");
            }

            var deleteUserResult = await _userManager.DeleteAsync(user).ConfigureAwait(false);

            if (deleteUserResult.Errors.Any())
            {
                foreach (var error in deleteUserResult.Errors)
                {
                    Log.Error($"Error in DeleteUser: {error.Code}: {error.Description}");
                }
            }

            return RedirectToAction("Index", "UserManagement");
        }
    }
}
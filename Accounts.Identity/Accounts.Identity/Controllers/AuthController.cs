using Accounts.Identity.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.Identity.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager; //for user sign in
        private readonly UserManager<AppUser> _userManager; //for searching for a user
        private readonly IIdentityServerInteractionService _interactionService; //for log out

        public AuthController(SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            IIdentityServerInteractionService interactionService) =>
            (_signInManager, _userManager, _interactionService) = (signInManager, userManager, interactionService);

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var viewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = await _userManager.FindByNameAsync(viewModel.Username);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found");
                return View(viewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(viewModel.Username,
                viewModel.Password, false, false);
            if(result.Succeeded)
            {
                return Redirect(viewModel.ReturnUrl);
            }
            ModelState.AddModelError(string.Empty, "Login Error");
            return View(viewModel);
        }
        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            var viewModel = new RegisterViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(returnUrl);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = new AppUser
            {
                UserName = viewModel.Username
            };
            var result = await _userManager.CreateAsync(user, viewModel.Password); //creating a user
            if(result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false); //sign in with user account
                return Redirect(viewModel.ReturnUrl);
            }
            ModelState.AddModelError(string.Empty, "An error occured!");
            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();
            //using interaction service to get logout context -> PostLogoutRedirectUri
            var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);
            return Redirect(logoutRequest.PostLogoutRedirectUri);
        }
    }
}

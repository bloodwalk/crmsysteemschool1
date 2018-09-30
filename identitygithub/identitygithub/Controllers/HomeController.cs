using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PluralsightDemo.Models;

namespace PluralsightDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<PluralsightUser> userManager;
        private readonly IUserClaimsPrincipalFactory<PluralsightUser> claimsPrincipalFactory;

        public HomeController(UserManager<PluralsightUser> userManager,
            IUserClaimsPrincipalFactory<PluralsightUser> claimsPrincipalFactory)
        {
            this.userManager = userManager;
            this.claimsPrincipalFactory = claimsPrincipalFactory;
        }
        

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //[HttpGet]
        //public IActionResult Register()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register(RegisterModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await userManager.FindByNameAsync(model.UserName);

        //        if (user == null)
        //        {
        //            user = new PluralsightUser
        //            {
        //                Id = Guid.NewGuid().ToString(),
        //                UserName = model.UserName,
        //                Email = model.UserName
        //            };

        //            var result = await userManager.CreateAsync(user, model.Password);

        //            if (result.Succeeded)
        //            {
        //                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        //                var confirmationEmail = Url.Action("ConfirmEmailAddress", "Home",
        //                    new {token = token, email = user.Email}, Request.Scheme);
        //                System.IO.File.WriteAllText("confirmationLink.txt", confirmationEmail);
        //            }
        //            else
        //            {
        //                foreach (var error in result.Errors)
        //                {
        //                    ModelState.AddModelError("", error.Description);
        //                }

        //                return View();
        //            }
        //        }

        //        return View("Success");
        //    }

        //    return View();
        //}

        ////[HttpGet]
        ////public async Task<IActionResult> ConfirmEmailAddress(string token, string email)
        ////{
        ////    var user = await userManager.FindByEmailAsync(email);

        ////    if (user != null)
        ////    {
        ////        var result = await userManager.ConfirmEmailAsync(user, token);

        ////        if (result.Succeeded)
        ////        {
        ////            return View("Success");
        ////        }
        ////    }

        ////    return View("Error");
        ////}

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                
                var user = await userManager.FindByNameAsync(model.UserName);

                if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                {
                    //if (!await userManager.IsEmailConfirmedAsync(user))
                    //{
                    //    ModelState.AddModelError("","Email is not confirmed");
                    //    return View();
                    //}

                    var principal = await claimsPrincipalFactory.CreateAsync(user);

                    await HttpContext.SignInAsync("Identity.Application", principal);

                    //inlog logic
                    var isAdmin = await userManager.IsInRoleAsync(user,Constants.AdministratorRole);

                    if (isAdmin) {

                        return RedirectToAction("Admin");
                        //sdfdsfsdfsdf
                    }
                
                    return RedirectToAction("Leerling");


                }

                ModelState.AddModelError("", "Invalid UserName or Password");
            }

            return View();
        }



        //// misschien voor later
        //[HttpGet]
        //public IActionResult ForgotPassword()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await userManager.FindByEmailAsync(model.Email);

        //        if (user != null)
        //        {
        //            var token = await userManager.GeneratePasswordResetTokenAsync(user);
        //            var resetUrl = Url.Action("ResetPassword", "Home",
        //                new {token = token, email = user.Email}, Request.Scheme);

        //            System.IO.File.WriteAllText("resetLink.txt", resetUrl);
        //        }
        //        else
        //        {
        //            // email user and inform them that they do not have an account
        //        }

        //        return View("Success");
        //    }
        //    return View();
        //}

        //[HttpGet]
        //public IActionResult ResetPassword(string token, string email)
        //{
        //    return View(new ResetPasswordModel { Token = token, Email = email });
        //}

        //[HttpPost]
        //public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await userManager.FindByEmailAsync(model.Email);

        //        if (user != null)
        //        {
        //            var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);

        //            if (!result.Succeeded)
        //            {
        //                foreach (var error in result.Errors)
        //                {
        //                    ModelState.AddModelError("", error.Description);
        //                }
        //                return View();
        //            }
        //            return View("Success");
        //        }
        //        ModelState.AddModelError("", "Invalid Request");
        //    }
        //    return View();
        //}
    }
}

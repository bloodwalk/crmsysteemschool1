﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PluralsightDemo.Models;

namespace PluralsightDemo.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<PluralsightUser> userManager;
        private readonly IUserClaimsPrincipalFactory<PluralsightUser> claimsPrincipalFactory;
        private readonly PluralsightUserDbContext _context;

        public HomeController(UserManager<PluralsightUser> userManager,
            IUserClaimsPrincipalFactory<PluralsightUser> claimsPrincipalFactory,
            PluralsightUserDbContext databasecontext
            )
        {
            this._context = databasecontext;
            this.userManager = userManager;
            this.claimsPrincipalFactory = claimsPrincipalFactory;
            
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Authorize(Roles = Constants.AdministratorRole)]
        public IActionResult Register()
        {
            
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.UserName);

                if (user == null)
                {
                    user = new PluralsightUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = model.UserName,
                        Email = model.UserName,
                        adress = model.adress,
                        woonplaats = model.woonplaats,
                        postcode = model.postcode,
                        naam = model.naam,
                        IsPending = false,
                    };

                    var result = await userManager.CreateAsync(user, model.Password);

                    //if (result.Succeeded)
                    //{
                    //    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    //    var confirmationEmail = Url.Action("ConfirmEmailAddress", "Home",
                    //        new { token = token, email = user.Email }, Request.Scheme);
                    //    System.IO.File.WriteAllText("confirmationLink.txt", confirmationEmail);
                    //}
                    //else

                    if(!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                        return View();
                    }
                }

                return View("Success");
            }

            return View();
        }

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
        [AllowAnonymous]
        public IActionResult Login()
        {
           
          // _context.UserLogt.Add() nu werkt het dus wel
            return View();
        }




        [HttpPost]
        [AllowAnonymous]
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

                    var isAdmin = await userManager.IsInRoleAsync(user, Constants.AdministratorRole);
                    var principal = await claimsPrincipalFactory.CreateAsync(user);

                    if (!isAdmin)
                    {
                        await HttpContext.SignInAsync("Identity.Application", principal);
                    }

                    //inlog logic
                    

                    if (isAdmin) {

                        principal.AddIdentity(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Role, Constants.AdministratorRole), }));

                        await HttpContext.SignInAsync("Identity.Application", principal);
                        return RedirectToAction("Admin");
                        //sdfdsfsdfsdf
                    }

                    return RedirectToAction("Leerling");


                }

                ModelState.AddModelError("", "Invalid UserName or Password");
            }

            return View();
        }

        [HttpGet]
        [Authorize(Roles = Constants.AdministratorRole)]
        public IActionResult Admin()
        {
            
            //var z = User.IsInRole(Constants.AdministratorRole);
           // var user = await userManager.FindByNameAsync("admin@todo.local");
           // ViewData["info"] = userManager.GetRolesAsync();

            return View();
        }

        [HttpGet]
        public IActionResult Leerling()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Identity.Application");
            
           
            return RedirectToAction("Login", "Home");
        }


        /// <summary>
        ///////////////////////////////////////////pending
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = Constants.AdministratorRole)]
        public IActionResult AddPending()
        {
            return View("AddPending");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPending(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.UserName);
                
                if (user == null)
                {
                    user = new PluralsightUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = model.UserName,                        
                        Email = model.UserName,
                        adress = model.adress,
                        woonplaats=model.woonplaats,
                        postcode=model.postcode,
                        naam=model.naam,
                        IsPending = true,
                    };

                    var result = await userManager.CreateAsync(user, model.Password);

                    //if (result.Succeeded)
                    //{
                    //    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    //    var confirmationEmail = Url.Action("ConfirmEmailAddress", "Home",
                    //        new { token = token, email = user.Email }, Request.Scheme);
                    //    System.IO.File.WriteAllText("confirmationLink.txt", confirmationEmail);
                    //}
                    //else

                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                        return View();
                    }
                }

                return View("Success");
            }

            return View();
            
        }



        [HttpGet]
        [Authorize(Roles = Constants.AdministratorRole)]
        public IActionResult ConvertPendingToStudent()
        {
            List<PluralsightUser> PendingUsers = _context.Users.Where(x=>x.IsPending==true).ToList();
         
            return View("ConvertPendingToStudent",PendingUsers);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConvertToStudent(string UserName) {


            
            
            if (UserName == null) {
                return RedirectToAction("ConvertPendingToStudent", "Home");// moet naar errorpage gaan
            }


            var user = await userManager.FindByNameAsync(UserName);
            if (user != null) {
                user.IsPending = false;
                // user.save                 //////////////// nieuwe code
               var result = await userManager.UpdateAsync(user);


                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View();
                }




            }
            return View("Success");
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePendingStudent(string UserName) {
         
            if (UserName == null) {
                return RedirectToAction("ConvertPendingToStudent", "Home");// moet naar error page gaan
            }


            var user = await userManager.FindByNameAsync(UserName);
            if (user != null) {
                               
               var result = await userManager.DeleteAsync(user);


                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View();
                }




            }
            return View("Success");
        }







        //[HttpGet]
        //[Authorize(Roles = Constants.AdministratorRole)]
        //public IActionResult ConvertToStudent(string Username)
        //{

        //    if (Username != null) {

        //        return RedirectToAction("Adminn", "Home");
        //    }

        //    return View("Success");
        //}

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

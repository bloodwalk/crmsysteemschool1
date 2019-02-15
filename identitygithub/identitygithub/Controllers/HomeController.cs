using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using identitygithub.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;// dit is nieuw
//using MailKit.Net.Smtp;
//using MailKit;
//using MimeKit;
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
                        deadline = model.deadline
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
                        woonplaats = model.woonplaats,
                        postcode = model.postcode,
                        naam = model.naam,
                        IsPending = true,
                        deadline = model.deadline
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
        public async Task<IActionResult> ConvertPendingToStudent()
        {
            List<PluralsightUser> PendingUsers = await _context.Users.Where(x => x.IsPending == true).ToListAsync();

            return View("ConvertPendingToStudent", PendingUsers);
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

        [HttpGet]
        [Authorize(Roles = Constants.AdministratorRole)]
        public async Task<IActionResult> Notifications()
        {
            // PluralsightUser x = new PluralsightUser();
            List<PluralsightUser> DeadlineUsers = _context.Users
                .Where(x => x.deadline < DateTime.Now).ToList();
            //var Lijst2 = DeadlineUsers;
            // DeadlineUsers.Where(async(x) => await userManager.IsInRoleAsync(x, Constants.AdministratorRole));

            foreach (var student in DeadlineUsers.ToList()) {

                var isAdmin = await userManager.IsInRoleAsync(student, Constants.AdministratorRole);

                if (isAdmin) {
                    // Lijst2.Remove(student);
                    DeadlineUsers.Remove(student);
                }
            }
            //.Where(x=> {

            //    var isAdmin = userManager.IsInRoleAsync(x, Constants.AdministratorRole);


            //    return Task.W;

            //})




            return View("Notifications", DeadlineUsers);
        }






        [HttpGet]
        [Authorize(Roles = Constants.AdministratorRole)]
        public IActionResult Edit()
        {


            return View("Edit");
        }


        [HttpGet]
        [Authorize(Roles = Constants.AdministratorRole)]        
        public async Task<IActionResult> GetEditDataAsync(string editsearch)
        {
           
            //if (editsearch == "naam3") {
            //    return new JsonResult("works");
            //}
            
            var Edituser = await _context.Users.Where(student => student.naam == editsearch).FirstOrDefaultAsync();//.ToListAsync();
            if (Edituser == null)
            {
                //return HttpNotFound(); moet nog wat anders worden
            }

            // var Edituser = await userManager.FindByNameAsync(editsearch);
            // return new JsonResult("demo1");
            return PartialView("_EditPartial",Edituser);
        }


        //QuikSearch

        [HttpGet]
        [Authorize(Roles = Constants.AdministratorRole)]
        public async Task<JsonResult> QuikSearchAsync(string term)
        {
           

            var QuikSearchResults = await _context.Users.Where(student => student.naam.ToLower().StartsWith(term.ToLower()))
               .Select(student => new { value = student.naam }).ToListAsync();

            //var result = QuikSearchResults[0].value;
            //string[] array = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat",result };

            //string[] weekDays = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" , result};

            return Json(QuikSearchResults);// JsonRequistBehavior.AllowGet);

            // var Edituser = await userManager.FindByNameAsync(editsearch);
            // return new JsonResult("demo1");
            
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
               
                var EditUser = await userManager.FindByNameAsync(model.UserName);
                

                if (EditUser != null)
                {
                    EditUser.woonplaats = model.woonplaats;
                    EditUser.postcode = model.postcode;
                    EditUser.naam = model.naam;
                    EditUser.deadline = model.deadline;
                    EditUser.adress = model.adress;
                    
                    var result = await userManager.UpdateAsync(EditUser);

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

                    return View("Success");
                }

                
            }

            return View();

        }

        [HttpGet]
        [Authorize(Roles = Constants.AdministratorRole)]
        public IActionResult NotitieToevoegen(Guid id)
        {

            @ViewBag.ProductId = id;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NotitieToevoegen(Notitie model)// moet nog Notitiemodel worden voor onverwachte input
        {
            if (ModelState.IsValid)
            {


               
                var user = await userManager.Users.Where(x => x.Id == model.User.Id).Include(x => x.Notities)
                    .FirstOrDefaultAsync();
               
                if (user != null)
                {

                   var notitie = new Notitie
                    {
                       onderwerp=model.onderwerp,
                       Datum= DateTime.Now,
                       Textarea=model.Textarea
                       
                    };


                    user.Notities.Add(notitie);
                    //var result = await _context.SaveChangesAsync();
                    

                    var result = await userManager.UpdateAsync(user);

                    //if (!result.Succeeded)
                    //{
                    //    foreach (var error in result.Errors)
                    //    {
                    //        ModelState.AddModelError("", error.Description);
                    //    }

                    //    return View();
                    //}
                    //return View("Success");
                    return RedirectToAction("AlleNotities", "Home");
                }

                
            }

            return View();

        }



        [HttpGet]
        [Authorize(Roles = Constants.AdministratorRole)]
        public async Task<IActionResult> AlleNotities()
        {

            var user = await userManager.Users.Include(x => x.Notities).OrderBy(p => p.naam).ToListAsync();
            // nog async maken
              //var g = user.OrderBy(p => p.naam);
          // kan ook zonder usermanger mocht het fout gaan

            // var user =await userManager.FindByNameAsync("jordytak@gmail.com").;



            foreach (var student in user.ToList())
            {

                var isAdmin = await userManager.IsInRoleAsync(student, Constants.AdministratorRole);

                if (isAdmin)
                {
                    // Lijst2.Remove(student);
                    user.Remove(student);
                }
            }


            return View(user);
        }



        [HttpGet]
        [Authorize(Roles = Constants.AdministratorRole)]
        public async Task<IActionResult> NotitieVanPersoon(Guid id, int notitieId)
        {
            //var user = await userManager.Users.Where(x => x.Id == id.ToString()).Include(x => x.Notities)
            //       .FirstOrDefaultAsync();
            var notitie = await userManager.Users.Where(x => x.Id == id.ToString())
                 .SelectMany(x => x.Notities.Where(notities => notities.Id == notitieId)).Include(x => x.User).FirstOrDefaultAsync();

            return View(notitie);
        }

        [HttpGet]
        [Authorize(Roles = Constants.AdministratorRole)]
        public async Task<IActionResult> EditNotities(int modelId,Guid gebruikerId)
        {
            @ViewBag.Id = gebruikerId;
            @ViewBag.notitieId = modelId;
           
            var Editnotitie = await userManager.Users.Where(x => x.Id == gebruikerId.ToString())             
            // .Select(y => y.Notities.Where(notitie => notitie.Id == id))
              .SelectMany(x => x.Notities.Where(notitie => notitie.Id == modelId)).FirstOrDefaultAsync(); 
            @ViewBag.datum = Editnotitie.Datum.ToShortDateString();
            // var number = user.Notities.Count;
            //var notitie = user.Notities.Where(x=>x.Id.ToString()==modelId.ToString()).FirstOrDefault();

            if (Editnotitie == null)
                {
                    //something went wrong 
                }

                return View(Editnotitie);
                    
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditNotities(Notitie model)// moet nog Notitiemodel worden voor onverwachte input
        {
            if (ModelState.IsValid)
            {

                var Editnotitie = await userManager.Users.Where(x => x.Id == model.User.Id.ToString())
                 .SelectMany(x => x.Notities.Where(notitie => notitie.Id == model.Id)).Include(x=>x.User).FirstOrDefaultAsync();
                // model id bestaat nog niet
                // var user = await userManager.Users.Where(x => x.Id == model.User.Id).Include(x => x.Notities)
                // .FirstOrDefaultAsync();
                var gebruiker = Editnotitie.User;

                if (Editnotitie != null)
                {

                    Editnotitie.onderwerp = model.onderwerp;
                    Editnotitie.Textarea = model.Textarea;

                   


                                      

                    var result = await userManager.UpdateAsync(Editnotitie.User);


                    return RedirectToAction("AlleNotities", "Home"); // misschien naar andere pagina verwijzen
                }


            }
            // model errors / meer info(ook voor een paar andere views )
            return View();

        }


        // Edit notities functionaliteit

        [HttpGet]
        [Authorize(Roles = Constants.AdministratorRole)]
        public async Task<IActionResult> NotitiesVanPersoonTabel(Guid id)
        {
            var user = await userManager.Users.Where(x => x.Id == id.ToString()).Include(x => x.Notities)
                   .FirstOrDefaultAsync();

            return View(user);
        }

        
        //[HttpGet]
        //[Authorize(Roles = Constants.AdministratorRole)]
        //public IActionResult SendInvitation()
        //{
        //    var message = new MimeMessage();
        //    message.From.Add(new MailboxAddress("jordy","jookkkm"));


        //    message.To.Add(new MailboxAddress("jordy", "jorkkkom"));

        //    message.Subject = "Email using asp.net core";

        //    message.Body = new TextPart("plain")
        //    {
        //        Text = "Im using Mailkit nuget package to send email easily"
        //    };


        //    using (var client = new SmtpClient()) {

        //        client.ServerCertificateValidationCallback = (s, c, h, e) => true;// nieuwe lijn
        //        client.Connect("smtp.gmail.com", 587, false);
        //        client.Authenticate("j);
        //        client.Send(message);
        //        client.Disconnect(true);
        //       // client.ServerCertificateValidationCallback=
        //    }

        //    return View();

        //}




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

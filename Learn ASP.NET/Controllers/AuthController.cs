using Learn_ASP.NET.IdentityStore;
using Learn_ASP.NET.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Learn_ASP.NET.Controllers
{
    public class AuthController : Controller
    {
        public UserManager<IdentityUser> UserManager => HttpContext.GetOwinContext().Get<UserManager<IdentityUser>>();
        public SignInManager<IdentityUser, string> SignInManager => HttpContext.GetOwinContext().Get<SignInManager<IdentityUser, string>>();
        // GET: Auth
         
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            var signInStatus = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, true, true);
            switch(signInStatus)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Home");
                default:
                    ModelState.AddModelError("", "Invalid Credentials");
                    return View(model);
            }
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
           var identityResult = await UserManager.CreateAsync(new IdentityUser(model.UserName), model.Password);
            if(identityResult != null)
            {
                ViewBag.UserError = "User";
              
            }
            if(identityResult.Succeeded)
            {
                return RedirectToAction("About", "Home");
            }
            ModelState.AddModelError("", identityResult.Errors.First());
            return View(model);
        }
    }
}
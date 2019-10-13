using Learn_ASP.NET.IdentityStore;
using Learn_ASP.NET.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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
        public ActionResult Logout()
        {
            if (Request.Cookies["Login"] != null)
            {
                Response.Cookies["Login"].Expires = DateTime.Now.AddDays(-1);
                
            }
            return RedirectToAction("Login", "Auth");
        }

        public async Task<ActionResult> Dashboard()
        {
            if (TempData.ContainsKey("doc"))
            {
                return View();
            }
            HttpCookie loginCookie = Request.Cookies["Login"];
            if(loginCookie!=null)
            {


                string name = loginCookie.Values["Name"];

                var bytes = Convert.FromBase64String(name);
                var output = MachineKey.Unprotect(bytes, "ProtectCookie");
                string result = Encoding.UTF8.GetString(output);

                string password = loginCookie.Values["Password"];
                var passBytes = Convert.FromBase64String(password);
                var passOutput = MachineKey.Unprotect(passBytes, "ProtectCookie");
                string passResult = Encoding.UTF8.GetString(passOutput);

         


                var signInStatus = await SignInManager.PasswordSignInAsync(result.ToString(), passResult.ToString(), true, true);
                
                switch(signInStatus)
                {
                    case SignInStatus.Success:
                        return View();
                    default:
                        return RedirectToAction("Login", "Auth");
                }
            }
            
            return RedirectToAction("Login", "Auth");

        }
        
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            var signInStatus = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, true, true);
            
            switch(signInStatus)
            {
                case SignInStatus.Success:
                    if(model.RememberMe == true)
                    {
                        HttpCookie loginCookie = new HttpCookie("Login");


                        var cookieText = Encoding.UTF8.GetBytes(model.UserName);
                        var encryptedValueUserName = Convert.ToBase64String(MachineKey.Protect(cookieText, "ProtectCookie"));
                        
                        //Set the Cookie value.
                        loginCookie.Values["Name"] = encryptedValueUserName;

                        var cookieTextPassword = Encoding.UTF8.GetBytes(model.Password);
                        var encryptedValuePassword = Convert.ToBase64String(MachineKey.Protect(cookieTextPassword, "ProtectCookie"));

                        loginCookie.Values["Password"] = encryptedValuePassword;
                        loginCookie.Path = Request.ApplicationPath;
                        //Set the Expiry date.
                        loginCookie.Expires = DateTime.Now.AddDays(1);
                        //Add the Cookie to Browser.
                        Response.Cookies.Add(loginCookie);
                    }
                    TempData["doc"] = "fromlogin";
                    return RedirectToAction("Dashboard", "Auth");
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
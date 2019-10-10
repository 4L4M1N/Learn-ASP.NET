using System;
using System.Configuration.Provider;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;

[assembly: OwinStartup(typeof(Learn_ASP.NET.Startup))]

namespace Learn_ASP.NET
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            const string connectionString =
                @"Data Source=(localdb)\MSSQLLocalDB;Database=learn;trusted_connection=yes;";
            app.CreatePerOwinContext(() => new IdentityDbContext(connectionString));
            //Register implementaiton of User Store
            app.CreatePerOwinContext<UserStore<IdentityUser>>((opt, cont) => new UserStore<IdentityUser>(cont.Get<IdentityDbContext>()));
            //UserStore to configure UserManager
            app.CreatePerOwinContext<UserManager<IdentityUser>>(
                (opt, cont) => new UserManager<IdentityUser>(cont.Get<UserStore<IdentityUser>>())
                );
            //Signin Manager
            app.CreatePerOwinContext<SignInManager<IdentityUser, string>>((opt, cont) => new SignInManager<IdentityUser, string>(cont.Get<UserManager<IdentityUser>>(), cont.Authentication));

            app.UseCookieAuthentication(new CookieAuthenticationOptions {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie
            });
        }
    }
}

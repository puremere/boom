using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.App_Start
{
    public class Startup
    {
        //public void Configuration(IAppBuilder app)
        //{
        //    app.UseCookieAuthentication(new CookieAuthenticationOptions
        //    {
        //        AuthenticationType = "ApplicationCookie",
        //        LoginPath = new PathString("/auth/login")
        //    });
        //}
        //public void ConfigureAuth(IAppBuilder app)
        //{
        //    app.UseCookieAuthentication(new CookieAuthenticationOptions
        //    {
        //        AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
        //        LoginPath = new PathString("/Home/Login"),
        //        CookieSecure = CookieSecureOption.Always,
        //        ExpireTimeSpan = System.TimeSpan.FromDays(3),
        //        SlidingExpiration = true
        //    });


        //}
    }
}
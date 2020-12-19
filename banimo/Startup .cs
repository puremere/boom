using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using banimo.Models;
using Microsoft.Owin.Security.WsFederation;
using Microsoft.Owin.Security;

[assembly: OwinStartup(typeof(banimo.Startup))]
namespace banimo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
          
        }


    }

}


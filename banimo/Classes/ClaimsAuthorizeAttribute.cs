using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace banimo.Classes
{
    public class ClaimsAuthorizeAttribute : AuthorizeAttribute
    {
        private string claimType;
        private string claimValue;
        public ClaimsAuthorizeAttribute(string claimType,
           string claimValue)
        {
            this.claimType = claimType;
            this.claimValue = claimValue;
        }
        public override void OnAuthorization(AuthorizationContext
           filterContext)
        {
            var user = filterContext.HttpContext.User as System.Security.Claims.ClaimsPrincipal;
            if (user.HasClaim(claimType, claimValue))
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}
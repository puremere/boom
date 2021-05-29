using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace banimo.Classes 
{
    public class SessionCheck : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var descriptor = filterContext.ActionDescriptor;
            var actionName = descriptor.ActionName;

            if (actionName != "Index" && actionName != "CustomerLogin" && actionName != "createUserReport")
            {
                HttpSessionStateBase session = filterContext.HttpContext.Session;
                string val = session["LogedInUser2"] == null ? "" : session["LogedInUser2"] as string;
                if (session["LogedInUser2"] == null)
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                                { "Controller", "Admin" },
                                { "Action", "Index" }
                                    });
                }
            }
           
        }
    }

    public class HomeSessionCheck : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            if (session["LogedInUser"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                                { "Controller", "Home" },
                                { "Action", "login" }
                                });
            }

        }
    }

    public class doForAll : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            if (session["LogedInUser"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                                { "Controller", "Home" },
                                { "Action", "login" }
                                });
            }

        }
    }
}
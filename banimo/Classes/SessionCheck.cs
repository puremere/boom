using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Security.Cryptography;
using System.Web.Caching;
using System.Text;
using System.Linq;
using MvcThrottle;

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
                else
                {
                    if (session["partner"] as string != "0")
                    {
                        if (actionName != "Edit" && actionName != "product" && actionName != "resetAdminProductPage" && actionName != "GetTheListOfItems" && actionName != "CustomerLogout")
                        {
                            filterContext.Result = new RedirectToRouteResult(
                      new RouteValueDictionary {
                                { "Controller", "Admin" },
                                { "Action", "product" }
                                  });
                        }
                    }
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
    public class CoreSessionCheck : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            if (session["CoreUser"] == null)
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
            Uri myUri = new Uri(filterContext.HttpContext.Request.Url.ToString());
            foreach (var item in HttpUtility.ParseQueryString(myUri.Query))
            {
                string param1 = (HttpUtility.ParseQueryString(myUri.Query).Get(item.ToString())).ToLower();

                if (param1.Contains("select") || param1.Contains("delete") || param1.Contains("update")  || param1.Contains("union") || param1.Contains(" or ") || param1.Contains(" and ") || param1.Contains(" group by ") || param1.Contains(" sum(") || param1.Contains(" count(") || param1.Contains(";") || param1.Contains("--") || param1.Contains("&&") || param1.Contains("&") || param1.Contains("||") || param1.Contains("|") || param1.Contains("$") || param1.Contains("()") || param1.Contains("alert()") ||  param1.Contains("<") || param1.Contains(">") || param1.Contains("%0d") || param1.Contains("%0a") || param1.Contains("%0c") || param1.Contains("``") )
                {
                     
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                                { "Controller", "Error" },
                                { "Action", "Error500" }
                                    });
                }


            }


        }
    }


    public enum TimeUnit
    {
        Minute = 60,
        Hour = 3600,
        Day = 86400
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ThrottleAttribute : ActionFilterAttribute
    {
        public TimeUnit TimeUnit { get; set; }
        public int Count { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var seconds = Convert.ToInt32(TimeUnit);

            var key = string.Join(
                "-",
                seconds,
                filterContext.HttpContext.Request.HttpMethod,
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                filterContext.ActionDescriptor.ActionName,
                filterContext.HttpContext.Request.UserHostAddress
            );

            // increment the cache value
            var cnt = 1;
            if (HttpRuntime.Cache[key] != null)
            {
                cnt = (int)HttpRuntime.Cache[key] + 1;
            }
            HttpRuntime.Cache.Insert(
                key,
                cnt,
                null,
                DateTime.UtcNow.AddSeconds(seconds),
                Cache.NoSlidingExpiration,
                CacheItemPriority.Low,
                null
            );

            if (cnt > Count)
            {
                filterContext.Result = new ContentResult
                {
                    Content = "You are allowed to make only " + Count + " requests per " + TimeUnit.ToString().ToLower()
                };
                filterContext.HttpContext.Response.StatusCode = 429;
            }
        }
    }
}



   
   



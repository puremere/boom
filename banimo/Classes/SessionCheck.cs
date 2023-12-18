﻿using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Security.Cryptography;
using System.Web.Caching;
using System.Text;
using System.Linq;
using MvcThrottle;
using System.Net.Http.Headers;
using System.Collections.Specialized;
using System.Net;
using System.Configuration;
using Newtonsoft.Json;

namespace banimo.Classes
{

    public class SessionCheck : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            
            var descriptor = filterContext.ActionDescriptor;
            var actionName = descriptor.ActionName.ToString().ToLower();

            if (actionName != "index" && actionName != "customerlogin" && actionName != "createuserreport")
            {
               
                string val = session["LogedInUser2"] == null ? "" : session["LogedInUser2"] as string;

                if (val == "")
                {
                    if (filterContext.HttpContext.Request.Cookies["AT"] != null)
                    {
                        var request = filterContext.HttpContext.Request.Cookies["AT"].Value;
                        if (!string.IsNullOrEmpty(request))
                        {
                            session["LogedInUser2"] = request.ToString();
                            string[] lst = { "factor", "dashboard", "product", "productdetail", "MenuNew", "Features", "Orders", "brand", "access", "slide", "newBanner", "productGroup", "wonderProduct", "portfolio", "comment", "blog", "Users", "Pages", };
                            if (lst.Contains(actionName))
                            {
                                try
                                {
                                    string result = "";
                                    using (WebClient client = new WebClient())
                                    {
                                        string token = session["CoreUser"].ToString();
                                        string action = "Admin/" + actionName;

                                        var collection = new NameValueCollection();
                                        collection.Add("token", token);
                                        collection.Add("action", action);
                                        collection.Add("servername", ConfigurationManager.AppSettings["servername"]);
                                        string url = ConfigurationManager.AppSettings["server"] + "/Admin/checkPartnerToken.php";
                                        byte[] response = client.UploadValues(url, collection);
                                        result = System.Text.Encoding.UTF8.GetString(response);
                                    }
                                    banimo.ViewModel.reponsVM model = JsonConvert.DeserializeObject<banimo.ViewModel.reponsVM>(result);
                                    if (model.status != "200")
                                    {
                                        filterContext.Result = new RedirectToRouteResult(
                                        new RouteValueDictionary {
                                        { "Controller", "admin" },
                                        { "Action", "Index" }
                                                       });
                                    }
                                }
                                catch (Exception)
                                {

                                    filterContext.Result = new RedirectToRouteResult(
                                    new RouteValueDictionary {
                                        { "Controller", "Admin" },
                                        { "Action", "Index" }
                                                });
                                        }


                            }



                            //string val = cookie["NameOfTheCookieIWant"].Value;
                        }
                        else
                        {
                            filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary {
                                { "Controller", "Admin" },
                                { "Action", "Index" }
                                        });
                        }

                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                                { "Controller", "Admin" },
                                { "Action", "Index" }
                                    });
                    }


                }
                else
                {
                    var request = filterContext.HttpContext.Request.Cookies["AT"].Value;
                    string[] lst = { "factor", "dashboard", "product", "productdetail", "MenuNew", "Features", "Orders", "brand", "access", "slide", "newBanner", "productGroup", "wonderProduct", "portfolio", "comment", "blog", "Users", "Pages", };
                    if (lst.Contains(actionName))
                    {
                        string result = "";
                        using (WebClient client = new WebClient())
                        {
                            string token = val;
                            string action = "Admin/" + actionName;

                            var collection = new NameValueCollection();
                            collection.Add("token", token);
                            collection.Add("action", action);
                            collection.Add("servername", ConfigurationManager.AppSettings["servername"]);
                            string url = ConfigurationManager.AppSettings["server"] + "/Admin/checkPartnerToken.php";
                            byte[] response = client.UploadValues(url, collection);
                            result = System.Text.Encoding.UTF8.GetString(response);
                        }
                        banimo.ViewModel.reponsVM model = JsonConvert.DeserializeObject<banimo.ViewModel.reponsVM>(result);
                        filterContext.Controller.ViewBag.Msg = model.message;
                        if (model.status != "200")
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

        }
    }
    public class NodeSessionCheck : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            var descriptor = filterContext.ActionDescriptor;
            var actionName = descriptor.ActionName;

            if (session["lang"] == null)
            {
                session["lang"] = "en";
            }

            if ((actionName != "Index" || actionName != "index" )&& actionName != "CustomerLogin")
            {
                session = filterContext.HttpContext.Session;
                string val = session["LogedInUser2"] == null ? "" : session["LogedInUser2"] as string;

                if (val == "")
                {
                    if (filterContext.HttpContext.Request.Cookies["AT"] != null)
                    {
                        var request = filterContext.HttpContext.Request.Cookies["AT"].Value;
                        if (!string.IsNullOrEmpty(request))
                        {
                            session["LogedInUser2"] = request.ToString();
                            string[] lst = { "factor", "dashboard", "product", "productdetail", "Menu", "bank", "orders", "access", "Slide", "banner", "Comment" };
                            if (lst.Contains(actionName))
                            {
                                try
                                {
                                    string result = "";
                                    using (WebClient client = new WebClient())
                                    {
                                        string token = session["LogedInUser2"].ToString();
                                        string action = "Node/" + actionName;

                                        var collection = new NameValueCollection();
                                        collection.Add("token", token);
                                        collection.Add("action", action);
                                        collection.Add("servername", ConfigurationManager.AppSettings["servername"]);
                                        string url = ConfigurationManager.AppSettings["server"] + "/Admin/checkPartnerToken.php";
                                        byte[] response = client.UploadValues(url, collection);
                                        result = System.Text.Encoding.UTF8.GetString(response);
                                    }
                                    banimo.ViewModel.reponsVM model = JsonConvert.DeserializeObject<banimo.ViewModel.reponsVM>(result);
                                    if (model.status != "200")
                                    {
                                        filterContext.Result = new RedirectToRouteResult(
                                        new RouteValueDictionary {
                                        { "Controller", "admin" },
                                        { "Action", "Index" }
                                                       });
                                    }
                                }
                                catch (Exception)
                                {

                                    filterContext.Result = new RedirectToRouteResult(
                                    new RouteValueDictionary {
                                        { "Controller", "Node" },
                                        { "Action", "Index" }
                                                });
                                }


                            }



                            //string val = cookie["NameOfTheCookieIWant"].Value;
                        }
                        else
                        {
                            filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary {
                                { "Controller", "Node" },
                                { "Action", "Index" }
                                        });
                        }

                    }
                    else
                    {
                        //filterContext.Result = new RedirectToRouteResult(
                        //new RouteValueDictionary {
                        //        { "Controller", "Node" },
                        //        { "Action", "Index" }
                        //            });
                    }


                }
                else
                {
                    var request = filterContext.HttpContext.Request.Cookies["AT"].Value;
                    string[] lst = { "factor", "dashboard", "product", "productdetail", "Menu", "bank", "orders", "access", "Slide", "banner", "Comment" };
                    if (lst.Contains(actionName))
                    {
                        string result = "";
                        using (WebClient client = new WebClient())
                        {
                            string token = val;
                            string action = "Node/" + actionName;

                            var collection = new NameValueCollection();
                            collection.Add("token", token);
                            collection.Add("action", action);
                            collection.Add("servername", ConfigurationManager.AppSettings["servername"]);
                            string url = ConfigurationManager.AppSettings["server"] + "/Admin/checkPartnerToken.php";
                            byte[] response = client.UploadValues(url, collection);
                            result = System.Text.Encoding.UTF8.GetString(response);
                        }
                        banimo.ViewModel.reponsVM model = JsonConvert.DeserializeObject<banimo.ViewModel.reponsVM>(result);
                        if (model.status != "200")
                        {
                            filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary {
                                        { "Controller", "Node" },
                                        { "Action", "Index" }
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

    public class PartnerSessionCheck : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var descriptor = filterContext.ActionDescriptor;
            var actionName = descriptor.ActionName;
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            
            if (actionName != "Index"  && actionName != "CustomerLogin" && actionName != "index" )
            {
                if (session["PartnerUser"] == null)
                {
                    if (filterContext.HttpContext.Request.Cookies["PC"] != null)
                    {
                        var request = filterContext.HttpContext.Request.Cookies["PC"].Value;
                        if (!string.IsNullOrEmpty(request))
                        {

                            
                            string[] lst = { "product", "Edit", "MainMenu", "bank", "partner", "access","orders" };
                            if (lst.Contains(actionName))
                            {
                                session["PartnerUser"] = request.ToString();
                                session["PartnerCat"] = filterContext.HttpContext.Request.Cookies["PC"].Value;
                                string result = "";
                                using (WebClient client = new WebClient())
                                {
                                    string token = session["PartnerUser"].ToString();
                                    string action = "Partner/" + actionName;

                                    var collection = new NameValueCollection();
                                    collection.Add("token", token);
                                    collection.Add("action", action);
                                    collection.Add("servername", ConfigurationManager.AppSettings["servername"]);
                                    string url = ConfigurationManager.AppSettings["server"] + "/Admin/checkPartnerToken.php";
                                    byte[] response = client.UploadValues(url, collection);
                                    result = System.Text.Encoding.UTF8.GetString(response);
                                }
                                banimo.ViewModel.reponsVM model = JsonConvert.DeserializeObject<banimo.ViewModel.reponsVM>(result);
                                if (model.status != "200")
                                {
                                    filterContext.Result = new RedirectToRouteResult(
                                    new RouteValueDictionary {
                                        { "Controller", "Partner" },
                                        { "Action", "Index" }
                                                   });
                                }

                            }
                            //string val = cookie["NameOfTheCookieIWant"].Value;
                        }
                        else
                        {
                            filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary {
                                { "Controller", "Partner" },
                                { "Action", "Index" }
                                        });
                        }

                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                                { "Controller", "Partner" },
                                { "Action", "Index" }
                                    });
                    }
                }
                else
                {

                    string[] lst = { "Index", "product", "MenuNew", "order", "bank", "partner", "access","orders" };
                    if (lst.Contains(actionName))
                    {
                        string result = "";
                        using (WebClient client = new WebClient())
                        {
                            string token = session["PartnerUser"].ToString();
                            string action = "Partner/" + actionName;

                            var collection = new NameValueCollection();
                            collection.Add("token", token);
                            collection.Add("action", action);
                            collection.Add("servername", ConfigurationManager.AppSettings["servername"]);
                            string url = ConfigurationManager.AppSettings["server"] + "/Admin/checkPartnerToken.php";
                            byte[] response = client.UploadValues(url, collection);
                            result = System.Text.Encoding.UTF8.GetString(response);
                        }
                        banimo.ViewModel.reponsVM model = JsonConvert.DeserializeObject<banimo.ViewModel.reponsVM>(result);
                        if (model.status != "200")
                        {
                            filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary {
                                        { "Controller", "Partner" },
                                        { "Action", "Index" }
                                           });
                        }

                    }

                }
            }
           

        }
    }
    public class DriverSessionCheck : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var descriptor = filterContext.ActionDescriptor;
            var actionName = descriptor.ActionName;
            HttpSessionStateBase session = filterContext.HttpContext.Session;

            if (actionName != "Index" && actionName != "CustomerLogin" && actionName != "index")
            {
                if (session["driverUser"] == null)
                {
                    if (filterContext.HttpContext.Request.Cookies["DU"] != null)
                    {
                        var request = filterContext.HttpContext.Request.Cookies["DU"].Value;
                        if (!string.IsNullOrEmpty(request))
                        {


                            string[] lst = { "list" };
                            if (lst.Contains(actionName))
                            {
                                session["driverUser"] = request.ToString();
                              
                                string result = "";
                                using (WebClient client = new WebClient())
                                {
                                    string token = session["driverUser"].ToString();
                                    string action = "Driver/" + actionName;

                                    var collection = new NameValueCollection();
                                    collection.Add("token", token);
                                    collection.Add("action", action);
                                    collection.Add("servername", ConfigurationManager.AppSettings["servername"]);
                                    string url = ConfigurationManager.AppSettings["server"] + "/Admin/checkPartnerToken.php";
                                    byte[] response = client.UploadValues(url, collection);
                                    result = System.Text.Encoding.UTF8.GetString(response);
                                }
                                banimo.ViewModel.reponsVM model = JsonConvert.DeserializeObject<banimo.ViewModel.reponsVM>(result);
                                if (model.status != "200")
                                {
                                    filterContext.Result = new RedirectToRouteResult(
                                    new RouteValueDictionary {
                                        { "Controller", "Driver" },
                                        { "Action", "Index" }
                                                   });
                                }

                            }
                            //string val = cookie["NameOfTheCookieIWant"].Value;
                        }
                        else
                        {
                            filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary {
                                { "Controller", "Driver" },
                                { "Action", "Index" }
                                        });
                        }

                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                                { "Controller", "Driver" },
                                { "Action", "Index" }
                                    });
                    }
                }
                else
                {

                    string[] lst = { "list" };
                    if (lst.Contains(actionName))
                    {
                        string result = "";
                        using (WebClient client = new WebClient())
                        {
                            string token = session["driverUser"].ToString();
                            string action = "Driver/" + actionName;

                            var collection = new NameValueCollection();
                            collection.Add("token", token);
                            collection.Add("action", action);
                            collection.Add("servername", ConfigurationManager.AppSettings["servername"]);
                            string url = ConfigurationManager.AppSettings["server"] + "/Admin/checkPartnerToken.php";
                            byte[] response = client.UploadValues(url, collection);
                            result = System.Text.Encoding.UTF8.GetString(response);
                        }
                        banimo.ViewModel.reponsVM model = JsonConvert.DeserializeObject<banimo.ViewModel.reponsVM>(result);
                        if (model.status != "200")
                        {
                            filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary {
                                        { "Controller", "Driver" },
                                        { "Action", "Index" }
                                           });
                        }

                    }

                }
            }


        }
    }
    public class CoreSessionCheck : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var descriptor = filterContext.ActionDescriptor;
            var actionName = descriptor.ActionName;
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            if (session["CoreUser"] == null)
            {
                if (filterContext.HttpContext.Request.Cookies["AT"] != null)
                {
                    var request = filterContext.HttpContext.Request.Cookies["AT"].Value;
                    if (!string.IsNullOrEmpty(request))
                    {
                        session["CoreUser"] = request.ToString();
                        string[] lst = { "Index", "product", "structure", "draft", "bank", "partner", "access" };
                        if (lst.Contains(actionName))
                        {
                            string result = "";
                            using (WebClient client = new WebClient())
                            {
                                string token = session["CoreUser"].ToString();
                                string action = "Core/" + actionName;

                                var collection = new NameValueCollection();
                                collection.Add("token", token);
                                collection.Add("action", action);
                                collection.Add("servername", ConfigurationManager.AppSettings["servername"]);
                                string url = ConfigurationManager.AppSettings["server"] + "/Admin/checkPartnerToken.php";
                                byte[] response = client.UploadValues(url, collection);
                                result = System.Text.Encoding.UTF8.GetString(response);
                            }
                            banimo.ViewModel.reponsVM model = JsonConvert.DeserializeObject<banimo.ViewModel.reponsVM>(result);
                            if (model.status != "200")
                            {
                                filterContext.Result = new RedirectToRouteResult(
                                new RouteValueDictionary {
                                        { "Controller", "core" },
                                        { "Action", "Index" }
                                               });
                            }

                        }
                        //string val = cookie["NameOfTheCookieIWant"].Value;
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                                { "Controller", "core" },
                                { "Action", "Index" }
                                    });
                    }

                }
                else
                {
                    if (actionName != "index")
                    {
                        filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                                { "Controller", "core" },
                                { "Action", "Index" }
                                });
                    }
                    
                }
            }
            else
            {

                string[] lst = { "Index", "product", "structure", "draft", "bank", "partner", "access" };
                if (lst.Contains(actionName))
                {
                    string result = "";
                    using (WebClient client = new WebClient())
                    {
                        string token = session["CoreUser"].ToString();
                        string action = "Core/" + actionName;

                        var collection = new NameValueCollection();
                        collection.Add("token", token);
                        collection.Add("action", action);
                        collection.Add("servername", ConfigurationManager.AppSettings["servername"]);
                        string url = ConfigurationManager.AppSettings["server"] + "/Admin/checkPartnerToken.php";
                        byte[] response = client.UploadValues(url, collection);
                        result = System.Text.Encoding.UTF8.GetString(response);
                    }
                    banimo.ViewModel.reponsVM model = JsonConvert.DeserializeObject<banimo.ViewModel.reponsVM>(result);
                    if (model.status != "200")
                    {
                        filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                                        { "Controller", "core" },
                                        { "Action", "Index" }
                                       });
                    }

                }

            }

        }
    }

    public class doForAll : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            //if (session["lang"] == null)
            //{
                
            //}
            //session["lang"] = "fa";
            Uri myUri = new Uri(filterContext.HttpContext.Request.Url.ToString());
            foreach (var item in HttpUtility.ParseQueryString(myUri.Query))
            {
                string param1 = (HttpUtility.ParseQueryString(myUri.Query).Get(item.ToString())).ToLower();

                if (param1.Contains("delete") || param1.Contains("update") || param1.Contains("union") || param1.Contains(" or ") || param1.Contains(" and ") || param1.Contains(" group by ") || param1.Contains(" sum(") || param1.Contains(" count(") || param1.Contains(";") || param1.Contains("--") || param1.Contains("&&") || param1.Contains("&") || param1.Contains("||") || param1.Contains("|") || param1.Contains("$") || param1.Contains("()") || param1.Contains("alert()") || param1.Contains("<") || param1.Contains(">") || param1.Contains("%0d") || param1.Contains("%0a") || param1.Contains("%0c") || param1.Contains("``"))


                {
                    
                    //filterContext.Result = new RedirectToRouteResult(
                    //    new RouteValueDictionary {
                    //            { "Controller", "Error" },
                    //            { "Action", "Error500" }
                    //                });
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








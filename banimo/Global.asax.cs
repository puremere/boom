using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace banimo
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            MvcHandler.DisableMvcResponseHeader = true;

        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            
            // gzip
            // Implement HTTP compression  
            HttpApplication app = (HttpApplication)sender;


            // Retrieve accepted encodings  
            //string encodings = app.Request.Headers.Get("Accept-Encoding");
            //if (encodings != null)
            //{
            //    // Check the browser accepts deflate or gzip (deflate takes preference)  
            //    encodings = encodings.ToLower();
            //    if (encodings.Contains("gzip"))
            //    {
            //        app.Response.Filter = new GZipStream(app.Response.Filter, CompressionMode.Compress);
            //        app.Response.AppendHeader("Content-Encoding", "gzip");
            //    }
            //    else if
            //        (encodings.Contains("deflate"))
            //    {
            //        app.Response.Filter = new DeflateStream(app.Response.Filter, CompressionMode.Compress);
            //        app.Response.AppendHeader("Content-Encoding", "deflate");
            //    }
            //}

            // ریدایرکت کردن از www
            //if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("www"))
            //{
            //    Response.RedirectPermanent(Request.Url.AbsoluteUri.Replace("www.", string.Empty), true);
            //}
            // حذف اطلاعات سرور از روی هدر
            if (app != null && app.Context != null)
            {
                app.Context.Response.Headers.Remove("Server");
            }
            // 
            //if (HttpContext.Current.Request.IsSecureConnection.Equals(false) && HttpContext.Current.Request.IsLocal.Equals(false))
            //{
            //    Response.Redirect("https://" + Request.ServerVariables["HTTP_HOST"]
            //   HttpContext.Current.Request.RawUrl);
            //}
            //if (!HttpContext.Current.Request.IsSecureConnection)
            //{
            //    var builder = new UriBuilder
            //    {
            //        Scheme = "https",
            //        Host = Request.Url.Host,
            //        // use the RawUrl since it works with URL Rewriting
            //        Path = Request.RawUrl
            //    };
            //    Response.Status = "301 Moved Permanently";
            //    Response.AddHeader("Location", builder.ToString());
            //}


        }
        //protected void Application_BeginRequest(Object sender, EventArgs e)
        //{
        //    if (!HttpContext.Current.Request.IsSecureConnection)
        //    {
        //        var builder = new UriBuilder
        //        {
        //            Scheme = "https",
        //            Host = Request.Url.Host,
        //            // use the RawUrl since it works with URL Rewriting
        //            Path = Request.RawUrl
        //        };
        //        Response.Status = "301 Moved Permanently";
        //        Response.AddHeader("Location", builder.ToString());
        //    }
        //}

        //protected void Application_BeginRequest(Object sender, EventArgs e)
        //{
        //    if (HttpContext.Current.Request.IsSecureConnection.Equals(false) && HttpContext.Current.Request.IsLocal.Equals(false))
        //    {
        //        string url = Request.ServerVariables["HTTP_HOST"];
        //        Response.Redirect("https://" + url
        //      + HttpContext.Current.Request.RawUrl);
        //    }
        //}

        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    Exception exception = Server.GetLastError();
        //    Response.Clear();

        //    var httpException = exception as HttpException;

        //    if (httpException != null)
        //    {
        //        string action;

        //        switch (httpException.GetHttpCode())
        //        {
        //            case 404:
        //                // page not found
        //                action = "/Error404";
        //                break;
        //            case 403:
        //                // forbidden
        //                action = "/Error403";
        //                break;
        //            case 500:
        //                // server error
        //                action = "/Error500";
        //                break;
        //            default:

        //                action = "/Error500";
        //                break;
        //        }

        //        // clear error on server
        //        Server.ClearError();

        //        Response.Redirect(String.Format("~/Error/{0}", action));
        //    }
        //    else
        //    {
        //        // this is my modification, which handles any type of an exception.
        //        Response.Redirect(String.Format("~/Errors/Unknown"));
        //    }
        //}
    }
}
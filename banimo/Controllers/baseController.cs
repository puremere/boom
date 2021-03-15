using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace banimo.Controllers
{
    public class baseController : Controller
    {
        //
        // GET: /base/

        protected override ViewResult View(IView view, object model)
        {
            string cartModelString = Request.Cookies["Modelcart"] != null ? Request.Cookies["Modelcart"].Value : "";// getCookie("cartModel");
            this.ViewBag.cookie = cartModelString;
            return base.View(view, model);
        }

    }
}

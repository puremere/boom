using System.Web;
using System.Web.Optimization;

namespace banimo
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/myscript").Include(
            //           "~/Scripts/jquery-3.2.1.min.js",
            //           "~/Scripts/jquery.flexslider2.js",
            //             "~/Scripts/bootstrap.js",
            //             "~/Scripts/imagezoom.js",
            //              "~/Scripts/toastr.js",
            //               "~/Scripts/jquery-ui.js"



            //           ));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/Scripts/js").Include(
                      "~/Scripts/jquery-2.1.4.min.js",
                      "~/Scripts/jquery.flexslider2.js",
                      "~/Scripts/bootstrap-3.1.1.min.js",
                      "~/Scripts/imagezoom.js",
                      "~/Scripts/toastr.js",
                      "~/Scripts/jquery-ui.js"
                    
                      ));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                 "~/Content/pignose.layerslider.css",
                  "~/Content/flexslider2.css",
                   "~/Content/style.css",
                    "~/Content/sm-core-css.css",
                     "~/Content/sm-mint.css",
                     "~/Content/resCarousel.css",
                     "~/Content/all.css",
                     "~/Content/toastr.css",
                      "~/Content/custom.css",
                       "~/Content/jquery-ui.css",
                        "~/Content/fontawesome-all.css"


                ));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}
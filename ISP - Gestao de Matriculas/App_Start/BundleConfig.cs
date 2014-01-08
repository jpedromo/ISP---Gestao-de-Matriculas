﻿using System.Web;
using System.Web.Optimization;

namespace ISP.GestaoMatriculas
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/jquery-ui-timepicker-addon.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/myWizardFuelux").Include(
                        "~/Scripts/myWizardFuelux.js"));

            bundles.Add(new ScriptBundle("~/bundles/monthpicker").Include(
                        "~/Scripts/jquery.mtz.monthpicker.js"));

            //bundles.Add(new ScriptBundle("~/bundles/morris").Include("~/Scripts/morris-0.4.3.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/flot").Include("~/Scripts/jquery.flot.js"));

            bundles.Add(new ScriptBundle("~/bundles/raphael").Include("~/Scripts/raphael-min.js"));

            bundles.Add(new ScriptBundle("~/bundles/dropzone/").Include("~/Scripts/dropzone/dropzone.js"));

            //bundles.Add(new ScriptBundle("~/bundles/fuelux").Include(
            //            "~/Scripts/fuelux-loader.min.js",
            //            "~/Scripts/require.js",
            //            "~/Scripts/wizard.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));

            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/Boot/bootstrap.css", "~/Content/sb-admin.css"));

            bundles.Add(new StyleBundle("~/Fonts/Font-Awesome").Include("~/fonts/font-awesome/css/font-awesome.css"));


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

            bundles.Add(new StyleBundle("~/Content/myWizardFuelux/css").Include(
                        "~/Content/myWizardFuelux.css"));

            bundles.Add(new StyleBundle("~/Content/myFuelux/css").Include(
                        "~/Content/myFuelux.css",
                        "~/Content/fuelux-responsive.css"));

            bundles.Add(new StyleBundle("~/Content/morris/css").Include("~/Content/morris-0.4.3.min.css"));

            bundles.Add(new StyleBundle("~/Content/dropzone/css/").Include("~/Scripts/dropzone/css/dropzone.css"));

        }
    }
}
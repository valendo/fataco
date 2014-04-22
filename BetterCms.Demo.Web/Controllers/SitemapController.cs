using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using BetterCms.Demo.Web.Models;

using BetterCms.Module.Api;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Infrastructure.Enums;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Exists;
using BetterCms.Module.Api.Operations.Pages.Sitemap;
using BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes;
using BetterCms.Module.Api.Operations.Pages.Sitemap.Tree;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Translations;
using System.Globalization;
using System.Web;
using BetterCms.Demo.Web.Helpers;


namespace BetterCms.Demo.Web.Controllers
{
    public class SiteMapController : Controller
    {
        private static Guid defaultSitemapId = new Guid("17ABFEE9-5AE6-470C-92E1-C2905036574B");

        public virtual ActionResult Index()
        {
            var menuItems = new List<MenuItemViewModel>();

            using (var api = ApiFactory.Create())
            {
                var sitemapId = GetSitemapId(api);
                if (sitemapId.HasValue)
                {
                    var request = new GetSitemapTreeRequest { SitemapId = sitemapId.Value, Data = new GetSitemapTreeModel { LanguageId = LanguageHelper.CurrentLanguageId } };

                    var response = api.Pages.Sitemap.Tree.Get(request);

                    if (response.Data.Count > 0)
                    {
                        menuItems = response.Data.Select(mi => new MenuItemViewModel { Caption = mi.Title, Url = mi.Url }).ToList();
                    }
                }
            }

            return View(menuItems);
        }

        public ActionResult TopMenu()
        {
            var menuItems = new List<MenuItemViewModel>();

            using (var api = ApiFactory.Create())
            {
                var sitemapId = GetSitemapId(api);
                if (sitemapId.HasValue)
                {
                    var request = new GetSitemapTreeRequest { SitemapId = sitemapId.Value, Data = new GetSitemapTreeModel { LanguageId = LanguageHelper.CurrentLanguageId } };

                    var response = api.Pages.Sitemap.Tree.Get(request);

                    if (response.Data.Count > 0)
                    {
                        menuItems = response.Data.Select(mi => new MenuItemViewModel { Caption = mi.Title, Url = mi.Url }).ToList();
                    }
                }
            }

            return View(menuItems);
        }


        public ActionResult FooterMenu()
        {
            var menuItems = new List<MenuItemViewModel>();

            using (var api = ApiFactory.Create())
            {
                var sitemapId = GetSitemapId(api);
                if (sitemapId.HasValue)
                {
                    var request = new GetSitemapTreeRequest { SitemapId = sitemapId.Value, Data = new GetSitemapTreeModel { LanguageId = LanguageHelper.CurrentLanguageId } };

                    var response = api.Pages.Sitemap.Tree.Get(request);
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    if (response.Data.Count > 0)
                    {
                        foreach (var item in response.Data)
                        {
                            if (item.ChildrenNodes.Count > 0)
                            {
                                sb.Append("<div class=\"col\">");
                                sb.Append("<h2>" + item.Title + "</h2>");
                                sb.Append("<ul>");
                                foreach (var item1 in item.ChildrenNodes)
                                {
                                    sb.Append("<li><a href=\"" + item1.Url + "\">" + item1.Title + "</a></li>");
                                }

                                sb.Append("<ul>");
                                sb.Append("</div>");

                            }
                        }
                        menuItems = response.Data.Select(mi => new MenuItemViewModel { Caption = mi.Title, Url = mi.Url }).ToList();
                    }
                    ViewBag.MenuString = sb.ToString();
                }
            }

            return View();
        }



        public ActionResult ChangeCulture(string lang, string returnUrl)
        {
            Session["Culture"] = new CultureInfo(lang);
            using (var api = ApiFactory.Create())
            {
                var request = new GetPageTranslationsRequest { PageUrl = returnUrl };

                var xx = api.Pages.Page.Translations.Get(request);
                if (xx.Data.TotalCount > 1)
                {

                    List<PageTranslationModel> pages1 = xx.Data.Items.ToList();
                    foreach (var item in pages1)
                    {
                        string language = item.LanguageCode != null ? item.LanguageCode : "vi";
                        if (language == lang)
                        {
                            return Redirect(item.PageUrl);
                        }
                    }
                }
            }
            return Redirect(returnUrl);
        }

        public virtual ActionResult SubMenu(string parentUrl)
        {
            //var menuItems = new List<MenuItemViewModel>();

            //using (var api = ApiFactory.Create())
            //{
            //    var pageRequest = new PageExistsRequest { PageUrl = parentUrl };
            //    var pageResponse = api.Pages.Page.Exists(pageRequest);

            //    var sitemapId = GetSitemapId(api);
            //    if (sitemapId.HasValue)
            //    {
            //        var parentRequest = new GetSitemapNodesRequest();
            //        parentRequest.SitemapId = sitemapId.Value;
            //        parentRequest.Data.Take = 1;
            //        parentRequest.Data.Filter.Add("ParentId", null);

            //        var filter = new DataFilter(FilterConnector.Or);
            //        parentRequest.Data.Filter.Inner.Add(filter);
            //        filter.Add("Url", parentUrl);
            //        if (pageResponse.Data.Exists)
            //        {
            //            filter.Add("PageId", pageResponse.Data.PageId.Value);
            //        }
            //        parentRequest.Data.Order.Add("DisplayOrder");

            //        var parentResponse = api.Pages.Sitemap.Nodes.Get(parentRequest);
            //        if (parentResponse.Data.Items.Count == 1)
            //        {
            //            var request = new GetSitemapTreeRequest { SitemapId = sitemapId.Value };
            //            request.Data.NodeId = parentResponse.Data.Items[0].Id;
            //            var response = api.Pages.Sitemap.Tree.Get(request);
            //            if (response.Data.Count > 0)
            //            {
            //                menuItems = response.Data.Select(mi => new MenuItemViewModel { Caption = mi.Title, Url = mi.Url }).ToList();
            //                //menuItems.Insert(0, new MenuItemViewModel { Caption = "Main", Url = parentUrl });
            //            }
            //        }
            //    }
            //}

            //return View(menuItems);
            var menuItems = new List<MenuItemViewModel>();

            using (var api = ApiFactory.Create())
            {
                var sitemapId = GetSitemapId(api);
                if (sitemapId.HasValue)
                {
                    var request = new GetSitemapTreeRequest { SitemapId = sitemapId.Value, Data = new GetSitemapTreeModel { LanguageId = LanguageHelper.CurrentLanguageId } };

                    var response = api.Pages.Sitemap.Tree.Get(request);
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    if (response.Data.Count > 0)
                    {
                        foreach (var item in response.Data)
                        {
                            if (item.ChildrenNodes.Count > 0)
                            {
                                if (item.Url == parentUrl)
                                {
                                    sb.Append("<div id=\"VMENU709487940\" class=\"VMENU\">");
                                    sb.Append("    <ul class=\"VMENU_0 VMENU_0_709487940 reset\">");
                                    foreach (var item1 in item.ChildrenNodes)
                                    {
                                        sb.Append("       <li>");
                                        sb.Append("       <a href=\""+item1.Url+"\" class=\"" + (System.Web.HttpContext.Current.Request.Path == item1.Url ? "\"a_s\"" : null) + "\"><span class=\"" + (System.Web.HttpContext.Current.Request.Path == item1.Url ? "\"s\"" : null) + "\">" + item1.Title + "</span></a>");
                                        sb.Append("       </li>");
                                        sb.Append("         <li class=\"d\"></li>");
                                    }
                                    sb.Append("    </ul>");
                                    sb.Append("</div>");


                                }

                            }
                        }
                        menuItems = response.Data.Select(mi => new MenuItemViewModel { Caption = mi.Title, Url = mi.Url }).ToList();
                    }
                    ViewBag.MenuString2 = sb.ToString();
                }
            }

            return View();
        }

       
        private Guid? GetSitemapId(IApiFacade api)
        {
            var allSitemaps = api.Pages.Sitemap.Get(new GetSitemapsRequest());
            if (allSitemaps.Data.Items.Count > 0)
            {
                var sitemap = allSitemaps.Data.Items.FirstOrDefault(map => map.Id == defaultSitemapId) ?? allSitemaps.Data.Items.First();
                return sitemap.Id;
            }

            return null;
        }
    }
}

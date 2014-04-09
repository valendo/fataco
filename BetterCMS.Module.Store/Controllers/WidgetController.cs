﻿using BetterCms.Module.Root.Mvc;
using BetterCMS.Module.Store.Commands.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using BetterCMS.Module.Store.ViewModels;
using System.Web.Configuration;
using System.Text;
using BetterCMS.Module.Store.Helpers;

namespace BetterCMS.Module.Store.Controllers
{
    public class WidgetController : CmsControllerBase
    {
        public ActionResult ProductList(string id, int? page, string detailUrl)
        {
            var listProduct = GetCommand<GetProductListByCategoryIdCommand>().ExecuteCommand(id);
            var pageNumber = page ?? 1;
            var pageSize = int.Parse(WebConfigurationManager.AppSettings["PageSize"].ToString());
            var pagedList = listProduct.ToPagedList(pageNumber, pageSize);
            bool showPager = false;
            if (listProduct.Count > pageSize)
            {
                showPager = true;
            }
            ViewBag.ShowPager = showPager;
            
            ViewBag.DetailUrl = detailUrl;
            return View(pagedList);
        }
        public ActionResult ProductDetail(string id)
        {
            var model = GetCommand<GetProductByIdCommand>().ExecuteCommand(id);
            return View(model);
        }

        public ActionResult CategoryList(string productUrl)
        {
            var list = GetCommand<GetCategoryListCommand>().ExecuteCommand(Guid.Empty);
            string id = "00000000";
            if (Request.QueryString["id"] != null)
            {
                id = Request.QueryString["id"];
            }
            
            StringBuilder sb = new StringBuilder();
            var root = list.Where(t => t.ParentId == Guid.Empty).ToList();
            sb.AppendLine("<div id=\"VMENU709487940\" class=\"VMENU\">");
            if (root.Count > 0)
            {
                sb.AppendLine("<ul class=\"VMENU_0 VMENU_0_709487940 reset\">");
                foreach (var item in root)
                {
                    string name = Utility.CurrentLanguageCode == "en" ? item.Name_en : item.Name;
                    string active_class = "";
                    if (item.Id.ToString().Contains(id))
                    {
                        active_class = "s";
                    }
                    var level1 = list.Where(t => t.ParentId == item.Id).OrderBy(t => t.SortOrder).ToList();
                    string url = "#";
                    if (!string.IsNullOrWhiteSpace(productUrl) && level1.Count == 0)
                    {
                        url = productUrl + "?t=" + item.Name.GenerateSlug() + "&id=" + item.Id.ToString().ShortGuid();
                    }
                    sb.AppendFormat("<li><a href=\"{0}\"><span class=\"{1}\">{2}</span></a>", url, active_class, name);
                    if (level1.Count > 0)
                    {
                        sb.AppendLine("<ul class=\"VMENU_1 VMENU_1_709487940 reset\">");
                        foreach (var item1 in level1)
                        {
                            string name1 = Utility.CurrentLanguageCode == "en" ? item1.Name_en : item1.Name;
                            string active_class1 = "";
                            if (item1.Id.ToString().Contains(id))
                            {
                                active_class1 = "s";
                            }
                            var level2 = list.Where(t => t.ParentId == item1.Id).OrderBy(t => t.SortOrder).ToList();
                            string url1 = "#";
                            if (!string.IsNullOrWhiteSpace(productUrl) && level2.Count == 0)
                            {
                                url1 = productUrl + "?t=" + item1.Name.GenerateSlug() + "&id=" + item1.Id.ToString().ShortGuid();
                            }
                            sb.AppendFormat("<li><a href=\"{0}\"><span class=\"{1}\">{2}</span></a>", url1, active_class1, name1);
                            if (level2.Count > 0)
                            {
                                sb.AppendLine("<ul class=\"VMENU_2 VMENU_2_709487940 reset\" style=\"display: none;\">");
                                foreach (var item2 in level2)
                                {
                                    string name2 = Utility.CurrentLanguageCode == "en" ? item2.Name_en : item2.Name;
                                    string active_class2 = "";
                                    if (item2.Id.ToString().Contains(id))
                                    {
                                        active_class2 = "s";
                                    }
                                    string url2 = "#";
                                    if (!string.IsNullOrWhiteSpace(productUrl))
                                    {
                                        url2 = productUrl + "?t=" + item2.Name.GenerateSlug() + "&id=" + item2.Id.ToString().ShortGuid();
                                    }
                                    sb.AppendFormat("<li><a href=\"{0}\"><span class=\"{1}\">{2}</span></a></li>", url2, active_class2, name2);
                                }
                                sb.AppendLine("</ul>");
                            }
                            sb.Append("</li>");
                        }
                        sb.AppendLine("</ul>");
                    }
                    sb.Append("</li>");
                    sb.AppendLine("<li class=\"d\"></li>");
                }
                sb.AppendLine("</ul>");
            }
            sb.AppendLine("</div>");
            ViewBag.MenuHtml = sb.ToString();
            return View(list);
        }
    }
}
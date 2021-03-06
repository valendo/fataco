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
        private void GetRecursiveCategories(string categoryId, ref List<string> categories)
        {
            categories.Add(categoryId);
            var subCategories = GetCommand<GetSubCategoriesCommand>().ExecuteCommand(categoryId);
            if (subCategories.Count > 0)
            {
                foreach (var item in subCategories)
                {
                    GetRecursiveCategories(item.Id.ToString().ShortGuid(), ref categories);
                }
            }
        }

        private List<ProductViewModel> GetProductList(List<string> categories)
        {
            List<ProductViewModel> allProducts = new List<ProductViewModel>();
            foreach (var item in categories)
            {
                var listProduct = GetCommand<GetProductListByCategoryIdCommand>().ExecuteCommand(item);
                if (listProduct.Count > 0)
                {
                    foreach (var product in listProduct)
                    {
                        allProducts.Add(product);
                    }
                }
            }
            return allProducts;
        }
        public ActionResult ProductList(string id, int? page, string detailUrl)
        {
            ViewBag.DetailUrl = detailUrl;
            if (!string.IsNullOrEmpty(id))
            {
                var category = GetCommand<GetCategoryCommand>().ExecuteCommand(id);
                //var listProduct = GetCommand<GetProductListByCategoryIdCommand>().ExecuteCommand(id);
                List<string> categories = new List<string>();
                GetRecursiveCategories(id, ref categories);
                var listProduct = GetProductList(categories);
                var pageNumber = page ?? 1;
                var pageSize = int.Parse(WebConfigurationManager.AppSettings["PageSize"].ToString());
                var pagedList = listProduct.ToPagedList(pageNumber, pageSize);
                bool showPager = false;
                if (listProduct.Count > pageSize)
                {
                    showPager = true;
                }
                ViewBag.ShowPager = showPager;
                ViewBag.Category = category;
                ViewBag.AllProduct = false;
                return View(pagedList);
            }
            else
            {
                var rootCategories = GetCommand<GetSubCategoriesCommand>().ExecuteCommand("00000000");
                if (rootCategories.Count > 2)
                {
                    var firstCategory = rootCategories.ElementAt(0);
                    var secondCategory = rootCategories.ElementAt(1);
                    var thirdCategory = rootCategories.ElementAt(2);
                    List<string> firstCategories = new List<string>();
                    GetRecursiveCategories(firstCategory.Id.ToString().ShortGuid(), ref firstCategories);
                    List<string> secondCategories = new List<string>();
                    GetRecursiveCategories(secondCategory.Id.ToString().ShortGuid(), ref secondCategories);
                    List<string> thirdCategories = new List<string>();
                    GetRecursiveCategories(thirdCategory.Id.ToString().ShortGuid(), ref thirdCategories);
                    var firstProducts = GetProductList(firstCategories);
                    var secondProducts = GetProductList(secondCategories);
                    var thirdProducts = GetProductList(thirdCategories);

                    ViewBag.FirstCategory = firstCategory;
                    ViewBag.SecondCategory = secondCategory;
                    ViewBag.ThirdCategory = thirdCategory;

                    var rand = new Random();

                    ViewBag.FirstProducts = firstProducts.OrderBy(t => rand.Next()).Take(4).ToList();
                    ViewBag.SecondProducts = secondProducts.OrderBy(t => rand.Next()).Take(4).ToList();
                    ViewBag.ThirdProducts = thirdProducts.OrderBy(t => rand.Next()).Take(4).ToList();
                }
                ViewBag.AllProduct = true;
                return View();
            }
        }

        public ActionResult FeaturedProduct(string detailUrl)
        {
            var listProduct = GetCommand<GetFeaturedProductCommand>().ExecuteCommand(string.Empty);
            ViewBag.DetailUrl = detailUrl;
            return View(listProduct);
        }

        public ActionResult SearchResult(string query, int? page, string detailUrl)
        {
            var listProduct = GetCommand<GetProductListByQueryCommand>().ExecuteCommand(query);
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

        private List<ProductViewModel> GetRealtedProducts(string[] shortIDs)
        {
            List<ProductViewModel> relatedProducts = new List<ProductViewModel>();
            foreach (var shortID in shortIDs)
            {
                var product = GetCommand<GetProductByShortIDCommand>().ExecuteCommand(shortID);
                if (product != null)
                {
                    relatedProducts.Add(product);
                }
            }
            return relatedProducts;
        }

        public ActionResult ProductDetail(string id)
        {
            var model = GetCommand<GetProductByIdCommand>().ExecuteCommand(id);
            var category = GetCommand<GetCategoryCommand>().ExecuteCommand(model.CategoryId.ToString());
            ViewBag.Category = category;
            if (model != null && !string.IsNullOrWhiteSpace(model.Size))
            {
                string[] arr = model.Size.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                if (arr.Length > 0)
                {
                    var relatedProducts = GetRealtedProducts(arr);
                    if (relatedProducts.Count > 0)
                    {
                        ViewBag.RelatedProducts = relatedProducts;
                    }
                }
            }
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
                    string active_class_a = "";
                    if (item.Id.ToString().Contains(id))
                    {
                        active_class = "s";
                        active_class_a = "a_s";
                    }
                    var level1 = list.Where(t => t.ParentId == item.Id).OrderBy(t => t.SortOrder).ToList();
                    string url = "#";
                    //if (!string.IsNullOrWhiteSpace(productUrl) && level1.Count == 0)
                    {
                        url = productUrl + "?t=" + name.GenerateSlug() + "&id=" + item.Id.ToString().ShortGuid();
                    }
                    sb.AppendFormat("<li><a class=\""+active_class_a+"\" href=\"{0}\"><span class=\"{1}\">{2}</span></a>", url, active_class, name);
                    if (level1.Count > 0)
                    {
                        sb.AppendLine("<ul class=\"VMENU_1 VMENU_1_709487940 reset\">");
                        foreach (var item1 in level1)
                        {
                            string name1 = Utility.CurrentLanguageCode == "en" ? item1.Name_en : item1.Name;
                            string active_class1 = "";
                            string active_class1_a = "";
                            if (item1.Id.ToString().Contains(id))
                            {
                                active_class1 = "s ";
                                active_class1_a = "a_s ";
                            }
                            var level2 = list.Where(t => t.ParentId == item1.Id).OrderBy(t => t.SortOrder).ToList();
                            string url1 = "#";
                            if (!string.IsNullOrWhiteSpace(productUrl) && level2.Count == 0)
                            {
                                url1 = productUrl + "?t=" + name1.GenerateSlug() + "&id=" + item1.Id.ToString().ShortGuid();
                            }
                            sb.AppendFormat("<li><a class=\"" + active_class1_a + "" + item1.Id.ToString().ShortGuid() + "\" href=\"{0}\"><span class=\"{1}\">{2}</span></a>", url1, active_class1, name1);
                            if (level2.Count > 0)
                            {
                                sb.AppendLine("<ul class=\"VMENU_2 VMENU_2_709487940 reset\" style=\"display: none;\">");
                                foreach (var item2 in level2)
                                {
                                    string name2 = Utility.CurrentLanguageCode == "en" ? item2.Name_en : item2.Name;
                                    string active_class2 = "";
                                    string active_class2_a = "";
                                    if (item2.Id.ToString().Contains(id))
                                    {
                                        active_class2 = "s ";
                                        active_class2_a = "a_s ";
                                    }
                                    string url2 = "#";
                                    if (!string.IsNullOrWhiteSpace(productUrl))
                                    {
                                        url2 = productUrl + "?t=" + name2.GenerateSlug() + "&id=" + item2.Id.ToString().ShortGuid();
                                    }
                                    sb.AppendFormat("<li><a class=\"" + active_class2_a + "" + item2.Id.ToString().ShortGuid() + "\" href=\"{0}\"><span class=\"{1}\">{2}</span></a></li>", url2, active_class2, name2);
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

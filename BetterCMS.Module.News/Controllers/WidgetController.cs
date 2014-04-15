using BetterCms.Module.Root.Mvc;
using BetterCMS.Module.News.Commands.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using BetterCMS.Module.News.ViewModels;
using System.Web.Configuration;

namespace BetterCMS.Module.News.Controllers
{
    public class WidgetController : CmsControllerBase
    {
        public ActionResult NewsList(string id, int? page, string detailUrl)
        {
            var listNews = GetCommand<GetNewsListByCategoryIdCommand>().ExecuteCommand(id);
            var pageNumber = page ?? 1;
            var pageSize = int.Parse(WebConfigurationManager.AppSettings["PageSize"].ToString());
            var pagedList = listNews.ToPagedList(pageNumber, pageSize);
            bool showPager = false;
            if (listNews.Count > pageSize)
            {
                showPager = true;
            }
            ViewBag.ShowPager = showPager;
            
            ViewBag.DetailUrl = detailUrl;
            return View(pagedList);
        }

        public ActionResult SearchResult(string query, int? page, string detailUrl)
        {
            var listNews = GetCommand<GetNewsListByQueryCommand>().ExecuteCommand(query);
            var pageNumber = page ?? 1;
            var pageSize = int.Parse(WebConfigurationManager.AppSettings["PageSize"].ToString());
            var pagedList = listNews.ToPagedList(pageNumber, pageSize);
            bool showPager = false;
            if (listNews.Count > pageSize)
            {
                showPager = true;
            }
            ViewBag.ShowPager = showPager;

            ViewBag.DetailUrl = detailUrl;
            return View(pagedList);
        }

        public ActionResult NewsDetail(string id)
        {
            var model = GetCommand<GetNewsByIdCommand>().ExecuteCommand(id);
            return View(model);
        }

        public ActionResult CategoryList(string newsUrl)
        {
            var list = GetCommand<GetCategoryListCommand>().ExecuteCommand(Guid.Empty);
            ViewBag.NewsUrl = newsUrl;
            return View(list);
        }
    }
}

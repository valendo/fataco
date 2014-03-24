//using BetterCms.Core.DataAccess;
//using BetterCms.Demo.Web.Infrastructure;
//using BetterCms.Module.MediaManager.Services;
//using BetterCms.Module.MediaManager.ViewModels;
//using BetterCMS.Module.News.Commands.News;
//using BetterCMS.Module.News.Models;
//using BetterCMS.Module.News.ViewModels;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using PagedList;
//using PagedList.Mvc;
//using BetterCMS.Module.News.Services;
//using BetterCMS.Module.News;
//using BetterCms.Module.Root.Mvc;

//namespace BetterCms.Demo.Web.Controllers
//{
//    public class NewsController : CmsControllerBase
//    {
//        public ActionResult List()
//        {
//            //using (var api = ApiFactory.Create())
//            //{
                
//            //    var list = api.News.GetNewsList(api.News.GetIdBySlug(categoryName));
//            //    var onePageOfList = list.ToPagedList(page ?? 1, 20);
//            //    ViewBag.OnePageOfList = onePageOfList;
//            //}
//            //var list = NewsService.GetNewsList(NewsService.GetIdBySlug(categoryName));
//            //var onePageOfList = list.ToPagedList(page ?? 1, 20);
//            //ViewBag.OnePageOfList = onePageOfList;

//            //var test = GetCommand<DefaultNewsService>().GetIdBySlug("cong-ty");
//            return View("List");
//        }

//    }
//}

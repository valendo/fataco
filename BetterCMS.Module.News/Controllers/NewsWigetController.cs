using BetterCms.Module.Blog.Services;
using BetterCms.Module.Root.Mvc;
using BetterCMS.Module.News.Commands.News;
using Microsoft.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BetterCMS.Module.News.Controllers
{
    [ActionLinkArea(NewsModuleDescriptor.NewsAreaName)]
    public class NewsWigetController : CmsControllerBase
    {
        private NewsWidgetCommand command = new NewsWidgetCommand();
      
        public ActionResult Index(int? page, string categoryName)
        {
            command.GetNewsList(command.GetIdBySlug(categoryName));
            return View();
        }

    }

    
}

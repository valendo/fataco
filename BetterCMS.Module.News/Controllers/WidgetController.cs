using BetterCms.Module.Root.Mvc;
using BetterCMS.Module.News.Commands.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BetterCMS.Module.News.Controllers
{
    public class WidgetController : CmsControllerBase
    {
        public ActionResult ListNews(string id)
        {
            Guid CategoryId = Guid.Empty;
            Guid.TryParse(id, out CategoryId);

            var model = GetCommand<GetNewsListByCategoryIdCommand>().ExecuteCommand(CategoryId);
            return View(model);
        }
    }
}

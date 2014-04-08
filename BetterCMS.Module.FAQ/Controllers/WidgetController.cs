using BetterCms.Module.Root.Mvc;
using BetterCMS.Module.FAQ.Commands.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BetterCMS.Module.FAQ.Controllers
{
    public class WidgetController : CmsControllerBase
    {
        public ActionResult FaqList(string id)
        {
            var listFaq = GetCommand<GetFaqListByCategoryIdCommand>().ExecuteCommand(id);
            return View(listFaq);
        }

        public ActionResult CategoryList()
        {
            var list = GetCommand<GetCategoryListCommand>().ExecuteCommand(Guid.Empty);
            return View(list);
        }
    }
}

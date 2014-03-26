using BetterCms.Core.Security;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCMS.Module.FAQ.Commands.Faq;
using BetterCMS.Module.FAQ.ViewModels;
using Microsoft.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BetterCMS.Module.FAQ.Controllers
{
    [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
    [ActionLinkArea(FaqModuleDescriptor.FaqAreaName)]
    public class FaqController : CmsControllerBase
    {
        public ActionResult Index(SearchableGridOptions request)
        {
            request.SetDefaultPaging();
            var model = GetCommand<GetFaqsCommand>().ExecuteCommand(request);

            return View(model);
        }

        private List<KeyValuePair<Guid, string>> GetCategories()
        {
            List<KeyValuePair<Guid, string>> categories = new List<KeyValuePair<Guid, string>>();
            var model = GetCommand<GetCategoriesCommand>().ExecuteCommand(Guid.Empty);
            if (model.Count > 0)
            {
                foreach (var item in model)
                {
                    categories.Add(new KeyValuePair<Guid, string>(item.Id, item.Name + " (" + item.Name_en + ")"));
                }
            }
            categories.Insert(0, new KeyValuePair<Guid, string>(Guid.Empty, "--None--"));
            return categories;
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = GetCommand<GetFaqCommand>().ExecuteCommand(System.Guid.Empty);
            ViewBag.Categories = GetCategories();
            var view = RenderView("Edit", model);

            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var model = GetCommand<GetFaqCommand>().ExecuteCommand(id.ToGuidOrDefault());
            ViewBag.Categories = GetCategories();
            var view = RenderView("Edit", model);

            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Save(FaqViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = GetCommand<SaveFaqCommand>().ExecuteCommand(model);
                if (response != null)
                {
                    if (model.Id.HasDefaultValue())
                    {
                        Messages.AddSuccess("Created successfully!");
                    }
                    return WireJson(true, response);
                }
            }

            return WireJson(false);
        }

        [HttpPost]
        public ActionResult Delete(string id, string version)
        {
            var success = GetCommand<DeleteFaqCommand>().ExecuteCommand(
                new FaqViewModel
                {
                    Id = id.ToGuidOrDefault(),
                    Version = version.ToIntOrDefault()
                });

            if (success)
            {
                Messages.AddSuccess("Deleted successfully!");
            }

            return WireJson(success);
        }
    }
}
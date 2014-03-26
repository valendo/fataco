using BetterCms.Core.Security;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCMS.Module.FAQ.Commands.Category;
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
    public class CategoryController : CmsControllerBase
    {
        public ActionResult Index(SearchableGridOptions request)
        {
            request.SetDefaultPaging();
            var model = GetCommand<GetCategoriesCommand>().ExecuteCommand(request);

            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = GetCommand<GetCategoryCommand>().ExecuteCommand(System.Guid.Empty);
            var view = RenderView("Edit", model);

            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var model = GetCommand<GetCategoryCommand>().ExecuteCommand(id.ToGuidOrDefault());
            var view = RenderView("Edit", model);
            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Save(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = GetCommand<SaveCategoryCommand>().ExecuteCommand(model);
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
        public ActionResult Delete (string id, string version)
        {
            var success = GetCommand<DeleteCategoryCommand>().ExecuteCommand(
                new CategoryViewModel
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

using BetterCms.Core.Security;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCMS.Module.Store.Commands;
using BetterCMS.Module.Store.Commands.ProductCategory;
using BetterCMS.Module.Store.ViewModels;
using Microsoft.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BetterCMS.Module.Store.Controllers
{
    [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
    [ActionLinkArea(StoreModuleDescriptor.StoreAreaName)]
    public class ProductCategoryController : CmsControllerBase
    {
        public ActionResult Index(SearchableGridOptions request)
        {
            request.SetDefaultPaging();
            var model = GetCommand<GetProductCategoriesCommand>().ExecuteCommand(request);

            return View(model);
        }

        [HttpGet]
        public ActionResult CreateCategory()
        {
            var model = GetCommand<GetProductCategoryCommand>().ExecuteCommand(System.Guid.Empty);
            ViewBag.ParentCategories = GetParentCategories();
            var view = RenderView("EditCategory", model);

            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        private List<KeyValuePair<Guid,string>> GetParentCategories()
        {
            List<KeyValuePair<Guid, string>> parentCategories = new List<KeyValuePair<Guid, string>>();
            GetParentCategoriesRecursive(new ProductCategoryViewModel { ParentId = Guid.Empty, Lang = "vi" }, ref parentCategories, 0);
            parentCategories.Insert(0, new KeyValuePair<Guid, string>(Guid.Empty, "--None--"));
            return parentCategories;
        }
        private void GetParentCategoriesRecursive(ViewModels.ProductCategoryViewModel request, ref List<KeyValuePair<Guid, string>> outCategories, int level)
        {
            string space = "";
            for (int i = 0; i < level; i++)
            {
                space += "---";
            }
            var model = GetCommand<GetParentCategoriesCommand>().ExecuteCommand(request);
            if (model.Count > 0)
            {
                foreach (var item in model)
                {
                    outCategories.Add(new KeyValuePair<Guid, string>(item.Id, space + item.Name));                    
                    GetParentCategoriesRecursive(new ViewModels.ProductCategoryViewModel { ParentId = item.Id, Lang = request.Lang }, ref outCategories, level + 1);
                }
            }
        }


        [HttpGet]
        public ActionResult EditCategory(string id)
        {
            var model = GetCommand<GetProductCategoryCommand>().ExecuteCommand(id.ToGuidOrDefault());
            ViewBag.ParentCategories = GetParentCategories();
            var view = RenderView("EditCategory", model);

            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveCategory(ProductCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = GetCommand<SaveProductCategoryCommand>().ExecuteCommand(model);
                if (response != null)
                {
                    if (model.Id.HasDefaultValue())
                    {
                        Messages.AddSuccess("created successfully");
                    }
                    return WireJson(true, response);
                }
            }

            return WireJson(false);
        }

        [HttpPost]
        public ActionResult DeleteCategory(string id, string version)
        {
            var success = GetCommand<DeleteProductCategoryCommand>().ExecuteCommand(
                new ProductCategoryViewModel
                {
                    Id = id.ToGuidOrDefault(),
                    Version = version.ToIntOrDefault()
                });

            if (success)
            {
                Messages.AddSuccess("deleted successfully");
            }

            return WireJson(success);
        }
    }
}

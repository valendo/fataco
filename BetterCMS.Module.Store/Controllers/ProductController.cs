using BetterCms.Core.Security;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCMS.Module.Store.Commands;
using BetterCMS.Module.Store.Commands.Product;
using BetterCMS.Module.Store.Commands.Category;
using BetterCMS.Module.Store.ViewModels;
using Microsoft.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BetterCMS.Module.Store.ViewModels.Filter;

namespace BetterCMS.Module.Store.Controllers
{
    [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
    [ActionLinkArea(StoreModuleDescriptor.StoreAreaName)]
    public class ProductController : CmsControllerBase
    {
        public ActionResult Index(ProductsFilter request)
        {
            request.SetDefaultPaging();
            ViewBag.ParentCategories = GetParentCategoriesForFilter();
            var model = GetCommand<GetProductsCommand>().ExecuteCommand(request);

            return View(model);
        }

        [HttpGet]
        public ActionResult CreateProduct()
        {
            var model = GetCommand<GetProductCommand>().ExecuteCommand(System.Guid.Empty);
            ViewBag.ParentCategories = GetParentCategories();
            var view = RenderView("EditProduct", model);

            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        private List<KeyValuePair<Guid,string>> GetParentCategoriesForFilter()
        {
            List<KeyValuePair<Guid, string>> parentCategories = new List<KeyValuePair<Guid, string>>();
            GetParentCategoriesRecursive(new CategoryViewModel { ParentId = Guid.Empty}, ref parentCategories, 0);
            parentCategories.Insert(0, new KeyValuePair<Guid, string>(Guid.Empty, "None Category"));
            parentCategories.Insert(0, new KeyValuePair<Guid, string>(Guid.Parse("99999999-9999-9999-9999-999999999999"), "--Select--"));
            return parentCategories;
        }
        private List<KeyValuePair<Guid, string>> GetParentCategories()
        {
            List<KeyValuePair<Guid, string>> parentCategories = new List<KeyValuePair<Guid, string>>();
            GetParentCategoriesRecursive(new CategoryViewModel { ParentId = Guid.Empty }, ref parentCategories, 0);
            parentCategories.Insert(0, new KeyValuePair<Guid, string>(Guid.Empty, "--None--"));
            return parentCategories;
        }

        private void GetParentCategoriesRecursive(ViewModels.CategoryViewModel request, ref List<KeyValuePair<Guid, string>> outCategories, int level)
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
                    GetParentCategoriesRecursive(new ViewModels.CategoryViewModel { ParentId = item.Id}, ref outCategories, level + 1);
                }
            }
        }


        [HttpGet]
        public ActionResult EditProduct(string id)
        {
            var model = GetCommand<GetProductCommand>().ExecuteCommand(id.ToGuidOrDefault());
            ViewBag.ParentCategories = GetParentCategories();
            var view = RenderView("EditProduct", model);

            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveProduct(EditProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = GetCommand<SaveProductCommand>().ExecuteCommand(model);
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
        public ActionResult DeleteProduct(string id, string version)
        {
            var success = GetCommand<DeleteProductCommand>().ExecuteCommand(
                new ProductViewModel
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

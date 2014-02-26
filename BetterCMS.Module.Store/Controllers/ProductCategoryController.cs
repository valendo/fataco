using BetterCms.Core.Security;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCMS.Module.Store.Commands;
using BetterCMS.Module.Store.ViewModels;
using Microsoft.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BetterCMS.Module.Store.Controllers
{
    [ActionLinkArea(StoreModuleDescriptor.StoreAreaName)]
    public class ProductCategoryController : CmsControllerBase
    {
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult ListTemplate()
        {
            var view = RenderView("List",null);
            var productCategories = GetCommand<GetProductCategoryListCommand>().ExecuteCommand(new SearchableGridOptions());
            return ComboWireJson(productCategories != null, view, productCategories, JsonRequestBehavior.AllowGet);
        }

        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult ProductCategoriesList(SearchableGridOptions request)
        {
            var model = GetCommand<GetProductCategoryListCommand>().ExecuteCommand(request);
            return WireJson(model != null, model);
        }

        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult SaveProductCategory(ProductCategoryViewModel model)
        {
            var success = false;
            ProductCategoryViewModel response = null;
            if (ModelState.IsValid)
            {
                response = GetCommand<SaveProductCategoryCommand>().ExecuteCommand(model);
                if (response != null)
                {
                    if (model.Id.HasDefaultValue())
                    {
                        Messages.AddSuccess("Created successfully.");
                    }
                    success = true;
                }
            }
            return WireJson(success, response);
        }

        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult DeleteProductCategory(string id, string version)
        {
            var request = new ProductCategoryViewModel { Id = id.ToGuidOrDefault(), Version = version.ToIntOrDefault() };
            var success = GetCommand<DeleteProductCategoryCommand>().ExecuteCommand(request);
            if (success)
            {
                if (!request.Id.HasDefaultValue())
                {
                    Messages.AddSuccess("Deleted successfully.");
                }
            }
            return WireJson(success);
        }
    }
}

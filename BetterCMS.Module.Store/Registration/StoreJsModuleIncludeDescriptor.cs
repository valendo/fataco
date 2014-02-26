using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCMS.Module.Store.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.Store.Registration
{
    public class StoreJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public StoreJsModuleIncludeDescriptor(ModuleDescriptor module) : base(module, "bcms.store")
        {
            Links = new IActionProjection[]
            {
                new JavaScriptModuleLinkTo<ProductCategoryController>(this, "loadSiteSettingsProductCategoriesUrl", c => c.ListTemplate()),
                new JavaScriptModuleLinkTo<ProductCategoryController>(this, "loadProductCategoriesUrl", c => c.ProductCategoriesList(null)),
                new JavaScriptModuleLinkTo<ProductCategoryController>(this, "saveProductCategoryUrl", c => c.SaveProductCategory(null)),
                new JavaScriptModuleLinkTo<ProductCategoryController>(this, "deleteProductCategoryUrl", c => c.DeleteProductCategory(null, null)),
            };
            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "productCategoryDialogTitle", () => "Product Categories"), 
                    new JavaScriptModuleGlobalization(this, "deleteProductCategoryDialogTitle", () => "Are you sure you want to delete?"), 
                };
        }
    }
}
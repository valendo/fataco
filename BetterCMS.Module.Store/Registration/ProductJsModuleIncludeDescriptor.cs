using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCMS.Module.Store.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.Store.Registration
{
    public class ProductJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public ProductJsModuleIncludeDescriptor(ModuleDescriptor module)
            : base(module, "bcms.product")
        {
            Links = new IActionProjection[]
            {
                new JavaScriptModuleLinkTo<ProductController>(this, "loadSiteSettingsProductsUrl", c => c.Index(null)),
                new JavaScriptModuleLinkTo<ProductController>(this, "loadEditProductUrl", c=> c.EditProduct("{0}")), 
                new JavaScriptModuleLinkTo<ProductController>(this, "loadCreateProductUrl", c=> c.CreateProduct()), 
                new JavaScriptModuleLinkTo<ProductController>(this, "deleteProductUrl", c => c.DeleteProduct("{0}", "{1}"))
            };
        }
    }
}
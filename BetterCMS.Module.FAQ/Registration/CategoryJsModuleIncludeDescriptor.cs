using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCMS.Module.FAQ.Commands;
using BetterCMS.Module.FAQ.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.FAQ.Registration
{
    public class CategoryJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public CategoryJsModuleIncludeDescriptor(ModuleDescriptor module)
            : base(module, "bcms.faq.category")
        {
            Links = new IActionProjection[]
                        {
                            new JavaScriptModuleLinkTo<CategoryController>(this, "loadSiteSettingsCategoriesUrl", c => c.Index(null)),
                            new JavaScriptModuleLinkTo<CategoryController>(this, "loadEditCategoryUrl", c=> c.Edit("{0}")), 
                            new JavaScriptModuleLinkTo<CategoryController>(this, "loadCreateCategoryUrl", c=> c.Create()), 
                            new JavaScriptModuleLinkTo<CategoryController>(this, "deleteCategoryUrl", c => c.Delete("{0}", "{1}"))
                        };
        }
    }
}
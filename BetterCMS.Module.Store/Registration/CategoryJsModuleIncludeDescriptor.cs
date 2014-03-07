using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCMS.Module.Store.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.Store.Registration
{
    public class CategoryJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public CategoryJsModuleIncludeDescriptor(ModuleDescriptor module) : base(module, "bcms.category")
        {
            Links = new IActionProjection[]
            {
                new JavaScriptModuleLinkTo<CategoryController>(this, "loadSiteSettingsCategoriesUrl", c => c.Index(null)),
                new JavaScriptModuleLinkTo<CategoryController>(this, "loadEditCategoryUrl", c=> c.EditCategory("{0}")), 
                new JavaScriptModuleLinkTo<CategoryController>(this, "loadCreateCategoryUrl", c=> c.CreateCategory()), 
                new JavaScriptModuleLinkTo<CategoryController>(this, "deleteCategoryUrl", c => c.DeleteCategory("{0}", "{1}")),
            };
            //Globalization = new IActionProjection[]
            //    {
            //        new JavaScriptModuleGlobalization(this, "categoriesListTabTitle", () =>"Categories"), 
            //        new JavaScriptModuleGlobalization(this, "categoriesAddNewTitle", () =>"Add new"),
            //        new JavaScriptModuleGlobalization(this, "deleteCategoryConfirmMessage", () => "Are you sure delete '{0}'?"),
            //        new JavaScriptModuleGlobalization(this, "editCategoryTitle", () => "Edit category"),
            //    };
        }
    }
}
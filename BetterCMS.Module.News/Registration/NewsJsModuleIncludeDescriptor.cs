using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCMS.Module.News.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.News.Registration
{
    public class NewsJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public NewsJsModuleIncludeDescriptor(ModuleDescriptor module)
            : base(module, "bcms.news")
        {
            Links = new IActionProjection[]
            {
                new JavaScriptModuleLinkTo<NewsController>(this, "loadSiteSettingsNewsUrl", c => c.Index(null)),
                new JavaScriptModuleLinkTo<NewsController>(this, "loadEditNewsUrl", c=> c.EditNews("{0}")), 
                new JavaScriptModuleLinkTo<NewsController>(this, "loadCreateNewsUrl", c=> c.CreateNews()), 
                new JavaScriptModuleLinkTo<NewsController>(this, "deleteNewsUrl", c => c.DeleteNews("{0}", "{1}"))
            };
        }
    }
}
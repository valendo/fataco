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
    public class FaqJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public FaqJsModuleIncludeDescriptor(ModuleDescriptor module)
            : base(module, "bcms.faq")
        {
            Links = new IActionProjection[]
            {
                new JavaScriptModuleLinkTo<FaqController>(this, "loadSiteSettingsFaqUrl", c => c.Index(null)),
                new JavaScriptModuleLinkTo<FaqController>(this, "loadEditFaqUrl", c=> c.Edit("{0}")), 
                new JavaScriptModuleLinkTo<FaqController>(this, "loadCreateFaqUrl", c=> c.Create()), 
                new JavaScriptModuleLinkTo<FaqController>(this, "deleteFaqUrl", c => c.Delete("{0}", "{1}"))
            };
        }
    }
}
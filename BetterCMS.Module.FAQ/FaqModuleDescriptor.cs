using BetterCms;
using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root;
using BetterCMS.Module.FAQ.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.FAQ
{
    public class FaqModuleDescriptor : ModuleDescriptor
    {
        internal const string ModuleName = "faq";
        internal const string FaqAreaName = "bcms-faq";
        public override string Description
        {
            get {return "FAQ module for BetterCMS"; }
        }

        public override string Name
        {
            get { return ModuleName; }
        }

        public override string AreaName
        {
            get
            {
                return FaqAreaName;
            }
        }

        private readonly CategoryJsModuleIncludeDescriptor faqJsModuleIncludeDescriptor;
        public FaqModuleDescriptor(ICmsConfiguration configuration)
            : base(configuration)
        {
            faqJsModuleIncludeDescriptor = new CategoryJsModuleIncludeDescriptor(this);
        }

        public override IEnumerable<CssIncludeDescriptor> RegisterCssIncludes()
        {
            return new[]
                {
                    new CssIncludeDescriptor(this, "style.css"),
                };
        }

        public override IEnumerable<JsIncludeDescriptor> RegisterJsIncludes()
        {
            return new JsIncludeDescriptor[]
                {
                    faqJsModuleIncludeDescriptor,
                    new FaqJsModuleIncludeDescriptor(this)
                };
        }

        public override IEnumerable<BetterCms.Core.Modules.Projections.IPageActionProjection> RegisterSiteSettingsProjections(Autofac.ContainerBuilder containerBuilder)
        {
            return new IPageActionProjection[]
            {
                new SeparatorProjection(10000), 
                new LinkActionProjection(faqJsModuleIncludeDescriptor, page => "loadSiteSettingsFaqModule")
                    {
                        Order = 10000,
                        Title = page => "FAQs",
                        CssClass = page => "bcms-sidebar-link",
                       AccessRole = RootModuleConstants.UserRoles.MultipleRoles(new string[]{RootModuleConstants.UserRoles.EditContent,RootModuleConstants.UserRoles.PublishContent,RootModuleConstants.UserRoles.DeleteContent})
                    }                                      
            };
        }
    }
}
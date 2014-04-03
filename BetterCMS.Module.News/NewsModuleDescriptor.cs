using BetterCms;
using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Core.Mvc.Extensions;
using BetterCms.Module.Root;
using BetterCMS.Module.News.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using System.Diagnostics.CodeAnalysis;

namespace BetterCMS.Module.News
{
    public class NewsModuleDescriptor : ModuleDescriptor
    {
        internal const string ModuleName = "news";
        internal const string NewsAreaName = "bcms-news";
        public override string Description
        {
            get { return "News module for BetterCms"; }
        }

        public override string Name
        {
            get { return ModuleName; }
        }

        public override string AreaName
        {
            get
            {
                return NewsAreaName;
            }
        }

        private readonly CategoryJsModuleIncludeDescriptor newsJsModuleIncludeDescriptor;
        public NewsModuleDescriptor(ICmsConfiguration configuration) : base(configuration)
        {
            newsJsModuleIncludeDescriptor = new CategoryJsModuleIncludeDescriptor(this);
        }

        public override IEnumerable<CssIncludeDescriptor> RegisterCssIncludes()
        {
            return new[]
                {
                    new CssIncludeDescriptor(this, "PagedList.css"),
                };
        }

        public override IEnumerable<JsIncludeDescriptor> RegisterJsIncludes()
        {
            return new []
                {
                    newsJsModuleIncludeDescriptor,
                    new NewsJsModuleIncludeDescriptor(this),
                    new JsIncludeDescriptor(this, "bcms.news.filter")
                };
        }

        public override IEnumerable<BetterCms.Core.Modules.Projections.IPageActionProjection> RegisterSiteSettingsProjections(Autofac.ContainerBuilder containerBuilder)
        {
            return new IPageActionProjection[]
            {
                new LinkActionProjection(newsJsModuleIncludeDescriptor, page => "loadSiteSettingsNewsModule")
                    {
                        Order = 10001,
                        Title = page => "News",
                        CssClass = page => "bcms-sidebar-link",
                        AccessRole = RootModuleConstants.UserRoles.MultipleRoles(RootModuleConstants.UserRoles.Administration)
                    }                                      
            };
        }
    }
}
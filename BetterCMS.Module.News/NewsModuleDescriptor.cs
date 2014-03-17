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

        private readonly CategoryJsModuleIncludeDescriptor storeJsModuleIncludeDescriptor;
        public NewsModuleDescriptor(ICmsConfiguration configuration) : base(configuration)
        {
            storeJsModuleIncludeDescriptor = new CategoryJsModuleIncludeDescriptor(this);
        }

        //public override IEnumerable<CssIncludeDescriptor> RegisterCssIncludes()
        //{
            
        //}

        public override IEnumerable<JsIncludeDescriptor> RegisterJsIncludes()
        {
            return new[]
                {
                    storeJsModuleIncludeDescriptor,
                    new NewsJsModuleIncludeDescriptor(this),
                    new JsIncludeDescriptor(this, "bcms.news.filter")
                };
        }

        private string minJsPath;
        private string minCssPath;

        public override string BaseModulePath
        {
            get
            {
                return VirtualPath.Combine("/", "file", AreaName);
            }
        }

        public override string MinifiedJsPath
        {
            get
            {
                return minJsPath ?? (minJsPath = VirtualPath.Combine(JsBasePath, string.Format("bcms.{0}.js", Name.ToLowerInvariant())));
            }
        }

        public override string MinifiedCssPath
        {
            get
            {
                return minCssPath ?? (minCssPath = VirtualPath.Combine(CssBasePath, string.Format("bcms.{0}.css", Name.ToLowerInvariant())));
            }
        }

        public override IEnumerable<BetterCms.Core.Modules.Projections.IPageActionProjection> RegisterSiteSettingsProjections(Autofac.ContainerBuilder containerBuilder)
        {
            return new IPageActionProjection[]
            {
                new SeparatorProjection(9999), 
                new LinkActionProjection(storeJsModuleIncludeDescriptor, page => "loadSiteSettingsNewsModule")
                    {
                        Order = 9999,
                        Title = page => "News",
                        CssClass = page => "bcms-sidebar-link",
                        AccessRole = RootModuleConstants.UserRoles.MultipleRoles(RootModuleConstants.UserRoles.Administration)
                    }                                      
            };
        }
    }
}
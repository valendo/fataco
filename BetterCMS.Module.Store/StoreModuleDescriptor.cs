using BetterCms;
using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Core.Mvc.Extensions;
using BetterCms.Module.Root;
using BetterCMS.Module.Store.Registration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.Store
{
    public class StoreModuleDescriptor : ModuleDescriptor
    {
        internal const string ModuleName = "Store";
        internal const string StoreAreaName = "bcms-store";

        public override string Description
        {
            get { return "Store module to manage products and categories."; }
        }

        public override string Name
        {
            get { return ModuleName; }
        }

        public override string AreaName
        {
            get
            {
                return StoreAreaName;
            }
        }

        private readonly StoreJsModuleIncludeDescriptor storeJsModuleIncludeDescriptor;
        public StoreModuleDescriptor(ICmsConfiguration configuration) : base(configuration)
        {
            storeJsModuleIncludeDescriptor = new StoreJsModuleIncludeDescriptor(this);
        }

        public override IEnumerable<JsIncludeDescriptor> RegisterJsIncludes()
        {
            return new[]
                {
                    storeJsModuleIncludeDescriptor,
                    new JsIncludeDescriptor(this, "bcms.category.filter")
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
                new LinkActionProjection(storeJsModuleIncludeDescriptor, page => "loadSiteSettingsStoreModule")
                    {
                        Order = 9999,
                        Title = page => "Products",
                        CssClass = page => "bcms-sidebar-link",
                        AccessRole = RootModuleConstants.UserRoles.MultipleRoles(RootModuleConstants.UserRoles.Administration)
                    }                                      
            };
        }
    }
}
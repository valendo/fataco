﻿using BetterCms;
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
        internal const string ModuleName = "store";
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

        private readonly CategoryJsModuleIncludeDescriptor storeJsModuleIncludeDescriptor;
        public StoreModuleDescriptor(ICmsConfiguration configuration) : base(configuration)
        {
            storeJsModuleIncludeDescriptor = new CategoryJsModuleIncludeDescriptor(this);
        }

        public override IEnumerable<CssIncludeDescriptor> RegisterCssIncludes()
        {
            return new[]
            {
                new CssIncludeDescriptor(this,"spectrum.css")
            };
        }

        public override IEnumerable<JsIncludeDescriptor> RegisterJsIncludes()
        {
            return new[]
                {
                    storeJsModuleIncludeDescriptor,
                    new ProductJsModuleIncludeDescriptor(this),
                    new JsIncludeDescriptor(this, "bcms.product.filter"),
                    new JsIncludeDescriptor(this, "spectrum")
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
                new LinkActionProjection(storeJsModuleIncludeDescriptor, page => "loadSiteSettingsStoreModule")
                    {
                        Order = 10000,
                        Title = page => "Products",
                        CssClass = page => "bcms-sidebar-link",
                        AccessRole = RootModuleConstants.UserRoles.MultipleRoles(RootModuleConstants.UserRoles.Administration)
                    }                                      
            };
        }
    }
}
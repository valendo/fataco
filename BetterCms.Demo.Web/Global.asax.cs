using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

using Autofac;

using BetterCMS.Module.Demo.Services;
using BetterCms.Core;
using BetterCms.Core.Dependencies;
using BetterCms.Core.Environment.Host;
using System.Globalization;
using System.Threading;
using System;

namespace BetterCms.Demo.Web
{
    public class MvcApplication : HttpApplication
    {
        private static ICmsHost cmsHost;

        protected void Application_Start()
        {
            cmsHost = CmsContext.RegisterHost();

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            cmsHost.OnApplicationStart(this);
        }

        protected void Application_BeginRequest()
        {
            cmsHost.OnBeginRequest(this);
        }

        protected void Application_EndRequest()
        {
            cmsHost.OnEndRequest(this);
        }

        protected void Application_Error()
        {
            cmsHost.OnApplicationError(this);
        }

        protected void Application_End()
        {
            cmsHost.OnApplicationEnd(this);
        }

        protected void Application_AuthenticateRequest()
        {
            using (var container = ContextScopeProvider.CreateChildContainer())
            {
                var installService = container.Resolve<IInstallService>();

                var dbShouldBeSet = installService.ShoulDatabaseBeSet();

                if (!dbShouldBeSet)
                {
                    cmsHost.OnAuthenticateRequest(this);
                }
                else
                {
                    installService.NavigateToDatabaseSetup();
                }
            }
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session != null)
            {
                CultureInfo ci = (CultureInfo)Session["Culture"];

                if (ci == null)
                {
                    ci = new CultureInfo("vi");
                    Session["Culture"] = ci;
                }

                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
            }
        }
    }
}

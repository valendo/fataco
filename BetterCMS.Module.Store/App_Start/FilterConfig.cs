﻿using System.Web;
using System.Web.Mvc;

namespace BetterCMS.Module.Store
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
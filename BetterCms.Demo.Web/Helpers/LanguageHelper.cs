using BetterCms.Module.Api;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace BetterCms.Demo.Web.Helpers
{
    public class LanguageHelper
    {
        public static Guid CurrentLanguageId { 
            get{
                using (var api = ApiFactory.Create())
                {
                    CultureInfo ci = (CultureInfo)HttpContext.Current.Session["Culture"];

                    string culture = string.Empty;
                    if (ci != null)
                    {
                        culture = ci.Name;
                    }
                    Guid LanguageId = Guid.Empty;
                    foreach (var item in api.Root.Languages.Get(new Module.Api.Operations.Root.Languages.GetLanguagesRequest()).Data.Items)
                    {
                        if (item.Code == culture)
                        {
                            LanguageId = item.Id;
                        }
                    }
                    return LanguageId;
                }
            }
        }

        public static string CurrentLanguageCode 
        {
            get
            {
                CultureInfo ci = (CultureInfo)HttpContext.Current.Session["Culture"];
                return ci.Name;
            }
        }
    }
}
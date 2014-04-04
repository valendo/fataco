using BetterCms.Core.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.News.Helpers
{
    public static class OptionsHelper
    {
        public static string GetValue(IList<IOptionValue> options, string key)
        {
            var option = options.FirstOrDefault(o => o.Key == key);
            if (option != null && option.Value != null)
            {
                return option.Value.ToString();
            }

            return null;
        }
    }
}
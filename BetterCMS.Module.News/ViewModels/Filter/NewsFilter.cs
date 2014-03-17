using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.News.ViewModels.Filter
{
    public class NewsFilter : SearchableGridOptions
    {
        public Guid? CategoryId { get; set; }
    }
}
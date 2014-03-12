using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.Store.ViewModels.Filter
{
    public class ProductsFilter : SearchableGridOptions
    {
        public Guid? CategoryId { get; set; }
        public bool? IsFeature { get; set; }
    }
}
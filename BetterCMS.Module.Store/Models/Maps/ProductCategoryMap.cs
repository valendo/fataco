using BetterCms.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.Store.Models.Maps
{
    public class ProductCategoryMap : EntityMapBase<BetterCMS.Module.Store.Models.ProductCategory>
    {
        public ProductCategoryMap() : base(StoreModuleDescriptor.ModuleName)
        {
            Table("Categories");
            Map(f => f.Name).Not.Nullable().Length(MaxLength.Name);
            Map(f => f.Name_en).Not.Nullable().Length(MaxLength.Name);
            Map(f => f.ParentId).Not.Nullable();
            Map(f => f.SortOrder).Nullable();
        }
    }
}
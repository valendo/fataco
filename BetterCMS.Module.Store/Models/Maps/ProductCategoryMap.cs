using BetterCms.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.Store.Models.Maps
{
    public class ProductCategoryMap : EntityMapBase<ProductCategory>
    {
        public ProductCategoryMap() : base(StoreModuleDescriptor.ModuleName)
        {
            Table("ProductCategories");
            Map(f => f.Name).Not.Nullable().Length(MaxLength.Name);
            Map(f => f.ParentId).Not.Nullable();
        }
    }
}
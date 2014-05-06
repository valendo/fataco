using BetterCms.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.Store.Models.Maps
{
    public class ProductMap : EntityMapBase<Product>
    {
        public ProductMap() : base(StoreModuleDescriptor.ModuleName)
        {
            Table("Products");
            Map(f => f.CategoryId).Nullable();
            Map(f => f.Code).Not.Nullable();
            Map(f => f.Size).Nullable();
            Map(f => f.Color).Nullable();
            Map(f => f.Description).Nullable().CustomType("StringClob").CustomSqlType("nvarchar(max)");
            Map(f => f.Description_en).Nullable().CustomType("StringClob").CustomSqlType("nvarchar(max)");
            Map(f => f.IsFeature);
            Map(f => f.SortOrder);
            References(f => f.Image).Cascade.SaveUpdate().LazyLoad();
        }
    }
}
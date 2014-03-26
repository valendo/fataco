using BetterCms.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.FAQ.Models.Maps
{
    public class FaqCategoryMap:EntityMapBase<FaqCategory>
    {
        public FaqCategoryMap()
            : base(FaqModuleDescriptor.ModuleName)
        {
            Table("Categories");
            Map(f => f.Name).Not.Nullable().Length(MaxLength.Name);
            Map(f => f.Name_en).Not.Nullable().Length(MaxLength.Name);
            Map(f => f.SortOrder).Nullable();
        }
    }
}
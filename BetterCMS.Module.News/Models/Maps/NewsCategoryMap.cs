using BetterCms.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.News.Models.Maps
{
    public class NewsCategoryMap : EntityMapBase<NewsCategory>
    {
        public NewsCategoryMap()
            : base(NewsModuleDescriptor.ModuleName)
        {
            Table("Categories");
            Map(f => f.Name).Not.Nullable().Length(MaxLength.Name);
            Map(f => f.Name_en).Not.Nullable().Length(MaxLength.Name);
            Map(f => f.ParentId).Not.Nullable();
            Map(f => f.SortOrder).Nullable();
        }
    }
}
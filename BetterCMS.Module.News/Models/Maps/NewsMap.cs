using BetterCms.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.News.Models.Maps
{
    public class NewsMap : EntityMapBase<News>
    {
        public NewsMap() : base(NewsModuleDescriptor.ModuleName)
        {
            Table("News");
            Map(f => f.CategoryId);
            Map(f => f.Title).Not.Nullable();
            Map(f => f.Title_en);
            Map(f => f.Summary);
            Map(f => f.Summary_en);
            Map(f => f.Content);
            Map(f => f.Content_en);
            Map(f => f.PublishDate);
            Map(f => f.SortOrder);
            References(f => f.Image).Cascade.SaveUpdate().LazyLoad();
        }
    }
}
using BetterCms.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.FAQ.Models.Maps
{
    public class FaqMap : EntityMapBase<Faq>
    {
        public FaqMap()
            : base(FaqModuleDescriptor.ModuleName)
        {
            Table("Faqs");
            Map(f => f.CategoryId);
            Map(f => f.Question).Not.Nullable();
            Map(f => f.Question_en);
            Map(f => f.Answer);
            Map(f => f.Answer_en);
            Map(f => f.SortOrder);
        }
    }
}
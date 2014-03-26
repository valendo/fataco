using BetterCms.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.FAQ.Models
{
    [Serializable]
    public class FaqCategory : EquatableEntity<FaqCategory>
    {
        public virtual string Name { get; set; }
        public virtual string Name_en { get; set; }
        public virtual int? SortOrder { get; set; }
    }
}
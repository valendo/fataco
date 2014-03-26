using BetterCms.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.FAQ.Models
{
    [Serializable]
    public class Faq : EquatableEntity<Faq>
    {
        public virtual Guid? CategoryId { get; set; }
        public virtual string Question { get; set; }
        public virtual string Question_en { get; set; }
        public virtual string Answer { get; set; }
        public virtual string Answer_en { get; set; }
        public virtual int? SortOrder { get; set; }
    }
}
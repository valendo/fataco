using BetterCms.Core.Models;
using BetterCms.Module.MediaManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.Store.Models
{
    [Serializable]
    public class Product : EquatableEntity<Product>
    {
        public virtual Guid? CategoryId { get; set; }
        public virtual string Code { get; set; }
        public virtual string Size { get; set; }
        public virtual string Color { get; set; }
        public virtual string Description { get; set; }
        public virtual string Description_en { get; set; }
        public virtual MediaImage Image { get; set; }
        public virtual bool IsFeature { get; set; }
        public virtual int? SortOrder { get; set; }

    }
}
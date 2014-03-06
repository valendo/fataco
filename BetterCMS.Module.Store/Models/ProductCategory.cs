using BetterCms.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.Store.Models
{
    [Serializable]
    public class ProductCategory : EquatableEntity<BetterCMS.Module.Store.Models.ProductCategory>
    {
        public virtual string Name { get; set; }
        public virtual string Name_en { get; set; }
        public virtual Guid ParentId { get; set; }
        public virtual int? SortOrder { get; set; }
    }
}
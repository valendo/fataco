using BetterCms.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.Store.Models
{
    [Serializable]
    public class ProductCategory : EquatableEntity<ProductCategory>
    {
        public virtual string Name { get; set; }
        public virtual Guid ParentId { get; set; }
        public virtual string Lang { get; set; }
    }
}
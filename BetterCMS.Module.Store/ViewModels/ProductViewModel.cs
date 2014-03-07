using BetterCms.Module.Root.Mvc.Grids;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.Store.ViewModels
{
    public class ProductViewModel : IEditableGridItem
    {
        [Required()]
        public Guid Id { get; set; }

        [Required()]
        public int Version { get; set; }
        public virtual Guid? CategoryId { get; set; }
        [Required()]
        public virtual string Code { get; set; }
        public virtual string Size { get; set; }
        public virtual string Color { get; set; }
        public virtual string Description { get; set; }
        public virtual string Description_en { get; set; }
        public virtual Guid? ImageId { get; set; }
        public virtual bool IsFeature { get; set; }
        public virtual int? SortOrder { get; set; }
    }
}
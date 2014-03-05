using BetterCms.Core.Models;
using BetterCms.Module.Root.Mvc.Grids;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.Store.ViewModels
{
    public class ProductCategoryViewModel : IEditableGridItem
    {
        [Required()]
        public virtual Guid Id { get; set; }

        [Required()]
        public virtual int Version { get; set; }


        [Required()]
        [StringLength(MaxLength.Name)]
        public virtual string Name { get; set; }

        [Required]
        public virtual Guid ParentId { get; set; }

        public virtual string Lang { get; set; }

        public virtual int? SortOrder { get; set; }
    }
}
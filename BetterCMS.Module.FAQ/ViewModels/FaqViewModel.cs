using BetterCms.Module.Root.Mvc.Grids;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.FAQ.ViewModels
{
    public class FaqViewModel: IEditableGridItem
    {
        [Required()]
        public Guid Id { get; set; }

        [Required()]
        public int Version { get; set; }
        public virtual Guid? CategoryId { get; set; }
        [Required()]
        public virtual string Question { get; set; }
        public virtual string Question_en { get; set; }
        public virtual string Answer { get; set; }
        public virtual string Answer_en { get; set; }
        public virtual int? SortOrder { get; set; }
    }
}
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Root.Mvc.Grids;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BetterCMS.Module.News.ViewModels
{
    public class EditNewsViewModel
    {
        public EditNewsViewModel()
        {
            Image = new ImageSelectorViewModel();
        }
        [Required()]
        public Guid Id { get; set; }

        [Required()]
        public int Version { get; set; }
        public virtual Guid? CategoryId { get; set; }
        [Required()]
        public virtual string Title { get; set; }
        public virtual string Title_en { get; set; }
        public virtual string Summary { get; set; }
        public virtual string Summary_en { get; set; }
        [AllowHtml]
        public virtual string Content { get; set; }
        [AllowHtml]
        public virtual string Content_en { get; set; }
        public virtual DateTime PublishDate { get; set; }
        public ImageSelectorViewModel Image { get; set; }
        public virtual int? SortOrder { get; set; }
    }
}
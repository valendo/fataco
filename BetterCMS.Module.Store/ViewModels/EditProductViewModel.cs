using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Root.Mvc.Grids;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BetterCMS.Module.Store.ViewModels
{
    public class EditProductViewModel
    {
        public EditProductViewModel()
        {
            Image = new ImageSelectorViewModel();
        }
        [Required()]
        public Guid Id { get; set; }

        [Required()]
        public int Version { get; set; }
        public virtual Guid? CategoryId { get; set; }
        [Required()]
        public virtual string Code { get; set; }
        public virtual string Size { get; set; }
        public virtual string Color { get; set; }
        [AllowHtml]
        public virtual string Description { get; set; }
        [AllowHtml]
        public virtual string Description_en { get; set; }
        public ImageSelectorViewModel Image { get; set; }
        public virtual bool IsFeature { get; set; }
        public virtual int? SortOrder { get; set; }
    }
}
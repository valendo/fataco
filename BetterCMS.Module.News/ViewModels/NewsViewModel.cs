﻿using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Root.Mvc.Grids;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.News.ViewModels
{
    public class NewsViewModel : IEditableGridItem
    {
        [Required()]
        public Guid Id { get; set; }

        [Required()]
        public int Version { get; set; }
        public virtual Guid? CategoryId { get; set; }
        [Required()]
        public virtual string Title { get; set; }
        public virtual string Title_en { get; set; }
        public virtual ImageSelectorViewModel Image { get; set; }
        public virtual int? SortOrder { get; set; }
    }
}
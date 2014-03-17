using BetterCms.Core.Models;
using BetterCms.Module.MediaManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.News.Models
{
    [Serializable]
    public class News : EquatableEntity<News>
    {
        public virtual Guid? CategoryId { get; set; }
        public virtual string Title { get; set; }
        public virtual string Title_en { get; set; }
        public virtual string Summary { get; set; }
        public virtual string Summary_en { get; set; }
        public virtual string Content { get; set; }
        public virtual string Content_en { get; set; }
        public virtual DateTime PublishDate { get; set; }
        public virtual MediaImage Image { get; set; }
        public virtual int? SortOrder { get; set; }

    }
}
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.News.Commands.Widget
{
    public class GetCategoryIdBySlugCommand : CommandBase, ICommand<string, Guid>
    {
        public Guid Execute(string slug)
        {
            //var query = Repository
            //    .AsQueryable<Models.NewsCategory>()
            //    .Select(t => new NewsViewModel
            //    {
            //        Id = t.Id,
            //        Version = t.Version,
            //        CategoryId = t.CategoryId,
            //        Title = t.Title,
            //        Title_en = t.Title_en,
            //        SortOrder = t.SortOrder,
            //        Image = t.Image != null && !t.Image.IsDeleted ?
            //                        new ImageSelectorViewModel
            //                        {
            //                            ImageId = t.Image.Id,
            //                            ImageUrl = fileUrlResolver.EnsureFullPathUrl(t.Image.PublicUrl),
            //                            ThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(t.Image.PublicThumbnailUrl),
            //                            ImageTooltip = t.Image.Caption,
            //                            FolderId = t.Image.Folder != null ? t.Image.Folder.Id : (Guid?)null
            //                        } : null
            //    });
            return Guid.Empty;
        }
    }
}
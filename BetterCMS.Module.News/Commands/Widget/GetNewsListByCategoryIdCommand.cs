using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Root.Mvc;
using BetterCMS.Module.News.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.News.Commands.Widget
{
    public class GetNewsListByCategoryIdCommand: CommandBase, ICommand<Guid, List<NewsViewModel>>
    {
        private readonly IMediaFileUrlResolver fileUrlResolver;
        public GetNewsListByCategoryIdCommand(IMediaFileUrlResolver fileUrlResolver)
        {
            this.fileUrlResolver = fileUrlResolver;
        }

        public List<NewsViewModel> Execute(Guid CategoryId)
        {
            var query = Repository
                .AsQueryable<Models.News>()
                .Where(t => t.CategoryId == CategoryId)
                .Select(t => new NewsViewModel
                {
                    Id = t.Id,
                    Version = t.Version,
                    CategoryId = t.CategoryId,
                    Title = t.Title,
                    Title_en = t.Title_en,
                    SortOrder = t.SortOrder,
                    Image = t.Image != null && !t.Image.IsDeleted ?
                                    new ImageSelectorViewModel
                                    {
                                        ImageId = t.Image.Id,
                                        ImageUrl = fileUrlResolver.EnsureFullPathUrl(t.Image.PublicUrl),
                                        ThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(t.Image.PublicThumbnailUrl),
                                        ImageTooltip = t.Image.Caption,
                                        FolderId = t.Image.Folder != null ? t.Image.Folder.Id : (Guid?)null
                                    } : null
                });

            return query.ToList();
        }
    }
}
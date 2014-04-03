using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Root.Mvc;
using BetterCMS.Module.News.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BetterCMS.Module.News.Helpers;

namespace BetterCMS.Module.News.Commands.Widget
{
    public class GetNewsListByCategoryIdCommand: CommandBase, ICommand<string, List<NewsViewModel>>
    {
        private readonly IMediaFileUrlResolver fileUrlResolver;
        public GetNewsListByCategoryIdCommand(IMediaFileUrlResolver fileUrlResolver)
        {
            this.fileUrlResolver = fileUrlResolver;
        }

        public List<NewsViewModel> Execute(string CategoryId)
        {
            var query = Repository
                .AsQueryable<Models.News>()
                .Where(t => t.CategoryId.ToString().Contains(CategoryId) || string.IsNullOrWhiteSpace(CategoryId))
                .OrderBy(t => t.SortOrder)
                .Select(t => new NewsViewModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    Title_en = t.Title_en,
                    Summary = t.Summary,
                    Summary_en = t.Summary_en,
                    Content = t.Content,
                    Content_en = t.Content_en,
                    PublishDate = t.PublishDate,
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
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
    public class GetNewsListByQueryCommand : CommandBase, ICommand<string, List<NewsViewModel>>
    {
        private readonly IMediaFileUrlResolver fileUrlResolver;
        public GetNewsListByQueryCommand(IMediaFileUrlResolver fileUrlResolver)
        {
            this.fileUrlResolver = fileUrlResolver;
        }
        public List<NewsViewModel> Execute(string query)
        {
            var list = Repository
                .AsQueryable<Models.News>()
                .Where(t => t.Title.Contains(query) || t.Title_en.Contains(query) || t.Summary.Contains(query) || t.Summary_en.Contains(query) || t.Content.Contains(query) || t.Content_en.Contains(query))
                .OrderBy(t => t.SortOrder).OrderByDescending(t => t.PublishDate)
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

            return list.ToList();
        }
    }
}
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;
using BetterCMS.Module.News.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Core.DataAccess.DataContext;
using NHibernate.Linq;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCMS.Module.News.ViewModels.Filter;

namespace BetterCMS.Module.News.Commands.News
{
    public class GetNewsListCommand : CommandBase, ICommand<NewsFilter, SearchableGridViewModel<NewsViewModel>>
    {
        private readonly IMediaFileUrlResolver fileUrlResolver;
        public GetNewsListCommand(IMediaFileUrlResolver fileUrlResolver)
        {
            this.fileUrlResolver = fileUrlResolver;
        }
        public SearchableGridViewModel<NewsViewModel> Execute(NewsFilter request)
        {
            request.SetDefaultSortingOptions("Title");

            var query = Repository
                .AsQueryable<Models.News>()
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

            //search
            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                query = query.Where(t => t.Title.Contains(request.SearchQuery) || t.Title_en.Contains(request.SearchQuery));
            }
            //filter
            if (request.CategoryId != null && request.CategoryId != Guid.Parse("99999999-9999-9999-9999-999999999999"))
            {
                query = query.Where(t => t.CategoryId == request.CategoryId);
            }
            //total count
            var count = query.ToRowCountFutureValue();
            //sorting, paging
            query = query.AddSortingAndPaging(request);
            return new SearchableGridViewModel<NewsViewModel>(query.ToList(), request, count.Value);
        }
    }
}
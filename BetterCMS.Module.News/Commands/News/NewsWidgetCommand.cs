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
using BetterCms.Module.Blog.Services;

namespace BetterCMS.Module.News.Commands.News
{
    public class NewsWidgetCommand : CommandBase
    {
        private readonly IMediaFileUrlResolver fileUrlResolver;
        private readonly IBlogService blogService;
        public NewsWidgetCommand(IMediaFileUrlResolver fileUrlResolver)
        {
            this.fileUrlResolver = fileUrlResolver;
        }
        public NewsWidgetCommand()
        { }
        public List<NewsViewModel> GetNewsList(Guid? categoryId)
        {
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

            //filter
            if (categoryId != null)
            {
                query = query.Where(t => t.CategoryId == categoryId);
            }
            return query.ToList();
        }

        public Guid GetIdBySlug(string str)
        {
            Guid id = Guid.Empty;
            var query = Repository.AsQueryable<Models.NewsCategory>()
                .Where(t => blogService.CreateBlogPermalink(t.Name) == str)
                .FirstOrDefault();
            if (query != null)
            {
                id = query.Id;
            }
            return id;
        }
    }
}
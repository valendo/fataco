using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Root.Mvc;
using BetterCMS.Module.News.Models;
using BetterCMS.Module.News.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.News.Services
{
    public class DefaultNewsService : CommandBase
    {
        //private readonly IMediaFileUrlResolver fileUrlResolver;
        //private readonly IBlogService blogService;

        //public DefaultNewsService(IMediaFileUrlResolver fileUrlResolver, IBlogService blogService)
        //{
        //    this.fileUrlResolver = fileUrlResolver;
        //    this.blogService = blogService;
        //}

        public Guid GetIdBySlug(string str)
        {
            Guid id = Guid.Empty;
            var query = Repository.AsQueryable<NewsCategory>()
                //.Where(t => blogService.CreateBlogPermalink(t.Name) == str)
                .Where(t => t.Id == Guid.Parse("F0C92167-126F-4B76-962B-A2F100E57F7B"))
                .FirstOrDefault();
            if (query != null)
            {
                id = query.Id;
            }
            return id;
        }

        //public List<ViewModels.NewsViewModel> GetNewsList(Guid? categoryId)
        //{
        //    var query = Repository
        //        .AsQueryable<Models.News>()
        //        .Select(t => new NewsViewModel
        //        {
        //            Id = t.Id,
        //            Version = t.Version,
        //            CategoryId = t.CategoryId,
        //            Title = t.Title,
        //            Title_en = t.Title_en,
        //            SortOrder = t.SortOrder,
        //            Image = t.Image != null && !t.Image.IsDeleted ?
        //                            new ImageSelectorViewModel
        //                            {
        //                                ImageId = t.Image.Id,
        //                                ImageUrl = fileUrlResolver.EnsureFullPathUrl(t.Image.PublicUrl),
        //                                ThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(t.Image.PublicThumbnailUrl),
        //                                ImageTooltip = t.Image.Caption,
        //                                FolderId = t.Image.Folder != null ? t.Image.Folder.Id : (Guid?)null
        //                            } : null
        //        });

        //    //filter
        //    if (categoryId != null)
        //    {
        //        query = query.Where(t => t.CategoryId == categoryId);
        //    }
        //    return query.ToList();
        //}
    }
}
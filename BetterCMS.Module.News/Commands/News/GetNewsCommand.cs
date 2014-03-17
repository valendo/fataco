using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCMS.Module.News.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Linq;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.MediaManager.Services;

namespace BetterCMS.Module.News.Commands.News
{
    public class GetNewsCommand : CommandBase, ICommand<Guid, EditNewsViewModel>
    {
        private readonly IMediaFileUrlResolver fileUrlResolver;
        public GetNewsCommand(IMediaFileUrlResolver fileUrlResolver)
        {
            this.fileUrlResolver = fileUrlResolver;
        }
        public EditNewsViewModel Execute(Guid productId)
        {
            EditNewsViewModel model;
            if (!productId.HasDefaultValue())
            {
                var listFuture = Repository.AsQueryable<Models.News>()
                    .Where(t => t.Id == productId)
                    .Select(
                        t =>
                            new EditNewsViewModel
                            {
                                Id = t.Id,
                                Version = t.Version,
                                CategoryId = t.CategoryId,
                                Title = t.Title,
                                Title_en = t.Title_en,
                                Summary = t.Summary,
                                Summary_en = t.Summary_en,
                                Content = t.Content,
                                Content_en = t.Content_en,
                                PublishDate = t.PublishDate,
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
                                
                            }
                    ).ToFuture();
                model = listFuture.FirstOne();
            }
            else
            {
                model = new EditNewsViewModel();
                model.PublishDate = DateTime.Today;
            }
            return model;
        }
    }
}
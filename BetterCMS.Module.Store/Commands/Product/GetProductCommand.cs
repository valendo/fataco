using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCMS.Module.Store.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Linq;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.MediaManager.Services;

namespace BetterCMS.Module.Store.Commands.Product
{
    public class GetProductCommand : CommandBase, ICommand<Guid, EditProductViewModel>
    {
        private readonly IMediaFileUrlResolver fileUrlResolver;
        public GetProductCommand(IMediaFileUrlResolver fileUrlResolver)
        {
            this.fileUrlResolver = fileUrlResolver;
        }
        public EditProductViewModel Execute(Guid productId)
        {
            EditProductViewModel model;
            if (!productId.HasDefaultValue())
            {
                var listFuture = Repository.AsQueryable<Models.Product>()
                    .Where(t => t.Id == productId)
                    .Select(
                        t =>
                            new EditProductViewModel
                            {
                                Id = t.Id,
                                Version = t.Version,
                                CategoryId = t.CategoryId,
                                Code = t.Code,
                                Size = t.Size,
                                Color = t.Color,
                                Description = t.Description,
                                Description_en = t.Description_en,
                                IsFeature = t.IsFeature,
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
                model = new EditProductViewModel();
            }
            return model;
        }
    }
}
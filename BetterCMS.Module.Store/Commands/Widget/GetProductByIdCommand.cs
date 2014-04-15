using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Root.Mvc;
using BetterCMS.Module.Store.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.Store.Commands.Widget
{
    public class GetProductByIdCommand : CommandBase, ICommand<string, ProductViewModel>
    {
        private readonly IMediaFileUrlResolver fileUrlResolver;
        public GetProductByIdCommand(IMediaFileUrlResolver fileUrlResolver)
        {
            this.fileUrlResolver = fileUrlResolver;
        }
        public ProductViewModel Execute(string id)
        {
            ProductViewModel model = new ProductViewModel();
            if (!string.IsNullOrWhiteSpace(id))
            {
                model = Repository.AsQueryable<Models.Product>()
                    .Where(t => t.Id.ToString().Contains(id))
                    .Select(
                        t =>
                            new ProductViewModel
                            {
                                Id = t.Id,
                                Version = t.Version,
                                CategoryId = t.CategoryId,
                                Code = t.Code,
                                Size = t.Size,
                                Color = t.Color,
                                Description = t.Description,
                                Description_en = t.Description_en,
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
                    ).FirstOrDefault();
            }
            return model;
        }
    }
}
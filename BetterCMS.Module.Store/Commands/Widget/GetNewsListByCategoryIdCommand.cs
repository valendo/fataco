using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Root.Mvc;
using BetterCMS.Module.Store.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BetterCMS.Module.Store.Helpers;

namespace BetterCMS.Module.Store.Commands.Widget
{
    public class GetProductListByCategoryIdCommand: CommandBase, ICommand<string, List<ProductViewModel>>
    {
        private readonly IMediaFileUrlResolver fileUrlResolver;
        public GetProductListByCategoryIdCommand(IMediaFileUrlResolver fileUrlResolver)
        {
            this.fileUrlResolver = fileUrlResolver;
        }

        public List<ProductViewModel> Execute(string CategoryId)
        {
            var query = Repository
                .AsQueryable<Models.Product>()
                .Where(t => t.CategoryId.ToString().Contains(CategoryId) || string.IsNullOrWhiteSpace(CategoryId))
                .OrderBy(t => t.SortOrder).OrderByDescending(t => t.Code)
                .Select(t => new ProductViewModel
                {
                    Id = t.Id,
                    Code = t.Code,
                    Size = t.Size,
                    Color = t.Color,
                    Description = t.Description,
                    Description_en = t.Description_en,
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
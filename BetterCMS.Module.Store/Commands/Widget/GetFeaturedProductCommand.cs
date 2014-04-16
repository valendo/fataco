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
    public class GetFeaturedProductCommand : CommandBase, ICommand<string, List<ProductViewModel>>
    {
        private readonly IMediaFileUrlResolver fileUrlResolver;
        public GetFeaturedProductCommand(IMediaFileUrlResolver fileUrlResolver)
        {
            this.fileUrlResolver = fileUrlResolver;
        }

        public List<ProductViewModel> Execute(string request)
        {
            var query = Repository
                .AsQueryable<Models.Product>()
                .Where(t => t.IsFeature)
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
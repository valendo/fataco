using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;
using BetterCMS.Module.Store.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Core.DataAccess.DataContext;
using NHibernate.Linq;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCMS.Module.Store.ViewModels.Filter;

namespace BetterCMS.Module.Store.Commands.Product
{
    public class GetProductsCommand : CommandBase, ICommand<ProductsFilter, SearchableGridViewModel<ProductViewModel>>
    {
        private readonly IMediaFileUrlResolver fileUrlResolver;
        public GetProductsCommand(IMediaFileUrlResolver fileUrlResolver)
        {
            this.fileUrlResolver = fileUrlResolver;
        }
        public SearchableGridViewModel<ProductViewModel> Execute(ProductsFilter request)
        {
            request.SetDefaultSortingOptions("Code");

            var query = Repository
                .AsQueryable<Models.Product>()
                .Select(t => new ProductViewModel
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
                });

            //search
            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                query = query.Where(t => t.Code.Contains(request.SearchQuery));
            }
            //filter
            if (request.CategoryId != null && request.CategoryId != Guid.Parse("99999999-9999-9999-9999-999999999999"))
            {
                query = query.Where(t => t.CategoryId == request.CategoryId);
            }
            if (request.IsFeature != null)
            {
                query = query.Where(t => t.IsFeature == request.IsFeature);
            }
            //total count
            var count = query.ToRowCountFutureValue();
            //sorting, paging
            query = query.AddSortingAndPaging(request);
            return new SearchableGridViewModel<ProductViewModel>(query.ToList(), request, count.Value);
        }
    }
}
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

namespace BetterCMS.Module.Store.Commands.Product
{
    public class GetProductsCommand : CommandBase, ICommand<SearchableGridOptions, SearchableGridViewModel<ProductViewModel>>
    {
        public SearchableGridViewModel<ProductViewModel> Execute(SearchableGridOptions request)
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
                    ImageId = t.ImageId,
                    IsFeature = t.IsFeature,
                    SortOrder = t.SortOrder
                });

            //search
            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                query = query.Where(t => t.Code.Contains(request.SearchQuery));
            }
            //total count
            var count = query.ToRowCountFutureValue();
            //sorting, paging
            query = query.AddSortingAndPaging(request);
            return new SearchableGridViewModel<ProductViewModel>(query.ToList(), request, count.Value);
        }
    }
}
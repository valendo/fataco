using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.ViewModels.SiteSettings;
using BetterCMS.Module.Store.Models;
using BetterCMS.Module.Store.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Input;
using BetterCms.Core.DataAccess.DataContext;

namespace BetterCMS.Module.Store.Commands
{
    public class GetProductCategoryListCommand : CommandBase, ICommand<SearchableGridOptions, SearchableGridViewModel<ProductCategoryViewModel>>
    {
        public SearchableGridViewModel<ProductCategoryViewModel> Execute(SearchableGridOptions request)
        {
            request.SetDefaultSortingOptions("Name");
            var query = Repository.AsQueryable<ProductCategory>();
            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                query = query.Where(a => a.Name.Contains(request.SearchQuery));
            }
            var productCategories = query
                .Select(productCategory =>
                    new ProductCategoryViewModel
                    {
                        Id = productCategory.Id,
                        Version = productCategory.Version,
                        Name = productCategory.Name,
                        ParentId = productCategory.ParentId
                    });

            var count = query.ToRowCountFutureValue();
            productCategories = productCategories.AddSortingAndPaging(request);
            return new SearchableGridViewModel<ProductCategoryViewModel>(productCategories.ToList(), request, count.Value);
        }
    }
}
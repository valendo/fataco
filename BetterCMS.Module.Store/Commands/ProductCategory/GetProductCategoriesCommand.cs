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
using BetterCMS.Module.Store.ViewModels.Filter;

namespace BetterCMS.Module.Store.Commands.ProductCategory
{
    public class GetProductCategoriesCommand : CommandBase, ICommand<CategoriesFilter, SearchableGridViewModel<ProductCategoryViewModel>>
    {
        public SearchableGridViewModel<ProductCategoryViewModel> Execute(CategoriesFilter request)
        {
            request.SetDefaultSortingOptions("Name");

            var query = Repository
                .AsQueryable<Models.ProductCategory>()
                .Select(category => new ProductCategoryViewModel
                {
                    Id= category.Id,
                    Version = category.Version,
                    Name = category.Name,
                    ParentId = category.ParentId,
                    Lang = category.Lang,
                    SortOrder = category.SortOrder
                });
            if (string.IsNullOrWhiteSpace(request.Lang))
            {
                request.Lang = "vi";
            }
            query = query.Where(t => t.Lang == request.Lang);

            //search
            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                query = query.Where(category => category.Name.Contains(request.SearchQuery));
            }
            //total count
            var count = query.ToRowCountFutureValue();
            //sorting, paging
            query = query.AddSortingAndPaging(request);
            return new SearchableGridViewModel<ProductCategoryViewModel>(query.ToList(), request, count.Value);
        }
    }
}
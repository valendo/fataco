using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;
using BetterCMS.Module.FAQ.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Core.DataAccess.DataContext;
using NHibernate.Linq;

namespace BetterCMS.Module.FAQ.Commands.Category
{
    public class GetCategoriesCommand : CommandBase, ICommand<SearchableGridOptions, SearchableGridViewModel<CategoryViewModel>>
    {
        public SearchableGridViewModel<CategoryViewModel> Execute(SearchableGridOptions request)
        {
            request.SetDefaultSortingOptions("Name");

            var query = Repository
                .AsQueryable<Models.FaqCategory>()
                .Select(category => new CategoryViewModel
                {
                    Id = category.Id,
                    Version = category.Version,
                    Name = category.Name,
                    Name_en = category.Name_en,
                    SortOrder = category.SortOrder
                });

            //search
            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                query = query.Where(category => category.Name.Contains(request.SearchQuery) || category.Name_en.Contains(request.SearchQuery));
            }
            //total count
            var count = query.ToRowCountFutureValue();
            //sorting, paging
            query = query.AddSortingAndPaging(request);
            return new SearchableGridViewModel<CategoryViewModel>(query.ToList(), request, count.Value);
        }
    }
}
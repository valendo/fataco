using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.Store.Commands.ProductCategory
{
    public class GetParentCategoriesCommand : CommandBase, ICommand<ViewModels.ProductCategoryViewModel, List<ViewModels.ProductCategoryViewModel>>
    {
        public List<ViewModels.ProductCategoryViewModel> Execute(ViewModels.ProductCategoryViewModel request)
        {
            var list = Repository.AsQueryable<Models.ProductCategory>()
                    .Where(t => t.ParentId == request.ParentId && t.Lang == request.Lang)
                    .Select(
                        category =>
                            new ViewModels.ProductCategoryViewModel
                            {
                                Id = category.Id,
                                Name = category.Name
                            }
                    ).ToList();
            return list;
        }
    }
}
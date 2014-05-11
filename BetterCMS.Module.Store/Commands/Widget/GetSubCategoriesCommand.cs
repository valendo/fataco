using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.Store.Commands.Widget
{
    public class GetSubCategoriesCommand : CommandBase, ICommand<string, List<ViewModels.CategoryViewModel>>
    {
        public List<ViewModels.CategoryViewModel> Execute(string categoryId)
        {
            var list = Repository.AsQueryable<Models.ProductCategory>()
                    .Where(t => t.ParentId.ToString().Contains(categoryId)).OrderBy(t => t.SortOrder)
                    .Select(
                        category =>
                            new ViewModels.CategoryViewModel
                            {
                                Id = category.Id,
                                Name_en = category.Name_en,
                                Name = category.Name
                            }
                    ).ToList();
            return list;
        }
    }
}
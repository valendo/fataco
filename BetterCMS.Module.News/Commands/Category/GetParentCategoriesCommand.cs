using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.News.Commands.Category
{
    public class GetParentCategoriesCommand : CommandBase, ICommand<ViewModels.CategoryViewModel, List<ViewModels.CategoryViewModel>>
    {
        public List<ViewModels.CategoryViewModel> Execute(ViewModels.CategoryViewModel request)
        {
            var list = Repository.AsQueryable<Models.NewsCategory>()
                    .Where(t => t.ParentId == request.ParentId).OrderBy(t => t.SortOrder)
                    .Select(
                        category =>
                            new ViewModels.CategoryViewModel
                            {
                                Id = category.Id,
                                Name = category.Name + " (" + category.Name_en + ")"
                            }
                    ).ToList();
            return list;
        }
    }
}
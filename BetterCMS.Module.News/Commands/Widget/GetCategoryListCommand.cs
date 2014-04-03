using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCMS.Module.News.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.News.Commands.Widget
{
    public class GetCategoryListCommand : CommandBase, ICommand<Guid, List<CategoryViewModel>>
    {
        public List<CategoryViewModel> Execute(Guid request)
        {
            var query = Repository
                .AsQueryable<Models.NewsCategory>()
                .OrderBy(t => t.SortOrder)
                .Select(category => new CategoryViewModel
                {
                    Id = category.Id,
                    Version = category.Version,
                    Name = category.Name,
                    Name_en = category.Name_en,
                    ParentId = category.ParentId,
                    SortOrder = category.SortOrder
                });

            return query.ToList();
        }
    }
}
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCMS.Module.FAQ.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.FAQ.Commands.Faq
{
    public class GetCategoriesCommand : CommandBase, ICommand<Guid? ,List<CategoryViewModel>>
    {
        public List<CategoryViewModel> Execute(Guid? request)
        {
            var query = Repository
                .AsQueryable<Models.FaqCategory>()
                .OrderBy(t => t.SortOrder)
                .Select(category => new CategoryViewModel
                {
                    Id = category.Id,
                    Version = category.Version,
                    Name = category.Name,
                    Name_en = category.Name_en,
                    SortOrder = category.SortOrder
                });
            return query.ToList();
        }
    }
}
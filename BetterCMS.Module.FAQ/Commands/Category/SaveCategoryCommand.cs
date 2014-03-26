using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataAccess.DataContext.Fetching;

namespace BetterCMS.Module.FAQ.Commands.Category
{
    public class SaveCategoryCommand: CommandBase, ICommand<ViewModels.CategoryViewModel, ViewModels.CategoryViewModel>
    {
        public ViewModels.CategoryViewModel Execute(ViewModels.CategoryViewModel request)
        {
            UnitOfWork.BeginTransaction();
            Models.FaqCategory category;
            if (!request.Id.HasDefaultValue())
            {
                category = Repository
                    .AsQueryable<Models.FaqCategory>()
                    .Where(c => c.Id == request.Id)
                    .ToList()
                    .FirstOne();
            }
            else
            {
                category = new Models.FaqCategory();
            }
            category.Version = request.Version;
            category.Name = request.Name;
            category.Name_en = request.Name_en;
            category.SortOrder = request.SortOrder;
            Repository.Save(category);
            UnitOfWork.Commit();

            return new ViewModels.CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Name_en = category.Name_en,
                Version = category.Version,
                SortOrder = category.SortOrder
            };
        }
    }
}
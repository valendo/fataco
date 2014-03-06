using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataAccess.DataContext.Fetching;

namespace BetterCMS.Module.Store.Commands.Category
{
    public class SaveCategoryCommand: CommandBase, ICommand<ViewModels.CategoryViewModel, ViewModels.CategoryViewModel>
    {
        public ViewModels.CategoryViewModel Execute(ViewModels.CategoryViewModel request)
        {
            //ValidateCategory(request);
            UnitOfWork.BeginTransaction();
            Models.ProductCategory category;
            if (!request.Id.HasDefaultValue())
            {
                category = Repository
                    .AsQueryable<Models.ProductCategory>()
                    .Where(c => c.Id == request.Id)
                    .ToList()
                    .FirstOne();
            }
            else
            {
                category = new Models.ProductCategory();
            }
            category.Version = request.Version;
            category.Name = request.Name;
            category.Name_en = request.Name_en;
            category.ParentId = request.ParentId;
            category.SortOrder = request.SortOrder;
            Repository.Save(category);
            UnitOfWork.Commit();

            return new ViewModels.CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Name_en = category.Name_en,
                Version = category.Version,
                ParentId = category.ParentId,
                SortOrder = category.SortOrder
            };
        }

        //private void ValidateCategory(ViewModels.CategoryViewModel model)
        //{
        //    var existingId = Repository
        //        .AsQueryable<Models.ProductCategory>(c => c.Name == model.Name.Trim() && c.Id != model.Id)
        //        .Select(r => r.Id)
        //        .FirstOrDefault();
        //    if (!existingId.HasDefaultValue())
        //    {
        //        var message = string.Format("Category name {0} exist", model.Name);
        //        var logMessage = string.Format("Faild to update category. Category name {0} already exist", model.Name);
        //    }
        //}
    }
}
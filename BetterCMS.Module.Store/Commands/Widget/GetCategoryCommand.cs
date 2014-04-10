using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCMS.Module.Store.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Linq;
using BetterCms.Core.DataAccess.DataContext;

namespace BetterCMS.Module.Store.Commands.Widget
{
    public class GetCategoryCommand : CommandBase, ICommand<string, CategoryViewModel>
    {
        public CategoryViewModel Execute(string categoryId)
        {
            CategoryViewModel model = new CategoryViewModel();
            model = Repository.AsQueryable<Models.ProductCategory>()
                    .Where(t => t.Id.ToString().Contains(categoryId))
                    .Select(
                        category =>
                            new CategoryViewModel
                            {
                                Id = category.Id,
                                Version = category.Version,
                                Name = category.Name,
                                Name_en = category.Name_en,
                                ParentId = category.ParentId,
                                SortOrder = category.SortOrder
                            }
                    ).FirstOrDefault();
            
            return model;
        }
    }
}
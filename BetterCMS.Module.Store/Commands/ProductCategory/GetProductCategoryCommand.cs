using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCMS.Module.Store.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Linq;
using BetterCms.Core.DataAccess.DataContext;

namespace BetterCMS.Module.Store.Commands.ProductCategory
{
    public class GetProductCategoryCommand : CommandBase, ICommand<Guid, ProductCategoryViewModel>
    {
        public ProductCategoryViewModel Execute(Guid categoryId)
        {
            ProductCategoryViewModel model;
            if (!categoryId.HasDefaultValue())
            {
                var listFuture = Repository.AsQueryable<Models.ProductCategory>()
                    .Where(bp => bp.Id == categoryId)
                    .Select(
                        category =>
                            new ProductCategoryViewModel
                            {
                                Id = category.Id,
                                Version = category.Version,
                                Name = category.Name,
                                ParentId = category.ParentId,
                                Lang = category.Lang
                            }
                    ).ToFuture();
                model = listFuture.FirstOne();
            }
            else
            {
                model = new ProductCategoryViewModel();
            }
            return model;
        }
    }
}
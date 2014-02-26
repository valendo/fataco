using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCMS.Module.Store.Models;
using BetterCMS.Module.Store.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BetterCms.Core.DataAccess.DataContext;

namespace BetterCMS.Module.Store.Commands
{
    public class SaveProductCategoryCommand : CommandBase, ICommand<ProductCategoryViewModel, ProductCategoryViewModel>
    {
        public ProductCategoryViewModel Execute(ProductCategoryViewModel request)
        {
            var isNew = request.Id.HasDefaultValue();
            var productCategory = isNew ? new ProductCategory() : Repository.AsQueryable<ProductCategory>(w => w.Id == request.Id).FirstOne();
            productCategory.Name = request.Name;
            productCategory.ParentId = request.ParentId;
            productCategory.Version = request.Version;
            Repository.Save(productCategory);
            UnitOfWork.Commit();
            return new ProductCategoryViewModel
            {
                Id = productCategory.Id,
                Version = productCategory.Version,
                Name = productCategory.Name,
                ParentId = productCategory.ParentId
            };
        }
    }
}
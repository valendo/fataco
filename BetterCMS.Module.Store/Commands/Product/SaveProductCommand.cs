using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataAccess.DataContext.Fetching;
using BetterCMS.Module.Store.ViewModels;

namespace BetterCMS.Module.Store.Commands.Product
{
    public class SaveProductCommand: CommandBase, ICommand<ProductViewModel, ProductViewModel>
    {
        public ProductViewModel Execute(ProductViewModel request)
        {
            UnitOfWork.BeginTransaction();
            Models.Product product;
            if (!request.Id.HasDefaultValue())
            {
                product = Repository
                    .AsQueryable<Models.Product>()
                    .Where(t => t.Id == request.Id)
                    .ToList()
                    .FirstOne();
            }
            else
            {
                product = new Models.Product();
            }
            product.Version = request.Version;
            product.CategoryId = request.CategoryId;
            product.Code = request.Code;
            product.Size = request.Size;
            product.Color = request.Color;
            product.Description = request.Description;
            product.Description_en = request.Description_en;
            product.ImageId = request.ImageId;
            product.IsFeature = request.IsFeature;
            product.SortOrder = request.SortOrder;
            Repository.Save(product);
            UnitOfWork.Commit();

            return new ProductViewModel
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                Code = product.Color,
                Size = product.Size,
                Color = product.Color,
                Description = product.Description,
                Description_en = product.Description_en,
                ImageId = product.ImageId,
                IsFeature = product.IsFeature,
                SortOrder = product.SortOrder
            };
        }
    }
}
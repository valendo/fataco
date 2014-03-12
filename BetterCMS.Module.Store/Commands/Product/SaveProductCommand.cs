using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataAccess.DataContext.Fetching;
using BetterCMS.Module.Store.ViewModels;
using BetterCms.Module.MediaManager.Models;

namespace BetterCMS.Module.Store.Commands.Product
{
    public class SaveProductCommand: CommandBase, ICommand<EditProductViewModel, ProductViewModel>
    {
        public ProductViewModel Execute(EditProductViewModel request)
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
            if (request.Image != null && request.Image.ImageId.HasValue)
            {
                product.Image = Repository.AsProxy<MediaImage>(request.Image.ImageId.Value);
            }
            else
            {
                product.Image = null;
            }
            product.IsFeature = request.IsFeature;
            product.SortOrder = request.SortOrder;
            Repository.Save(product);
            UnitOfWork.Commit();

            if (!request.Image.ImageId.HasValue)
            {
                request.Image.ThumbnailUrl = "/file/bcms-pages/content/styles/images/bcms-no-image-2.png";
            }

            return new ProductViewModel
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                Code = product.Code,
                Size = product.Size,
                Color = product.Color,
                Description = product.Description,
                Description_en = product.Description_en,
                IsFeature = product.IsFeature,
                SortOrder = product.SortOrder,
                Image = request.Image
            };
        }
    }
}
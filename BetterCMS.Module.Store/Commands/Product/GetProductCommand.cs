using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCMS.Module.Store.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Linq;
using BetterCms.Core.DataAccess.DataContext;

namespace BetterCMS.Module.Store.Commands.Product
{
    public class GetProductCommand : CommandBase, ICommand<Guid, ProductViewModel>
    {
        public ProductViewModel Execute(Guid productId)
        {
            ProductViewModel model;
            if (!productId.HasDefaultValue())
            {
                var listFuture = Repository.AsQueryable<Models.Product>()
                    .Where(t => t.Id == productId)
                    .Select(
                        t =>
                            new ProductViewModel
                            {
                                Id = t.Id,
                                Version = t.Version,
                                CategoryId = t.CategoryId,
                                Code = t.Code,
                                Size = t.Size,
                                Color = t.Color,
                                Description = t.Description,
                                Description_en = t.Description_en,
                                ImageId = t.ImageId,
                                IsFeature = t.IsFeature,
                                SortOrder = t.SortOrder
                                
                            }
                    ).ToFuture();
                model = listFuture.FirstOne();
            }
            else
            {
                model = new ProductViewModel();
            }
            return model;
        }
    }
}
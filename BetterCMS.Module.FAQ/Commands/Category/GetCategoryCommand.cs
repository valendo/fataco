﻿using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCMS.Module.FAQ.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Linq;
using BetterCms.Core.DataAccess.DataContext;

namespace BetterCMS.Module.FAQ.Commands.Category
{
    public class GetCategoryCommand : CommandBase, ICommand<Guid, CategoryViewModel>
    {
        public CategoryViewModel Execute(Guid categoryId)
        {
            CategoryViewModel model;
            if (!categoryId.HasDefaultValue())
            {
                var listFuture = Repository.AsQueryable<Models.FaqCategory>()
                    .Where(bp => bp.Id == categoryId)
                    .Select(
                        category =>
                            new CategoryViewModel
                            {
                                Id = category.Id,
                                Version = category.Version,
                                Name = category.Name,
                                Name_en = category.Name_en,
                                SortOrder = category.SortOrder
                            }
                    ).ToFuture();
                model = listFuture.FirstOne();
            }
            else
            {
                model = new CategoryViewModel();
            }
            return model;
        }
    }
}
﻿using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCMS.Module.Store.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.Store.Commands.ProductCategory
{
    public class DeleteProductCategoryCommand : CommandBase, ICommand<ProductCategoryViewModel, bool>
    {
        public bool Execute(ProductCategoryViewModel request)
        {
            Repository.Delete<Models.ProductCategory>(request.Id, request.Version);
            UnitOfWork.Commit();
            return true;
        }
    }
}
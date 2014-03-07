using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCMS.Module.Store.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.Store.Commands.Product
{
    public class DeleteProductCommand : CommandBase, ICommand<ProductViewModel, bool>
    {
        public bool Execute(ProductViewModel request)
        {
            Repository.Delete<Models.Product>(request.Id, request.Version);
            UnitOfWork.Commit();
            return true;
        }
    }
}
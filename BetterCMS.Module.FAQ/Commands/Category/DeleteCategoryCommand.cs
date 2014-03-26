using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCMS.Module.FAQ.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.FAQ.Commands.Category
{
    public class DeleteCategoryCommand : CommandBase, ICommand<CategoryViewModel, bool>
    {
        public bool Execute(CategoryViewModel request)
        {
            Repository.Delete<Models.FaqCategory>(request.Id, request.Version);
            UnitOfWork.Commit();
            return true;
        }
    }
}
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCMS.Module.FAQ.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.FAQ.Commands.Faq
{
    public class DeleteFaqCommand : CommandBase, ICommand<FaqViewModel, bool>
    {
        public bool Execute(FaqViewModel request)
        {
            Repository.Delete<Models.Faq>(request.Id, request.Version);
            UnitOfWork.Commit();
            return true;
        }
    }
}
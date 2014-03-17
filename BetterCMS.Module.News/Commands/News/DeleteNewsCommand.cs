using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCMS.Module.News.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.News.Commands.News
{
    public class DeleteNewsCommand : CommandBase, ICommand<NewsViewModel, bool>
    {
        public bool Execute(NewsViewModel request)
        {
            Repository.Delete<Models.News>(request.Id, request.Version);
            UnitOfWork.Commit();
            return true;
        }
    }
}
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCMS.Module.FAQ.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Linq;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.MediaManager.Services;

namespace BetterCMS.Module.FAQ.Commands.Faq
{
    public class GetFaqCommand : CommandBase, ICommand<Guid, FaqViewModel>
    {
        public FaqViewModel Execute(Guid id)
        {
            FaqViewModel model;
            if (!id.HasDefaultValue())
            {
                var listFuture = Repository.AsQueryable<Models.Faq>()
                    .Where(t => t.Id == id)
                    .Select(
                        t =>
                            new FaqViewModel
                            {
                                Id = t.Id,
                                Version = t.Version,
                                CategoryId = t.CategoryId,
                                Question = t.Question,
                                Question_en = t.Question_en,
                                Answer = t.Answer,
                                Answer_en = t.Answer_en,
                                SortOrder = t.SortOrder
                            }
                    ).ToFuture();
                model = listFuture.FirstOne();
            }
            else
            {
                model = new FaqViewModel();
            }
            return model;
        }
    }
}
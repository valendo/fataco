using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataAccess.DataContext.Fetching;
using BetterCMS.Module.FAQ.ViewModels;
using BetterCms.Module.MediaManager.Models;

namespace BetterCMS.Module.FAQ.Commands.Faq
{
    public class SaveFaqCommand: CommandBase, ICommand<FaqViewModel, FaqViewModel>
    {
        public FaqViewModel Execute(FaqViewModel request)
        {
            UnitOfWork.BeginTransaction();
            Models.Faq model;
            if (!request.Id.HasDefaultValue())
            {
                model = Repository
                    .AsQueryable<Models.Faq>()
                    .Where(t => t.Id == request.Id)
                    .ToList()
                    .FirstOne();
            }
            else
            {
                model = new Models.Faq();
            }
            model.Version = request.Version;
            model.CategoryId = request.CategoryId;
            model.Question = request.Question;
            model.Question_en = request.Question_en;
            model.Answer = request.Answer;
            model.Answer_en = request.Answer_en;
            model.SortOrder = request.SortOrder;
            
            Repository.Save(model);
            UnitOfWork.Commit();

            return new FaqViewModel
            {
                Id = model.Id,
                CategoryId = model.CategoryId,
                Question = model.Question,
                Question_en = model.Question_en,
                Answer = model.Answer,
                Answer_en = model.Answer_en,
                SortOrder = model.SortOrder
            };
        }
    }
}
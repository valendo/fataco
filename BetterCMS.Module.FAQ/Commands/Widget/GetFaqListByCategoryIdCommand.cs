using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Root.Mvc;
using BetterCMS.Module.FAQ.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BetterCMS.Module.FAQ.Helpers;

namespace BetterCMS.Module.FAQ.Commands.Widget
{
    public class GetFaqListByCategoryIdCommand: CommandBase, ICommand<string, List<FaqViewModel>>
    {
        public List<FaqViewModel> Execute(string CategoryId)
        {
            var query = Repository
                .AsQueryable<Models.Faq>()
                .Where(t => t.CategoryId.ToString().Contains(CategoryId) || string.IsNullOrWhiteSpace(CategoryId))
                .OrderBy(t => t.SortOrder)
                .Select(t => new FaqViewModel
                {
                    Id = t.Id,
                    Question = t.Question,
                    Question_en = t.Question_en,
                    Answer = t.Answer,
                    Answer_en = t.Answer_en
                });

            return query.ToList();
        }
    }
}
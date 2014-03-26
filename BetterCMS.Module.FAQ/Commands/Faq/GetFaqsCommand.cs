using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;
using BetterCMS.Module.FAQ.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Core.DataAccess.DataContext;
using NHibernate.Linq;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels;

namespace BetterCMS.Module.FAQ.Commands.Faq
{
    public class GetFaqsCommand : CommandBase, ICommand<SearchableGridOptions, SearchableGridViewModel<FaqViewModel>>
    {
        public SearchableGridViewModel<FaqViewModel> Execute(SearchableGridOptions request)
        {
            request.SetDefaultSortingOptions("Question");

            var query = Repository
                .AsQueryable<Models.Faq>()
                .Select(t => new FaqViewModel
                {
                    Id = t.Id,
                    Version = t.Version,
                    CategoryId = t.CategoryId,
                    Question = t.Question,
                    Question_en = t.Question_en,
                    Answer = t.Answer,
                    Answer_en = t.Answer_en,
                    SortOrder = t.SortOrder
                });

            //search
            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                query = query.Where(t => t.Question.Contains(request.SearchQuery) || t.Question_en.Contains(request.SearchQuery));
            }
            //total count
            var count = query.ToRowCountFutureValue();
            //sorting, paging
            query = query.AddSortingAndPaging(request);
            return new SearchableGridViewModel<FaqViewModel>(query.ToList(), request, count.Value);
        }
    }
}
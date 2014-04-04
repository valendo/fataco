using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCMS.Module.News.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.News.Commands.Widget
{
    public class GetNewsByIdCommand : CommandBase, ICommand<string, NewsViewModel>
    {
        public NewsViewModel Execute(string id)
        {
            NewsViewModel model = new NewsViewModel();
            if (!string.IsNullOrWhiteSpace(id))
            {
                model = Repository.AsQueryable<Models.News>()
                    .Where(t => t.Id.ToString().Contains(id))
                    .Select(
                        t =>
                            new NewsViewModel
                            {
                                Id = t.Id,
                                Version = t.Version,
                                CategoryId = t.CategoryId,
                                Title = t.Title,
                                Title_en = t.Title_en,
                                Summary = t.Summary,
                                Summary_en = t.Summary_en,
                                Content = t.Content,
                                Content_en = t.Content_en,
                                PublishDate = t.PublishDate,
                                SortOrder = t.SortOrder
                            }
                    ).FirstOrDefault();
            }
            return model;
        }
    }
}
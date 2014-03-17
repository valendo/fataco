using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataAccess.DataContext.Fetching;
using BetterCMS.Module.News.ViewModels;
using BetterCms.Module.MediaManager.Models;

namespace BetterCMS.Module.News.Commands.News
{
    public class SaveNewsCommand: CommandBase, ICommand<EditNewsViewModel, NewsViewModel>
    {
        public NewsViewModel Execute(EditNewsViewModel request)
        {
            UnitOfWork.BeginTransaction();
            Models.News model;
            if (!request.Id.HasDefaultValue())
            {
                model = Repository
                    .AsQueryable<Models.News>()
                    .Where(t => t.Id == request.Id)
                    .ToList()
                    .FirstOne();
            }
            else
            {
                model = new Models.News();
            }
            model.Version = request.Version;
            model.CategoryId = request.CategoryId;
            model.Title = request.Title;
            model.Title_en = request.Title_en;
            model.Summary = request.Summary;
            model.Summary_en = request.Summary_en;
            model.Content = request.Content;
            model.Content_en = request.Content_en;
            model.PublishDate = request.PublishDate;
            model.SortOrder = request.SortOrder;
            if (request.Image != null && request.Image.ImageId.HasValue)
            {
                model.Image = Repository.AsProxy<MediaImage>(request.Image.ImageId.Value);
            }
            else
            {
                model.Image = null;
            }
            Repository.Save(model);
            UnitOfWork.Commit();

            if (!request.Image.ImageId.HasValue)
            {
                request.Image.ThumbnailUrl = "/file/bcms-pages/content/styles/images/bcms-no-image-2.png";
            }

            return new NewsViewModel
            {
                Id = model.Id,
                CategoryId = model.CategoryId,
                Title = model.Title,
                Title_en = model.Title_en,
                SortOrder = model.SortOrder,
                Image = request.Image
            };
        }
    }
}
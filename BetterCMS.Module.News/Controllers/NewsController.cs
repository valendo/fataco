using BetterCms.Core.Security;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCMS.Module.News.Commands;
using BetterCMS.Module.News.Commands.News;
using BetterCMS.Module.News.Commands.Category;
using BetterCMS.Module.News.ViewModels;
using Microsoft.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BetterCMS.Module.News.ViewModels.Filter;


namespace BetterCMS.Module.News.Controllers
{
    [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
    [ActionLinkArea(NewsModuleDescriptor.NewsAreaName)]
    public class NewsController : CmsControllerBase
    {
        public ActionResult Index(NewsFilter request)
        {
            request.SetDefaultPaging();
            ViewBag.ParentCategories = GetParentCategoriesForFilter();
            var model = GetCommand<GetNewsListCommand>().ExecuteCommand(request);

            return View(model);
        }

        [HttpGet]
        public ActionResult CreateNews()
        {
            var model = GetCommand<GetNewsCommand>().ExecuteCommand(System.Guid.Empty);
            ViewBag.ParentCategories = GetParentCategories();
            var view = RenderView("EditNews", model);

            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        private List<KeyValuePair<Guid,string>> GetParentCategoriesForFilter()
        {
            List<KeyValuePair<Guid, string>> parentCategories = new List<KeyValuePair<Guid, string>>();
            GetParentCategoriesRecursive(new CategoryViewModel { ParentId = Guid.Empty}, ref parentCategories, 0);
            parentCategories.Insert(0, new KeyValuePair<Guid, string>(Guid.Empty, "None Category"));
            parentCategories.Insert(0, new KeyValuePair<Guid, string>(Guid.Parse("99999999-9999-9999-9999-999999999999"), "--Select--"));
            return parentCategories;
        }
        private List<KeyValuePair<Guid, string>> GetParentCategories()
        {
            List<KeyValuePair<Guid, string>> parentCategories = new List<KeyValuePair<Guid, string>>();
            GetParentCategoriesRecursive(new CategoryViewModel { ParentId = Guid.Empty }, ref parentCategories, 0);
            parentCategories.Insert(0, new KeyValuePair<Guid, string>(Guid.Empty, "--None--"));
            return parentCategories;
        }

        private void GetParentCategoriesRecursive(ViewModels.CategoryViewModel request, ref List<KeyValuePair<Guid, string>> outCategories, int level)
        {
            string space = "";
            for (int i = 0; i < level; i++)
            {
                space += "---";
            }
            var model = GetCommand<GetParentCategoriesCommand>().ExecuteCommand(request);
            if (model.Count > 0)
            {
                foreach (var item in model)
                {
                    outCategories.Add(new KeyValuePair<Guid, string>(item.Id, space + item.Name));                    
                    GetParentCategoriesRecursive(new ViewModels.CategoryViewModel { ParentId = item.Id}, ref outCategories, level + 1);
                }
            }
        }


        [HttpGet]
        public ActionResult EditNews(string id)
        {
            var model = GetCommand<GetNewsCommand>().ExecuteCommand(id.ToGuidOrDefault());
            ViewBag.ParentCategories = GetParentCategories();
            var view = RenderView("EditNews", model);

            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveNews(EditNewsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = GetCommand<SaveNewsCommand>().ExecuteCommand(model);
                if (response != null)
                {
                    if (model.Id.HasDefaultValue())
                    {
                        Messages.AddSuccess("Created successfully!");
                    }
                    return WireJson(true, response);
                }
            }

            return WireJson(false);
        }

        [HttpPost]
        public ActionResult DeleteNews(string id, string version)
        {
            var success = GetCommand<DeleteNewsCommand>().ExecuteCommand(
                new NewsViewModel
                {
                    Id = id.ToGuidOrDefault(),
                    Version = version.ToIntOrDefault()
                });

            if (success)
            {
                Messages.AddSuccess("Deleted successfully!");
            }

            return WireJson(success);
        }
    }
}

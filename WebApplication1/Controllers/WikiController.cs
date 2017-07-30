using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.Web.Mvc;
using RuPM.Models.Database;

namespace RuPM.Controllers
{
    public class EditPageFormModel
    {
        public int Id { get; set; }
        [AllowHtml]
        public string Content { get; set; }

        public string Title { get; set; }
        public int? LayoutPageId { get; set; }
        public bool IsLayout { get; set; }
    }

    public class WikiPageModel
    {
        public MainModelContainer Database { get; set; }
        public WikiPage Page { get; set; }
    }

    public class WikiController : ControllerBase
    {
        [Route("/wiki/page/{pageId:int}/")]
        public ActionResult GetPage(int pageId)
        {
            var page = _db.WikiPages.Find(pageId);
            var model = new WikiPageModel
            {
                Page = page,
                Database = ServiceLocator.Instance.CreateReadOnlyDatabase(),
            };

            return View(pageId.ToString(), model);
        }

        public ActionResult Index()
        {
            return View();
        }

        [Route("/Wiki/Delete/{wikiPageId}")]
        public ActionResult Delete(int wikiPageId)
        {
            var wikiPage = _db.WikiPages.Find(wikiPageId);
            _db.WikiPages.Remove(wikiPage);
            _db.SaveChanges();
            return RedirectToAction("Pages");
        }

        public class FormContainer<T>
        {
            public T Form { get; set; }
        }

        public class PagesViewModel
        {
            public List<WikiPage> Pages { get; set; }
        }
        public ActionResult Pages()
        {
            var model = new PagesViewModel()
            {
                Pages = _db.WikiPages.ToList(),
            };
            return View(model);
        }

        public class EditPageModel
        {
            public EditPageFormModel Form { get; set; }
            public WikiPage WikiPage { get; set; }
            public List<SelectListItem> LayoutPagesSelectList { get; set; }
        }
        public ActionResult EditPage(int? pageId)
        {
            var wikiPage = _db.WikiPages.Find(pageId) ?? new WikiPage();
            var selectList = _db.WikiPages.Where(x => x.IsLayout).Select(x => new SelectListItem()
            {
                Text = x.PageTitle,
                Value = x.Id.ToString(),
                Selected = x.Id == wikiPage.LayoutPageId,
            }).ToList();

            selectList.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "-",
            });
            var model = new EditPageModel()
            {
                WikiPage = wikiPage,
                LayoutPagesSelectList = selectList,
                Form = new EditPageFormModel()
                {
                    Content = ParseContentForEditor(wikiPage.Content ?? ""),
                    Id = wikiPage.Id,
                    Title = wikiPage.PageTitle,
                    IsLayout = wikiPage.IsLayout,
                }
            };

            return View(model);
        }


        public class CommentFormModel
        {
            public int? Id { get; set; }
            public int WikiPageId { get; set; }

            [Required]
            [StringLength(10000, MinimumLength = 1)]
            public string Text { get; set; }
        }
        public class CommentsModel
        {
            public CommentFormModel Form { get; set; }
            public IList<WikiComment> Comments { get; set; }
        }
        public ActionResult Comments(int wikiPageId, int page = 1)
        {
            var model = new CommentsModel();
            var perPage = 30;
            var comments = _db.WikiComments
                .Where(x => x.WikiPage.Id == wikiPageId)
                .OrderByDescending(x => x.CreatedDate)
                .Skip(perPage * (page - 1))
                .Take(perPage)
                .ToList();
            model.Comments = comments;
            model.Form = new CommentFormModel()
            {
                WikiPageId = wikiPageId,
            };
            if (TempData.ContainsKey("ModelState"))
                ModelState.Merge((ModelStateDictionary)TempData["ModelState"]);
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult CreateComment(CommentFormModel Form)
        {
            if (ModelState.IsValid)
            {
                var comment = (Form.Id != null ? _db.WikiComments.Find(Form.Id.Value) : null) ?? new WikiComment()
                {
                    CreatedDate = DateTimeOffset.Now,
                    WikiPage = _db.WikiPages.Find(Form.WikiPageId),
                };

                if (comment.WikiPage != null)
                {
                    comment.Text = Form.Text;
                    _db.WikiComments.AddOrUpdate(comment);
                    _db.SaveChanges();
                    return RedirectToAction("GetPage", new { pageId = Form.WikiPageId });
                }
            }
            this.TempData["ModelState"] = ModelState;
            return GetPage(Form.WikiPageId);
        }

        public string ParseContentForEditor(string text)
        {
            return text.Replace("@*<!---*@", "<!---")
                .Replace("@*--->*@", "--->");

            return text.Replace("@*<div class='razorCode'>@", "<div class='razorCode'>")
                .Replace("@*</div class='razorCode'>*@", "</div class='razorCode'>");
        }
        public string ParseContentForRazor(string text)
        {
            return text.Replace("<!---", "@*<!---*@")
                .Replace("--->", "@*--->*@");

            return text.Replace("<div class='razorCode'>", "@*<div class='razorCode'>@")
                .Replace("</div class='razorCode'>", "@*</div class='razorCode'>*@");
        }
        [HttpPost]
        public ActionResult EditPage(EditPageFormModel form)
        {
            if (ModelState.IsValid)
            {
                var wikiPage = _db.WikiPages.Find(form.Id) ?? _db.WikiPages.Add(new WikiPage());
                wikiPage.Content = ParseContentForRazor(form.Content);
                wikiPage.PageTitle = form.Title;
                wikiPage.IsLayout = form.IsLayout;
                wikiPage.LayoutPageId = form.LayoutPageId;
                _db.SaveChanges();

                SavePage(wikiPage);

                if (wikiPage.IsLayout)
                {
                    return RedirectToAction("EditPage", new { pageId = wikiPage.Id });
                }

                return RedirectToAction("GetPage", new { pageId = wikiPage.Id });
            }
            return EditPage(form.Id);
        }

        private void SavePage(WikiPage wikiPage)
        {
            var path = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "Views",
                wikiPage.ViewPath ?? $"Wiki/{wikiPage.Id}.cshtml");

            var content = wikiPage.Content;
            if (wikiPage.LayoutPageId != null)
            {
                content = $@"@*<!---*@ @{{
Layout = ""~/Views/Wiki/{wikiPage.LayoutPageId}.cshtml"";
}} @*--->*@
{content}
";
            }
            System.IO.File.WriteAllText(path, content);
        }

        public ActionResult SavePagesFromDb()
        {
            return Content("");
        }
    }
}
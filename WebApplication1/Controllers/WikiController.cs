using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        [Display(Name = "Layout page")]
        public int? LayoutPageId { get; set; }
        public bool IsLayout { get; set; }
        public string Tags { get; set; }
        public bool StickGlobal { get; set; }
        public bool StickCategory { get; set; }
        public bool IsSystemPage { get; set; }
    }

    public class WikiPageModel
    {
        public MainModelContainer Database { get; set; }
        public WikiPage Page { get; set; }
        public bool CanEdit { get; set; }
    }

    public class WikiController : ControllerBase
    {
        [Route("wiki/page/{pageId:int}/")]
        public ActionResult GetPage(int pageId)
        {
            var page = _db.WikiPages.Find(pageId);
            var model = new WikiPageModel
            {
                Page = page,
                CanEdit = CanEditPage(page),
                Database = ServiceLocator.Instance.CreateReadOnlyDatabase(),
            };

            return View(pageId.ToString(), model);
        }

        public ActionResult Index()
        {
            return View();
        }

        [Route("wiki/Delete/{wikiPageId}")]
        public ActionResult Delete(int wikiPageId)
        {
            var wikiPage = _db.WikiPages.Find(wikiPageId);
            if (wikiPage == null)
                return HttpNotFound();

            if (!CanEditPage(wikiPage))
                return new HttpUnauthorizedResult();

            _db.WikiPages.Remove(wikiPage);
            _db.SaveChanges();
            return RedirectToAction("AdminPages");
        }

        public class FormContainer<T>
        {
            public T Form { get; set; }
        }

        public class PagesViewModel
        {
            public List<WikiPageViewModel> Pages { get; set; }
        }
        public ActionResult AdminPages()
        {
            var pages = _db.WikiPages.Include(x => x.Author);
            if (!IsWikiAdmin)
                pages = pages.Where(x => !x.IsSystemPage && x.Author.Login == CurrentUser.Login);

            var model = new PagesViewModel()
            {
                Pages = GetPages(pages),
            };
            return View(model);
        }

        private List<WikiPageViewModel> GetPages(IQueryable<WikiPage> dbWikiPages, bool global = true, int page = 1)
        {
            var query = dbWikiPages
                .Include(x => x.Tags)
                ;

            if (global)
            {
                query = query.OrderBy(x => x.StickGlobal).ThenByDescending(x => x.CreatedDate);
            }
            else
            {
                query = query.OrderBy(x => x.StickCategory).ThenByDescending(x => x.CreatedDate);
            }
            var perPage = 10;
            var pages = query
                .Take(perPage)
                .Skip((page - 1) * perPage)
                .Select(x => new
                {
                    Page = x,
                    Comments = x.Comments.Count,
                }).ToList();

            var result = pages.Select(x => new WikiPageViewModel()
            {
                Tags = x.Page.Tags,
                IsSticky = global ? x.Page.StickGlobal : x.Page.StickCategory,
                CommentsCount = x.Comments,
                Page = x.Page,
            })
            .ToList();

            return result;
        }

        public class WikiPageViewModel
        {
            public IList<WikiTag> Tags { get; set; }
            public int CommentsCount { get; set; }
            public bool IsSticky { get; set; }
            public WikiPage Page { get; set; }
        }

        public ActionResult Pages(string tag)
        {
            var pages = _db.WikiPages
                .Include(x => x.Tags)
                ;
            if (!string.IsNullOrEmpty(tag))
            {
                tag = tag.ToLower();
                pages = pages.Where(x => x.Tags.Any(z => z.TagForLink == tag));
            }

            var model = new PagesViewModel()
            {
                Pages = GetPages(pages, global: string.IsNullOrEmpty(tag?.Trim())),
            };
            return View(model);
        }


        public class EditPageModel
        {
            public EditPageFormModel Form { get; set; }
            public WikiPage WikiPage { get; set; }
            public List<SelectListItem> LayoutPagesSelectList { get; set; }
            public bool IsAdmin { get; set; }
        }

        public bool IsWikiAdmin => true;
        public User CurrentUser => new User()
        {
            Login = "tst",
        };

        private bool CanEditPage(WikiPage wikiPage)
        {
            return IsWikiAdmin || wikiPage.Author.Login == CurrentUser.Login;
        }

        public ActionResult EditPage(int? pageId)
        {
            var wikiPage = _db.WikiPages.Find(pageId) ?? new WikiPage();
            if (pageId.HasValue && !CanEditPage(wikiPage))
                return this.HttpNotFound();

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
                IsAdmin = IsWikiAdmin,
                Form = new EditPageFormModel()
                {
                    Content = ParseContentForEditor(wikiPage.Content ?? ""),
                    Id = wikiPage.Id,
                    Title = wikiPage.PageTitle,
                    IsLayout = wikiPage.IsLayout,
                    StickCategory = wikiPage.StickCategory,
                    StickGlobal = wikiPage.StickGlobal,
                    IsSystemPage = wikiPage.IsSystemPage,
                    Tags = string.Join(", ", wikiPage.Tags.Select(x => x.TagForLink)),
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
                    comment.Author = CurrentUser;
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
            var result = text;
            result = Regex.Replace(result, "(@model.*)$", "<razor>$1</razor>", RegexOptions.Multiline);
            return result
                .Replace("@*<razor>*@", "<razor>")
                .Replace("@*</razor>*@", "</razor>");
        }

        public string ParseContentForRazor(string text)
        {
            var result = text;
            result = Regex.Replace(result, "<razor>(@model.*?)</razor>", "$1");
            var matches = Regex.Matches(text, "<razor>(.*?)</razor>", RegexOptions.Singleline);
            foreach (Match match in matches)
            {
                result = result.Replace(match.Value, "@*<razor>*@" +
                    match.Groups[1].Value
                        .Replace("&lt;", "<")
                        .Replace("&gt;", ">")
                    + "@*</razor>*@");
            }
            return result;
        }

        //public string ParseContentForRazor(string text)
        //{
        //    //Regex.Replace(text, "<razor>(.*?)</razor>", )
        //    return text.Replace("<!---", "@*<!---*@")
        //        .Replace("--->", "@*--->*@");

        //    return text.Replace("<div class='razorCode'>", "@*<div class='razorCode'>@")
        //        .Replace("</div class='razorCode'>", "@*</div class='razorCode'>*@");
        //}
        [HttpPost]
        public ActionResult EditPage(EditPageFormModel form)
        {
            if (ModelState.IsValid)
            {
                var wikiPage = _db.WikiPages.Find(form.Id) ?? _db.WikiPages.Add(new WikiPage()
                {
                    Author = CurrentUser,
                });

                if (wikiPage.Id != 0 && !CanEditPage(wikiPage))
                    return EditPage(wikiPage.Id);

                var history = new WikiPageHistory()
                {
                    ChangedDate = DateTimeOffset.Now,
                    Content = wikiPage.Content,
                    PageTitle = wikiPage.PageTitle,
                    WikiPage = wikiPage,
                };
                _db.WikiPageHistory.Add(history);

                wikiPage.Content = ParseContentForRazor(form.Content ?? "");
                wikiPage.PageTitle = form.Title;

                if (IsWikiAdmin)
                {
                    wikiPage.IsLayout = form.IsLayout;
                    wikiPage.LayoutPageId = form.LayoutPageId;
                    wikiPage.IsSystemPage = form.IsLayout || form.IsSystemPage;
                    wikiPage.StickGlobal = form.StickGlobal;
                    wikiPage.StickCategory = form.StickCategory;
                }
                else
                {
                    if (wikiPage.LayoutPageId == null)
                        wikiPage.LayoutPage = _db.WikiPages.FirstOrDefault(x => x.ViewPath == "Wiki/Blog_Post_Layout.cshtml");
                }

                var dbTags = _db.WikiTags.ToList();
                var tags = (form.Tags ?? "").Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim().ToLower())
                    .Where(x => !string.IsNullOrEmpty(x));

                wikiPage.Tags.Clear();
                foreach (var tag in tags)
                {
                    var dbTag = dbTags.FirstOrDefault(x => x.TagForLink == tag);
                    if (dbTag == null)
                    {
                        dbTag = new WikiTag()
                        {
                            FullTag = tag,
                            IsSystemTag = false,
                            TagForLink = tag,
                        };
                        _db.WikiTags.Add(dbTag);
                    }
                    wikiPage.Tags.Add(dbTag);
                }


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
            System.IO.File.WriteAllText(path, content, Encoding.UTF8);
        }

        public ActionResult SavePagesFromDb()
        {
            foreach (var dbWikiPage in _db.WikiPages.ToList())
            {
                SavePage(dbWikiPage);
            }

            return Content("");
        }
    }
}
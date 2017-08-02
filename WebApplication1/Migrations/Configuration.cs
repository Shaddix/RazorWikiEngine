using RuPM.Models.Database;

namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<RuPM.Models.Database.MainModelContainer>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "RuPM.Models.Database.MainModelContainer";
        }

        protected override void Seed(RuPM.Models.Database.MainModelContainer context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            SeedWiki(context);
        }

        private void SeedWiki(MainModelContainer context)
        {
            if (!context.WikiPages.Any(x => x.ViewPath == "Wiki/Index.cshtml"))
                context.WikiPages.Add(new WikiPage()
                {
                    IsSystemPage = true,
                    ViewPath = "Wiki/Index.cshtml",
                    PageTitle = "Wiki Index",
                    Content = @"@*<razor>*@ @{Html.RenderAction(""Pages"");}@*</razor>*@",
                });


            if (!context.WikiPages.Any(x => x.ViewPath == "Wiki/Comments.cshtml"))
                context.WikiPages.AddOrUpdate(x => x.ViewPath, new WikiPage()
                {
                    ViewPath = "Wiki/Comments.cshtml",
                    PageTitle = "Wiki Comments",
                    IsSystemPage = true,
                    Content = @"@model RuPM.Controllers.WikiController.CommentsModel
<div class=""comments"">@*<razor>*@@foreach (var comment in Model.Comments) {
<article class=""comment byuser comment-author-admin bypostauthor even thread-even depth-1"" id=""li-comment-@comment.Id""><header>
<h4>@comment.Author.Login</h4>
<time>@comment.CreatedDate.ToString(""dd/MM/yyyy hh:mm"")</time></header>
<section>
<p>@comment.Text</p>
</section>
<footer><a rel=""nofollow"" class=""comment-reply-link"" href=""https://edu.rubius.com/a-ty-znaesh-chto-takoe-scada/?replytocom=8#respond"" onclick=""return addComment.moveForm( &quot;comment-8&quot;, &quot;8&quot;, &quot;respond&quot;, &quot;232&quot; )"" aria-label=""Reply to admin"">Reply</a> <a class=""comment-edit-link"" href=""https://edu.rubius.com/wp-admin/comment.php?action=editcomment&amp;c=8"">Edited</a></footer></article>
}@*</razor>*@</div>
<div id=""respond"" class=""comment-respond"">@*<razor>*@
<h3 id=""reply-title"" class=""comment-reply-title"">Leave a Reply</h3>
@using (Html.BeginForm(""CreateComment"", ""Wiki"", FormMethod.Post, new {@class=""comment-form""})) {
<p class=""comment-form-comment"">@Html.ValidationSummary() @Html.HiddenFor(x => x.Form.WikiPageId) @Html.TextAreaFor(x => x.Form.Text, new {rows=8})</p>
<p class=""form-submit""><input name=""submit"" type=""submit"" id=""submit"" class=""submit"" value=""Post Comment"" /></p>
}@*</razor>*@</div>
<!-- #respond -->
<p></p>"
                });

            if (!context.WikiPages.Any(x => x.ViewPath == "Wiki/LeftMenu.cshtml"))
                context.WikiPages.AddOrUpdate(x => x.ViewPath, new WikiPage()
                {
                    IsSystemPage = true,
                    ViewPath = "Wiki/LeftMenu.cshtml",
                    PageTitle = "Left Menu",
                    Content = "<aside class=\"l-sidebar col-md-2 l-sidebar--totop\">\r\n<div class=\"l-sidebar__fixed\"><a class=\"btn\" href=\"/wiki/editPage\"><span style=\"font-size: 18px; display: inline-block; margin-right: 5px;\">+</span> Add new post</a>\r\n<ul class=\"b-category\">\r\n<ul class=\"b-category\">\r\n<li class=\"cat-item cat-item-10\"><a href=\"https://edu.rubius.com/category/1/\">Education</a>\r\n<ul class=\"children\">\r\n<li class=\"cat-item cat-item-11\"><a href=\"https://edu.rubius.com/category/1/developer/\">Developer</a>\r\n<ul class=\"children\">\r\n<li class=\"cat-item cat-item-25\"><a href=\"https://edu.rubius.com/category/1/developer/c_plusplus/\">C++</a></li>\r\n<li class=\"cat-item cat-item-26\"><a href=\"https://edu.rubius.com/category/1/developer/js/\">JS</a></li>\r\n<li class=\"cat-item cat-item-24\"><a href=\"https://edu.rubius.com/category/1/developer/net/\">.Net</a></li>\r\n<li class=\"cat-item cat-item-27\"><a href=\"https://edu.rubius.com/category/1/developer/sql/\">SQL</a></li>\r\n</ul>\r\n</li>\r\n<li class=\"cat-item cat-item-14\"><a href=\"https://edu.rubius.com/category/1/devops/\">DevOps</a></li>\r\n<li class=\"cat-item cat-item-5\"><a href=\"https://edu.rubius.com/category/1/qualityassurance/\">QAs</a></li>\r\n</ul>\r\n</li>\r\n<li class=\"cat-item cat-item-2\"><a href=\"https://edu.rubius.com/category/2/\">Videos</a></li>\r\n<li class=\"cat-item cat-item-19\"><a href=\"https://edu.rubius.com/category/4/\">How To</a></li>\r\n<li class=\"cat-item cat-item-19\"><a href=\"https://pm.rubius.com/Account/GetRubiusGuide/\" target=\"_blank\" rel=\"noopener\">Rubius Guide</a></li>\r\n<li class=\"cat-item cat-item-19\"><a href=\"https://docs.google.com/spreadsheets/d/1-UgUY2Tyshs6WvU1VqopaJ-NToUvIfsErA1NDkRkcZU/edit?usp=sharing\" target=\"_blank\" rel=\"noopener\">Books</a></li>\r\n</ul>\r\n</ul>\r\n<div class=\"l-sidebar__footer\">\r\n<div class=\"b-copy\">&copy; Rubius</div>\r\n<ul class=\"b-social\">\r\n<li class=\"b-social--vk\"><a href=\"https://vk.com/rubiuscompany\" target=\"_blank\" rel=\"noopener\"><i class=\"fa fa-vk\" aria-hidden=\"true\"></i></a></li>\r\n<li class=\"b-social--fb\"><a href=\"https://www.facebook.com/RubiusCompany\" target=\"_blank\" rel=\"noopener\"><i class=\"fa fa-facebook\" aria-hidden=\"true\"></i></a></li>\r\n<li class=\"b-social--yt\"><a href=\"https://www.youtube.com/c/RubiusCompany\" target=\"_blank\" rel=\"noopener\"><i class=\"fa fa-youtube-play\" aria-hidden=\"true\"></i></a></li>\r\n</ul>\r\n</div>\r\n</div>\r\n</aside>"
                });

            if (!context.WikiPages.Any(x => x.ViewPath == "Wiki/General_Layout.cshtml"))
                context.WikiPages.AddOrUpdate(x => x.ViewPath, new WikiPage()
                {
                    IsSystemPage = true,
                    ViewPath = "Wiki/General_Layout.cshtml",
                    PageTitle = "General Layout",
                    Content = "@*<razor>*@@{ Layout = \"~/Views/Shared/_Layout.cshtml\"; }@*</razor>*@@*<razor>*@@Html.Partial(\"ScriptsAndStylesAdmin\")@*</razor>*@\r\n<div class=\"container\">\r\n<div class=\"row\">@*<razor>*@@Html.Partial(\"LeftMenu\") @* Left Menu *@@*</razor>*@\r\n<section class=\"l-content col-md-10\">@*<razor>*@@RenderBody()@*</razor>*@</section>\r\n</div>\r\n</div>"
                });
            context.SaveChanges();

            if (!context.WikiPages.Any(x => x.ViewPath == "Wiki/Pages.cshtml"))
                context.WikiPages.AddOrUpdate(x => x.ViewPath, new WikiPage()
                {
                    ViewPath = "Wiki/Pages.cshtml",
                    PageTitle = "Wiki Pages List",
                    ViewUrl = "/Wiki/Pages",
                    IsSystemPage = true,
                    LayoutPage = context.WikiPages.FirstOrDefault(x => x.ViewPath == "Wiki/General_Layout.cshtml"),
                    Content = @"@*<razor>*@@{ var actionName = ""GetPage""; }@*</razor>*@@*<razor>*@
<div class=""b-last-activity"">
<ul class=""b-last-activity__list"">@foreach (var wikiPage in Model.Pages) {
<li class=""post type-post status-publish format-standard sticky hentry"">
<div class=""b-last-activity__left""><a href=""@Url.Action(actionName, new{pageId=wikiPage.Page.Id})""> @wikiPage.Page.PageTitle </a>
<div class=""b-last-activity__tags"">@foreach (var tag in wikiPage.Tags) { <a href=""tag/@tag.TagForLink/"" rel=""tag"">@tag.TagForLink</a> }</div>
</div>
<div class=""right""><span class=""b-last-activity__subinfo"">@wikiPage.Page.CreatedDate.ToString(""dd/MM/yyyy"")</span> <span class=""b-last-activity__subinfo""><i class=""fa fa-comment-o"" aria-hidden=""true""></i> @wikiPage.CommentsCount</span></div>
</li>
}</ul>
</div>
@*</razor>*@",
                });

            if (!context.WikiPages.Any(x => x.ViewPath == "Wiki/Blog_Post_Layout.cshtml"))
                context.WikiPages.AddOrUpdate(x => x.ViewPath, new WikiPage()
                {
                    IsLayout = true,
                    IsSystemPage = true,
                    LayoutPage = context.WikiPages.FirstOrDefault(x => x.ViewPath == "Wiki/General_Layout.cshtml"),
                    ViewPath = "Wiki/Blog_Post_Layout.cshtml",
                    PageTitle = "Blog Post Layout",
                    Content = @"<div class=""l-content__title"">@*<razor>*@@Model.Page.PageTitle<a class=""post-edit-link"" href=""EditPage?pageId=@Model.Page.Id"">Edit This</a>@*</razor>*@</div>
<article class=""l-content__text"">@*<razor>*@@RenderBody()@*</razor>*@<footer class=""l-content__footer"">
<div class=""l-content__autor""><i class=""fa fa-user-o"" aria-hidden=""true""></i> Автор:@*<razor>*@admin@*</razor>*@</div>
<div class=""l-content__updated""><i class=""fa fa-calendar-o"" aria-hidden=""true""></i> Изменён&nbsp;@*<razor>*@@Model.Page.ChangedDate.ToString(""dd/MM/yyyy"")@*</razor>*@</div>
</footer>@*<razor>*@@{ Html.RenderAction(""Comments"", ""Wiki"", new {wikiPageId = Model.Page.Id}); }@*</razor>*@
<p>&nbsp;</p>
</article>",
                });


            if (!context.WikiPages.Any(x => x.ViewPath == "Wiki/ScriptsAndStylesAdmin.cshtml"))
                context.WikiPages.AddOrUpdate(x => x.ViewPath, new WikiPage()
                {
                    IsLayout = true,
                    IsSystemPage = true,
                    ViewPath = "Wiki/ScriptsAndStylesAdmin.cshtml",
                    Content = @"<link rel=""stylesheet"" href=""/Content/Wiki/style.css"">
                    <link rel=""stylesheet"" href=""/Content/Wiki/font-awesome.min.css"">
                    <link rel=""stylesheet"" href=""/Content/Wiki/wp-syntax.css"">",
                    PageTitle = "Scripts And Styles Admin",
                });


            context.SaveChanges();
        }

    }
}

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
            if (!context.WikiPages.Any(x => x.ViewPath == "Wiki/Index.cshtml"))
                context.WikiPages.Add(new WikiPage()
                {
                    ViewPath = "Wiki/Index.cshtml",
                    PageTitle = "Wiki Index",
                });


            if (!context.WikiPages.Any(x => x.ViewPath == "Wiki/Comments.cshtml"))
                context.WikiPages.AddOrUpdate(x => x.ViewPath, new WikiPage()
                {
                    ViewPath = "Wiki/Comments.cshtml",
                    PageTitle = "Wiki Comments",
                });


            if (!context.WikiPages.Any(x => x.ViewPath == "Wiki/Pages.cshtml"))
                context.WikiPages.AddOrUpdate(x => x.ViewPath, new WikiPage()
                {
                    ViewPath = "Wiki/Pages.cshtml",
                    PageTitle = "Wiki Pages List",
                    ViewUrl = "/Wiki/Pages"
                });
        }
    }
}

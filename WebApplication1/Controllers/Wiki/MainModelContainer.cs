using System.Data.Entity;
using WebApplication1.Migrations;

namespace RuPM.Models.Database
{
    public class MainModelContainer : DbContext
    {

        public DbSet<WikiPage> WikiPages { get; set; }
        public DbSet<WikiTag> WikiTags { get; set; }
        public DbSet<WikiComment> WikiComments { get; set; }
        public DbSet<WikiPageHistory> WikiPageHistory { get; set; }
        public DbSet<User> Users { get; set; }

        static MainModelContainer()
        {
            System.Data.Entity.Database.SetInitializer<MainModelContainer>(new MigrateDatabaseToLatestVersion<MainModelContainer, Configuration>());
        }

        public MainModelContainer() : base("Data Source=(LocalDB)\\mssqllocaldb;Initial Catalog=Wiki;MultipleActiveResultSets=True;App=EntityFramework")
        {

        }
    }
}
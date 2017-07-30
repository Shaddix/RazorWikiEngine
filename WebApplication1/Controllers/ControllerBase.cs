using System.Web.Mvc;
using RuPM.Models.Database;

namespace RuPM.Controllers
{
    public class ServiceLocator
    {
        public static ServiceLocator Instance { get; set; } = new ServiceLocator();

        public MainModelContainer CreateReadOnlyDatabase()
        {
            return new MainModelContainer();
        }
    }

    public class ControllerBase : Controller
    {
        protected MainModelContainer _db;

        public ControllerBase()
        {
            _db = new MainModelContainer();
        }
    }
}
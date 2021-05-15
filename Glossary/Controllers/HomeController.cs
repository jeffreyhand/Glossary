using Glossary.Models;
using System.Linq;
using System.Web.Mvc;

namespace Glossary.Controllers
{
    public class HomeController : Controller
    {
        private GlossaryContext _context;

        /// <summary>
        /// Release memory resources used by the database context.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        /// <summary>
        /// Create an instance of the Home Controller.
        /// </summary>
        public HomeController()
        {
            _context = new GlossaryContext();
        }

        /// <summary>
        /// GET: /Index/ displays the main dashboard. Displays current number of terms in the Glossary.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.GlossaryCount = _context.Entries.Count();
            return View();
        }

    }
}
using System.Web.Mvc;

namespace Glossary.Controllers
{
    public class ErrorController : Controller
    {

        /// <summary>
        /// GET: /Error/ displays 500 error message
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// GET: /Error/NotFound/ displays 404 error message
        /// </summary>
        public ActionResult NotFound()
        {
            return View();
        }
    }
}
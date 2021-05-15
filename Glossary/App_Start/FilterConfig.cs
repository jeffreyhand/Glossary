using System.Web;
using System.Web.Mvc;

namespace Glossary
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
             filters.Add(new HandleErrorAttribute());
        }
    }
}

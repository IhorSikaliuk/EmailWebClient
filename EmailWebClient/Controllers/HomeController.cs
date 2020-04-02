using EmailWebClient.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace EmailWebClient.Controllers
{
    public class HomeController : Controller
    {
        private DataBaseContext DBContext = new DataBaseContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
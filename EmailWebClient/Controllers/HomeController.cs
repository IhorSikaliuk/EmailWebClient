using EmailWebClient.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using MailKit;
using MimeKit;
using System;

namespace EmailWebClient.Controllers
{
    public class HomeController : Controller
    {
        private DataBaseContext DBContext = new DataBaseContext();
        private ServerConfig GetServer(int Id) {
            foreach (var temp in DBContext.ServerConfig)
                if (temp.Id == Id)
                    return temp;

            throw new IndexOutOfRangeException();
        }
        public ActionResult Index() {
            ViewBag.Servers = DBContext.ServerConfig;
            
            return View();
        }

        [HttpPost]
        public ActionResult Index(Authentication login) {
            try {
                login.Server = GetServer(login.Server.Id);
            }
            catch (Exception) {
                ViewData["Login error"] = "Неизвесная ошибка";
                return Index();
            }
            ViewBag.Login = login;
            Session["login"] = login;

            return View("test");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            Session.Abandon();
            
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            
            return View();
        }
    }
}
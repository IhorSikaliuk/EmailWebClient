using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EmailWebClient.Models;
using MailKit;
using MimeKit;
using MailKit.Net.Imap;

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
        private MimeMessage[] GetMessages(IMailFolder folder) {


            return new MimeMessage[0];
        }
        public ActionResult Index() {
            ViewBag.Servers = DBContext.ServerConfig;
            if (Request.Cookies["serverId"] != null)
                ViewBag.ServerId = Request.Cookies["serverId"].Value;


            return View();
        }
        [HttpPost]
        public ActionResult Index(Authentication login) {
            Response.Cookies["serverId"].Value = login.Server.Id.ToString();
            try {
                login.Server = GetServer(login.Server.Id);
            }
            catch (Exception) {
                ViewData["Login error"] = "Неизвесная ошибка";
                return Index();
            }
            
            ImapClient client = new ImapClient();
            try {
                client.Connect(login.Server.Ip, login.Server.Port, useSsl: login.Server.Ssl);
                client.Authenticate(login.Email, login.Password);
            }
            catch(Exception exception) {
                ViewData["Login error"] = exception.ToString();
            }
            if (!client.IsAuthenticated) {
                ViewData["Login error"] = "Неверные ел. почта и/или пароль";
                return Index();
            }
            Session["login"] = login;
            Session["connection"] = client;
            IMailFolder inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadWrite);
            Session["inbox"] = inbox;


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
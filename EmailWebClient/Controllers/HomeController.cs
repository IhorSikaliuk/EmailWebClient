using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EmailWebClient.Models;
using MailKit;
using MimeKit;
using MailKit.Net.Imap;
using System.Threading;

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

        private void MailUpdate() {
            MailFolder folder;
            try {
                while (Session.Count > 0)
                {
                    folder = (MailFolder)Session["folder"];
                    Session["messages"] = GetMessages(folder);
                    System.Diagnostics.Debug.WriteLine("DONE");
                }

            }
            catch (System.NullReferenceException) {
                System.Diagnostics.Debug.WriteLine("return");
                return;
            }
            catch (Exception e) {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                Session.Abandon();
            }
        }
        private List<MimeMessage> GetMessages(IMailFolder folder) {
            int count = folder.Count;
            List<MimeMessage> messages = new List<MimeMessage>();
            for (int i = count - 1; i >= count - 11; i--)
                messages.Add(folder.GetMessage(i));

            return messages;
        }

        [HttpGet]
        public ActionResult Index() {
            ViewBag.Servers = DBContext.ServerConfig;
            if (Request.Cookies["serverId"] != null)
                ViewBag.ServerId = Request.Cookies["serverId"].Value;
                        
            return View("Index");
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
            inbox.Open(FolderAccess.ReadOnly);
            
            Session["folder"] = inbox;
            Session["messages"] = new List<MimeMessage>();
            
            return Index();
        }

        public ActionResult Mail(int id) {
            //List<MimeMessage> messages = new List<MimeMessage>();
            //if (Session["updated"] != true)
            //{
            /*    ImapClient client = (ImapClient)Session["client"];
                IMailFolder inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);
                messages = GetMessages((IMailFolder)Session["inbox"]);
                Session["updated"] = true;*/
            //}

            return View("Index");
        }

        public ActionResult MailList(int page = 0) {
            IMailFolder inbox = (IMailFolder)Session["folder"];
            List<MimeMessage> messages = GetMessages(inbox);
            List<string> subjects = new List<string>();
            List<DateTimeOffset> dates = new List<DateTimeOffset>();
            List<MailboxAddress> senders = new List<MailboxAddress>();
            foreach (var message in messages)
            {
                subjects.Add(message.Subject);
                dates.Add(message.Date);
                senders.Add(message.Sender);
            }
            ViewBag.Subjects = subjects;
            ViewBag.Dates = dates;
            ViewBag.Senders = senders;
            ViewBag.Count = messages.Count;

            return PartialView();
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

        public ActionResult Exit() {
            Session.Clear();
            Session.Abandon();

            return Index();
        }
    }
}
﻿using System;
using System.Web.Mvc;
using EmailWebClient.Models;
using MailKit;
using MimeKit;
using MailKit.Net.Imap;
using System.IO;

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
            }
            catch(Exception) {
                ViewData["Login error"] = $"Почтовый сервер {login.Server.Name} не доступен. Попробуйте позже.";
                return Index();
            }
            if (!client.IsConnected) {
                ViewData["Login error"] = $"Почтовый сервер {login.Server.Name} не доступен. Попробуйте позже.";
                return Index();
            }

            try {
                client.Authenticate(login.Email, login.Password);
            }
            catch (Exception) {
                ViewData["Login error"] = "Неверные ел. почта и/или пароль.";
                return Index();
            }
            if (!client.IsAuthenticated) {
                ViewData["Login error"] = "Неверные ел. почта и/или пароль.";
                return Index();
            }

            Session["login"] = login;
            Session["connection"] = client;
            IMailFolder inbox = client.Inbox;
            Session["folder"] = inbox;
            Session["messages"] = null;
            
            return Index();
        }
        public ActionResult OpenMail(uint Uid, bool seen = true) {
            IMailFolder folder = (IMailFolder)Session["folder"];
            UniqueId uid = new UniqueId(Uid);
            MimeMessage message = folder.GetMessageAsync(uid).Result;
            if (!seen) folder.AddFlagsAsync(new UniqueId(Uid), MessageFlags.Seen, true);
            Mail mail = new Mail(uid, message);

            return PartialView(mail);
        }

        public void DeleteMail(uint Uid){
            IMailFolder folder = (IMailFolder)Session["folder"];
            UniqueId uid = new UniqueId(Uid);
            folder.AddFlags(uid, MessageFlags.Deleted, true);

            Response.StatusCode = 303;
            Response.RedirectLocation = "/";
        }

        public FileResult Download(uint Uid, string Name) { //retuns file from message attachments
            IMailFolder folder = (IMailFolder)Session["folder"];
            UniqueId uid = new UniqueId(Uid);
            var task = folder.GetMessageAsync(uid);
            var attachments = task.Result.Attachments;
            
            foreach (MimeEntity attachment in attachments) {
                if (attachment.ContentDisposition.FileName == Name) {
                    MemoryStream stream = new MemoryStream();
                    string fileType = attachment.ContentType.MimeType;
                    if (attachment is MessagePart)
                    {
                        var part = (MessagePart)attachment;
                        part.Message.WriteTo(stream);
                    }
                    else
                    {
                        var part = (MimePart)attachment;
                        part.Content.DecodeTo(stream);
                    }


                    byte[] file = stream.ToArray();
                    return File(file, fileType, Name);
                }
            }

            return null;
        }

        public void Exit() {
            Session.Clear();
            Session.Abandon();
            Response.StatusCode = 303;
            Response.RedirectLocation = "/";
        }
    }
}
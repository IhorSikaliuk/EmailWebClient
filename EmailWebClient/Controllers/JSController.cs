using MailKit;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace EmailWebClient.Controllers
{
    public class JSController : Controller
    {
        private int mailsOnPage = 15; //количество отправляемых писем (на странице) за один AJAX запрос "MailList"
        private List<MimeMessage> GetMessages(IMailFolder folder, int page = 0)
        {
            int skip = page * mailsOnPage;
            int start = folder.Count - (page * mailsOnPage);
            int end = start > mailsOnPage ? start - mailsOnPage : 0;
            List<MimeMessage> messages = new List<MimeMessage>();
            for (int i = start - 1; i >= end; i--)
                messages.Add(folder.GetMessage(i));

            return messages;
        }
        public ActionResult MailList(int page = 0)
        {
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
        public int GetMaxPages()
        {
            IMailFolder inbox = (IMailFolder)Session["folder"];
            int maxPages = Convert.ToInt32( Math.Ceiling((float)inbox.Count / mailsOnPage) );

            return maxPages;
        }
    }
}
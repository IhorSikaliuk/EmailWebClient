using EmailWebClient.Models;
using MailKit;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Net;
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
            folder.Close();

            return messages;
        }
        public ActionResult MailList(int page = 0)
        {
            IMailFolder folder = (IMailFolder)Session["folder"];
            List<IMessageSummary> fetch = (List<IMessageSummary>) Session["messages"];
            if (page == 0) {
                folder.Open(FolderAccess.ReadWrite);
                fetch = new List<IMessageSummary>(folder.Fetch(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Flags));
                fetch.Reverse();
                Session["messages"] = fetch;
            }
            int start = page * mailsOnPage;
            int end = ((fetch.Count - mailsOnPage) > start) ? start + mailsOnPage - 1 : fetch.Count - 1;
            List<Mail> mails = new List<Mail>();
            for (int i = start; i <= end; i++) {
                var headers = folder.GetHeaders(fetch[i].UniqueId);
                mails.Add(new Mail(fetch[i], headers));
            }

            return PartialView(mails);
        }
        public int GetMaxPages(){
            IMailFolder folder = (IMailFolder)Session["folder"];
            folder.Open(FolderAccess.ReadOnly);
            int maxPages = Convert.ToInt32( Math.Ceiling((float)folder.Count / mailsOnPage) );

            return maxPages;
        }
        public ActionResult OpenMail(uint Uid, bool seen = false) {
            IMailFolder folder = (IMailFolder)Session["folder"];
            ViewBag.Text = new MvcHtmlString(folder.GetMessage(new UniqueId(Uid)).TextBody);
            ViewBag.Body = new MvcHtmlString(folder.GetMessage(new UniqueId(Uid)).HtmlBody);
            if (seen) folder.AddFlagsAsync(new UniqueId(Uid), MessageFlags.Seen, true);

            return PartialView();
        }
    }
}
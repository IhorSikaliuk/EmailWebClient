using EmailWebClient.Models;
using MailKit;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EmailWebClient.Controllers
{
    public class JSController : Controller
    {
        private int mailsOnPage = 15; //number of mails on page for one AJAX

        public async Task<ActionResult> MailList(int page = 0) {
            IMailFolder folder = (IMailFolder)Session["folder"];
            List<IMessageSummary> fetch = (List<IMessageSummary>) Session["messages"];
            if (page == 0) {
                folder.Open(FolderAccess.ReadWrite);
                fetch = new List<IMessageSummary>(await folder.FetchAsync(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Flags));
                fetch.Reverse();
                Session["messages"] = fetch;
            }
            int start = page * mailsOnPage;
            int end = ((fetch.Count - mailsOnPage) > start) ? start + mailsOnPage - 1 : fetch.Count - 1;
            List<Mail> mails = new List<Mail>();
            for (int i = start; i <= end; i++) {
                var headers = folder.GetHeaders(fetch[i].UniqueId);
                await Task.Run(() => mails.Add(new Mail(fetch[i], headers)));
            }

            return PartialView(mails);
        }
        public async Task<int> GetMaxPages() {
            IMailFolder folder = (IMailFolder)Session["folder"];
            if (!folder.IsOpen) await folder.OpenAsync(FolderAccess.ReadWrite);
            int maxPages = Convert.ToInt32( Math.Ceiling((float)folder.Count / mailsOnPage) );

            return maxPages;
        }

    }
}
﻿using MailKit;
using MimeKit;
using System;
using System.Web.Mvc;

namespace EmailWebClient.Models
{
    public class Mail
    {
        public UniqueId Uid { get; set; }
        public bool Seen { get; set; }
        public string Subject { get; set; }
        public string From { get; set; }
        public DateTime Date { get; set; }
        public MvcHtmlString TextBody { get; set; } = new MvcHtmlString("");
        public MvcHtmlString HtmlBody { get; set; } = new MvcHtmlString("");
        public Mail (IMessageSummary fetch, HeaderList headers) {
            Uid = fetch.UniqueId;
            Seen = fetch.Flags > 0;
            Subject = headers["subject"];
            From = headers["from"];
            int cut = headers["date"].Length;
            if (headers["date"].Contains("+")) cut = headers["date"].IndexOf('+') + 5;
            else if (headers["date"].Contains("-")) cut = headers["date"].IndexOf('-') + 5;
            Date = Convert.ToDateTime(headers["date"].Substring(0, cut));
        }
        public Mail (UniqueId Uid, MimeMessage message) {
            this.Uid = Uid;
            Seen = true;
            Subject = message.Subject;
            From = message.From.ToString();
            Date = message.Date.DateTime;
            TextBody = new MvcHtmlString(message.TextBody);
            HtmlBody = new MvcHtmlString(message.HtmlBody);
        }
    }
}
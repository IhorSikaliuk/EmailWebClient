using MailKit;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailWebClient.Models
{
    public class Mail
    {
        public UniqueId Uid { get; set; }
        public bool Seen { get; set; }
        public string Subject { get; set; }
        public string From { get; set; }
        public DateTime Date { get; set; }
        public Mail(IMessageSummary fetch, HeaderList headers) {
            Uid = fetch.UniqueId;
            Seen = fetch.Flags > 0;
            Subject = headers["subject"];
            From = headers["from"];
            Date = Convert.ToDateTime(headers["date"].Substring(0, headers["date"].IndexOf('+') + 5));
        }
    }
}
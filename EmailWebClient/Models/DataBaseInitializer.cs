using System.Data.Entity;

namespace EmailWebClient.Models
{
    public class DataBaseInitializer : DropCreateDatabaseAlways<DataBaseContext>
    {
        protected override void Seed(DataBaseContext context)
        {
            context.ServerConfig.Add(new ServerConfig { Name = "Gmail.com", Ip = "imap.gmail.com", Port = 993, Ssl = true });
            context.ServerConfig.Add(new ServerConfig { Name = "Mail.ru", Ip = "imap.mail.ru", Port = 993, Ssl = true });
            context.ServerConfig.Add(new ServerConfig { Name = "Outlook.com", Ip = "imap-mail.outlook.com", Port = 993, Ssl = true });
            context.ServerConfig.Add(new ServerConfig { Name = "Ukr.net", Ip = "imap.ukr.net", Port = 993, Ssl = true });

            base.Seed(context);
        }
    }
}
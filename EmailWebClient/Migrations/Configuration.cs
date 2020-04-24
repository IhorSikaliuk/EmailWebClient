namespace EmailWebClient.Migrations
{
    using EmailWebClient.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EmailWebClient.Models.DataBaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "EmailWebClient.Models.DataBaseContext";
        }

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

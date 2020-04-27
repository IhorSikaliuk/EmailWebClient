using System.Data.Entity;

namespace EmailWebClient.Models
{
    public class DataBaseContext : DbContext
    {
        public DbSet<ServerConfig> ServerConfig { get; set; }

        public DataBaseContext() {
            /*var task = ServerConfig.CountAsync();
            int count = task.Result;
            if (count < 1) {
                ServerConfig.Add(new ServerConfig { Name = "Gmail.com", Ip = "imap.gmail.com", Port = 993, Ssl = true });
                ServerConfig.Add(new ServerConfig { Name = "Mail.ru", Ip = "imap.mail.ru", Port = 993, Ssl = true });
                ServerConfig.Add(new ServerConfig { Name = "Outlook.com", Ip = "imap-mail.outlook.com", Port = 993, Ssl = true });
                ServerConfig.Add(new ServerConfig { Name = "Ukr.net", Ip = "imap.ukr.net", Port = 993, Ssl = true });

                SaveChanges();
            }*/
        }
    }
}
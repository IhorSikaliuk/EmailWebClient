using System.Data.Entity;

namespace EmailWebClient.Models
{
    public class DataBaseInitializer : DropCreateDatabaseAlways<DataBaseContext>
    {
        protected override void Seed(DataBaseContext context)
        {
            context.ServerConfig.Add(new ServerConfig { Name = "Ukr.net", Ip = "imap.ukr.net", Port = 993, Ssl = true });

            base.Seed(context);
        }
    }
}
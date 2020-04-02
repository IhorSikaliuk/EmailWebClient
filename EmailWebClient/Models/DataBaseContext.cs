using System.Data.Entity;

namespace EmailWebClient.Models
{
    public class DataBaseContext : DbContext
    {
        public DbSet<ServerConfig> ServerConfig { get; set; }
    }
}
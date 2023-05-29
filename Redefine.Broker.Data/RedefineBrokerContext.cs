using Microsoft.EntityFrameworkCore;
using Redefine.Broker.Data.Models.Security;

namespace Redefine.Broker.Data
{
    public partial class RedefineBrokerContext : DbContext 
    {
       public RedefineBrokerContext(DbContextOptions options) : base(options)
       {}

        public DbSet<User> Users { get; set; }
    }
}
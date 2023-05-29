using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redefine.Broker.Data
{
    public partial class RedefineBrokerContext : DbContext 
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //  Load a config file if one exists
                var configuration = new ConfigurationBuilder()
                                        .SetBasePath(Directory.GetCurrentDirectory())
                                        .AddJsonFile("local.settings.json", true)
                                        .AddJsonFile("appsettings.json", true)
                                        .Build();

                //  DB Connection String Name
                var connectionString = configuration.GetConnectionString("RedefineBrokerConnectionString")
                                        ?? configuration["RedefineBrokerConnectionString"]
                                        ?? configuration["RedefineBrokerDb"]
                                        ?? configuration["Values:RedefineBrokerDb"]
                                        ?? "Data Source=localhost;Initial Catalog=LeadManagement;Trusted_Connection=True;";

                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
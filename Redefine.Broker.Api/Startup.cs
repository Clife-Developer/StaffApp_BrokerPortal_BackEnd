using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Redefine.Broker.Data;
using Redefine.Broker.Data.Identity;
using Redefine.Broker.Data.Models.Security;
using Redefine.Broker.Repository.Interface.Security;
using Redefine.Broker.Repository.Repository.Security;
using Redefine.Broker.Security;
using Redefine.Broker.Services.Interfaces;
using Redefine.Broker.Services.Services.Security;

namespace Redefine.Broker.Api
{
    public class Startup
    {
        private IConfigurationRoot _configuration;
        private const string _connectionString = "StaffApp";
        public Startup(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public void ServiceConfigurations(IServiceCollection services)
        {
            BuildConfiguration();
            ConfigureSQL(services);
            ConfigureServices(services);
        }
        private void BuildConfiguration()
        {
            _configuration = new ConfigurationBuilder()
                        .AddEnvironmentVariables()
                        .AddJsonFile("local.settings.json", true)
                        .AddJsonFile("appsettings.json", true)
                        .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddControllers();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddTransient<IUserStore<User>, UserStore>();
            services.AddTransient<IUserPasswordStore<User>, UserStore>();

            services.AddTransient<UserManager<User, UserStore, UserStore>>();

            services.AddTransient<IJwtFactory, JwtFactory>();

        }
        private void ConfigureSQL(IServiceCollection services)
        {
            var sqlConnection = _configuration.GetConnectionString(_connectionString)
                                ?? Environment.GetEnvironmentVariable($"ConnectionStrings:{_connectionString}")
                                ?? Environment.GetEnvironmentVariable(_connectionString);

            services.AddDbContext<Data.RedefineBrokerContext>(options => options.UseSqlServer(sqlConnection), contextLifetime: ServiceLifetime.Transient);
        }
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (app.Environment.IsDevelopment())
            {
                //app.UseExceptionHandler("/Error");
                //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.Run();
        }
    }
}
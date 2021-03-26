using CommandAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace CommandAPI
{
    public static class ServiceExtensions
    {
        public static void ConfigureNpgSql(this IServiceCollection services, IConfiguration Configuration)
        {
            var builder = new NpgsqlConnectionStringBuilder();
            builder.ConnectionString = Configuration.GetConnectionString("PostgreSqlConnection");
            builder.Username = Configuration["UserID"];
            builder.Password = Configuration["Password"];
            services.AddDbContext<CommandContext>(opt => opt.UseNpgsql(builder.ConnectionString));
        }
    }
}
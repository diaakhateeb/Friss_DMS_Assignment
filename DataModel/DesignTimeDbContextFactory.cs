using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DataModel
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FRISSDmsContext>
    {
        public FRISSDmsContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<FRISSDmsContext>();
            var connectionString = configuration.GetConnectionString("FRISS_DMSDatabase");
            builder.UseSqlServer(connectionString);
            return new FRISSDmsContext(builder.Options, new HttpContextAccessor());
        }
    }
}

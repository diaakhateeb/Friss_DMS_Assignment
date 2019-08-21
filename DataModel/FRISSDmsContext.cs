using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DataModel
{
    public class FRISSDmsContext : IdentityDbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FRISSDmsContext(DbContextOptions<FRISSDmsContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
            optionsBuilder.UseSqlServer(config.GetConnectionString("FRISS_DMSDatabase"));
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<User> User { get; set; }
        public DbSet<Document> Document { get; set; }
    }
}

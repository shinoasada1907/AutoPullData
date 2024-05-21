using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoData
{
    public class MyDbContext : DbContext
    {
        private const string connectionString = "Data Source=10.155.128.23;Initial Catalog=SEMV_AM;Persist Security Info=True;User ID=semv;Password=Semv@123;MultipleActiveResultSets=true;TrustServerCertificate=True;";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
        public DbSet<Model> MyTableAM { get; set; }
    }
}

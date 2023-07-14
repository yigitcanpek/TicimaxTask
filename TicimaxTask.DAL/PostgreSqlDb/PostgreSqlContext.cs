using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicimaxTask.Entities.Entities.Models;
using TicimaxTask.MAP.DataBaseConfigurations;

namespace TicimaxTask.DAL.PostgreSqlDb
{
    public class PostgreSqlContext : DbContext
    {
        public PostgreSqlContext(DbContextOptions<PostgreSqlContext> options) : base(options)
        {
          
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.ApplyConfiguration(new AppUserConfiguration());
            modelBuilder.ApplyConfiguration(new CheckInOutConfiguration());

        }

        DbSet<AppUser> AppUsers { get; set; }
        DbSet<CheckInOut> CheckInOuts { get; set; }
    }
}

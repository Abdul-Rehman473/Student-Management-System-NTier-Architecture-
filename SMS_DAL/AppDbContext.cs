using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using SMS_Objects;

namespace SMS_DAL.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<studentBO> Students { get; set; } 
        
        public DbSet<courseBO> Courses { get; set; }  
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NTier_Final.Areas.Identity.Data;
using SMS_Objects;

namespace NTier_Final.Areas.Identity.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }


    public virtual DbSet<studentBO> Students { get; set; }
    public virtual DbSet<courseBO> Courses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Configure the relationship between Student and Course
        builder.Entity<courseBO>()
            .HasOne(c => c.Student)
            .WithMany(s => s.AssignedCourses)
            .HasForeignKey(c => c.StudentId);
    }
}

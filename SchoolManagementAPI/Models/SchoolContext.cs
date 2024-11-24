using Microsoft.EntityFrameworkCore;

namespace SchoolManagementAPI.Models;
public class SchoolContext : DbContext
{
    public SchoolContext(DbContextOptions<SchoolContext> options)
            : base(options) 
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Class> Classes { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlServer("Server=NVSITPC0E07QG;Database=SchoolManagementDB;Trusted_Connection=True;");
    //}
}

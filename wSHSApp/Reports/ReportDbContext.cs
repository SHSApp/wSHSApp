using Microsoft.EntityFrameworkCore;
using wSHSApp.Reports.DisciplineReport.Data;

namespace wSHSApp.Reports;

public class ReportDbContext : DbContext
{
    public DbSet<Department> Departments { get; set; }
    public DbSet<Rank> Ranks { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<Employee> Personal { get; set; }
    public DbSet<Place> Places { get; set; }
    public DbSet<Violation> Material { get; set; }
    public DbSet<Option> Options { get; set; }

    public ReportDbContext(DbContextOptions<ReportDbContext> options)
        : base(options)
    {
        //Database.EnsureDeleted();
        //Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

    }
}

using Microsoft.EntityFrameworkCore;

namespace wSHSApp.Reports;

public class ReportDbContext : DbContext
{
    public ReportDbContext(DbContextOptions<ReportDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

    }
}

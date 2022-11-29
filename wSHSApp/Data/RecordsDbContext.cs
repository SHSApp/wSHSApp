using Microsoft.EntityFrameworkCore;
using wSHSApp.Models.Records;

namespace wSHSApp.Data;

public class RecordsDbContext : DbContext
{
    public RecordsDbContext(DbContextOptions<RecordsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Prisoner> Prisoners { get; set; }

    public DbSet<Record> RecordList { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
       
    }
}

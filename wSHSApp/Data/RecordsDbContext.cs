using Microsoft.EntityFrameworkCore;

namespace wSHSApp.Data;

public class RecordsDbContext : DbContext
{
    public RecordsDbContext(DbContextOptions<RecordsDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
       
    }
}

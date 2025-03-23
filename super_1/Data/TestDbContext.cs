using Microsoft.EntityFrameworkCore;
using super_1.Models;

namespace super_1.Data
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<SleepRecord> SleepRecords { get; set; }
    }
}

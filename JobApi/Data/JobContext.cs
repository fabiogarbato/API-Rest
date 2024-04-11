using Microsoft.EntityFrameworkCore;

namespace JobApi.Models
{
    public class JobContext : DbContext
    {
        public JobContext(DbContextOptions<JobContext> options) : base(options)
        {
        }

        public DbSet<Job> Jobs { get; set; }
    }
}

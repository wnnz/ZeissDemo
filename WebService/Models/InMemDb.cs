using Microsoft.EntityFrameworkCore;

namespace WebService.Models
{
    public class InMemDb : DbContext
    {
        public InMemDb(DbContextOptions<InMemDb> options) : base(options) { }

        public DbSet<SocketResult> socketResults => Set<SocketResult>();
    }
}

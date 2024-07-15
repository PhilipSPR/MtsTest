using Microsoft.EntityFrameworkCore;
using MtsTest.Models;

namespace MtsTest.Controllers
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ImageFile> ImageFiles { get; set; }
    }
}

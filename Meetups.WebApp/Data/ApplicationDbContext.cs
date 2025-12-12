using Meetups.WebApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Meetups.WebApp.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): DbContext(options)
    {
        public DbSet<Event> Events { get; set; }
    }
}

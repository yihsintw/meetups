using Meetups.WebApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Meetups.WebApp.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): DbContext(options)
    {
        public DbSet<Event> Events { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<RSVP> RSVPs { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // Configure decimal precision for TicketPrice
            modelBuilder.Entity<Event>()
                .Property(e => e.TicketPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Transaction>()
                .Property(e => e.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.RSVP)
                .WithMany(r => r.Transactions)
                .HasForeignKey(r => r.RsvpId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure relationships and constraints if needed
            modelBuilder.Entity<RSVP>()
                .HasOne(r => r.Event)
                .WithMany(e => e.RSVPs) 
                .HasForeignKey(r => r.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RSVP>()
                .HasOne(r => r.User)
                .WithMany(u => u.RSVPs)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Event)
                .WithMany(e => e.Comments)
                .HasForeignKey(c => c.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

using Meetups.WebApp.Data;
using Meetups.WebApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Meetups.WebApp.Features.ViewTransactions
{
    public class ViewTransactionsService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        public async Task<List<Transaction>> GetTransactionsAsync(int organizerId, DateTime beginDate, DateTime endDate)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var transactions = await dbContext.Transactions
                .Include(t => t.RSVP)
                .ThenInclude(r => r.Event)
                .Where(t => t.RSVP != null && t.RSVP.Event != null && t.RSVP.Event.OrganizerId == organizerId && 
                t.PaymentDate >= beginDate && t.PaymentDate < endDate.AddDays(1))
                .Include(t => t.RSVP.User)
                .ToListAsync();

            return transactions;
        }
    }
}
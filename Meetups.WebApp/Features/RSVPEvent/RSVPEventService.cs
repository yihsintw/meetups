using Meetups.WebApp.Data;
using Meetups.WebApp.Shared;
using Microsoft.EntityFrameworkCore;

namespace Meetups.WebApp.Features.RSVPEvent
{
    public class RSVPEventService(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        public IDbContextFactory<ApplicationDbContext> ContextFactory { get; } = contextFactory;


        public async Task<bool> RSVPToEventAsync(int eventId, string email, string? paymentId = "")
        {
            using var dbContext = ContextFactory.CreateDbContext();
            // Find the user by email
            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) {
                // User not found
                return false;
            }
            var userId = user.UserId;
            // Check if the RSVP already exists
            var existingRSVP = await dbContext.RSVPs
                .FirstOrDefaultAsync(r => r.EventId == eventId && r.UserId == userId);
            if (existingRSVP != null)
            {
                // RSVP already exists
                if (!string.IsNullOrEmpty(paymentId))
                {
                    existingRSVP.PaymentId = paymentId;
                    await dbContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            else
            {
                // Create a new RSVP
                try
                {
                    var rsvp = new Data.Entities.RSVP
                    {
                        EventId = eventId,
                        UserId = userId,
                        RSVPDate = DateTime.Now,
                        Status = SharedHelper.GoingStatus, // Assuming "Going" status for RSVP
                        PaymentId = paymentId
                    };
                    dbContext.RSVPs.Add(rsvp);
                    await dbContext.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                    return false;
                }
            }
            
            
        }
    }
}

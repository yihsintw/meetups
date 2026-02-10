using Meetups.WebApp.Data;
using Meetups.WebApp.Shared;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;

namespace Meetups.WebApp.Features.RSVPEvent
{
    public class RSVPEventService(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        public IDbContextFactory<ApplicationDbContext> ContextFactory { get; } = contextFactory;


        public async Task<bool> RSVPToEventAsync(int eventId, string email,
            string? paymentId = "",
            string? paymentStatus = "")
        {
            using var dbContext = ContextFactory.CreateDbContext();
            // Find the user by email
            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
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
                    existingRSVP.PaymentStatus = paymentStatus;

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
                        PaymentId = paymentId,
                        PaymentStatus = paymentStatus
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

        public async Task<string> IsUserRSVPedAsync(int eventId, int userId)
        {
            string retVal = string.Empty;
            using var dbContext = ContextFactory.CreateDbContext();
            // Find the user by email
            
            // Check if the RSVP already exists
            var existingRSVP = await dbContext.RSVPs
                .FirstOrDefaultAsync(r => r.EventId == eventId && r.UserId == userId);

            if (existingRSVP == null) return retVal;

            if (existingRSVP?.Status == SharedHelper.CancelledStatus)
            {
                retVal = "RSVP has been cancelled";
            }
            else if (existingRSVP?.Status == SharedHelper.GoingStatus)
            {
                retVal = "User is going to the event";
            }
            else
            {
                retVal = "User has not RSVPed to the event";
            }

            return retVal;
        }
    }
}

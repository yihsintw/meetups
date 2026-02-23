using AutoMapper;
using Meetups.WebApp.Data;
using Meetups.WebApp.Data.Entities;
using Meetups.WebApp.Shared;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Meetups.WebApp.Features.CancelRSVP
{
    public class CancelRSVPService(IDbContextFactory<ApplicationDbContext> dbContextFactory,
        IConfiguration configuration)
    {

        private readonly string stripeApiKey = configuration["Stripe:SecretKey"] ?? string.Empty;

        /// <summary>
        /// return RSVP Id
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> CancelRSVPAsync(int eventId, int userId)
        {
            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var rsvp = await dbContext.RSVPs
                .Include(r => r.Event)
                .FirstOrDefaultAsync(r => 
                        r.EventId == eventId && r.UserId == userId && r.Status == SharedHelper.GoingStatus);

            if (rsvp == null) return 0;

            rsvp.Status = SharedHelper.CancelledStatus;

            if (rsvp.Event is not null && rsvp.Event.TicketPrice > 0 && rsvp.Event.Refundable)
            {
                //process refund process here, for example, call payment gateway API to refund the payment
                // using stripeApiKey for authentication
                StripeConfiguration.ApiKey = stripeApiKey;
                var refund = ProcessRefund(rsvp);
                rsvp.RefundId = refund.Id; // Store the refund ID in the RSVP record
                rsvp.RefundStatus = refund.Status; // Update the refund status in the RSVP record

                Transaction transaction = new()
                {
                    RsvpId = rsvp.RsvpId,
                    PaymentDate = DateTime.UtcNow,
                    Amount = -rsvp.Event.TicketPrice.Value, 
                    TransactionType = SharedHelper.TransactionTypeRefund ,
                    Status = refund.Status
                };

                dbContext.Transactions.Add(transaction);

            }

            await dbContext.SaveChangesAsync();

            return rsvp.RsvpId;
        }

        //Process refund logic here, for example, call payment gateway API to refund the payment
        public Refund ProcessRefund(RSVP rsvp)
        {

            var paymentId = rsvp.PaymentId; 

            if(string.IsNullOrEmpty(paymentId))
            {
                return new Refund
                {
                    Id = "N/A",
                    Status = "No payment to refund"
                };
            }
            // Implement your refund logic here, for example, using Stripe API to process the refund
            var options = new RefundCreateOptions
            {
                PaymentIntent = paymentId,
                //full refund, not partial refund, so we don't specify the amount here  
                //Amount = 1000, // Amount in cents, adjust as needed
                Reason = "requested_by_customer",
            };
            var service = new RefundService();
            Refund refund = service.Create(options);

            return refund;
        }

        //Get RSVP instance by rsvpId
        public async Task<RSVP?> GetRSVPByIdAsync(int rsvpId)
        {
            var paymentId = rsvpId;

            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var rsvp = await dbContext.RSVPs
                .Include(r => r.Event)
                .FirstOrDefaultAsync(r =>
                        r.RsvpId == rsvpId);

            return rsvp;
        }
        
    }
}

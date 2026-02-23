using AutoMapper;
using Meetups.WebApp.Data;
using Meetups.WebApp.Features.ViewEvent;
using Meetups.WebApp.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Stripe;
using Stripe.Checkout;
using Meetups.WebApp.Data.Entities;
using Meetups.WebApp.Shared;

namespace Meetups.WebApp.Features.MakePayment
{
    public class MakePaymentService(IDbContextFactory<ApplicationDbContext> dbContextFactory,
            IMapper mapper,
            IConfiguration configuration)
    {
        private readonly string stripeApiKey = configuration["Stripe:SecretKey"]??string.Empty;

    
        

        public async Task<EventViewModel?> GetEventByIdAsync(int eventId)
        {
            using var context = dbContextFactory!.CreateDbContext();

            var existingEvent = await context.Events
                .FirstOrDefaultAsync(e => e.EventId == eventId);
            if (existingEvent == null)
                return null;

            var eventViewModel = mapper!.Map<EventViewModel>(existingEvent);
            return eventViewModel;


        }

        public async Task<string> CreateCheckoutSession(EventViewModel _event, string baseUrl, string cancelUrl)
        {
            //var _event = await GetEventByIdAsync(eventId);
            var eventId = _event.EventId;
            var options = new SessionCreateOptions
            {
                Mode = "payment",
                SuccessUrl =  $"{baseUrl.TrimEnd("/")}/payment-success/{eventId}/{{CHECKOUT_SESSION_ID}}",
                CancelUrl = cancelUrl,
                LineItems =
                [
                    new()
                    {
                        PriceData =
                        new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            UnitAmount = (long?)((_event?.TicketPrice ?? 0) * 100), // Stripe expects the amount in cents
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = _event?.Title,
                                Description = $"Date and Time:{ _event?.BeginDate} {_event?.BeginTime:h:mm tt} - { _event?.EndDate} {_event?.EndTime:h:mm tt}",   
                            },
                        },  
                        Quantity = 1
                        
                        
                    }

                ],
                

            };
            
            StripeConfiguration.ApiKey = stripeApiKey;

            var service = new SessionService();
            var session = await service.CreateAsync(options);
            
            return session.Url;
        }

        public async Task<Session> GetCheckoutSessionAsync(string sessionId)
        {
            StripeConfiguration.ApiKey = stripeApiKey;

            var service = new SessionService();
            var session = await service.GetAsync(sessionId);
            return session;
        }

        //record a transaction by providing Rsvp Id and Payment Status
        public async Task RecordTransaction(int rsvpId, string? paymentStatus)
        {
            //bool result = false;

            using var context = dbContextFactory!.CreateDbContext();
            var rsvp = await context.RSVPs
                .Include(r => r.Event)
                .FirstOrDefaultAsync(r=> r.RsvpId == rsvpId);

            if (rsvp != null && rsvp.Event != null && rsvp.Event.TicketPrice.HasValue && rsvp.Event.TicketPrice.Value > 0)
            {
                Transaction transaction = new()
                {
                    RsvpId = rsvpId,
                    PaymentDate = DateTime.Now,
                    Amount = rsvp.Event.TicketPrice.Value,
                    TransactionType = SharedHelper.TransactionTypeCharge,
                    Status = paymentStatus
                };

                context.Transactions.Add(transaction);
                await context.SaveChangesAsync();
            }
            
            //return result;
        }

    }
}

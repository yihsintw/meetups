namespace Meetups.WebApp.Data.Entities
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int RsvpId { get; set; }

        public RSVP RSVP { get; set; } = default!;

        public DateTime PaymentDate { get; set; }

        public decimal Amount { get; set; }
        
        public string TransactionType { get; set; } = default!; // e.g., "Payment", "Refund"

        public string? Status { get; set; }


    }
}

using Stripe;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Meetups.WebApp.Data.Entities
{
    public class RSVP
    {
        public int RsvpId { get; set; }

        public int EventId { get; set; }

        [JsonIgnore]
        public Event? Event { get; set; }

        public int UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        [Required]
        public DateTime RSVPDate { get; set; } // Date and Time when the RSVP was made

        //status
        [Required]
        public string? Status { get; set; } // e.g., "Going", "Not Going", "Cancelled"

        //payment info
        public string? PaymentId { get; set; }

        //Payment status
        public string? PaymentStatus { get; set; } // e.g., "Paid", "Pending", "Failed","Refunded"

        //Refund id
        public string? RefundId { get; set; } = string.Empty;

        public string? RefundStatus { get; set; } = string.Empty;









    }
}

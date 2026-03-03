using System.ComponentModel.DataAnnotations;

namespace Meetups.WebApp.Data.Entities
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(maximumLength:200)]
        public string? Name { get; set; }

        [Required]
        [StringLength(maximumLength: 200)]
        public string? Email { get; set; }

        public string? Role { get; set; }

        public List<RSVP>? RSVPs { get; set; } = [];

        public List<OrganizerReview> OrganizerReviews { get; set; } = [];

        public List<OrganizerReview> ReviewsWritten { get; set; } = [];

    }
}

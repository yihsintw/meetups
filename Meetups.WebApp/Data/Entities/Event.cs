using Meetups.WebApp.Features.CreateEvent;
using System.ComponentModel.DataAnnotations;

namespace Meetups.WebApp.Data.Entities
{
    public class Event
    {
        public int EventId { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string? Title { get; set; }
        [StringLength(maximumLength: 4000)]
        public string? Description { get; set; }
        [Required]
        public DateOnly BeginDate { get; set; }
        [Required]
        public TimeOnly BeginTime { get; set; }
        [Required]
        public DateOnly EndDate { get; set; }
        [Required]
        public TimeOnly EndTime { get; set; }
        public string? Location { get; set; }
        public string? MeetupLink { get; set; }
        [Required]
        public string? Category { get; set; }
        [Range(0, int.MaxValue)]
        public int Capacity { get; set; }
        [Required]
        public string? ImageUrl { get; set; }
        public int OrganizerId { get; set; }

        public List<RSVP>? RSVPs { get; set; } = [];

    }
}

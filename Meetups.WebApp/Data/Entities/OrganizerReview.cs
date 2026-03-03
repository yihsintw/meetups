using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Meetups.WebApp.Data.Entities
{
    public class OrganizerReview
    {
        // create organizer review properties
        // 

        public int OrganizerReviewId { get; set; }

        public int ReviewerUserId { get; set; }

        [JsonIgnore]
        public User? ReviewerUser { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public string? ReviewText { get; set; }

        public DateTime ReviewDate { get; set; } = DateTime.Now;

        public int OrganizerId { get; set; }

        [JsonIgnore]
        public User? Organizer { get; set; }

        





    }
}

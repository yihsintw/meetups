using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Meetups.WebApp.Data.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }
        [Required]
        public int EventId { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string? UserName { get; set; }
        [Required]
        public DateTime PostedOn { get; set; }
        [Required]
        [StringLength(1000)]
        public string? Message { get; set; }

        [JsonIgnore]
        public Event? Event { get; set; }
    }
}

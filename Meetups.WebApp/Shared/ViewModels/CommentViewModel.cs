using Meetups.WebApp.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Meetups.WebApp.Shared.ViewModels
{
    public class CommentViewModel
    {
        public int CommentId { get; set; }
        [Required]
        public int EventId { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string? UserName { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime PostedOn { get; set; }
        [Required]
        public string? Message { get; set; }



    }
}

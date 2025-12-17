using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace Meetups.WebApp.Shared.ViewModels
{
    public class EventViewModel
    {
        public int EventId { get; set; }
        [Required]
        [StringLength(maximumLength:100)]
        public string? Title { get; set; }
        [StringLength(maximumLength:500)]
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
        public string? Category { get; set; } = string.Empty;
        [Range(0,int.MaxValue)]
        public int Capacity { get; set; }
        [Required(ErrorMessage = "Please upload an image for the meetup.")]
        public IBrowserFile? CoverImage { get; set; }
        public string ImageUrl { get; set; }
        public int OrganizerId { get; set; }

        
        public string ValidateDates()
        {
            var begin = BeginDate.ToDateTime(BeginTime);
            var end = EndDate.ToDateTime(EndTime);
            if (end <= begin)
                return "Begin Date should be ealier than End Date.";
            if ((end - begin) > TimeSpan.FromDays(1))
            {
                return "The event should not last more than 24 hours.";
            }
            return string.Empty;
        }

        public string ValidateLocation()
        {
            if (Category== MeetupCategoriesEnum.InPerson.ToString() && string.IsNullOrWhiteSpace(Location))
                return "Venue Address is required for In-Person Meetup.";
            return string.Empty;
        }

        public string ValidateMeetupLink()
        {
            if (Category == MeetupCategoriesEnum.Online.ToString() && string.IsNullOrWhiteSpace(MeetupLink))
                return "Meetup Link is required for Online Meetup.";
            return string.Empty;
        }

       public EventViewModel()
        {
            BeginDate = DateOnly.FromDateTime(DateTime.Now);
            EndDate = DateOnly.FromDateTime(DateTime.Now);
            BeginTime= TimeOnly.FromDateTime(DateTime.Now); 
            EndTime= TimeOnly.FromDateTime(DateTime.Now);

            //Category= MeetupCategoriesEnum.InPerson.ToString();
            ImageUrl= "/images/events/image-holder.png";

            //CoverImage = IBrowserFile.
           
        }

    }
}

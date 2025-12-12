using System.ComponentModel.DataAnnotations;

namespace Meetups.WebApp.Features.Events.CreateEvent
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
        public string? Category { get; set; }
        [Range(0,int.MaxValue)]
        public int Capacity { get; set; }
        public int OrganizerId { get; set; }

        
        public string ValidateDates()
        {
            var begin = BeginDate.ToDateTime(BeginTime);
            var end = EndDate.ToDateTime(EndTime);
            if (end <= begin)
                return "Begin Date should be ealier than End Date.";
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

            Category= MeetupCategoriesEnum.InPerson.ToString();
        }

    }
}

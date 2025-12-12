using Meetups.WebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace Meetups.WebApp.Features.Events.CreateEvent
{
    public class CreateEventService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        public CreateEventService()
        {
            
        }
        public CreateEventService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreateEventAsync(EventViewModel model)
        {
            using var context = _contextFactory.CreateDbContext();
            var newEvent = new Data.Entities.Event
            {
                Title = model.Title,
                Description = model.Description,
                BeginDate = model.BeginDate,
                BeginTime = model.BeginTime,
                EndDate = model.EndDate,
                EndTime = model.EndTime,
                Location = model.Location,
                MeetupLink = model.MeetupLink,
                Category = model.Category,
                Capacity = model.Capacity,
                OrganizerId = model.OrganizerId
            };
            context.Events.Add(newEvent);
            await context.SaveChangesAsync();
        }

        public string ValicateEvent(EventViewModel model)
        {
            if (model is null)
            {
                return "Event is null";
            }
            var dateValidationMessage = model.ValidateDates();
            if (!string.IsNullOrEmpty(dateValidationMessage))
                return dateValidationMessage;
            var locationValidationMessage = model.ValidateLocation();
            if (!string.IsNullOrEmpty(locationValidationMessage))
                return locationValidationMessage;
            var linkValidationMessage = model.ValidateMeetupLink();
            if (!string.IsNullOrEmpty(linkValidationMessage))
                return linkValidationMessage;
            return string.Empty;
        }
    }
}

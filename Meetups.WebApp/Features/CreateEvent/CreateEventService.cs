using AutoMapper;
using Meetups.WebApp.Data;
using Meetups.WebApp.Shared;
using Meetups.WebApp.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Meetups.WebApp.Features.CreateEvent
{
    public class CreateEventService
    {
        private readonly IDbContextFactory<ApplicationDbContext>? _contextFactory;
        private readonly IMapper? mapper;

        public CreateEventService()
        {
            
        }
        public CreateEventService(IDbContextFactory<ApplicationDbContext> contextFactory,
            IMapper mapper)
        {
            _contextFactory = contextFactory;
            this.mapper = mapper;
        }

        public async Task CreateEventAsync(EventViewModel model)
        {
            using var context = _contextFactory!.CreateDbContext();

            /*
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
            */
            var eventEntity = mapper?.Map<Data.Entities.Event>(model);
            if (eventEntity!=null)
            {
                context.Events.Add(eventEntity);
                await context.SaveChangesAsync();
            }
            
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

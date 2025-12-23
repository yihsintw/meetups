using AutoMapper;
using Meetups.WebApp.Data;
using Meetups.WebApp.Shared;
using Meetups.WebApp.Shared.ViewModels;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace Meetups.WebApp.Features.EditEvent
{
    public class EditEventService
    {
        private readonly IDbContextFactory<ApplicationDbContext>? _contextFactory;
        private readonly IMapper? mapper;

        public EditEventService()
        {
            _contextFactory = null;
            mapper = null;
        }
        

        public EditEventService(IDbContextFactory<ApplicationDbContext> dbContextFactory, IMapper mapper)
        {
            _contextFactory = dbContextFactory;
            this.mapper = mapper;
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

        public async Task UpdateEventAsync(EventViewModel model)
        {
            // Implementation for editing an event goes here
            using var _dbContext = _contextFactory!.CreateDbContext();
            var existingEvent = await _dbContext!.Events
                .Where(e => e.EventId == model.EventId)
                .FirstAsync();
            if (existingEvent != null)
            {
                mapper!.Map(model, existingEvent);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<EventViewModel?> GetEventForEditAsync(int eventId)
        {
            // Implementation for retrieving the event for editing goes here
            using var _dbContext = _contextFactory!.CreateDbContext();
            var editEvent = await _dbContext!.Events
                .Where(e => e.EventId == eventId)
                .FirstOrDefaultAsync();

            if (editEvent == null)
                return null;

            
            var imageUrl = editEvent.ImageUrl;
            var eventViewModel = mapper!.Map<EventViewModel>(editEvent);
            
            
            return eventViewModel;
        }
    }
}

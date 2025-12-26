using AutoMapper;
using Meetups.WebApp.Data;
using Meetups.WebApp.Shared;
using Meetups.WebApp.Shared.ViewModels;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace Meetups.WebApp.Features.DeleteEvent
{
    public class DeleteEventService
    {
        private readonly IDbContextFactory<ApplicationDbContext>? _contextFactory;
        private readonly IMapper? mapper;

        public DeleteEventService()
        {
            _contextFactory = null;
            mapper = null;
        }
        

        public DeleteEventService(IDbContextFactory<ApplicationDbContext> dbContextFactory, IMapper mapper)
        {
            _contextFactory = dbContextFactory;
            this.mapper = mapper;
        }


        public async Task<string> DeleteEventAsync(int eventId)
        {
            // Implementation for editing an event goes here
            using var _dbContext = _contextFactory!.CreateDbContext();
            var existingEvent = await _dbContext!.Events
                .Where(e => e.EventId == eventId)
                .FirstAsync();
            if (existingEvent == null)
            {
                return "Event not found.";
            }

            var beginDateTime = existingEvent.BeginDate.ToDateTime(existingEvent.BeginTime);
            var endDateTime = existingEvent.EndDate.ToDateTime(existingEvent.EndTime);
            if (beginDateTime < DateTime.Now)
            {
                return "Cannot delete past events.";
            }

            _dbContext!.Events.Remove(existingEvent);
            await _dbContext.SaveChangesAsync();

            return string.Empty;
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

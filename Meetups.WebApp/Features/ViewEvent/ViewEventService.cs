using AutoMapper;
using Meetups.WebApp.Data;
using Meetups.WebApp.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Meetups.WebApp.Features.ViewEvent
{
    public class ViewEventService(IDbContextFactory<ApplicationDbContext> contextFactory,
            IMapper _mapper)
    {

        private readonly IDbContextFactory<ApplicationDbContext>? _contextFactory = contextFactory;
        private readonly IMapper? mapper = _mapper;

        public async Task<EventViewModel?> GetEventByIdAsync(int eventId)
        {
            using var context = _contextFactory!.CreateDbContext();

            var existingEvent = await context.Events
                .FirstOrDefaultAsync(e => e.EventId == eventId);
            if (existingEvent == null)
                return null;

            var eventViewModel = mapper!.Map<EventViewModel>(existingEvent);
            return eventViewModel;


        }
    }
}

using AutoMapper;
using Meetups.WebApp.Data;
using Meetups.WebApp.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Meetups.WebApp.Features.DiscoverEvents
{
    public class DiscoverEventsService(IDbContextFactory<ApplicationDbContext> _contextFactory,
            IMapper mapper)
    {
        //private readonly IDbContextFactory<ApplicationDbContext>? _contextFactory = contextFactory;
        //private readonly IMapper? mapper = _mapper;

        

        public async Task<List<EventViewModel>> GetEventsAsync(string? filter = "")
        {
            using var context = _contextFactory!.CreateDbContext();

            // get events by filter: if filter exists, then filter by title, description or location
            // if filter is null or empty, then get all events
            // whether filter is empty or not, only get future events
            // the result should be ordered by begin date and time descending

            var query = context.Events.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(p =>
                    (p.Title + "").Contains(filter) ||
                    (p.Description + "").Contains(filter) ||
                    (p.Location + "").Contains(filter));

                if (!query.Any())
                {
                    query = context.Events.AsQueryable();
                }
            }

            var events = await query
                .Where(p => p.BeginDate.ToDateTime(p.BeginTime) > DateTime.UtcNow)
                .OrderByDescending(p => p.BeginDate.ToDateTime(p.BeginTime))
                .ToListAsync();

            return mapper!.Map<List<EventViewModel>>(events);
        }


    }
}

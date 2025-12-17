using AutoMapper;
using Meetups.WebApp.Data;
using Meetups.WebApp.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Meetups.WebApp.Features.ViewCreatedEvents
{
    public class ViewCreatedEventsService
    {
        private readonly IDbContextFactory<ApplicationDbContext>? _contextFactory;
        private readonly IMapper? mapper;

        public ViewCreatedEventsService()
        {
            
        }
        public ViewCreatedEventsService(IDbContextFactory<ApplicationDbContext> contextFactory,
            IMapper mapper) {
            _contextFactory = contextFactory;
            this.mapper = mapper;
        }

        public async Task<List<EventViewModel>> GetAllEventsAsync()
        {
            await using var context = _contextFactory!.CreateDbContext();
            var events = await context.Events.ToListAsync();

            return  mapper!.Map<List<EventViewModel>>(events);
        }

    }
}

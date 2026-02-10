using AutoMapper;
using Meetups.WebApp.Data;
using Meetups.WebApp.Data.Entities;
using Meetups.WebApp.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Meetups.WebApp.Features.ManageUserRSVPEvents
{
    public class ManageUserRSVPEventsService(IDbContextFactory<ApplicationDbContext> contextFactory, IMapper mapper)
    {
        public IDbContextFactory<ApplicationDbContext> ContextFactory { get; } = contextFactory;
        public IMapper Mapper { get; } = mapper;

        public async Task<List<EventViewModel>> GetUserRSVPEventByEmail(string email)
        {
            using var dbContext = ContextFactory.CreateDbContext();
            /*var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return [];
            }
            var userId = user.UserId;
            var events = await dbContext.RSVPs.Where(r => r.UserId == userId)
                .Include(e => e.Event).ToListAsync();
            */

            var events = await dbContext.RSVPs
                .Include(r => r.User)
                .Where(r => r.User!.Email == email && r.Status == Shared.SharedHelper.GoingStatus)
                .Include(r => r.Event)
                .Select(r => r.Event!)
                .ToListAsync();

            return Mapper.Map<List<EventViewModel>>(events);
        }

        public async Task<List<EventViewModel>> GetUserRSVPEventByUserId(int userId)
        {
            using var dbContext = ContextFactory.CreateDbContext();
            
            var events = await dbContext.RSVPs
                .Where(r => r.UserId == userId && r.Status == Shared.SharedHelper.GoingStatus)     
                .Include(r => r.Event)
                .Select(r => r.Event!)
                .ToListAsync();
            return Mapper.Map<List<EventViewModel>>(events);
        }






    }
}

using Meetups.WebApp.Data;
using Meetups.WebApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Meetups.WebApp.Features.ViewOrganizerReviews
{
    public class ViewOrganizerReviewsService(IDbContextFactory<ApplicationDbContext> contextFactory)
    {

        public async Task<User?> GetUserByIdAsync(int userId)
        {
             using var context = await contextFactory.CreateDbContextAsync();
            return await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);   
        }

        public async Task<List<OrganizerReview>> GetOrganizerReviewsAsync(int organizerId)
        {
            using var context = await contextFactory.CreateDbContextAsync();
            return await context.OrganizerReviews.Where(r => r.OrganizerId == organizerId)
                .Include(r => r.ReviewerUser)
                .ToListAsync();
        }

        // Get organizer average rating by organizer id
        public async Task<(int reviewCount, int averageRating)> GetOrganizerAverageRatingAsync(int organizerId)
        {
            using var context = await contextFactory.CreateDbContextAsync();

            var reviewCount = await context.OrganizerReviews
                    .Where(r => r.OrganizerId == organizerId)
                    .CountAsync();

            double averageRating = 0;


            if (reviewCount > 0)
            {
                averageRating = await context.OrganizerReviews
                .Where(r => r.OrganizerId == organizerId)
                .AverageAsync(r => r.Rating);
            }

            return (reviewCount, (int)Math.Round(averageRating, MidpointRounding.AwayFromZero));
        }
    }
}

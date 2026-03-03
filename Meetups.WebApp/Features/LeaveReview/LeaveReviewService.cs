using Meetups.WebApp.Data;
using Meetups.WebApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Meetups.WebApp.Features.LeaveReview
{
    public class LeaveReviewService(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        public async Task<User?> GetUserByIdAsync(int userId)
        {
            using var context = await contextFactory.CreateDbContextAsync();
            return await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<bool> ReviewOrganizerAsync(OrganizerReview review)
        {
            if(review == null || review.OrganizerId == 0 || 
                review.ReviewerUserId <= 0 ||
                review.Rating < 1 ||
                review.Rating > 5 ) return false;

            using var context = await contextFactory.CreateDbContextAsync();

            // check if the reviewer has attended at least one event organized by the organizer
            var hasAttended = await context.RSVPs
                .Include(r => r.Event)
                .AnyAsync(r => r.UserId == review.ReviewerUserId && 
                               r.Event!.OrganizerId == review.OrganizerId);
            
            if (!hasAttended) return false;

            // check if the reviewer has already reviewed this organizer
            var hasReviewed = await context.OrganizerReviews
                .AnyAsync(r => r.ReviewerUserId == review.ReviewerUserId && 
                               r.OrganizerId == review.OrganizerId);
            
            if (hasReviewed) return false;

            context.OrganizerReviews.Add(review);
            await context.SaveChangesAsync();
            return true;
        }
    }
}

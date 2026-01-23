using AutoMapper;
using Meetups.WebApp.Data;
using Meetups.WebApp.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Meetups.WebApp.Features.LeaveEventComments
{
    public class LeaveEventCommentsService(IDbContextFactory<ApplicationDbContext> contextFactory, IMapper mapper)
    {
        public IDbContextFactory<ApplicationDbContext> ContextFactory { get; } = contextFactory;
        public IMapper Mapper { get; } = mapper;

        //Get CommentViewModels By Event Id
        public async Task<List<CommentViewModel>> GetCommentsByEventIdAsync(int eventId)
        {
            using var dbContext = ContextFactory.CreateDbContext();
            var comments = await dbContext.Comments
                .Where(c => c.EventId == eventId)
                .ToListAsync();
            var commentViewModels = Mapper.Map<List<CommentViewModel>>(comments);
            return commentViewModels;
        }

        //Add Comment
        public async Task<string> AddCommentAsync(CommentViewModel commentViewModel)
        {
            using var dbContext = ContextFactory.CreateDbContext();
            try
            {
                var comment = Mapper.Map<Data.Entities.Comment>(commentViewModel);
                dbContext.Comments.Add(comment);
                await dbContext.SaveChangesAsync();
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


        }
    }
}

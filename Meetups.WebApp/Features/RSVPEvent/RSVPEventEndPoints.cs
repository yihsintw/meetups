using Meetups.WebApp.Data;
using Meetups.WebApp.Data.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Meetups.WebApp.Features.RSVPEvent
{
    public static class RSVPEventEndPoints
    {
        public static void MapRSVPEventEndPoints(this WebApplication app)
        {
            app.MapGet("/rsvp/{eventId:int}/{paymentId?}",
                async (int eventId,
                    string? paymentId,
                    HttpContext context,
                    RSVPEventService rsvpEventService,
                    IDbContextFactory<ApplicationDbContext> contextFactory) =>
                {
                    var claims = context.User?.Claims;
                    //var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                    var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                    //var phoneNumber = claims?.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value;

                    bool result = await rsvpEventService.RSVPToEventAsync(eventId, email ?? "", paymentId);
                    var errorMessage = string.Empty;
                    //result = false; // For testing error message display
                    if (result)
                    {
                        using var dbContext = await contextFactory.CreateDbContextAsync();
                        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

                        context.Response.Redirect($"/users/{user?.UserId}/manage-rsvp-events");
                    }
                    else
                    {
                        context.Response.Redirect($"/rsvp-error/{eventId}");
                    }

                }
            );
        }
    }
}

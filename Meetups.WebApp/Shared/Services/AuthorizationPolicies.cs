using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Meetups.WebApp.Shared.Services
{
    public static class AuthorizationPolicies
    {
        public static void AddCustomPolicies(this AuthorizationBuilder authorizationBuilder)
        {
            authorizationBuilder.AddPolicy("SameUserPolicy", policy =>
            {
                policy.RequireAssertion(context =>
                {
                    var user = context.User;
                    var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
                    var userId = userIdClaim?.Value;
                    if (int.TryParse(userId, out var authenticationUserId))
                    {
                        // Check if the requested user ID matches the authenticated user ID
                        var routeData = context.Resource as Microsoft.AspNetCore.Http.HttpContext;
                        if (routeData is not null)
                        {
                            var routeUserIdString = routeData.Request.RouteValues["userId"]?.ToString();
                            if (int.TryParse(routeUserIdString, out var routeId))
                            {
                                return authenticationUserId == routeId;
                            }
                        }

                    }
                    return false;

                });
            });
        }
    }
}
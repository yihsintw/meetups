using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Meetups.WebApp.Shared.EndPoints
{
    public static class AuthenticationEndPoints
    {
        public static void MapAuthenticationEndPoints(this WebApplication app)
        {
            app.MapGet("/authentication/{providerName}",
            async (string providerName, HttpContext context) =>
            {
                //系統判斷斷要redirect的Url
                var redirectUrl = $"{context.Request.Scheme}://{context.Request.Host}/signin-callback";
                //或是Hard-Coding
                redirectUrl = "signin-callback";


                var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
                await context.ChallengeAsync(providerName, properties);
            });



            app.MapGet("/signin-callback",
            async context =>
            {
                var authenticateResult = await context.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                if (!authenticateResult.Succeeded || authenticateResult.Principal == null)
                {
                    context.Response.Redirect("/");
                    return;
                }

                //將登入資訊寫入Cookie
                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticateResult.Principal);
                //登入成功後的邏輯
                context.Response.Redirect("/");
            });

            app.MapGet("/signout",
            async context =>
            {
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                context.Response.Redirect("/");
            });
        }
    }
}

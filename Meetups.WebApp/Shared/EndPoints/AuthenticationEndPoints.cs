using Meetups.WebApp.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
            async (HttpContext context,IDbContextFactory<ApplicationDbContext> contextFactory) =>
            {
                var authenticateResult = await context.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                if (!authenticateResult.Succeeded || authenticateResult.Principal == null)
                {
                    context.Response.Redirect("/");
                    return;
                }

                //取得使用者資訊,例如: Name, Email等,可以根據需求進行處理並寫入資料庫
                var claims = authenticateResult.Principal.Claims;
                var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var phoneNumber = claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value; 

                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(email)) {
                    using var dbContext = contextFactory.CreateDbContext();
                    var user = dbContext.Users.FirstOrDefault(
                        u => u.Email == email);

                    //如果使用者不存在，則新增使用者
                    if (user == null) {
                        user = new Data.Entities.User
                        {
                            Name = name,
                            Email = email,
                            Role = SharedHelper.AttendeeRole
                        };
                        dbContext.Users.Add(user);
                        await dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        //使用者已存在，可以更新資訊或進行其他操作
                        user.Name = name; //例如更新名稱
                        //dbContext.Users.Update(user);
                        await dbContext.SaveChangesAsync();
                    }
                }
                else
                {
                    //缺少必要資訊，導向錯誤頁面或請求補充資訊
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

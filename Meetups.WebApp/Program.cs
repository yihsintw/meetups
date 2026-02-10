using Microsoft.EntityFrameworkCore;
using Meetups.WebApp.Shared;
using Meetups.WebApp.Data;
using Meetups.WebApp.Features.ViewCreatedEvents;
using Meetups.WebApp.Features.CreateEvent;
using Meetups.WebApp.Features.EditEvent;
using Meetups.WebApp.Features.DeleteEvent;
using Meetups.WebApp.Features.DiscoverEvents;
using Meetups.WebApp.Features.ViewEvent;
using Meetups.WebApp.Shared.Services;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Client;
using Meetups.WebApp.Shared.EndPoints;
using Meetups.WebApp.Features.RSVPEvent;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Security.Claims;
using Meetups.WebApp.Features.ManageUserRSVPEvents;
using Meetups.WebApp.Features.LeaveEventComments;
using Meetups.WebApp.Features.MakePayment;
using Meetups.WebApp.Features.CancelRSVP;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<LayoutService>();

builder.Services.AddTransient<SharedHelper>();
builder.Services.AddTransient<CreateEventService>();
builder.Services.AddTransient<EditEventService>();
builder.Services.AddTransient<DeleteEventService>();
builder.Services.AddTransient<ViewCreatedEventsService>();
builder.Services.AddTransient<DiscoverEventsService>();
builder.Services.AddTransient<ViewEventService>();
builder.Services.AddTransient<RSVPEventService>();
builder.Services.AddTransient<ManageUserRSVPEventsService>();
builder.Services.AddTransient<LeaveEventCommentsService>();
builder.Services.AddTransient<MakePaymentService>();
builder.Services.AddTransient<CancelRSVPService>();




// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
{
    //options.UseInMemoryDatabase("Meetups");
    options.UseSqlServer(builder.Configuration.GetConnectionString("MeetupConnection"));
}); 
//builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
//{
//    options.UseInMemoryDatabase("Meetups");
//});

builder.Services.AddAutoMapper(typeof(MappingProfile));

//configure google authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Google:ClientId"] ?? string.Empty;
        options.ClientSecret = builder.Configuration["Google:ClientSecret"] ?? string.Empty;
        
        options.Events = new OAuthEvents
        {
            //設定取得使用者的哪些資訊,例如: email, profile等及導向的callback至/sigin-callback
            //receive ticket after successful authentication
            OnTicketReceived = async context =>
            {
                if (context.Principal != null)
                {
                    //sign in the user with cookie authentication
                    //在這時候還沒有簽cookie,所以要自己簽
                    await context.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, context.Principal);
                    //然後redirect回應用程式
                    context.Response.Redirect(location: context.ReturnUri??"");
                    //為了避免後續的處理,要告訴系統這個request(如後續會建立Cookie等)已經處理完了,
                    context.HandleResponse(); // Prevent further processing
                }
            },
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddCustomPolicies();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

//Google authentication
app.UseAuthentication();
app.UseAuthorization(); // Add this line to enable authorization middleware

app.MapStaticAssets();

//Authentication Endpoints
app.MapAuthenticationEndPoints();

//Map RSVP Event Endpoints
app.MapRSVPEventEndPoints();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<LayoutService>();

builder.Services.AddTransient<SharedHelper>();
builder.Services.AddTransient<CreateEventService>();
builder.Services.AddTransient<EditEventService>();
builder.Services.AddTransient<DeleteEventService>();
builder.Services.AddTransient<ViewCreatedEventsService>();
builder.Services.AddTransient<DiscoverEventsService>();
builder.Services.AddTransient<ViewEventService>();
    

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
        options.ClientSecret = builder.Configuration["Google:ClientSecret"] ??  string.Empty;
    });


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

app.MapStaticAssets();

//Authentication Endpoints
app.MapAuthenticationEndPoints();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

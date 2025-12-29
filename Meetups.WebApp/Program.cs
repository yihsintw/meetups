using Microsoft.EntityFrameworkCore;
using Meetups.WebApp.Shared;
using Meetups.WebApp.Data;
using Meetups.WebApp.Features.ViewCreatedEvents;
using Meetups.WebApp.Features.CreateEvent;
using Meetups.WebApp.Features.EditEvent;
using Meetups.WebApp.Features.DeleteEvent;
using Meetups.WebApp.Features.DiscoverEvents;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<SharedHelper>();
builder.Services.AddTransient<CreateEventService>();
builder.Services.AddTransient<EditEventService>();
builder.Services.AddTransient<DeleteEventService>();
builder.Services.AddTransient<ViewCreatedEventsService>();
builder.Services.AddTransient<DiscoverEventsService>();

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

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

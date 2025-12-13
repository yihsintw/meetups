using Microsoft.EntityFrameworkCore;
using Meetups.WebApp.Features.Events.CreateEvent;
using Meetups.WebApp.Shared;
using Meetups.WebApp.Data;
using Meetups.WebApp.Features.Events.ViewCreatedEvents;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<CreateEventService>();
builder.Services.AddTransient<ViewCreatedEventsService>();

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

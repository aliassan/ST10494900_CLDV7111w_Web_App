using Azure.Identity;
using Azure.Storage.Blobs;
using EventEase.Context;
using EventEase.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton(x => new BlobServiceClient(
    builder.Configuration.GetValue<string>("AzureBlobStorage:ConnectionString")
));

builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

builder.Services.AddScoped<IVenueAvailabilityService, VenueAvailabilityService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
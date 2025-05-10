using Azure.Identity;
using Azure.Storage.Blobs;
using EventEase.Context;
using EventEase.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// For Azure AD auth:
builder.Services.AddSingleton(x => new BlobServiceClient(
    new Uri("https://st10494900.blob.core.windows.net"),
    new DefaultAzureCredential()));

builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

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
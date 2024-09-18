using Microsoft.EntityFrameworkCore;
using BookStoreRazor.Data;
using RestSharp;
using BookStoreRazor.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSingleton<RestClient>(new RestClient("https://localhost:7180/"));

// Register the ApiDbContext service here, before calling builder.Build()
builder.Services.AddDbContext<ApiDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}
app.UseMiddleware<AccessDeniedMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

using PadillaEmpleosParteFofo.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<empleoDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BDConnection")));
/*
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<empleoDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Damian")));
*/
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

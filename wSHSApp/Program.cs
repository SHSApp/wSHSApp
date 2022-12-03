using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using wSHSApp.Areas.Identity;
using Microsoft.AspNetCore.Builder;
using System.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using wSHSApp.Data;
using wSHSApp.Models;
using wSHSApp.Areas.Identity.Data;
using wSHSApp.Reports;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbConnectionString") ?? throw new InvalidOperationException("Connection string not found.");
var akusConnectionPath = builder.Configuration.GetConnectionString("AkusDbPath") ?? throw new InvalidOperationException("AKUS Database path not found.");
var recordsConnectionString = builder.Configuration.GetConnectionString("RecordsDbConnectionString") ?? throw new InvalidOperationException("Connection string not found.");

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
builder.Services.AddDbContext<RecordsDbContext>(options => options.UseSqlite(recordsConnectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddIdentity<AkusUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 5;
    options.Password.RequiredUniqueChars = 0;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-.";
    options.User.RequireUniqueEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedEmail = false;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultUI().AddDefaultTokenProviders();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<AkusUser>>();
builder.Services.AddScoped<AppData>();
builder.Services.AddSingleton<CommonInfoService>();
builder.Services.AddSingleton<ExtraInfoService>();
builder.Services.AddSingleton<AdditionalInfoService>();
builder.Services.AddScoped<BrowserService>();
builder.Services.AddSingleton<StatisticInfoService>();
builder.Services.AddScoped<IReport, LettersReport>();

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

Tools.DataBasePath = akusConnectionPath;

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(Tools.DataBasePath, "Foto")),
    RequestPath = "/Foto"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(app.Environment.ContentRootPath, "Reports/Output")),
    RequestPath = "/Documents"
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

AkusConnect.Init();
AkusService.Init();
//File.

app.Run();

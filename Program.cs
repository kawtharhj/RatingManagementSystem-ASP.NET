using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using RatingManagementSystem.Data;
using RatingManagementSystem.Data.Models;
using RatingManagementSystem.Services;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<RatingSystemDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);


builder.Services.AddIdentity<UserData, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<RatingSystemDbContext>();

builder.Services.AddTransient<IEmailSender,EmailSender>();

builder.Services.AddScoped<AppRequestService>();


builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.

    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.Configure<EmailSetting>(
    builder.Configuration.GetSection("EmailSettings"));


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AccessApplication", policy =>
    {
        policy.RequireRole("Doctor");
        policy.RequireClaim("Major", "Informatics", "BioChemistry", "Chemistry",
                                     "Biology", "Geology", "Statistics",
                                     "Math", "Physics", "Electronic");
    });
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();


app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
    RequestPath = "/Uploads"
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

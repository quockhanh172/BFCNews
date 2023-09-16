using BFCNews.Controllers;
using BFCNews.Data;
using BFCNews.Service;
using BinhdienNews.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Set the access denied path to the custom page or URL
    options.AccessDeniedPath = "/Error/AccessDenied"; // Replace with the desired path
    options.LoginPath = "/User/Login";
});

//build policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOrSuperAdmin", policy => 
    policy.RequireAssertion(context =>
        context.User.IsInRole("SuperAdmin")||
        context.User.IsInRole("Admin")
    ));
    options.AddPolicy("VipManager", policy =>
         policy.RequireAssertion(context =>
         context.User.HasClaim(c =>c.Type == "Permission" && c.Value == "Level1") ||
         context.User.IsInRole("SuperAdmin") || context.User.IsInRole("Admin")
         )) ;
    options.AddPolicy("DeputyCEO", policy =>
        policy.RequireAssertion(context=>
        context.User.HasClaim(c => c.Value == "Level2" && c.Type == "Permission") ||
        context.User.IsInRole("SuperAdmin") || context.User.IsInRole("Admin")
         ));
    options.AddPolicy("DHD", policy =>
       policy.RequireAssertion(context =>
        context.User.HasClaim(c => c.Value == "Level3" && c.Type == "Permission") ||
        context.User.IsInRole("SuperAdmin") || context.User.IsInRole("Admin")
         ));
    options.AddPolicy("Employee", policy =>
       policy.RequireAssertion(context =>
        context.User.HasClaim(c => c.Value == "Level4" && c.Type == "Permission") ||
        context.User.IsInRole("SuperAdmin")||context.User.IsInRole("Admin")
       ));
});

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default SignIn settings.
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
});
builder.Services.AddScoped<IFileService,FileService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddMvc();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error/AccessDenied");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

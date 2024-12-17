using AspNetCoreHero.ToastNotification;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using RPInventories.Data;
using RPInventories.Helpers;
using RPInventories.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Profiles", "Administrators");
    options.Conventions.AuthorizeFolder("/Users", "Administrators");
    options.Conventions.AuthorizeFolder("/Departments", "Administrators");
    options.Conventions.AuthorizeFolder("/Brands", "CompanyEmployees");
    options.Conventions.AuthorizeFolder("/Products", "Organization");
});
// Auth policy's
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrators", policy => 
        policy.RequireRole("Admin"));
    
    options.AddPolicy("CompanyEmployees", policy => 
        policy.RequireRole("Admin", "Employee"));
    
    options.AddPolicy("Organization", policy => 
        policy.RequireRole("Admin", "Employee", "Guest"));
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/AccessDenied";
        options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
        options.SlidingExpiration = true;
    });

builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.PageViewLocationFormats.Add("/Pages/Partials/{0}" + RazorViewEngine.ViewExtension);
});
// toast notification
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5;
    config.IsDismissable = true;
    config.Position = NotyfPosition.BottomRight;
});
// DB Context
builder.Services.AddDbContext<InventoriesContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("InventoriesContext") ?? throw new InvalidOperationException("Connection string 'InventoriesContext' not found.")));

builder.Services.AddSingleton<FactoryUser>();
builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<InventoriesContext>();
    // context.Database.EnsureCreated(); // Only for testing, when migrations and data persistence are disabled
    DbInitializer.Initialize(context);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
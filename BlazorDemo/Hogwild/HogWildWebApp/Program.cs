using HogWildSystem;
using HogWildWebApp.Areas.Identity;
using HogWildWebApp.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//  :given (This is code that is provided when we create our application)
//  supplied database connection due to the fact that we created this
//      web app to use Individual accounts
//  Core retrieves the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

//  :added
//  code retrieves the HogWild connection string
var connectionStringHogWild = builder.Configuration.GetConnectionString("OLTP-DMIT2018");

//  :given
//  register the supplied connections string with the IServiceCollection (.Services)
//  Register the connection string for individual accounts
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

//  added:
//  Code the logic to add our class library services to IServiceCollection
//  One could do the registration code here in Program.cs
//  HOWEVER, every time a service class is added, you would be changing this file
//  The implementation of the DBContent and AddTransient(...) code in this example
//        will be done in an extension method to IServiceCollection
//  The extension method will be code inside the HogWildSystem class library
//  The extension method will have a paramater: options.UseSqlServer()
builder.Services.AddBackendDependencies(options =>
    options.UseSqlServer(connectionStringHogWild));

builder.Services.AddMudServices();
builder.Services.AddBlazorDialog();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

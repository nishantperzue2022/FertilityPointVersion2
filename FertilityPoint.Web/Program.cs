using FertilityPoint.BLL.Repositories.ApplicationUserModule;
using FertilityPoint.BLL.Repositories.AppointmentModule;
using FertilityPoint.BLL.Repositories.CountyModule;
using FertilityPoint.BLL.Repositories.MpesaStkModule;
using FertilityPoint.BLL.Repositories.PatientModule;
using FertilityPoint.BLL.Repositories.ServiceModule;
using FertilityPoint.BLL.Repositories.SpecialityModule;
using FertilityPoint.BLL.Repositories.SubCountyModule;
using FertilityPoint.BLL.Repositories.TimeSlotModule;
using FertilityPoint.DAL.MapperProfiles;
using FertilityPoint.DAL.Modules;
using FertilityPoint.Web.Extensions;
using FertilityPoint.SeedAppUsers;
using FertilityPoint.Services.EmailModule;

using FertilityPoint.Services.SMSModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FertilityPoint.BLL.Repositories.EnquiryModule;
using FertilityPoint.BLL.Repositories.PayPalModule;
using FertilityPoint.Web.Helpers;
using FertilityPoint.Chat.VideoChart;

using FertilityPoint.Chat.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>

options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);

builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddTransient<IUserClaimsPrincipalFactory<AppUser>, ApplicationUserClaimsPrincipalFactory>();

builder.Services.AddTransient<IMailService, MailService>();

builder.Services.AddTransient<IAppointmentRepository, AppointmentRepository>();

builder.Services.AddTransient<ICountyRepository, CountyRepository>();

builder.Services.AddTransient<ISubCountyRepository, SubCountyRepository>();

builder.Services.AddTransient<ISpecialityRepository, SpecialityRepository>();

builder.Services.AddTransient<IPaymentRepository, PaymentRepository>();

builder.Services.AddTransient<IApplicationUserRepository, ApplicationUserRepository>();

builder.Services.AddTransient<ITimeSlotRepository, TimeSlotRepository>();

builder.Services.AddTransient<IPatientRepository, PatientRepository>();

builder.Services.AddTransient<IMessagingService, MessagingService>();

builder.Services.AddTransient<IServicesRepository, ServicesRepository>();

builder.Services.AddTransient<IEnquiryRepository, EnquiryRepository>();

builder.Services.AddTransient<IPayPalRepository, PayPalRepository>();

builder.Services.AddSingleton<List<User>>();

builder.Services.AddSingleton<List<UserCall>>();

builder.Services.AddSingleton<List<CallOffer>>();


//Cross-origin policy to accept request from localhost:8084.
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",

        x => x.AllowAnyOrigin()

            .AllowAnyMethod()

            .AllowAnyHeader());
});



builder.Services.AddSignalR();


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

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles();

app.UseFileServer();

app.UseCors("CorsPolicy");


app.MapHub<SignalrServer>("/signalrServer");

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<VideoHub>("/VideoHub");

    endpoints.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
});

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

using (var scope = scopeFactory.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    SeedUsers.Seed(userManager, roleManager);
}


app.Run();

using C2108G1_Project_3.Controllers;
using C2108G1_Project_3.Data;
using C2108G1_Project_3.JOB;
using C2108G1_Project_3.Models;
using C2108G1_Project_3.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Impl;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<Users, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IBooksRepository, BooksRepository>();
builder.Services.AddScoped<IFlavorsRepostiory, FlavorsRepository>();
builder.Services.AddScoped<IRegisterUsersRepository, RegisterUsersRepository>();
builder.Services.AddScoped<IOrdersrepository, OrdersRepository>();
builder.Services.AddScoped<IUsersRepository<Users>, UsersRepository<Users>>();
builder.Services.AddScoped<IRecipesRepository, RecipesRepository>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
});
builder.Services.AddSwaggerGen(options =>
{
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});
var app = builder.Build();
builder.Services.AddControllersWithViews();

async Task ConfigureQuartzScheduler()
{
    // Tạo một đối tượng Scheduler
    ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
    IScheduler scheduler = await schedulerFactory.GetScheduler();

    // Khởi động Scheduler
    await scheduler.Start();

    // Tạo một công việc
    IJobDetail job = JobBuilder.Create<UpdateRoleJob>()
        .WithIdentity("UpdateRole", "YourJobGroup")
        .Build();

    // Tạo một Trigger để chạy công việc mỗi ngày lúc 12:00 PM
    ITrigger trigger = TriggerBuilder.Create()
        .WithIdentity("YourTriggerName", "YourTriggerGroup")
        .WithSimpleSchedule(x => x.WithIntervalInSeconds(5).RepeatForever())
        .Build();

    // Lên lịch công việc với Trigger
    await scheduler.ScheduleJob(job, trigger);
}
// Đường dẫn cho trang đăng nhập
//builder.Services.AddMvc().AddRazorPagesOptions(options =>
//{
//    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Login");
//});
//builder.Services.AddMvc().AddRazorPagesOptions(options =>
//{
//    options.Conventions.AuthorizeAreaFolder("Admin", "/");
//});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//        name: "admin",
//        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
//    );
//});
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Art Institute");
});
await ConfigureQuartzScheduler();
app.Run();





//abc adsdsdsdas => aspnetuser

//=> regiuser abc 
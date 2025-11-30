using GymManagementBLL.Mapping;
using GymManagementBLL.Services.AttachmentServices;
using GymManagementBLL.Services.Implementation;
using GymManagementBLL.Services.Interface;
using GymManagementDAL.Data.Context;
using GymManagementDAL.Data.DataSeed;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Implementation;
using GymManagementDAL.Repositories.Interfaces;
using GymManagementDAL.Repositories.UnitOfWorks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<GymDbContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"));
});


builder.Services.AddScoped<IMemberService,MemberService>();
builder.Services.AddScoped<IPlanService,PlanService>();
builder.Services.AddScoped<IAnalyticsService,AnalyticsService>();
builder.Services.AddScoped<ITrainerService,TrainerService>();
builder.Services.AddScoped<ISessionService,SessionService>();
builder.Services.AddScoped<IMemberPlanService,MemberPlanService>();
builder.Services.AddScoped<IMemberSessionService,MemberSessionService>();
builder.Services.AddScoped<IAttachmentService,AttachmentService>();
builder.Services.AddScoped<IAccountService,AccountService>();

builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<IMemberPlanRepository,MemberPlanRepository>();
builder.Services.AddScoped<IMemberSessionRepository,MemberSessionRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();

builder.Services.AddAutoMapper(Mapping => Mapping.AddProfile(new MappingProfile()));

// ===== Identity Configuration =====
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(Config =>
{
    Config.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<GymDbContext>();

builder.Services.ConfigureApplicationCookie(options => 
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();

GymDbContextSeeding.SeedData(app.Services.CreateScope().ServiceProvider.GetRequiredService<GymDbContext>());

// ===== SeedData Identity =====
var Scope = app.Services.CreateScope();
var RoleManager = Scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var UserManager = Scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
IdentityDbContextSeeding.SeedData(RoleManager, UserManager);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();

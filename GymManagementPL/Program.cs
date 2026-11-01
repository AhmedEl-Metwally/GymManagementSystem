var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddGymDbContext(builder.Configuration);
builder.Services.AddGymRepositories();
builder.Services.AddGymApplicationServices();
builder.Services.AddGymAutoMapper();
builder.Services.AddFunctionFilters();
builder.Services.AddGymAssistant(builder.Configuration);
builder.Services.AddGymIdentity();

var app = builder.Build();

app.SeedGymData();
app.UseGymManagementPipeline();

app.Run();

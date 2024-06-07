using AutoMapper;
using Forums.BusinessLogic;
using Forums.BusinessLogic.DBModel;
using Forums.BusinessLogic.Interfaces;
using Forums.Domain.Entities.User;
using Forums.Web.Filters;
using Forums.Web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new ServiceFilterAttribute(typeof(AuthorisedActionFilter))); // Use typeof() to specify the filter type
    options.Filters.Add(new ServiceFilterAttribute(typeof(AdminActionFilter))); // Use typeof() to specify the filter type
});

// Register the filters as services
builder.Services.AddScoped<AuthorisedActionFilter>();
builder.Services.AddScoped<AdminActionFilter>();
builder.Services.AddRazorPages();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<UDbTable, UserMinimal>();
    cfg.CreateMap<ULoginData, UserLogin>();
    cfg.CreateMap<URegisterData, UserRegister>();
});

// Register DbContexts 
builder.Services.AddDbContext<UserContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<SessionContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register ISession, IUser
builder.Services.AddScoped<Forums.BusinessLogic.Interfaces.ISession, SessionBL>();
builder.Services.AddScoped<IUser, UserBL>();

// Register BusinessLogic 
builder.Services.AddScoped<BusinessLogic>();

// Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Enable session
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=HomePage}/{id?}");
app.MapRazorPages();

app.Run();

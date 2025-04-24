using Autofac.Extensions.DependencyInjection;
using Autofac;
using CampusTradingPlatform.AutofaceDI;
using EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CampusTradingPlatform.Email;
using CampusTradingPlatform.Filters;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 注册 DbContext
var connStr = builder.Configuration.GetConnectionString("StrConn");
builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("StrConn"));
}, contextLifetime: ServiceLifetime.Scoped);
builder.Services.Configure<MvcOptions>(opts =>
{
    opts.Filters.Add<UnitOfWorkFilter>();
    opts.Filters.Add<CheckSessionFilter>();
});
//builder.Services.AddScoped<IUserInfoRepository, UserInfoRepository>();
//builder.Services.AddScoped<IUserInfoService, UserInfoService>();
// 替换容器，初始化一个Autofac的示例。
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new AutofacModuleRegister());
});
builder.Services.AddSession();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<MailQueueManager>();
var app = builder.Build();
app.Services.GetService<MailQueueManager>()!.Run();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

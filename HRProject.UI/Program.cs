using HRProject.Repositories.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<HRProjectContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    option.AccessDeniedPath = "/Home/ErisimEngellendi";//Yetkisiz kişiler girmek istediğinde bu actiona yönlendirilir.
    option.ExpireTimeSpan = TimeSpan.FromDays(30); //Cookinin ne zama sona ereceği
    option.SlidingExpiration = true; //Ötelemeli zaman aşımı aktif
    option.LoginPath = "/Home/Login";



    //option.ReturnUrlParameter = "returnUrl";
    option.Events.OnRedirectToLogin = context =>
    {
        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };



});

//AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

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

app.UseAuthentication(); // eklendi
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
        name: "SiteManagement",
        areaName: "SiteManagement",
        pattern: "SiteManagement/{controller=SiteManagement}/{action=Index}/{id?}"
        );
    //endpoints.MapAreaControllerRoute(
    //   name: "SiteManagement",
    //   areaName: "SiteManagement",
    //   pattern: "SiteManagement/{controller=Company}/{action=Index}/{id?}"
    //   );

    endpoints.MapAreaControllerRoute(
        name: "Users",
        areaName: "Users",
        pattern: "Users/{controller=Account}/{action=Index}/{id?}"
        );

    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");
});


app.Run();

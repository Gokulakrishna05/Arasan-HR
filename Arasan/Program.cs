using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Interface.Master;
using Arasan.Services.Master;
using Arasan.Controllers.Master;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Host.ConfigureServices(services =>
        {
            services.TryAddSingleton<IBranchService, BranchService>();
        });
        builder.Services.TryAddSingleton<IBranchService, BranchService>();
        builder.Services.TryAddSingleton<ILoginService, LoginService>();
        builder.Services.TryAddSingleton<ICityService, CityService>();
        builder.Services.TryAddSingleton<IExchangeRateService, ExchangeRateService>();
        builder.Services.TryAddSingleton<IItemGroupService, ItemGroupService>();


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

        app.UseAuthorization();


        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Account}/{action=Login}/{id?}");
        app.Run();
    }
}
using Microsoft.Extensions.DependencyInjection.Extensions;
using Arasan.Interface;
using Arasan.Services;

using Arasan.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Hosting;
using Arasan.Controllers;
using Arasan.Interface.Report;
using Arasan.Services.Report;




//using Arasan.Services.Store_Management;



internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
       
        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Host.ConfigureServices(services =>
        {
        });
        builder.Services.TryAddSingleton<ILoginService, LoginService>();
       
        builder.Services.TryAddSingleton<IBranchSelectionService, BranchSelectionService>();
       
        builder.Services.TryAddSingleton<IHomeService, HomeService>();
       
        builder.Services.TryAddSingleton<IMailService, MailService>();
     
        //builder.Services.TryAddSingleton<IAccGroup, AccGroupService>();
       
        builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

      
        builder.Services.TryAddSingleton<ISectionService, SectionService>();

       
        builder.Services.TryAddSingleton<IQcDashboardService, QcDashboardService>();


        builder.Services.TryAddSingleton<IEmployeeAttendanceDetails, EmployeeAttendanceDetailsService>();


        builder.Services.TryAddSingleton<IEmpShiftSchedule, EmpShiftScheduleService>();

        builder.Services.TryAddSingleton<IResignation, ResignationService>();
        builder.Services.TryAddSingleton<IEmpLoginDet, EmpLoginDetService>();
        builder.Services.TryAddSingleton<IJournalVoucher, JournalVoucherService>();
        builder.Services.TryAddSingleton<IServicePO, ServicePOService>();
        builder.Services.TryAddSingleton<IPendingIndentApprove, PendingIndentApproveService>();
        builder.Services.TryAddSingleton<IAssetStock, AssetStockService>();
        builder.Services.TryAddSingleton<IStockStatementDrumwise, StockStatementDrumwiseService>();
         

        //builder.Services.TryAddSingleton<ISubContractingMaterialReceipt, SubContractingMaterialReceiptService>();











        builder.Services.AddSession();
        var emailConfig = builder.Configuration.GetSection("MailSettings").Get<MailSettings>();
        builder.Services.TryAddSingleton(emailConfig);

        builder.Services.AddControllers();
        builder.Services.Configure<FormOptions>(x =>
        {
            x.ValueCountLimit = int.MaxValue;
        });
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
        //DirectoryInfo di = new DirectoryInfo(@"~/pdfdownload");
        //app.UseReporting(settings =>
        //{
        //    //settings.UseFileStore(new System.IO.DirectoryInfo(IWebHostEnvironment.ContentRootPath + @"\pdfdownload\"));
        //   settings.UseFileStore(di);
        //    settings.UseCompression = true;
        //});
        app.UseRouting();
       

        app.UseAuthorization();
        app.UseSession();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Account}/{action=Login}/{fid?}");


      
        app.Run();
    }
    
    public void ConfigureServices(IServiceCollection service)
    {
        service.AddMvc();
        service.AddOptions();
      
        service.AddControllersWithViews().AddRazorRuntimeCompilation();
    }
    public void Configure(IApplicationBuilder app,IWebHostEnvironment env)
    {
        if(env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

        }
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "defalut",
                pattern: "{controller=Report}/(action=Print}/{id?})");
        });

    }
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
            webBuilder.UseWebRoot("wwwroot");
            webBuilder.UseStartup<IStartup>();

        });

    [Obsolete]
    public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
    {
        // Other configurations...

        app.UseStaticFiles(); // Enable static file serving, e.g., for wwwroot folder

        // More configurations...
    }
}
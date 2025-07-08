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
using Arasan.Services.Sales_Export;
using Arasan.Interface.Master;
using Arasan.Services.Master;
using Arasan.Interface.Transaction;
using Arasan.Models.Transaction;
using Arasan.Services.Transaction;




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
        builder.Services.TryAddSingleton<ITrialBalanceReport, TrialBalanceReportService>();





        builder.Services.TryAddSingleton<IBatchReportService, BatchReportService>();
        builder.Services.TryAddSingleton<IDirectPurchaseReportService, DirectPurchaseReportService>();
        builder.Services.TryAddSingleton<IGRNReportService, GRNReportService>();
        builder.Services.TryAddSingleton<IPurchasePend, PurchasePendService>();
        builder.Services.TryAddSingleton<IPurchaseRepItemReportService, PurchaseRepItemReportService>();
        builder.Services.TryAddSingleton<IPurchaseReportService, PurchaseReportService>();
        builder.Services.TryAddSingleton<IPurchaseRepPartyReportService, PurchaseRepPartyReportService>();
        builder.Services.TryAddSingleton<IPurMonReport, PurMonReportService>();
        builder.Services.TryAddSingleton<IReceiptReport, ReceiptReportService>();
        builder.Services.TryAddSingleton<IWorkCenterReportService, WorkCenterReportService>();
        builder.Services.TryAddSingleton<IBalanceSheet, BalanceSheetService>();
        builder.Services.TryAddSingleton<IReceiptAgainstReturnableGoods, ReceiptAgainstReturnableGoodsService>();
        builder.Services.TryAddSingleton<ICuringShedStockPyro, CuringShedStockPyroService>();



        builder.Services.TryAddSingleton<IActivitiesCO, ActivitiesCOService>();
        builder.Services.TryAddSingleton<IActPlanning, ActPlanningService>();
        builder.Services.TryAddSingleton<IFGDailyStockReport, FGDailyStockReportService>();
        builder.Services.TryAddSingleton<IOnDuty, OnDutyService>();
        builder.Services.TryAddSingleton<IPayTransaction, PayTransactionService>();
        builder.Services.TryAddSingleton<IStockStatement, StockStatementService>();
        builder.Services.TryAddSingleton<IStockStatementLotwise, StockStatementLotwiseService>();
        builder.Services.TryAddSingleton<ISubContractingMonthwiseReport, SubContractingMonthwiseReportService>();
        builder.Services.TryAddSingleton<ISubContractingReport, SubContractingReportService>();

        builder.Services.TryAddSingleton<IFGStockDetailReport, FGStockDetailReportService>();
        builder.Services.TryAddSingleton<IOpeningSql, OpeningSqlService>();
        builder.Services.TryAddSingleton<IItemsToBeReceived, ItemsToBeReceivedService>();
        builder.Services.TryAddSingleton<IFinishedGoodsStockDetailsDrumwise, FinishedGoodsStockDetailsDrumwiseService>();
        builder.Services.TryAddSingleton<IApsIdleStock, ApsIdleStockService>();
        builder.Services.TryAddSingleton<IAPPowderStockInPyro, APPowderStockInPyroService>();
        builder.Services.TryAddSingleton<IAPSDailyReport, APSDailyReportService>();
        builder.Services.TryAddSingleton<IRVDPowderStockInPaste, RVDPowderStockInPasteService>();
        builder.Services.TryAddSingleton<ICakeStockInPaste, CakeStockInPasteService>();
        builder.Services.TryAddSingleton<IPasteStockInMixing, PasteStockInMixingService>();
        builder.Services.TryAddSingleton<IBatchProductionSummary, BatchProductionSummaryService>();
        builder.Services.TryAddSingleton<ICuringShedStockPolish, CuringShedStockPolishService>();
        builder.Services.TryAddSingleton<ICuringShedStockPyroPolished, CuringShedStockPyroPolishedService>();
        builder.Services.TryAddSingleton<IFGStockSivakasiDepot, FGStockSivakasiDepotService>();
        builder.Services.TryAddSingleton<IPurchaseIndentPendingForApproval, PurchaseIndentPendingForApprovalService>();
        builder.Services.TryAddSingleton<IRVDPowderStockInPolish, RVDPowderStockInPolishService>();
        builder.Services.TryAddSingleton<IPendingPaymentOrReceiptBillWise, PendingPaymentOrReceiptBillWiseService>();
        builder.Services.TryAddSingleton<IFGBalancePowderStock, FGBalancePowderStockService>();
        builder.Services.TryAddSingleton<IBatchProductionSummaryReportBatchwise, BatchProductionSummaryReportBatchwiseService>();
        builder.Services.TryAddSingleton<IExportEnquiry, ExportEnquiryService>();
        builder.Services.TryAddSingleton<IEnquiryQuotation, EnquiryQuotationService>();
        builder.Services.TryAddSingleton<IExportWorkOrder, ExportWorkOrderService>();
        builder.Services.TryAddSingleton<IExportDC, ExportDCService>();
        builder.Services.TryAddSingleton<ILicence, LicenceService>();
        builder.Services.TryAddSingleton<IWorkOrder, ExWorkOrderCloseService>();
        builder.Services.TryAddSingleton<ICustomerComplaint, CustomerComplaintService>();
        builder.Services.TryAddSingleton<IExportInvoice, ExportInvoiceService>();
        builder.Services.TryAddSingleton<ILeaveMaster, LeaveMasterService>();
        builder.Services.TryAddSingleton<ILeaveTypeMaster, LeaveTypeMasterService>();
        builder.Services.TryAddSingleton<ILeaveRequest, LeaveRequestService>();
        builder.Services.TryAddSingleton<IPermissions, PermissionsService>();
        builder.Services.TryAddSingleton<IAllowanceMaster, AllowanceMasterService>();
        builder.Services.TryAddSingleton<IAssignAllowance, AssignAllowanceService>();
        builder.Services.TryAddSingleton<IAttendanceReport, AttendanceReportService>();
        builder.Services.TryAddSingleton<ISalaryStructure, SalaryStructureService>();
        builder.Services.TryAddSingleton<IHolidayMaster, HolidayMasterService>();
        builder.Services.TryAddSingleton<IPermissions, PermissionsService>();
        builder.Services.TryAddSingleton<IBonusMaster, BonusMasterService>();
        builder.Services.TryAddSingleton<IAdvanceTM, AdvanceTMService>();
        builder.Services.TryAddSingleton<IOTEntry, OTEntryService>();

        builder.Services.TryAddSingleton<IPayPeriodService, PayPeriodService>();

        builder.Services.TryAddSingleton<IDepartment, DepartmentService>();


        builder.Services.TryAddSingleton<IPrivilegesService, PrivilegesService>();
        builder.Services.TryAddSingleton<IDesignation, DesignationService>();
        builder.Services.TryAddSingleton<IPaycodeService, PaycodeService>();

        builder.Services.TryAddSingleton<IPayCategory, PayCategoryService>();

        builder.Services.TryAddSingleton<IEmployee, EmployeeService>();
        builder.Services.TryAddSingleton<IShift, ShiftService>();
        builder.Services.TryAddSingleton<IMissingPunchEntry, MissingPunchEntryService>();








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
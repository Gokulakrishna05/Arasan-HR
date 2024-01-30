using Microsoft.Extensions.DependencyInjection.Extensions;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Interface.Master;
using Arasan.Services.Master;
using Arasan.Interface.Production;
using Arasan.Services.Production;
using Arasan.Models;
using Arasan.Interface.Store_Management;
using Arasan.Services.Store_Management;
using Arasan.Services.Qualitycontrol;
using Arasan.Interface.Qualitycontrol;
using Arasan.Interface.Stores_Management;
using Arasan.Interface.Sales;
using Arasan.Services.Sales;
using System.Collections.Generic;
using Arasan.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Hosting;
using Arasan.Interface.Account;
using Arasan.Services.Report;
using Arasan.Interface.Report;



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
            services.TryAddSingleton<IBranchService, BranchService>();
        });
        builder.Services.TryAddSingleton<IBranchService, BranchService>();
        builder.Services.TryAddSingleton<ILoginService, LoginService>();
        builder.Services.TryAddSingleton<ICityService, CityService>();
        builder.Services.TryAddSingleton<IExchangeRateService, ExchangeRateService>();
        builder.Services.TryAddSingleton<IItemGroupService, ItemGroupService>();
        builder.Services.TryAddSingleton<IItemSubGroupService, ItemSubGroupService>();
        builder.Services.TryAddSingleton<IProcessCostEntryService, ProcessCostEntryService>();
        builder.Services.TryAddSingleton<IDirectDeductionService, DirectDeductionService>();
        builder.Services.TryAddSingleton<IBranchSelectionService, BranchSelectionService>();
        builder.Services.TryAddSingleton<IStockService, StockService>();
        builder.Services.TryAddSingleton<IPurchaseEnqService, PurchaseEnqService>();
        builder.Services.TryAddSingleton<IHomeService, HomeService>();
        builder.Services.TryAddSingleton<IPurchaseEnqService, PurchaseEnqService>();
        builder.Services.TryAddSingleton<IItemTransferService, ItemTransferService>();
        builder.Services.TryAddSingleton<IQCResultService, QCResultService>();
        builder.Services.TryAddSingleton<ISalesEnq, SalesEnqService>();

        builder.Services.TryAddSingleton<IItemNameService, ItemNameService>();
        builder.Services.TryAddSingleton<IItemCategoryService, ItemCategoryService>();
        builder.Services.TryAddSingleton<IPO, POService>();
        builder.Services.TryAddSingleton<IGRN, GRNService>();
        builder.Services.TryAddSingleton<ICompanyService, CompanyService>();
        builder.Services.TryAddSingleton<ICompanyService, CompanyService>();
        builder.Services.TryAddSingleton<ICountryService, CountryService>();
        builder.Services.TryAddSingleton<IStateService, StateService>();
        builder.Services.TryAddSingleton<ICurrencyService, CurrencyService>();
        builder.Services.TryAddSingleton<IUnitService, UnitService>();
        builder.Services.TryAddSingleton<IQCTestingService, QCTestingService>();
        builder.Services.TryAddSingleton<IHSNcodeService, HSNcodeService>();
        builder.Services.TryAddSingleton<ITaxService, TaxService>();
        builder.Services.TryAddSingleton<IPartyMasterService, PartyMasterService>();
        builder.Services.TryAddSingleton<ILocationService, LocationService>();
        builder.Services.TryAddSingleton<IMaterialRequisition, MaterialRequisitionService>();
        builder.Services.TryAddSingleton<IStoreAccService, StoreAccService>();
        builder.Services.TryAddSingleton<IPurchaseIndent, PurchaseIndentService>();
        builder.Services.TryAddSingleton<IPurchaseQuo, PurchaseQuoService>();
        builder.Services.TryAddSingleton<IMailService, MailService>();
        builder.Services.TryAddSingleton<IDirectPurchase, DirectPurchaseService>();
        builder.Services.TryAddSingleton<IStoreIssueConsumables, StoreIssueConsumablesService>();
        builder.Services.TryAddSingleton<IStoreIssueProduction, StoreIssueProductionService>();
        builder.Services.TryAddSingleton<IStoreIssueProduction, StoreIssueProductionService>();
        builder.Services.TryAddSingleton<IDirectAddition, DirectAdditionService>();
        builder.Services.TryAddSingleton<IEmployee, EmployeeService>();
        builder.Services.TryAddSingleton<IStockIn, StockInService>();
        builder.Services.TryAddSingleton<IPurchaseReturn, PurchaseReturnService>();
        builder.Services.TryAddSingleton<IQCTestValueEntryService, QCTestValueEntryService>();
        builder.Services.TryAddSingleton<IAccGroup, AccGroupService>();
        builder.Services.TryAddSingleton<IStoresReturnService, StoresReturnService>();
        builder.Services.TryAddSingleton<IReasonCodeService, ReasonCodeService>();

        builder.Services.TryAddSingleton<IProductionEntry, ProductionEntryService>();
        builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        builder.Services.TryAddSingleton<ISalesQuotationService, SalesQuotationService>();
        builder.Services.TryAddSingleton<ICustomerType, CustomerTypeService>();
        builder.Services.TryAddSingleton<IBatchCreation, BatchCreationService>();
        builder.Services.TryAddSingleton<IWorkOrderService, WorkOrderService>();

        builder.Services.TryAddSingleton<ICuringInwardService, CuringInwardService>();
        builder.Services.TryAddSingleton<IBatchProduction, BatchProductionService>();

        builder.Services.TryAddSingleton<IProductionScheduleService, ProductionScheduleService>();
        builder.Services.TryAddSingleton<IProductionLog, ProductionLogService>();
        builder.Services.TryAddSingleton<ICuringService, CuringService>();
        builder.Services.TryAddSingleton<IDrumIssueEntryService, DrumIssueEntryService>();

        builder.Services.TryAddSingleton<IPackingNote, PackingNoteService>();
        builder.Services.TryAddSingleton<ICuringOutward, CuringOutwardService>();
        builder.Services.TryAddSingleton<IQCFinalValueEntryService, QCFinalValueEntryService>();
        builder.Services.TryAddSingleton<IProductionForecastingService, ProductionForecastingService>();
        builder.Services.TryAddSingleton<ISectionService, SectionService>();

        builder.Services.TryAddSingleton<IDrumMaster, DrumMasterService>();
        builder.Services.TryAddSingleton<IDrumCategory, DrumCategoryService>();


        builder.Services.TryAddSingleton<IDrumCategory, DrumCategoryService>();
        builder.Services.TryAddSingleton<IWorkCentersService, WorkCentersService>();


        builder.Services.TryAddSingleton<IDrumLocation, DrumLocationService>();
        builder.Services.TryAddSingleton<IDepartment, DepartmentService>();


        builder.Services.TryAddSingleton<IAccTreeView, AccTreeviewService>();


        builder.Services.TryAddSingleton<ILedger, LedgerService>();
        builder.Services.TryAddSingleton<IPaymentVoucher, PaymentVoucherService>();
        builder.Services.TryAddSingleton<ISequence, SequenceService>();

        builder.Services.TryAddSingleton<IAccountGroup, AccountGroupService>();


        builder.Services.TryAddSingleton<IPaymentRequest, PaymentRequestService>();


        builder.Services.TryAddSingleton<IAccountType, AccountTypeService>();

        builder.Services.TryAddSingleton<IAccConfig, AccConfigService>();

        builder.Services.TryAddSingleton<IProFormaInvoiceService, ProFormaInvoiceService>();



        builder.Services.TryAddSingleton<IWorkOrderShortClose, WorkOrderShortCloseService>();

        builder.Services.TryAddSingleton<ISalesReturn, SalesReturnService>();

        builder.Services.TryAddSingleton<IDesignation, DesignationService>();

        builder.Services.TryAddSingleton<IDepotInvoiceService, DepotInvoiceService>();
		builder.Services.TryAddSingleton<IAPProductionEntry, APProductionentryService>();

		builder.Services.TryAddSingleton<IEmailConfig, EmailConfigService>();
		builder.Services.TryAddSingleton<IDebitNoteBillService, DebitNoteBillService>();
		builder.Services.TryAddSingleton<IEmpMultipleAllocationService, EmpMultipleAllocationService>();
		builder.Services.TryAddSingleton<IItemDescriptionService, ItemDescriptionService>();

        builder.Services.TryAddSingleton<IPyroProduction, PyroProductionService>();
        builder.Services.TryAddSingleton<IPromotionMailService, PromotionMailService>();

 
        builder.Services.TryAddSingleton<IIssueToProduction, IssueToProductionService>();
 
        builder.Services.TryAddSingleton<IQcTemplateService, QcTemplateService>();

        builder.Services.TryAddSingleton<IAccDescrip, AccDescripService>();
        builder.Services.TryAddSingleton<IQcDashboardService, QcDashboardService>();
        builder.Services.TryAddSingleton<IPackingQCFinalValueEntry, PackingQCFinalValueEntryService>();
        builder.Services.TryAddSingleton<IORSAT, ORSATService>();
        builder.Services.TryAddSingleton<IItemConversionEntryService,ItemConversionEntryService>();
        builder.Services.TryAddSingleton<IAccClass, AccClassService>();
        builder.Services.TryAddSingleton<IPackDrumAllocation, PackDrumAllocationService>();

        builder.Services.TryAddSingleton<IPackingEntry, PackingEntryService>();

        builder.Services.TryAddSingleton<IETariff, ETariffService>();
        builder.Services.TryAddSingleton<ICreditorDebitNote, CreditorDebitNoteService>();

        builder.Services.TryAddSingleton<ISubContractingDC, SubContractingDCService>();


        builder.Services.TryAddSingleton<IReceiptSubContract, ReceiptSubContractService>();

        builder.Services.TryAddSingleton<IRetNonRetDc, RetNonRetDcService>();
        builder.Services.TryAddSingleton<IAssetAddDed, AssetAddDedService>();
        builder.Services.TryAddSingleton<IContToAsset, ContToAssetService>();

        builder.Services.TryAddSingleton<IReceiptAgtRetDC, ReceiptAgtRetDCService>();


        builder.Services.TryAddSingleton<ISubContractingMaterialReceipt, SubContractingMaterialReceiptService>();
        builder.Services.TryAddSingleton<IAssetTransfer, AssetTransferService>();
        builder.Services.TryAddSingleton<ISalesTargetService, SalesTargetService>();
        builder.Services.TryAddSingleton<IPurchaseReportService, PurchaseReportService>();

        builder.Services.TryAddSingleton<IIPO, ImportPOService>();

        builder.Services.TryAddSingleton<IPurchaseImportIndent, PurchaseImportIndentService>();
        builder.Services.TryAddSingleton<IPurchaseImportEnqService, PurchaseImportEnqService>();
        builder.Services.TryAddSingleton<IPurchaseImportQuo, PurchaseImportQuoService>();

        builder.Services.TryAddSingleton<IStop, StopService>();

        builder.Services.TryAddSingleton<IRateCodeService, RateCodeService>();
        builder.Services.TryAddSingleton<IRateService, RateService>();

        builder.Services.TryAddSingleton<IGRNReportService, GRNReportService>();
        builder.Services.TryAddSingleton<IReceiptReport, ReceiptReportService>();

        builder.Services.TryAddSingleton<IDirectPurchaseReportService, DirectPurchaseReportService>();

        builder.Services.TryAddSingleton<IPurchasePend, PurchasePendService>();

        builder.Services.TryAddSingleton<IPurchaseRepItemReportService, PurchaseRepItemReportService>();
        builder.Services.TryAddSingleton<IPurchaseRepPartyReportService, PurchaseRepPartyReportService>();
        builder.Services.TryAddSingleton<IWorkCenterReportService, WorkCenterReportService>();

        builder.Services.TryAddSingleton<IPurMonReport, PurMonReportService>();
        builder.Services.TryAddSingleton<IWCSieveService, WCSieveService>();







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
        service.AddTransient<IPO, POService>();
        service.AddTransient<ISalesQuotationService, SalesQuotationService>();
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
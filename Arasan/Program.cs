using Microsoft.Extensions.DependencyInjection.Extensions;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Interface.Master;
using Arasan.Services.Master;
using Arasan.Interface.Production;
using Arasan.Services.Production;
using Arasan.Models;
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


        builder.Services.TryAddSingleton<IPurchaseEnqService,PurchaseEnqService>();
        builder.Services.TryAddSingleton<ISalesEnq, SalesEnqService>();
        builder.Services.TryAddSingleton<ICompanyService, CompanyService>();
        builder.Services.TryAddSingleton<ICompanyService, CompanyService>();
        builder.Services.TryAddSingleton<IItemNameService, ItemNameService>();
        builder.Services.TryAddSingleton<IItemCategoryService,ItemCategoryService>();
        builder.Services.TryAddSingleton<IPO, POService>();
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

        builder.Services.TryAddSingleton<IPurchaseIndent, PurchaseIndentService>();
        builder.Services.TryAddSingleton<IPurchaseQuo, PurchaseQuoService>();
        builder.Services.TryAddSingleton<IMailService,MailService>();
        builder.Services.TryAddSingleton<IDirectPurchase, DirectPurchaseService>();

        builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddSession();
        var emailConfig = builder.Configuration.GetSection("MailSettings").Get<MailSettings>();
        builder.Services.TryAddSingleton(emailConfig);

        builder.Services.AddControllers();
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
        app.UseSession();
        
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Account}/{action=Login}/{fid?}");
        app.Run();
    }
}
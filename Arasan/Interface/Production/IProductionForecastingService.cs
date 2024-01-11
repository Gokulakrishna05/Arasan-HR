using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
namespace Arasan.Interface.Production
{
    public interface IProductionForecastingService
    {
        IEnumerable<ProductionForecasting> GetAllProductionForecasting();
        DataTable GetPFDeatils(string id);
        DataTable GetProdForecastDetail(string id);
        DataTable GetProdForecastDGPasteDetail(string id);
        DataTable GetProdForecastPolishDetail(string id);
        DataTable GetProdForecastRVDDetail(string id);
        DataTable GetProdForecastPasteDetail(string id);
        DataTable GetAPSDeatils(string id);
        DataTable GetAPReqDeatils(string id);
        DataTable GetProdForecastPyroDetail(string id);
        DataTable GetProdForecastAPProdDetail(string id);
        DataTable GetProdForecastPackDetail(string id);
        DataTable GetAllProdFC(string id);
        DataTable GetWorkCenter();
        DataTable GetMnth();
        DataTable GetPYROWC();
        DataTable GetPolishWC();
        DataTable GetRVDWC();
        DataTable GetPasteWC();
        DataTable GetDGPaste(string mnth, string type);
        List<PFCPYROItem> GetPyroForecast(string mnth, string type); 
        List<PFCPOLIItem> GetPolishForecast(string mnth, string type); 
        List<PFCPASTEItem> GetPasteForecast(string mnth, string type); 
        List<PFCPACKItem> GetPackForecast(string mnth, string type); 
        string ProductionForecastingCRUD(ProductionForecasting cy);
    }
}

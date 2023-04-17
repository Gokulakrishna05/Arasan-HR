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
        DataTable GetProdForecastPyroDetail(string id);
        DataTable GetWorkCenter();
        string ProductionForecastingCRUD(ProductionForecasting cy);
    }
}

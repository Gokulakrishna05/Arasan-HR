using Arasan.Interface;
using Arasan.Models;

using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;
namespace Arasan.Services
{
    public class StockStatementLotwiseService : IStockStatementLotwise
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public StockStatementLotwiseService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }


        public DataTable GetAllStatementLotwise(string dtFrom)
        {

            try
            {
                string SvSql = "";

                SvSql = @"Select L.LocID , I.ItemID , Ls.LotNo , ( Sum(Ls.PlusQty)-Sum(Ls.MinusQty) ) Qty
                    From LStockValue Ls , ItemMaster I, LocDetails L
                    Where Ls.ItemID = I.ItemMasterID
                    And Ls.LocID = L.LocDetailsID
                    And Upper(I.LotYN) = 'YES'
                    And Upper(I.DrumYN) = 'NO'
                    And Ls.DocDate <= '" + dtFrom + "'";

                SvSql += @" Group By L.LocID , I.ItemID , Ls.LotNo
                    Having(Sum(Ls.PlusQty) - Sum(Ls.MinusQty)) <> 0
                    Union
                    Select L.LocID , I.ItemID , Ls.LotNo , (Sum(Ls.PlusQty) - Sum(Ls.MinusQty)) Qty
                    From PLStockValue Ls, ItemMaster I , LocDetails L
                    Where Ls.ItemID = I.ItemMasterID
                    And Ls.LocID = L.LocDetailsID
                    And Upper(I.LotYN) = 'YES'
                    And Upper(I.DrumYN) = 'NO'
                    And Ls.DocDate <= '" + dtFrom + "'";

                SvSql += @" Group By L.LocID , I.ItemID , Ls.LotNo
                    Having(Sum(Ls.PlusQty) - Sum(Ls.MinusQty)) <> 0
                    Order By ItemID , LotNo , LocID";

                OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
                DataTable dtReport = new DataTable();
                adapter.Fill(dtReport);
                return dtReport;

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

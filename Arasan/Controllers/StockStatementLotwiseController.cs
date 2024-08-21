using Microsoft.AspNetCore.Mvc;
using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using ClosedXML.Excel;
using Oracle.ManagedDataAccess.Client;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Elasticsearch.Net;
using Microsoft.VisualBasic;
using Nest;

namespace Arasan.Controllers
{
    public class StockStatementLotwiseController : Controller
    {
        IStockStatementLotwise stockStatementLotwise;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public StockStatementLotwiseController(IStockStatementLotwise _stockStatementLotwise, IConfiguration _configuratio)
        {
            stockStatementLotwise = _stockStatementLotwise;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult StockStatementLotwise(string strfrom)
        {
            try
            {
                StockStatementLotwise objR = new StockStatementLotwise();

                objR.dtFrom = strfrom;
                return View(objR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult MyListStatementLotwiseGrid(string dtFrom)
        {
            List<StockStatementLotwiseItems> Reg = new List<StockStatementLotwiseItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)stockStatementLotwise.GetAllStatementLotwise(dtFrom);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                Reg.Add(new StockStatementLotwiseItems
                {

                    lid = dtUsers.Rows[i]["LOCID"].ToString(),
                    iid = dtUsers.Rows[i]["ITEMID"].ToString(),
                    lno = dtUsers.Rows[i]["LOTNO"].ToString(),
                    qty = dtUsers.Rows[i]["QTY"].ToString(),
                    
                });
            }

            return Json(new
            {
                Reg
            });

        }

        public IActionResult ExportToExcel(string dtFrom)
        {
            DataTransactions _datatransactions;
            DataTable dtNew1 = new DataTable();

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
            using (DataTable dtReport = new DataTable())
            {
                adapter.Fill(dtReport);

                DataView dv1 = dtReport.DefaultView;
                dtNew1 = dv1.ToTable();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtNew1, "StockStatementLotwise");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=StockStatementLotwise.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "StockStatementLotwise.xlsx");
                    }
                }

            }

        }
    }
}

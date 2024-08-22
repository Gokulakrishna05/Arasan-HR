using Arasan.Interface.Report;
using Arasan.Models;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ClosedXML.Excel;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using Microsoft.CodeAnalysis.Operations;
using Intuit.Ipp.DataService;
using Arasan.Interface;
using Elasticsearch.Net;
using Arasan.Services.Report;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Arasan.Controllers
{
    public class ApsIdleStockController : Controller
    {
        IApsIdleStock ApsIdleStockService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        private object excelData;

        public ApsIdleStockController(IApsIdleStock _ApsIdleStockService, IConfiguration _configuratio)
        {
            ApsIdleStockService = _ApsIdleStockService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ApsIdleStock(string strfrom)
        {
            try
            {
                ApsIdleStockModel objR = new ApsIdleStockModel();

                objR.dtFrom = strfrom;
                return View(objR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public JsonResult GetBranchJSON(string ItemId)
        //{

        //    return Json(BindMaster(ItemId));

        //}


        public ActionResult MyListPurchaseRepItemReportGrid(string dtFrom)
        {
            List<ApsIdleStockModelItems> Reg = new List<ApsIdleStockModelItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)ApsIdleStockService.GetAllApsIdleStock(dtFrom);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                Reg.Add(new ApsIdleStockModelItems
                {

                    locid = dtUsers.Rows[i]["LOCID"].ToString(),
                    drumno = dtUsers.Rows[i]["DRUMNO"].ToString(),
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    itemid = dtUsers.Rows[i]["ITEMID"].ToString(),
                    batchno = dtUsers.Rows[i]["BATCHNO"].ToString(),
                    qty = dtUsers.Rows[i]["QTY"].ToString(),
                    rc = dtUsers.Rows[i]["RC"].ToString(),
                    asondt = dtUsers.Rows[i]["ASONDT"].ToString(),
                    days = dtUsers.Rows[i]["DAYS"].ToString(),

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
            //if (dtFrom == null && dtTo == null)
            //{
            SvSql = " select LocID,DrumNo,Docdate,ItemId,BatchNo,Qty,RC,asondt,Days from (Select M.DrumMastID,LM.Docdate,  M.DrumNo, I.ItemID, Nvl(B.BatchNo, 'Empty') BatchNo, Nvl(Sum(B.CLQty), 0) Qty ,  Nvl((B.Rate), 0) Rate, L.LocID, Decode(Nvl(LM.RCFlag, 0), 0, 'No', 'Yes') RC,'" + dtFrom + "' asondt, To_date('" + dtFrom + "', 'DD/MM/YYYY')-To_Date(lm.docdate, 'DD/MM/YYYY') Days From  DrumMast M, LocDetails L, ItemMaster I, LotMast LM, (Select S.DrumNo, S.CDrumNo, S.Location, Sum(Decode(PlusOrMinus, 'p', S.Qty, -S.Qty)) DStkQty From  DrumStock S WHERE s.docdate <= '" + dtFrom + "'Group By S.DrumNo, S.CDrumNo, S.Location Having Sum(Decode(PlusOrMinus, 'p', S.Qty, -S.Qty)) > 0 ) S, (Select B.DrumNo,  B.LotNo BatchNo, B.LocID, B.ItemID, Sum(B.PlusQty) - Sum(B.MinusQty) ClQty, Sum(B.StockValue) / (Sum(B.PlusQty) + Sum(B.MinusQty)) Rate From LStockValue B Where B.DrumNo Is Not Null and B.DOCDATE <= '" + dtFrom + "' Group By B.LotNo, B.DrumNo, B.LocID, B.ItemID Having(Sum(B.PlusQty)-Sum(B.MinusQty)) > 0)  B Where  S.CDrumNo = B.Drumno(+) And L.LocDetailsID = S.Location And S.Location = B.LocID(+) And L.LOCATIONTYPE in ('AP MILL', 'SIEVE & BLEND')And S.DrumNo = M.DrumMastID And B.ItemID = I.ItemMasterID(+) And B.BatchNo = LM.LotNo(+) Group By M.DrumMastID,LM.Docdate, M.DrumNo, I.ItemID, B.BatchNo, M.Partial, S.Location, B.ItemID, B.LOCID, L.LocID, B.Rate, LM.RCFlag Having(Sum(DStkQty) > 0 And  Sum(Nvl(B.ClQty, 0)) = 0) Or(Sum(Nvl(B.ClQty, 0)) > 0)Order By L.LocID, DrumNo) Where BatchNo<>'Empty' And Days >= 30 Order By LocID, ItemID , docdate, DrumNo";

            //}

            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            using (DataTable dtReport = new DataTable())
            {
                adapter.Fill(dtReport);

                DataView dv1 = dtReport.DefaultView;
                // dv1.RowFilter = " TotalOutstandingAmount > 0 AND  OutstandingPrinciple > 0  ";

                dtNew1 = dv1.ToTable();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtNew1, "ApsIdleStockDetails");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=ApsIdleStockDetails.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "ApsIdleStockDetails.xlsx");
                    }
                }

            }

        }
    }
}

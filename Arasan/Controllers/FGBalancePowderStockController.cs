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
    public class FGBalancePowderStockController : Controller
    {
        IFGBalancePowderStock FGBalancePowderStockService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        private object excelData;

        public FGBalancePowderStockController(IFGBalancePowderStock _FGBalancePowderStockService, IConfiguration _configuratio)
        {
            FGBalancePowderStockService = _FGBalancePowderStockService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult FGBalancePowderStock(string strfrom, string strto)
        {
            try
            {
                FGBalancePowderStockModel objR = new FGBalancePowderStockModel();

                objR.dtFrom = strfrom;
                objR.dtTo = strto;
                return View(objR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult MyListPurchaseRepItemReportGrid(string dtFrom, string dtTo)
        {
            List<FGBalancePowderStockModelItems> Reg = new List<FGBalancePowderStockModelItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)FGBalancePowderStockService.GetAllFGBalancePowderStock(dtFrom, dtTo);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                Reg.Add(new FGBalancePowderStockModelItems
                {
                    itemid = dtUsers.Rows[i]["ITEMID"].ToString(),
                    oq = dtUsers.Rows[i]["OQ"].ToString(),
                    rq = dtUsers.Rows[i]["RQ"].ToString(),
                    orq = dtUsers.Rows[i]["ORQ"].ToString(),
                    pkq = dtUsers.Rows[i]["PKQ"].ToString(),
                    oiss = dtUsers.Rows[i]["OISS"].ToString(),


                });
            }
            return Json(new
            {
                Reg
            });

        }




        public IActionResult ExportToExcel(string dtFrom, string dtTo)
        {
            DataTransactions _datatransactions;
            DataTable dtNew1 = new DataTable();

            string SvSql = "";
            SvSql = "Select I.ItemID , Sum(x.OQty) OQ , Sum(x.RQty) RQ, Sum(x.ORec) ORQ , Sum(x.PkQty) PkQ , Sum(x.OIss) Oiss \r\nFrom \r\n(\r\nSelect ItemID,Sum(Oqty) Oqty,SUm(RQty) Rqty,Sum(Orec) Orec, Sum(PkQty) PkQty,Sum(Oiss) Oiss From ( \r\nSelect  S.Itemid,Sum(S.PlusQty-S.MinusQty) OQty , 0 RQty,0 Orec , 0 PkQty , 0 OIss \r\nFrom LStockValue S,LOCDETAILS L , DrumMast D\r\nWhere S.LocID = L.LocDetailsID\r\n       And S.DrumNo = D.DrumNo\r\n       And L.LocID in ('PYRO PACKING','PYRO MACHINE PACKING','POLISH MACHINE PACK','POLISH PACKING','PYRO MACHINE PACK-1','PYRO MACHINE PACK-2')\r\n       And S.DocDate < '" + dtFrom + "'\r\nGroup By S.itemid\r\nHaving ( (Sum(S.PlusQty-S.MinusQty) > 0))\r\nUnion All\r\nSelect  S.Itemid,0 OQty , Sum(S.PlusQty) RQty,0 Orec, 0 PkQty , 0 Oiss\r\nFrom LStockValue S,LOCDETAILS L , DrumMast D\r\nWhere S.LocID = L.LocDetailsID\r\n       And s.Stocktranstype='CURING PACK'\r\n       And S.DrumNo = D.DrumNo and S.Plusqty>0\r\n       And L.LocID in ('PYRO PACKING','PYRO MACHINE PACKING','POLISH MACHINE PACK','POLISH PACKING','PYRO MACHINE PACK-1','PYRO MACHINE PACK-2')\r\n       And S.DocDate Between '" + dtFrom + "' And '" + dtTo + "'\r\nGroup By S.itemid\r\nUnion All\r\nSelect  S.Itemid,0 OQty ,0 Rqty, Sum(S.PlusQty) Orec , 0 PkQty , 0 Oiss \r\nFrom LStockValue S,LOCDETAILS L , DrumMast D\r\nWhere S.LocID = L.LocDetailsID\r\n       And s.Stocktranstype<>'CURING PACK'\r\n       And S.DrumNo = D.DrumNo and S.Plusqty>0\r\n       And L.LocID in ('PYRO PACKING','PYRO MACHINE PACKING','POLISH MACHINE PACK','POLISH PACKING','PYRO MACHINE PACK-1','PYRO MACHINE PACK-2')\r\n       And S.DocDate Between '" + dtFrom + "' And '" + dtTo + "'\r\nGroup By S.itemid\r\nUnion All\r\nSelect  S.Itemid,0 OQty , 0 RQty , 0, Sum(S.MinusQty) PkQty , 0 BQty\r\nFrom LStockValue S,LOCDETAILS L , DrumMast D\r\nWhere S.LocID = L.LocDetailsID\r\n       And S.DrumNo = D.DrumNo and S.MinusQty>0\r\n        And S.STOCKTRANSTYPE='PACKING INPUT'\r\n       And L.LocID in ('PYRO PACKING','PYRO MACHINE PACKING','POLISH MACHINE PACK','POLISH PACKING','PYRO MACHINE PACK-1','PYRO MACHINE PACK-2')\r\n       And S.DocDate Between '" + dtFrom + "' And '" + dtTo + "'\r\nGroup By S.itemid\r\nUnion All\r\nSelect  S.Itemid,0 OQty , 0 RQty ,0, 0 PkQty ,Sum(S.MinusQty) OIss \r\nFrom LStockValue S,LOCDETAILS L , DrumMast D\r\nWhere S.LocID = L.LocDetailsID\r\n       And S.DrumNo = D.DrumNo\r\n        And S.STOCKTRANSTYPE<>'PACKING INPUT' and S.MinusQty>0\r\n       And L.LocID in ('PYRO PACKING','PYRO MACHINE PACKING','POLISH MACHINE PACK','POLISH PACKING','PYRO MACHINE PACK-1','PYRO MACHINE PACK-2')\r\n       And S.DocDate Between '" + dtFrom + "' And '" + dtTo + "'\r\nGroup By S.itemid)Group By ItemID\r\n) x , ItemMaster I \r\nWhere x.ItemID = I.ItemMasterID\r\nGroup By I.ItemID\r\nOrder By I.ItemID";


            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            using (DataTable dtReport = new DataTable())
            {
                adapter.Fill(dtReport);

                DataView dv1 = dtReport.DefaultView;
 
                dtNew1 = dv1.ToTable();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtNew1, "FGBalancePowderStock");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=FGBalancePowderStockDetails.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "FGBalancePowderStockDetails.xlsx");
                    }
                }

            }

        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Arasan.Interface;
using Arasan.Interface.Report;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Report;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using ClosedXML.Excel;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using Microsoft.CodeAnalysis.Operations;
using Intuit.Ipp.DataService;

namespace Arasan.Controllers
{
    public class StockStatementDrumwiseController : Controller
    {
        IStockStatementDrumwise StockStatementDrumwiseService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        private object excelData;
        public StockStatementDrumwiseController(IStockStatementDrumwise _StockStatementDrumwise, IConfiguration _configuratio)
        {
            StockStatementDrumwiseService = _StockStatementDrumwise;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult StockStatementDrumwise(string strfrom, string strTo)
        {
            try
            {
                StockStatementDrumwiseModel objR = new StockStatementDrumwiseModel();
                objR.SNlst = BindSN("");
                objR.Locationlst = BindLocation("");

                objR.dtFrom = strfrom;
                 return View(objR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindLocation(string Id)
        {
            try
            {
                DataTable dtDesg = StockStatementDrumwiseService.GetLocation(Id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["Location"].ToString(), Value = dtDesg.Rows[i]["Location"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindSN(string Id)
        {
            try
            {
                DataTable dtDesg = StockStatementDrumwiseService.GetSN(Id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SNCATEGORY"].ToString(), Value = dtDesg.Rows[i]["SNCATEGORY"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult MyListStockStatementDrumwiseGrid(string dtFrom, string WEDyn, string SN, string Location)
        {
            List<StockStatementDrumwiseModelItems> Reg = new List<StockStatementDrumwiseModelItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)StockStatementDrumwiseService.GetAllStockStatementDrumwise(dtFrom, WEDyn, SN,Location);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                Reg.Add(new StockStatementDrumwiseModelItems
                {

                    drummastid = dtUsers.Rows[i]["DrumMastID"].ToString(),
                    drumno = dtUsers.Rows[i]["DrumNo"].ToString(),
                    itemid = dtUsers.Rows[i]["ItemID"].ToString(),
                    batchno = dtUsers.Rows[i]["batchno"].ToString(),
                    qty = dtUsers.Rows[i]["Qty"].ToString(),
                    rate = dtUsers.Rows[i]["rate"].ToString(),
                    locid = dtUsers.Rows[i]["locid"].ToString(),
                    completed = dtUsers.Rows[i]["Completed"].ToString(),
                    inspection = dtUsers.Rows[i]["Inspection"].ToString(),
                    curinginward = dtUsers.Rows[i]["CuringInward"].ToString(),
                    curinges = dtUsers.Rows[i]["CuringES"].ToString(),
                    curingoutward = dtUsers.Rows[i]["CuringOutward"].ToString(),
                    recharge = dtUsers.Rows[i]["Recharge"].ToString(),
                    ncrelease = dtUsers.Rows[i]["NCRelease"].ToString(),
                    packing = dtUsers.Rows[i]["Packing"].ToString(),
                    packings = dtUsers.Rows[i]["PackingIns"].ToString(),
                    docdate = dtUsers.Rows[i]["DocDate"].ToString(),
                    subcategory = dtUsers.Rows[i]["subcategory"].ToString(),
                    batch = dtUsers.Rows[i]["Batch"].ToString(),
                    fidrms = dtUsers.Rows[i]["FiDrms"].ToString(),
                    curdays = dtUsers.Rows[i]["CURDAYS"].ToString(),
                    idle = dtUsers.Rows[i]["idle"].ToString(),
                    igroup = dtUsers.Rows[i]["igroup"].ToString(),
                    status = dtUsers.Rows[i]["Status"].ToString(),
                      
                     
                    
                     

                });
            }

            return Json(new
            {
                Reg
            });

        }
        public IActionResult ExportToExcel(string dtFrom, string WEDyn, string SN, string Location)
        {
            DataTransactions _datatransactions;
            DataTable dtNew1 = new DataTable();

            string SvSql = "";
            //if (dtFrom == null && dtTo == null)
            //{
            SvSql = "Select x.DrumMastID,x.DrumNo, x.ItemID,X.batchno,Sum(X.qty) Qty,round(Sum(x.rate), 2) rate,X.locid,x.Completed,X.Inspection,x.CuringInward,x.CuringES,x.CuringOutward,x.Recharge,x.NCRelease,x.Packing, x.PackingIns,x.DocDate,x.subcategory,x.Batch,x.FiDrms,x.CURDAYS,x.idle,x.igroup,Decode(FiDrms, 0, Decode(Sign(sum(x.idle) + 1), 1, Decode(x.Igroup, 'FINISHED', Decode(x.Recharge, 'No', 'Ready For Packing', 'Ready For Re-Process'), 'Ready for Next Process'), 'Not Ready'), 'Not Ready') Status From(Select M.DrumMastID, M.DrumNo, I.ItemID, Nvl(B.BatchNo, 'Empty') BatchNo, Nvl(Sum(B.CLQty), 0) Qty,Nvl((B.Rate), 0) Rate, L.LocID, Decode(Nvl(LM.Compflag, 0), 0, 'No', 'Yes') Completed,Decode(Nvl(LM.InsFlag, 0), 0, 'No', 'Yes') Inspection,Decode(Nvl(LM.CurInwFlag, 0), 0, 'No', 'Yes') CuringInward,Decode(Nvl(LM.EStatus, 0), 0, 'No', 'Yes') CuringES,Decode(Nvl(LM.CurOutFlag, 0), 0, 'No', 'Yes') CuringOutward,Decode(Nvl(LM.RCFlag, 0), 0, 'No', 'Yes') Recharge,Decode(Nvl(LM.QcRelaseFlag, 0), 0, 'No', 'Yes') NCRelease,Decode(Nvl(LM.PackFlag, 0), 0, 'No', 'Yes') Packing,Decode(Nvl(LM.PackInsFlag, 0), 0, 'No', 'Yes') PackingIns,LM.DocDate, Initcap(i.SubCategory) subcategory, LM.Batch, Nvl(LM.FIDRMS, 0) FiDrms, IC.CURDAYS, (((sysdate) - LM.DocDate) - nvl(IC.CURDAYS, 0)) IDLE, I.IGROUP From  DrumMast M, LocDetails L, ItemMaster I, LotMast LM, itemmCurInfo IC,(Select S.DrumNo, S.CDrumNo, S.Location, Sum(Decode(PlusOrMinus, 'p', S.Qty, -S.Qty)) DStkQty From  DrumStock S Where S.DocDate <= '" + dtFrom + "'Group By S.DrumNo, S.CDrumNo, S.Location Having Sum(Decode(PlusOrMinus, 'p', S.Qty, -S.Qty)) > 0 order by 2) S, (Select B.DrumNo,  B.LotNo BatchNo, B.LocID, B.ItemID, Sum(B.PlusQty) - Sum(B.MinusQty) ClQty,Sum(B.StockValue) / (Sum(B.PlusQty) + Sum(B.MinusQty)) Rate From LStockValue B Where B.DrumNo Is Not Null And B.DocDate <= '" + dtFrom + "'Group By B.LotNo, B.DrumNo, B.LocID, B.ItemID Having(Sum(B.PlusQty-B.MinusQty)) > 0)  B Where  S.CDrumNo = B.Drumno(+) And L.LocDetailsID = S.Location And S.Location = B.LocID(+) And S.DrumNo = M.DrumMastID And B.ItemID = I.ItemMasterID(+) AND I.ITEMMASTERID = IC.ITEMMASTERID(+) AND(I.SNCATEGORY = '" + SN + "' OR 'ALL' = '" + SN + "') And B.BatchNo = LM.LotNo(+) And(L.LocID = '" + Location + "' Or '" + Location + "' = 'ALL LOCATIONS') Group By M.DrumMastID, M.DrumNo, I.ItemID, B.BatchNo, M.Partial, S.Location, B.ItemID, B.LOCID, L.LocID, B.Rate, LM.Compflag,LM.InsFlag, LM.CurInwFlag,LM.EStatus,LM.CurOutFlag, LM.RCFlag,LM.QcRelaseFlag,LM.PackFlag,LM.PackInsFlag,LM.DocDate,i.SubCategory,LM.Batch,LM.FIDRMS,IC.CURDAYS,I.IGROUP Having(Sum(DStkQty) > 0 And  Sum(Nvl(B.ClQty, 0)) = 0) Or(Sum(Nvl(B.ClQty, 0)) > 0) Order By L.LocID, I.ItemID , DrumNo) x Where(('" + WEDyn + "' = 'Yes' ) Or(x.BatchNo<>'Empty'  And '" + WEDyn + "' = 'No')) Group by x.DrumMastID,x.DrumNo, x.ItemID,X.batchno,x.Completed,X.Inspection,x.CuringInward,x.CuringES,x.CuringOutward,x.Recharge,x.NCRelease,x.Packing,x.PackingIns,x.DocDate,x.subcategory,x.Batch,x.FiDrms,x.CURDAYS,x.idle,x.igroup,X.locid Order By 24,x.FIDRMS, x.LocID, x.ItemID , x.docdate, x.DrumNo";

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
                    wb.Worksheets.Add(dtNew1, "StockStatementDrumwiseDetails");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=StockStatementDrumwiseDetails.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "StockStatementDrumwiseDetails.xlsx");
                    }
                }

            }

        }
    }
}

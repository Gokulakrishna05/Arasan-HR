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
    public class FinishedGoodsStockDetailsDrumwiseController : Controller
    {
        IFinishedGoodsStockDetailsDrumwise FinishedGoodsStockDetailsDrumwiseService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        private object excelData;

        public FinishedGoodsStockDetailsDrumwiseController(IFinishedGoodsStockDetailsDrumwise _FinishedGoodsStockDetailsDrumwiseService, IConfiguration _configuratio)
        {
            FinishedGoodsStockDetailsDrumwiseService = _FinishedGoodsStockDetailsDrumwiseService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult FinishedGoodsStockDetailsDrumwise(string strfrom)
        {

            try
            {
                FinishedGoodsStockDetailsDrumwiseModel objR = new FinishedGoodsStockDetailsDrumwiseModel();
                 objR.Loclst = BindLocation("");

                objR.dtFrom = strfrom;
                 return View(objR);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult MyListPurchaseRepItemReportGrid(string dtFrom,string Location)
        {
            List<FinishedGoodsStockDetailsDrumwiseModelItems> Reg = new List<FinishedGoodsStockDetailsDrumwiseModelItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)FinishedGoodsStockDetailsDrumwiseService.GetAllFinishedGoodsStockDetailsDrumwise(dtFrom,Location);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                Reg.Add(new FinishedGoodsStockDetailsDrumwiseModelItems
                {
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    locid = dtUsers.Rows[i]["LOCID"].ToString(),
                    itemid = dtUsers.Rows[i]["ITEMID"].ToString(),
                    branchno = dtUsers.Rows[i]["BRANCHNO"].ToString(),
                    drumno = dtUsers.Rows[i]["DRUMNO"].ToString(),
                    qty = dtUsers.Rows[i]["QTY"].ToString(),
                    rate = dtUsers.Rows[i]["RATE"].ToString(),
                    packinsflag = dtUsers.Rows[i]["PACKINSFLAG"].ToString(),
                    status = dtUsers.Rows[i]["STATUS"].ToString(),
                    asondate = dtUsers.Rows[i]["ASONDATE"].ToString(),
                    laydays = dtUsers.Rows[i]["LAYDAYS"].ToString(),





                });
            }

            return Json(new
            {
                Reg
            });

        }
        
       public List<SelectListItem> BindLocation(string id)
        {
            try
            {
                DataTable dtDesg = FinishedGoodsStockDetailsDrumwiseService.GetLocation(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCID"].ToString(), Value = dtDesg.Rows[i]["LOCID"].ToString() });


                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       

       

        public IActionResult ExportToExcel(string dtFrom, string Location)
        {
            DataTransactions _datatransactions;
            DataTable dtNew1 = new DataTable();

            string SvSql = "";
            //if (dtFrom == null && dtTo == null)
            //{
            SvSql = "Select  M.Docdate,L.LocID , I.Itemid,M.LotNo BatchNo, M.DrumNo, Sum(S.PlusQty-S.MinusQty) Qty , M.Rate Rate,PackInsFlag , Decode(PACKINSFLAG,0,'Q.C Not Completed','Ready to Despatch') Status,sysdate Asondate, To_date(sysdate, 'DD/MM/YYYY')-To_Date(m.docdate, 'DD/MM/YYYY') Laydays From PLotMast M, PLStockValue S,LOCDETAILS L, ITEMMASTER I, SelectedValues SS Where M.LotNo = S.LotNo And S.LocID = L.LOCDETAILSID And S.ItemID = I.ItemMasterID And S.Cancel = 'F' And M.DrumNo is Not Null And(L.LocID = '" + Location + "' Or 'ALL' = '" + Location + "') And S.DocDate <= '" + dtFrom + "' And I.ItemMasterID = SS.SelectedID And Not Exists(Select DrumNo from DrumMast D Where D.DrumNo = M.DrumNo) Group By M.Docdate,L.LocID , I.itemid,M.PLotMastID, M.DrumNo, M.LotNo, M.Rate,PackInsFlag Having((Sum(S.PlusQty - S.MinusQty) > 0)) Order by  2, 3, 9, 5";

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
                    wb.Worksheets.Add(dtNew1, "FinishedGoodsStockDetailsDrumwise");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=FinishedGoodsStockDetailsDrumwise.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "FinishedGoodsStockDetailsDrumwise.xlsx");
                    }
                }

            }

        }
    }
}

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
    public class ReceiptAgainstReturnableGoodsController : Controller
    {
        IReceiptAgainstReturnableGoods ReceiptAgainstReturnableGoodsService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        private object excelData;

        public ReceiptAgainstReturnableGoodsController(IReceiptAgainstReturnableGoods _ReceiptAgainstReturnableGoodsService, IConfiguration _configuratio)
        {
            ReceiptAgainstReturnableGoodsService = _ReceiptAgainstReturnableGoodsService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ReceiptAgainstReturnableGoods(string strfrom, string strto)
        {
            try
            {
                ReceiptAgainstReturnableGoodsModel objR = new ReceiptAgainstReturnableGoodsModel();
                objR.Brlst = BindBranch();
 
                objR.dtFrom = strfrom;
                objR.dtTo = strto;
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


        public ActionResult MyListPurchaseRepItemReportGrid(string dtFrom,string dtTo, string Branch)
        {
            List<ReceiptAgainstReturnableGoodsItems> Reg = new List<ReceiptAgainstReturnableGoodsItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)ReceiptAgainstReturnableGoodsService.GetAllReceiptAgainstReturnableGoods(dtFrom,dtTo, Branch);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                Reg.Add(new ReceiptAgainstReturnableGoodsItems
                {
                    branchid = dtUsers.Rows[i]["BRANCHID"].ToString(),
                    locid = dtUsers.Rows[i]["LOCID"].ToString(),
                    docid = dtUsers.Rows[i]["DOCID"].ToString(),
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    partyid = dtUsers.Rows[i]["PARTYID"].ToString(),
                    dcno = dtUsers.Rows[i]["DCNO"].ToString(),
                    dcdt = dtUsers.Rows[i]["DCDT"].ToString(),
                    //refno = dtUsers.Rows[i]["REFNO"].ToString(),
                    //refdate = dtUsers.Rows[i]["REFDATE"].ToString(),
                    itemid = dtUsers.Rows[i]["ITEMID"].ToString(),
                    unit = dtUsers.Rows[i]["UNIT"].ToString(),
                    qty = dtUsers.Rows[i]["QTY"].ToString(),
                    rejqty = dtUsers.Rows[i]["REJQTY"].ToString(),
                    accqty = dtUsers.Rows[i]["ACCQTY"].ToString(),
                    dcqty = dtUsers.Rows[i]["DCQTY"].ToString(),

                });
            }
            return Json(new
            {
                Reg
            });

        }
        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = datatrans.GetBranch();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["BRANCHID"].ToString(), Value = dtDesg.Rows[i]["BRANCHID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       

        public IActionResult ExportToExcel(string dtFrom,string dtTo, string Branch)
        {
            DataTransactions _datatransactions;
            DataTable dtNew1 = new DataTable();

            string SvSql = "";
            //if (dtFrom == null && dtTo == null)
            //{
            SvSql = " SELECT BR.BRANCHID , L.LOCID , B.DOCID , B.DOCDATE , P.PARTYID , RB.DOCID DCNO , RB.DOCDATE DCDT, B.REFNO , B.REFDATE , D.ITEMID , U.UnitID Unit , D.QTY , D.REJQTY , D.ACCQTY  , Rd.Qty DcQty FROM RECDCBASIC B , RECDCDETAIL D, BRANCHMAST BR , LOCDETAILS L, RDELBASIC RB , UnitMast U, RDelDetail Rd,Partymast P WHERE B.RECDCBASICID = D.RECDCBASICID AND RB.DOCDATE BETWEEN '" + dtFrom + "' AND '" + dtTo + "' AND B.BRANCHID = BR.BRANCHMASTID And D.Unit = U.UnitMastID And P.partymastid = RB.Partyid AND(BR.BRANCHID = '" + Branch + "' OR 'ALL BRANCHES' ='" + Branch + "') AND B.LOCID(+) = L.LOCDETAILSID AND B.DCNO = RB.RDELBASICID(+) And Rb.RDelBasicID = Rd.RDelBasicID And Rd.ItemID = D.ItemID ORDER BY 7,6,2";

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
                    wb.Worksheets.Add(dtNew1, "ReceiptAgainstReturnableGoods");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=ReceiptAgainstReturnableGoodsDetails.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "ReceiptAgainstReturnableGoodsDetails.xlsx");
                    }
                }

            }

        }
    }
}

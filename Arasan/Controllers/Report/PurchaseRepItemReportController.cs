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

namespace Arasan.Controllers.Report
{
    public class PurchaseRepItemReportController : Controller
    {
        IPurchaseRepItemReportService PurchaseRepItemReportService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        private object excelData;

        public PurchaseRepItemReportController(IPurchaseRepItemReportService _PurchaseRepItemReportService, IConfiguration _configuratio)
        {
            PurchaseRepItemReportService = _PurchaseRepItemReportService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult PurchaseRepItemReport(string strfrom, string strTo)
        {
            try
            {
                PurchaseRepItemReport objR = new PurchaseRepItemReport();
                objR.Brlst = BindBranch();
                objR.Suplst = BindSupplier();
                objR.ItemGrouplst = BindItemGrplst();
                objR.Itemlst = BindItemlst("");
                objR.dtFrom = strfrom;
                objR.dtTo = strTo;
                return View(objR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public IActionResult ListPurchaseRepItemReport()
        {
            return View();
        }
        public List<SelectListItem> BindItemlst(string id)
        {
            try
            {
                DataTable dtDesg = PurchaseRepItemReportService.GetItem(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["ITEMID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindItemGrplst()
        {
            try
            {
                DataTable dtDesg = datatrans.GetItemSubGrp();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SGCODE"].ToString(), Value = dtDesg.Rows[i]["ITEMSUBGROUPID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindSupplier()
        {
            try
            {
                DataTable dtDesg = datatrans.GetSupplier();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTYNAME"].ToString(), Value = dtDesg.Rows[i]["PARTYNAME"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
        public JsonResult GetItemJSON(string itemid)
        {
            PurchaseRepItemReport model = new PurchaseRepItemReport();
            model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));

        }
        public ActionResult MyListPurchaseRepItemReportGrid(string dtFrom, string dtTo, string Branch, string Customer, string Item)
        {
            List<PurchaseRepItemReportItems> Reg = new List<PurchaseRepItemReportItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)PurchaseRepItemReportService.GetAllPurchaseItemReport(dtFrom, dtTo, Branch, Customer, Item);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                Reg.Add(new PurchaseRepItemReportItems
                {
                    branch = dtUsers.Rows[i]["BRANCHID"].ToString(),
                    docNo = dtUsers.Rows[i]["DOCID"].ToString(),
                    docDate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    party = dtUsers.Rows[i]["PARTYID"].ToString(),
                    item = dtUsers.Rows[i]["ITEMID"].ToString(),
                    unit = dtUsers.Rows[i]["UNIT"].ToString(),
                    qty = dtUsers.Rows[i]["QTY"].ToString(),
                    rate = dtUsers.Rows[i]["RATE"].ToString(),
                    amount = dtUsers.Rows[i]["AMOUNT"].ToString(),
                    cost = dtUsers.Rows[i]["COSTRATE"].ToString(),





                });
            }

            return Json(new
            {
                Reg
            });

        }
        public IActionResult ExportToExcel(string dtFrom, string dtTo, string Branch, string Customer, string Item)
        {
            DataTransactions _datatransactions;

            DataTable dtNew = new DataTable();

            string SvSql = "Select Br.BranchID , P.PartyID , Db.DocID ,to_char(Db.DocDate,'dd-MON-yyyy')DocDate , I.ItemID , U.UnitID Unit , Dd.Qty,DD.PriQty , Dd.Rate , Dd.Amount,to_char(Db.Refno) Refno,DD.Costrate From DPBasic Db , DPDetail Dd , ItemMaster I , PartyMast P , BranchMast Br , UnitMast U  Where Db.DPBasicID = Dd.DPBasicID And Db.PartyID = P.PartyMastID And Dd.ItemID = I.ItemMasterID And Dd.Unit = U.UnitMastID And Db.BranchID = Br.BranchMastID";
            if (dtFrom != null && dtTo != null)
            {
                SvSql += " and Db.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "'";
            }
          
            if (Branch != null)
            {
                SvSql += " and Br.BranchID='" + Branch + "'";
            }
            
            if (Customer != null)
            {
                SvSql += " and P.PartyID='" + Customer + "'";
            }
           
            if (Item != null)
            {
                SvSql += " and I.ItemID='" + Item + "'";
            }
           
            SvSql += "Union Select Br.BranchID , P.PartyID , Db.DocID , to_char(Db.DocDate,'dd-MON-yyyy')DocDate , I.ItemID , U.UnitID Unit , Dd.Qty,DD.PriQty , Dd.Rate , Dd.Amount,to_char(Db.Refno),DD.Costrate From grnBLbasic Db , grnBLdetail Dd , ItemMaster I , PartyMast P , BranchMast Br , UnitMast U Where Db.grnBLbasicID = Dd.grnBLbasicID And Db.PartyID = P.PartyMastID And Dd.ItemID = I.ItemMasterID And Dd.Unit = U.UnitMastID And Db.BranchID = Br.BranchMastID\r\n";
            if (dtFrom != null && dtTo != null)
            {
                SvSql += " and Db.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "'";
            }
           
            if (Branch != null)
            {
                SvSql += " and Br.BranchID='" + Branch + "'";
            }
           
            if (Customer != null)
            {
                SvSql += " and P.PartyID='" + Customer + "'";
            }
           
            if (Item != null)
            {
                SvSql += " and I.ItemID='" + Item + "'";
            }
           
            SvSql += "Union Select Br.BranchID , P.PartyID , Db.DocID , to_char(Db.DocDate,'dd-MON-yyyy')DocDate , I.ItemID , U.UnitID Unit, Dd.Qty,DD.PriQty , Dd.Rate , Dd.Amount,Db.Refno,DD.Costrate From igrnbasic Db, igrndetail Dd , ItemMaster I, PartyMast P , BranchMast Br, UnitMast U Where Db.igrnbasicID = Dd.igrnbasicID And Db.PartyID = P.PartyMastID And Dd.ItemmasterID = I.ItemMasterID And Dd.Unit = U.UnitMastID And Db.BranchID = Br.BranchMastID";
            if (dtFrom != null && dtTo != null)
            {
                SvSql += " and Db.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "'";
            }
           
            if (Branch != null)
            {
                SvSql += " and Br.BranchID='" + Branch + "'";
            }
            
            if (Customer != null)
            {
                SvSql += " and P.PartyID='" + Customer + "'";
            }
           
            if (Item != null)
            {
                SvSql += " and I.ItemID='" + Item + "'";
            }
            
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            using (DataTable dtReport = new DataTable())
            {
                adapter.Fill(dtReport);

                DataView dv1 = dtReport.DefaultView;
                // dv1.RowFilter = " TotalOutstandingAmount > 0 AND  OutstandingPrinciple > 0  ";

                dtNew = dv1.ToTable();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtNew, "PurchaseRepItemwise");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=PurchaseRepItemwise.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "PurchaseRepItemwise.xlsx");
                    }
                }

                //OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
                //DataTable dtReport = new DataTable();
                //adapter.Fill(dtReport);
                //return dtReport;}
            }

        }
        private IActionResult File(object excelData, string v)
        {
            throw new NotImplementedException();
        }

    }
}

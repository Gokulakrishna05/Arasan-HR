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
using Oracle.ManagedDataAccess.Client;

namespace Arasan.Controllers.Report
{
    public class GRNReportController : Controller
    {
        IGRNReportService GRNReportService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public GRNReportController(IGRNReportService _GRNReportService, IConfiguration _configuratio)
        {
            GRNReportService = _GRNReportService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult GRNReport(string strfrom, string strTo)
        {
            try
            {
                GRNReport objR = new GRNReport();
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
        public IActionResult ListGRNReport()
        {
            return View();
        }
        public List<SelectListItem> BindItemlst(string id)
        {
            try
            {
                DataTable dtDesg = GRNReportService.GetItem(id);
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
            GRNReport model = new GRNReport();
            model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));

        }
        public ActionResult MyListGRNReportGrid(string dtFrom, string dtTo, string Branch, string Customer, string Item)
        {
            List<GRNReportItems> Reg = new List<GRNReportItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)GRNReportService.GetAllReport(dtFrom, dtTo, Branch, Customer,Item);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                Reg.Add(new GRNReportItems
                {
                    branch = dtUsers.Rows[i]["BRANCHID"].ToString(),
                    docNo = dtUsers.Rows[i]["DOCID"].ToString(),
                    po = dtUsers.Rows[i]["doc"].ToString(),
                    docDate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    party = dtUsers.Rows[i]["PARTYID"].ToString(),
                    item = dtUsers.Rows[i]["ITEMID"].ToString(),
                    loc = dtUsers.Rows[i]["LOCID"].ToString(),
                    unit = dtUsers.Rows[i]["UNITID"].ToString(),
                    qty = dtUsers.Rows[i]["QTY"].ToString(),
                    rate = dtUsers.Rows[i]["RATE"].ToString(),
                    amount = dtUsers.Rows[i]["AMOUNT"].ToString(),

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
            DataTable dtNew1 = new DataTable();

            string SvSql = "SELECT A.DOCID,to_char(A.DOCDATE,'dd-MON-yyyy')DOCDATE,D.DOCID AS doc,C.BRANCHID,L.LOCID,P.PARTYID,I.ITEMID,U.UNITID,B.QTY,B.RATE,B.AMOUNT FROM GRNBLBASIC A,GRNBLDETAIL B,BRANCHMAST C,LOCDETAILS L,PARTYMAST P,POBASIC D,ITEMMASTER I,UNITMAST U ,ITEMGROUP P,ITEMSUBGROUP B WHERE A.CANCEL <> 'T' AND A.GRNBLBASICID=B.GRNBLBASICID AND C.BRANCHMASTID=A.BRANCHID AND L.LOCDETAILSID =A.LOCID AND P.PARTYMASTID=A.PARTYID AND I.ITEMMASTERID=B.ITEMID AND P.ITEMGROUPID=B.ITEMGROUPID AND I.SUBGROUPCODE=B.ITEMSUBGROUPID AND U.UNITMASTID=B.UNIT\r\n";
            if (dtFrom != null && dtTo != null)
            {
                SvSql += " and A.DOCDATE BETWEEN '" + dtFrom + "' AND '" + dtTo + "'";
            }


            if (Branch != null)
            {
                SvSql += " and C.BRANCHID='" + Branch + "'";
            }


            if (Customer != null)
            {
                SvSql += " and P.PARTYID='" + Customer + "'";
            }

            if (Item != null)
            {
                SvSql += " and I.ITEMID='" + Item + "'";
            }
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            using (DataTable dtReport = new DataTable())
            {
                adapter.Fill(dtReport);

                DataView dv1 = dtReport.DefaultView;
                // dv1.RowFilter = " TotalOutstandingAmount > 0 AND  OutstandingPrinciple > 0  ";

                dtNew1 = dv1.ToTable();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtNew1, "GRNCumBill");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=GRNCumBill.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "GRNCumBill.xlsx");
                    }
                }

            }

        }
        private IActionResult File(object excelData, string v)
        {
            throw new NotImplementedException();
        }

    }
}

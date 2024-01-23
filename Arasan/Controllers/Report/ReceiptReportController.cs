using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Arasan.Interface;
using Arasan.Interface.Report;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Report;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;
using ClosedXML.Excel;
using Oracle.ManagedDataAccess.Client;

namespace Arasan.Controllers
{
    public class ReceiptReportController : Controller
    {
        IReceiptReport ReceiptReportService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public ReceiptReportController(IReceiptReport _ReceiptReportService, IConfiguration _configuratio)
        {
            ReceiptReportService = _ReceiptReportService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ReceiptReport(string strfrom, string strTo) 
        {
            try
            {
                ReceiptReport objR = new ReceiptReport();
                objR.Brlst = BindBranch();
                objR.Sdate = strfrom;
                objR.Edate = strTo;
                return View(objR);
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
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["BRANCHID"].ToString(), Value = dtDesg.Rows[i]["BRANCHMASTID"].ToString() });
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
            ReceiptReport model = new ReceiptReport();
            model.Brlst = BindBranch();
            return Json(BindBranch());

        }

        public ActionResult MyListReceiptReportGrid(string Branch, string Sdate, string Edate)
        {
            List<ReceiptReportItem> Reg = new List<ReceiptReportItem>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)ReceiptReportService.GetAllReport(Branch, Sdate, Edate); 
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                Reg.Add(new ReceiptReportItem
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["RECDCBASICID"].ToString()),
                    loc = dtUsers.Rows[i]["LOCID"].ToString(),
                    dcdate = dtUsers.Rows[i]["DCDATE"].ToString(),
                    docNo = dtUsers.Rows[i]["DOCID"].ToString(),
                    des = dtUsers.Rows[i]["ITEMID"].ToString(),
                    unit = dtUsers.Rows[i]["UNITID"].ToString(),
                    dcqty = dtUsers.Rows[i]["QTY"].ToString(),
                    recdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    recno = dtUsers.Rows[i]["DOCID"].ToString(),
                    //recqty = dtUsers.Rows[i]["ITEMID"].ToString(),
                    rejqty = dtUsers.Rows[i]["REJQTY"].ToString(),
                    accqty = dtUsers.Rows[i]["ACCQTY"].ToString(),
                    pend = dtUsers.Rows[i]["PENDQTY"].ToString(),


                });
            }

            return Json(new
            {
                Reg
            });

        }


        public ActionResult ExportLeadProReport(string Branch, string Sdate, string Edate)
        {
            //_connectionString = _configuratio.GetConnectionString("OracleDBConnection");


            DataTransactions datatrans;

            DataTable dtNew = new DataTable();


            string SvSql = "select RECDCBASIC.RECDCBASICID,LOCDETAILS.LOCID,to_char(RECDCBASIC.DCDATE,'dd-MON-yyyy')DCDATE,BRANCHMAST.BRANCHID,RDELBASIC.DOCID,RECDCDETAIL.ITEMID,UNITMAST.UNITID,RECDCDETAIL.QTY,to_char(RECDCBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,RECDCBASIC.DOCID,RECDCDETAIL.REJQTY,RECDCDETAIL.ACCQTY,RECDCDETAIL.PENDQTY from RECDCBASIC INNER JOIN RECDCDETAIL ON RECDCDETAIL.RECDCDETAILID = RECDCBASIC.RECDCBASICID LEFT OUTER JOIN BRANCHMAST  ON BRANCHMAST.BRANCHMASTID=RECDCBASIC.BRANCHID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID = RECDCBASIC.LOCID LEFT OUTER JOIN RDELBASIC ON RDELBASIC.RDELBASICID = RECDCBASIC.DCNO LEFT OUTER JOIN UNITMAST on UNITMAST.UNITMASTID = RECDCDETAIL.UNIT LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID = RECDCBASIC.PARTYID ";
            if (Sdate != null && Edate != null)
            {
                SvSql += " and RECDCBASIC.DOCDATE BETWEEN '" + Sdate + "' AND '" + Edate + "'";
            }
            else
            {
                SvSql += "";
            }
            if (Branch != null)
            {
                SvSql += " and BRANCHMAST.BRANCHID='" + Branch + "'";
            }
            else
            {
                SvSql += "";
            }

            OracleDataAdapter dat = new OracleDataAdapter(SvSql, _connectionString);

            using (DataTable dt = new DataTable())
            {
                dat.Fill(dt);

                DataView dv1 = dt.DefaultView;
                // dv1.RowFilter = " TotalOutstandingAmount > 0 AND  OutstandingPrinciple > 0  ";

                dtNew = dv1.ToTable();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtNew, "ReturnableDCReport");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=CallsProReport.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "ReturnableDCReport.xlsx");
                    }
                }
            }

        }



    }
}

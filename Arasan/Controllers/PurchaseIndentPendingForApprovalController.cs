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
    public class PurchaseIndentPendingForApprovalController : Controller
    {
        IPurchaseIndentPendingForApproval PurchaseIndentPendingForApprovalService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        private object excelData;

        public PurchaseIndentPendingForApprovalController(IPurchaseIndentPendingForApproval _PurchaseIndentPendingForApprovalService, IConfiguration _configuratio)
        {
            PurchaseIndentPendingForApprovalService = _PurchaseIndentPendingForApprovalService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult PurchaseIndentPendingForApproval(string strfrom)
        {
            try
            {
                PurchaseIndentPendingForApprovalModel objR = new PurchaseIndentPendingForApprovalModel();

                objR.dtFrom = strfrom;
                return View(objR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult MyListPurchaseRepItemReportGrid(string dtFrom)
        {
            List<PurchaseIndentPendingForApprovalModelItems> Reg = new List<PurchaseIndentPendingForApprovalModelItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)PurchaseIndentPendingForApprovalService.GetAllPurchaseIndentPendingForApproval(dtFrom);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                Reg.Add(new PurchaseIndentPendingForApprovalModelItems
                {

                    docid = dtUsers.Rows[i]["DOCID"].ToString(),
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    itemid = dtUsers.Rows[i]["ITEMID"].ToString(),
                    unitid = dtUsers.Rows[i]["UNITID"].ToString(),
                    ord_qty = dtUsers.Rows[i]["ORD_QTY"].ToString(),
                    pur_qty = dtUsers.Rows[i]["PUR_QTY"].ToString(),
                    pend_qty = dtUsers.Rows[i]["PEND_QTY"].ToString(),
                    duedate = dtUsers.Rows[i]["DUEDATE"].ToString(),
                    locid = dtUsers.Rows[i]["LOCID"].ToString(),
                    narration = dtUsers.Rows[i]["NARRATION"].ToString(),
                    app1dt = dtUsers.Rows[i]["APP1DT"].ToString(),
                    app2dt = dtUsers.Rows[i]["APP2DT"].ToString(),
                    entdt = dtUsers.Rows[i]["ENTDT"].ToString(),


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
            SvSql = "SELECT PB.DOCID,PB.DOCDATE,IM.ITEMID,UM.UNITID,QTY ORD_QTY,POQTY PUR_QTY,((QTY+RETQTY)-(POQTY+SHCLQTY)) \r\nPEND_QTY,PD.DUEDATE,L.Locid,PD.Narration , Pd.App1Dt , Pd.App2Dt , Pb.EntryDate EntDt\r\nFROM PINDBASIC PB,PINDDETAIL PD,ITEMMASTER IM,UNITMAST UM,Locdetails L\r\nWHERE PB.PINDBASICID = PD.PINDBASICID\r\nAND IM.PRIUNIT = UM.UNITMASTID\r\nAND IM.ITEMMASTERID = PD.ITEMID\r\nAND (PD.APPROVED1 is null or (PD.APPROVED2  is null and Pd.Approved1='YES')  )\r\nAnd L.Locdetailsid(+) = PD.Department\r\nand pb.docdate <='" + dtFrom + "'\r\nAnd ((QTY+RETQTY)-(POQTY+SHCLQTY)) >0\r\nORDER BY  PB.DOCDATE , PB.DOCID,IM.ITEMID";


            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            using (DataTable dtReport = new DataTable())
            {
                adapter.Fill(dtReport);

                DataView dv1 = dtReport.DefaultView;
                // dv1.RowFilter = " TotalOutstandingAmount > 0 AND  OutstandingPrinciple > 0  ";

                dtNew1 = dv1.ToTable();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtNew1, "PurchaseIndentPendingForApprovalDetails");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=PurchaseIndentPendingForApprovalDetails.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "PurchaseIndentPendingForApprovalDetails.xlsx");
                    }
                }

            }

        }
    }
}

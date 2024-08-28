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
    public class PendingPaymentOrReceiptBillWiseController : Controller
    {
        IPendingPaymentOrReceiptBillWise PendingPaymentOrReceiptBillWiseService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        private object excelData;

        public PendingPaymentOrReceiptBillWiseController(IPendingPaymentOrReceiptBillWise _PendingPaymentOrReceiptBillWiseService, IConfiguration _configuratio)
        {
            PendingPaymentOrReceiptBillWiseService = _PendingPaymentOrReceiptBillWiseService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult PendingPaymentOrReceiptBillWise(string strfrom)
        {
            try
            {
                PendingPaymentOrReceiptBillWiseModel objR = new PendingPaymentOrReceiptBillWiseModel();
                objR.Brlst = BindBranch();

                objR.dtFrom = strfrom;
                return View(objR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        public ActionResult MyListPurchaseRepItemReportGrid(string dtFrom, string Branch)
        {
            List<PendingPaymentOrReceiptBillWiseModelItems> Reg = new List<PendingPaymentOrReceiptBillWiseModelItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)PendingPaymentOrReceiptBillWiseService.GetAllPendingPaymentOrReceiptBillWise(dtFrom, Branch);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                Reg.Add(new PendingPaymentOrReceiptBillWiseModelItems
                {

                    grouporder = dtUsers.Rows[i]["GROUPORDER"].ToString(),
                    slno = dtUsers.Rows[i]["SLNO"].ToString(),
                    docid = dtUsers.Rows[i]["DOCID"].ToString(),
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    groupno = dtUsers.Rows[i]["GROUPNO"].ToString(),
                    mname = dtUsers.Rows[i]["MNAME"].ToString(),
                    matchdate = dtUsers.Rows[i]["MATCHDATE"].ToString(),
                    amount = dtUsers.Rows[i]["AMOUNT"].ToString(),
                    pending = dtUsers.Rows[i]["PENDING"].ToString(),
                    userid = dtUsers.Rows[i]["USERID"].ToString(),

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




        public IActionResult ExportToExcel(string dtFrom, string Branch)
        {
            DataTransactions _datatransactions;
            DataTable dtNew1 = new DataTable();

            string SvSql = "";
            SvSql = "SELECT a.grouporder, Decode(A.GROUPORDER ,1,Decode(sign(A.AMOUNT),1,'Bills','Rec/Adv/Unadj') , 'Rec/Adv/Unadj')  AS  slno, A.DOCID, A.DOCDATE, A.GROUPNO, DECODE(C.CURRENCYID , 1, M.MNAME, M.MNAME||'       [ in '||c.maincurr||' ]') AS  MNAME, A.MATCHDATE, A.DUEDATE,  A.AMOUNT AS AMOUNT,  DECODE ( a.grouporder , 1, ROUND(R.AMT,2), 0)  AS PENDING,A.UserID FROM RPDETAILS A , ( select partyname, groupno, sum(amount) as amt from rpdetails where docdate <= '" + dtFrom + "' group by partyname, groupno having sum(amount) <> 0 ) r, master m, \r\n currency c,BRANCHMAST B1,COMPANYMAST CM\r\nwhere a.docdate <=  '" + dtFrom + "' and m.masterid = a.partyname and m.masterid = r.partyname and a.groupno = r.groupno and c.currencyid = a.maincurr    \r\nAND B1.BRANCHMASTID=A.BRANCHID  \r\nAND B1.COMPANYID=CM.COMPANYMASTID\r\nAND (B1.BRANCHID='" + Branch + "' OR    'ALL BRANCH' ='" + Branch + "')\r\norder by 6, A.MATCHDATE , 5, a.grouporder";


            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            using (DataTable dtReport = new DataTable())
            {
                adapter.Fill(dtReport);

                DataView dv1 = dtReport.DefaultView;
                // dv1.RowFilter = " TotalOutstandingAmount > 0 AND  OutstandingPrinciple > 0  ";

                dtNew1 = dv1.ToTable();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtNew1, "PendingPaymentDetails");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=PendingPaymentDetails.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "PendingPaymentDetails.xlsx");
                    }
                }

            }

        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Arasan.Interface;
//using Arasan.Interface.Report;
using Arasan.Models;
using Arasan.Services;
//using Arasan.Services.Report;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using ClosedXML.Excel;
using Oracle.ManagedDataAccess.Client;

namespace Arasan.Controllers
{
    public class PendingIndentApproveController : Controller
    {
        IPendingIndentApprove PendingIndentApproveService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public PendingIndentApproveController(IPendingIndentApprove _PendingIndentApproveService, IConfiguration _configuratio)
        {
            PendingIndentApproveService = _PendingIndentApproveService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult PendingIndentApprove(string strfrom, string strTo)
        {

            try
            {
                PendingIndentApproveModel objR = new PendingIndentApproveModel();
                //objR.Worklst = BindWorkCenter();
                //objR.Processlst = BindProcess();
                objR.dtFrom = strfrom;
                //objR.dtTo = strTo;
                return View(objR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult MyListPendingIndentApproveGrid(string dtFrom)
        {
            List<PendingIndentApproveModelItems> Reg = new List<PendingIndentApproveModelItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)PendingIndentApproveService.GetAllPendingIndentApprove(dtFrom);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                Reg.Add(new PendingIndentApproveModelItems
                {

                    docid = dtUsers.Rows[i]["DOCID"].ToString(),
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    itemid = dtUsers.Rows[i]["ITEMID"].ToString(),
                    unitid = dtUsers.Rows[i]["UNITID"].ToString(),
                    ordqty = dtUsers.Rows[i]["ORD_QTY"].ToString(),
                    purqty = dtUsers.Rows[i]["PUR_QTY"].ToString(),
                    pendqty = dtUsers.Rows[i]["PEND_QTY"].ToString(),
                    duedate = dtUsers.Rows[i]["DUEDATE"].ToString(),
                    locid = dtUsers.Rows[i]["Locid"].ToString(),
                    narration = dtUsers.Rows[i]["Narration"].ToString(),
                    app1dt = dtUsers.Rows[i]["App1Dt"].ToString(),
                    app2dt = dtUsers.Rows[i]["App2Dt"].ToString(),
                    enddate = dtUsers.Rows[i]["EntDt"].ToString(),

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
                SvSql = "SELECT PB.DOCID,PB.DOCDATE,IM.ITEMID,UM.UNITID,QTY ORD_QTY,POQTY PUR_QTY,((QTY+RETQTY)-(POQTY+SHCLQTY))  PEND_QTY,PD.DUEDATE,L.Locid,PD.Narration , Pd.App1Dt , Pd.App2Dt , Pb.EntryDate EntDt FROM PINDBASIC PB,PINDDETAIL PD,ITEMMASTER IM,UNITMAST UM,Locdetails L WHERE PB.PINDBASICID = PD.PINDBASICID AND IM.PRIUNIT = UM.UNITMASTID AND IM.ITEMMASTERID = PD.ITEMID  And L.Locdetailsid(+) = PD.Department and pb.docdate <='" + dtFrom + "' And ((QTY+RETQTY)-(POQTY+SHCLQTY)) >0 ORDER BY  PB.DOCDATE , PB.DOCID,IM.ITEMID";

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
                    wb.Worksheets.Add(dtNew1, "PendingIndentApproveDetails");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=PendingIndentApproveDetails.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "PendingIndentApproveDetails.xlsx");
                    }
                }

            }

        }
    }
}

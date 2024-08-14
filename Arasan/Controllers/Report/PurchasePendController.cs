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
    public class PurchasePendController : Controller
    {
        IPurchasePend PurchasePendService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public PurchasePendController(IPurchasePend _PurchasePendService, IConfiguration _configuratio)
        {
            PurchasePendService = _PurchasePendService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult PurchasePend(string strfrom, string strTo)
        {
            try
            {
                PurchasePend objR = new PurchasePend();
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
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["BRANCHID"].ToString(), Value = dtDesg.Rows[i]["BRANCHID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult MyListPurchasePendGrid(string Branch, string Sdate, string Edate)
        {
            List<PurchasePendItem> Reg = new List<PurchasePendItem>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)PurchasePendService.GetAllReport(Branch, Sdate, Edate);
            for (int i = 0; i < dtUsers.Rows.Count; i++) 
            {
                Reg.Add(new PurchasePendItem
                {
                    //id = Convert.ToInt64(dtUsers.Rows[i]["PINDBASICID"].ToString()),
                    did  = dtUsers.Rows[i]["DOCID"].ToString(),
                    dcdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    loc = dtUsers.Rows[i]["LOCID"].ToString(),
                    item = dtUsers.Rows[i]["ITEMID"].ToString(),
                    pend = dtUsers.Rows[i]["PEND_QTY"].ToString(),
                    pur = dtUsers.Rows[i]["PUR_QTY"].ToString(),
                    due = dtUsers.Rows[i]["DUEDATE"].ToString(),
                    unit = dtUsers.Rows[i]["UNITID"].ToString(),
                    ord = dtUsers.Rows[i]["ORD_QTY"].ToString(),
                    narr = dtUsers.Rows[i]["Narration"].ToString(),
                    app2 = dtUsers.Rows[i]["App2Dt"].ToString(),
                    trans = dtUsers.Rows[i]["TransType"].ToString(),
                    entry = dtUsers.Rows[i]["EntDt"].ToString(),
                    pdays = dtUsers.Rows[i]["Pdays"].ToString(),
                    

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


            string SvSql = "SELECT Br.BranchID,PB.DOCID,to_char(PB.DOCDATE,'dd-MON-yyyy')DOCDATE,IM.ITEMID,UM.UNITID,QTY ORD_QTY,PD.GRNQTY PUR_QTY,((QTY+RETQTY)-(nvl(GRNQTY,0)+SHCLQTY+PD.POQTY)) PEND_QTY,to_char(PD.DUEDATE,'dd-MON-yyyy')DUEDATE,L.Locid,PD.Narration  , Pd.App2Dt ,Decode(sign(pd.qty-pd.POQTY),1,'DIRECT PURCHASE','PURCHASE ORDER') TransType,  Pb.EntryDate EntDt,Round((Sysdate-PD.DueDate),0) Pdays FROM PINDBASIC PB,PINDDETAIL PD,ITEMMASTER IM,UNITMAST UM,Locdetails L ,BranchMast Br WHERE PB.PINDBASICID = PD.PINDBASICID AND IM.PRIUNIT = UM.UNITMASTID AND IM.ITEMMASTERID = PD.ITEMID AND PB.BranchID = Br.BranchMastID AND PD.APPROVED2='YES' And L.Locdetailsid(+) = PD.Department AND (PD.POQTY=0 or PD.POQTY is null or (Pd.qty-Pd.POQTY)>0) ";
            if (Sdate != null && Edate != null)
            {
                SvSql += " and PB.DOCDATE BETWEEN '" + Sdate + "' AND '" + Edate + "' ";
            }
            else
            {
                SvSql += "";
            }
            if (Branch != null)
            {
                SvSql += " and Br.BRANCHID='" + Branch + "' ";
            }
            else
            {
                SvSql += "";
            }

            SvSql += "Union All SELECT Br.BranchID,(B.DOCID||'--'||pb.DOCID) docid,to_char(B.DOCDATE,'dd-MON-yyyy')DOCDATE ,IM.ITEMID,UM.UNITID,Po.QTY ORD_QTY,Po.GRNQTY PUR_QTY,((Po.QTY+Po.REJQTY)-(nvl(Po.GRNQTY,0)+Po.SHCLQTY)) PEND_QTY,to_char(PD.DUEDATE,'dd-MON-yyyy')DUEDATE,L.Locid,PD.Narration ,  Pd.App2Dt ,Decode((pd.POQTY),0,'DIRECT PURCHASE','PURCHASE ORDER') T,  Pb.EntryDate EntDt,Round((Sysdate-PD.DueDate),0) Pdays FROM PINDBASIC PB,PINDDETAIL PD,ITEMMASTER IM,UNITMAST UM,Locdetails L,PoDetail PO,PoBasic B,BranchMast Br WHERE PB.PINDBASICID = PD.PINDBASICID AND IM.PRIUNIT = UM.UNITMASTID AND IM.ITEMMASTERID = PD.ITEMID AND PO.PINDDETAILID=PD.PINDDETAILID And B.POBASICID=Po.POBASICID ANd B.Active=0 AND PD.APPROVED2='YES' And L.Locdetailsid(+) = PD.Department AND PB.BranchID = Br.BranchMastID AND (PD.POQTY)>0  ";
            if (Sdate != null && Edate != null)
            {
                SvSql += " and PB.DOCDATE BETWEEN '" + Sdate + "' AND '" + Edate + "'";
            }
            else
            {
                SvSql += "";
            }
            if (Branch != null)
            {
                SvSql += " and Br.BRANCHID='" + Branch + "' ";
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
                    wb.Worksheets.Add(dtNew, "PurchasePendingReport");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=CallsProReport.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "PurchasePendingReport.xlsx");
                    }
                }
            }

        }

    }
}


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
    public class DirectPurchaseReportController : Controller
    {
        IDirectPurchaseReportService DirectPurchaseReportService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public DirectPurchaseReportController(IDirectPurchaseReportService _DirectPurchaseReportService, IConfiguration _configuratio)
        {
            DirectPurchaseReportService = _DirectPurchaseReportService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult DirectPurchaseReport(string strfrom, string strTo)
        {
            try
            {
                DirectPurchaseReport objR = new DirectPurchaseReport();
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
        public IActionResult ListDirectPurchaseReport()
        {
            return View();
        }
        public List<SelectListItem> BindItemlst(string id)
        {
            try
            {
                DataTable dtDesg = DirectPurchaseReportService.GetItem(id);
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
            DirectPurchaseReport model = new DirectPurchaseReport();
            model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));

        }
        public ActionResult MyListDirectPurchaseReportGrid(string dtFrom, string dtTo, string Branch, string Customer, string Item)
        {
            List<DirectPurchaseReportItems> Reg = new List<DirectPurchaseReportItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)DirectPurchaseReportService.GetAllDPReport(dtFrom, dtTo, Branch, Customer, Item);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                Reg.Add(new DirectPurchaseReportItems
                {
                   
                    branch = dtUsers.Rows[i]["BRANCHID"].ToString(),
                    docNo = dtUsers.Rows[i]["DOCID"].ToString(),
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
            DataTable dtNew = new DataTable();

            string SvSql = "SELECT A.DOCID,to_char(A.DOCDATE,'dd-MON-yyyy')DOCDATE,C.BRANCHID,L.LOCID,P.PARTYID,I.ITEMID,U.UNITID,B.QTY,B.PRIQTY,B.RATE,B.AMOUNT,B.BRATE,B.BAMOUNT,IFREIGHTCH FREIGHT,IPKNFDCH PKNFD,IDELCH DELCH,ICSTCH ,ILRCH LORRY,-ISPLDISCF IsplD,IOTHERCH OTHER,B.COSTRATE , A.RefNo , A.RefDt,To_char(A.Docdate,'MON') Mon FROM DPBASIC A,DPDETAIL B,BRANCHMAST C,LOCDETAILS L,PARTYMAST P,ITEMMASTER I,UNITMAST U,ITEMGROUP P,ITEMSUBGROUP B WHERE A.CANCEL <> 'T'AND A.DPBASICID=B.DPBASICID AND C.BRANCHMASTID=A.BRANCHID AND  L.LOCDETAILSID =A.LOCID AND P.PARTYMASTID=A.PARTYID AND I.ITEMMASTERID=B.ITEMID  AND P.ITEMGROUPID=B.ITEMGROUPID AND I.SUBGROUPCODE=B.ITEMSUBGROUPID AND U.UNITMASTID=B.UNIT";
            if (dtFrom != null && dtTo != null)
            {
                SvSql += " and A.DOCDATE BETWEEN '" + dtFrom + "' AND '" + dtTo + "'";
            }
            else
            {
                SvSql += "";
            }
            if (Branch != null)
            {
                SvSql += " and C.BRANCHID='" + Branch + "'";
            }
            else
            {
                SvSql += "";
            }
            if (Customer != null)
            {
                SvSql += " and P.PARTYID='" + Customer + "'";
            }
            else
            {
                SvSql += "";
            }
            if (Item != null)
            {
                SvSql += " and I.ITEMID='" + Item + "'";
            }
            else
            {
                SvSql += "";
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
                    wb.Worksheets.Add(dtNew, "LandedCostDetails");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=LandedCostDetails.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "LandedCostDetails.xlsx");
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

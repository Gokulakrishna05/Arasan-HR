using Microsoft.AspNetCore.Mvc;
using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using ClosedXML.Excel;
using Oracle.ManagedDataAccess.Client;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Elasticsearch.Net;


namespace Arasan.Controllers
{
    public class SubContractingReportController : Controller
    {
        ISubContractingReport subContracting;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public SubContractingReportController(ISubContractingReport _subContracting, IConfiguration _configuratio)
        {
            subContracting = _subContracting;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult SubContractingReport(string strfrom, string strTo)
        {
            try
            {
                SubContractingReport objR = new SubContractingReport();

                objR.dtFrom = strfrom;
                objR.dtTo = strTo;
                return View(objR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult MyListSubConReportGrid(string dtFrom, string dtTo)
        {
            List<SubContractItems> Reg = new List<SubContractItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)subContracting.GetAllSubContReport(dtFrom, dtTo);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                Reg.Add(new SubContractItems
                {

                    pid = dtUsers.Rows[i]["PARTYID"].ToString(),
                    dno = dtUsers.Rows[i]["DCNO"].ToString(),
                    ddt = dtUsers.Rows[i]["DCDATE"].ToString(),
                    rno = dtUsers.Rows[i]["REFNO"].ToString(),
                    rdt = dtUsers.Rows[i]["REFDATE"].ToString(),
                    typ = dtUsers.Rows[i]["TYPE"].ToString(),
                    iid = dtUsers.Rows[i]["ITEMID"].ToString(),
                    ides = dtUsers.Rows[i]["ITEMDESC"].ToString(),
                    uid = dtUsers.Rows[i]["UNITID"].ToString(),
                    mqty = dtUsers.Rows[i]["MRQTY"].ToString(),
                    brat = dtUsers.Rows[i]["BRATE"].ToString(),
                    icat = dtUsers.Rows[i]["ICAT"].ToString(),
                    lid = dtUsers.Rows[i]["LOCID"].ToString(),
                    net = dtUsers.Rows[i]["NET"].ToString(),

                });
            }

            return Json(new
            {
                Reg
            });

        }

        public IActionResult ExportToExcel(string dtFrom, string dtTo)
        {
            DataTransactions _datatransactions;
            DataTable dtNew1 = new DataTable();

            string SvSql = "";
            //if (dtFrom == null && dtTo == null)
            //{
            SvSql = "Select P.PartyID, B.DocID DCNo, B.DocDate DCDate, B.RefNo, B.RefDate, 'Item Detail' Type, I.ItemID, I.ItemDesc, U.UnitID, D.MrQty,D.BRATE,I.SnCategory Icat,L.LOCID,Round((D.MrQty*D.BRATE)*1.18,2) Net From SubMRBasic B, subactmrdet D, PartyMast P, LocDetails L, ItemMaster  I, UnitMast U Where B.SubMRBasicID = D.SubMRBasicID and L.LOCDETAILSID = B.RLOCID And B.PartyID = P.PartyMastID And D.MItemID = I.ItemMasterID And B.DocDate Between '" + dtFrom + "' And '" + dtTo + "' And I.PriUnit = U.UnitMastID Order by PartyID,Locid,Icat,DcNo";

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
                    wb.Worksheets.Add(dtNew1, "SubContractingReport");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=SubContractingReport.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "SubContractingReport.xlsx");
                    }
                }

            }

        }
    }
}


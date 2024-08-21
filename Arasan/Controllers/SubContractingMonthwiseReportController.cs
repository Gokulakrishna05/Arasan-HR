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
    public class SubContractingMonthwiseReportController : Controller
    {

        ISubContractingMonthwiseReport subContractingMW;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public SubContractingMonthwiseReportController(ISubContractingMonthwiseReport _subContractingMW, IConfiguration _configuratio)
        {
            subContractingMW = _subContractingMW;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult SubContractingMonthwiseReport(string strfrom, string strTo)
        {
            try
            {
                SubContractingMonthwiseReport objR = new SubContractingMonthwiseReport();

                objR.dtFrom = strfrom;
                objR.dtTo = strTo;
                return View(objR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult MyListSubConMonWiseGrid(string dtFrom, string dtTo)
        {
            List<SubContracMonthwisetItems> Reg = new List<SubContracMonthwisetItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)subContractingMW.GetAllSubContMonWisReport(dtFrom, dtTo);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                Reg.Add(new SubContracMonthwisetItems
                {

                    pid = dtUsers.Rows[i]["PARTYID"].ToString(),
                    iid = dtUsers.Rows[i]["ITEMID"].ToString(),
                    uid = dtUsers.Rows[i]["UNITID"].ToString(),
                    jan = dtUsers.Rows[i]["JAN"].ToString(),
                    feb = dtUsers.Rows[i]["FEB"].ToString(),
                    mar = dtUsers.Rows[i]["MAR"].ToString(),
                    apr = dtUsers.Rows[i]["APR"].ToString(),
                    may = dtUsers.Rows[i]["MAY"].ToString(),
                    jun = dtUsers.Rows[i]["JUN"].ToString(),
                    jul = dtUsers.Rows[i]["JUL"].ToString(),
                    aug = dtUsers.Rows[i]["AUG"].ToString(),
                    sep = dtUsers.Rows[i]["SEP"].ToString(),
                    oct = dtUsers.Rows[i]["OCT"].ToString(),
                    nov = dtUsers.Rows[i]["NOV"].ToString(),
                    dec = dtUsers.Rows[i]["DECM"].ToString(),
                    tot = dtUsers.Rows[i]["TOT"].ToString(),

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

            SvSql = " SELECT PartyID, ItemID, UnitID, round(SUM(apr), 0) apr,round(SUM(may), 0) may,round(Sum(jun), 0) jun,round(Sum(jul), 0) jul,round(Sum(aug), 0) aug,round(Sum(sep), 0) sep,round(Sum(oct), 0) oct,round(Sum(nov), 0) nov,round(Sum(decm), 0) decm,round(Sum(jan), 0) jan,round(Sum(feb), 0) feb,round(Sum(mar), 0) mar,round(Sum(apr + may + jun + jul + aug + sep + oct + nov + decm + jan + feb + mar), 0) tot FROM( SELECT PartyID, ItemID, UnitID, DECODE(m, 'OP', SUM(ff), 0) OP, DECODE(m, 'APR', SUM(ff), 0) apr, DECODE(m, 'MAY', SUM(ff), 0) MAY, DECODE(m, 'JUN', SUM(ff), 0) JUN, DECODE(m, 'JUL', SUM(ff), 0) JUL, DECODE(m, 'AUG', SUM(ff), 0) aug, DECODE(m, 'SEP', SUM(ff), 0) sep, DECODE(m, 'OCT', SUM(ff), 0) OCT, DECODE(m, 'NOV', SUM(ff), 0) NOV, DECODE(m, 'DEC', SUM(ff), 0) DEcm, DECODE(m, 'JAN', SUM(ff), 0) JAN, DECODE(m, 'FEB', SUM(ff), 0) FEB, DECODE(m, 'MAR', SUM(ff), 0) MAR from( Select P.PartyID, B.DocID DCNo, TO_CHAR(B.DOCDATE, 'MON') m, B.RefNo, B.RefDate, 'Item Detail' Type, I.ItemID, I.ItemDesc, U.UnitID, D.MrQty ff, D.MRRATE From SubMRBasic B, subactmrdet D, PartyMast P, ItemMaster  I, UnitMast U Where B.SubMRBasicID = D.SubMRBasicID And B.PartyID = P.PartyMastID And D.MItemID = I.ItemMasterID And B.DocDate Between '" + dtFrom + "' And '" + dtTo + "' And I.PriUnit = U.UnitMastID )Group by PartyID, ItemID, UnitID, m )Group by PartyID,ItemID,UnitID Order by 1,2";

            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            using (DataTable dtReport = new DataTable())
            {
                adapter.Fill(dtReport);

                DataView dv1 = dtReport.DefaultView;

                dtNew1 = dv1.ToTable();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtNew1, "SubContractingMonthwiseReport");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=SubContractingMonthwiseReport.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "SubContractingMonthwiseReport.xlsx");
                    }
                }

            }

        }
    }
}

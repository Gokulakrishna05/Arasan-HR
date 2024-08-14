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
    public class PurMonReportController : Controller
    {
        IPurMonReport PurMonReportService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public PurMonReportController(IPurMonReport _PurMonReportService, IConfiguration _configuratio)
        {
            PurMonReportService = _PurMonReportService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult PurMonReport(string stryr, string strfrom, string strTo)
        {
            try
            {
                PurMonReport objR = new PurMonReport();
                objR.Brlst = BindBranch();
                objR.Finyrlst = BindFinyr();
                objR.Sdate = strfrom;
                objR.Edate = strTo;
                objR.SFINYR = stryr;
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
        public List<SelectListItem> BindFinyr()
        {
            try
            {
                DataTable dtDesg = PurMonReportService.GetFinyr();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SFINYR"].ToString(), Value = dtDesg.Rows[i]["SFINYR"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult MyListPurMonReportGrid(string SFINYR /*,string Sdate, string Edate*/)
        {
            List<PurMonReportItem> Reg = new List<PurMonReportItem>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)PurMonReportService.GetAllReport(SFINYR/*,Sdate, Edate*/);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                Reg.Add(new PurMonReportItem 
                {
                    //id = Convert.ToInt64(dtUsers.Rows[i]["SFINYR"].ToString()),
                    part = dtUsers.Rows[i]["PARTYID"].ToString(),
                    item = dtUsers.Rows[i]["ITEMID"].ToString(),
                    unit = dtUsers.Rows[i]["UNITID"].ToString(),
                    jan = dtUsers.Rows[i]["Jan"].ToString(),
                    feb = dtUsers.Rows[i]["Feb"].ToString(),
                    mar = dtUsers.Rows[i]["Mar"].ToString(),
                    april = dtUsers.Rows[i]["Apr"].ToString(),
                    may = dtUsers.Rows[i]["May"].ToString(),
                    june = dtUsers.Rows[i]["Jun"].ToString(),
                    july = dtUsers.Rows[i]["Jul"].ToString(),
                    aug = dtUsers.Rows[i]["Aug"].ToString(),
                    sep = dtUsers.Rows[i]["Sep"].ToString(),
                    oct = dtUsers.Rows[i]["Oct"].ToString(),
                    nov = dtUsers.Rows[i]["Nov"].ToString(),
                    dec = dtUsers.Rows[i]["Dec"].ToString(),

                });
            }

            return Json(new
            {
                Reg
            });

        }


        public ActionResult ExportLeadProReport(string SFINYR)
        {
            //_connectionString = _configuratio.GetConnectionString("OracleDBConnection");


            DataTransactions datatrans;

            DataTable dtNew = new DataTable();


            string SvSql = " Select P.PartyID , I.ItemID , U.UnitID , Sum(Decode(To_Char(Db.DocDate,'MON'),'APR',Dd.PriQty)) Apr, Sum(Decode(To_Char(Db.DocDate,'MON'),'MAY',Dd.PriQty)) May , Sum(Decode(To_Char(Db.DocDate,'MON'),'JUN',Dd.PriQty)) Jun , Sum(Decode(To_Char(Db.DocDate,'MON'),'JUL',Dd.PriQty)) Jul , Sum(Decode(To_Char(Db.DocDate,'MON'),'AUG',Dd.PriQty)) Aug , Sum(Decode(To_Char(Db.DocDate,'MON'),'SEP',Dd.PriQty)) Sep , Sum(Decode(To_Char(Db.DocDate,'MON'),'OCT',Dd.PriQty)) Oct , Sum(Decode(To_Char(Db.DocDate,'MON'),'NOV',Dd.PriQty)) Nov , Sum(Decode(To_Char(Db.DocDate,'MON'),'DEC',Dd.PriQty)) Dec, Sum(Decode(To_Char(Db.DocDate,'MON'),'JAN',Dd.PriQty)) Jan , Sum(Decode(To_Char(Db.DocDate,'MON'),'FEB',Dd.PriQty)) Feb , Sum(Decode(To_Char(Db.DocDate,'MON'),'MAR',Dd.PriQty)) Mar From DPBasic Db , DPDetail Dd , PartyMast P , ItemMaster I , UnitMast U , finyrsplit f Where Db.DPBasicID = Dd.DPBasicID And Db.PartyID = P.PartyMastID And Dd.ItemID = I.ItemMasterID And  Dd.Unit = U.UnitMastID And f.sfinyr = '" + SFINYR + "'  And Db.DocDate Between f.SFinyrst And f.SFinyred Group By  P.PartyID , I.ItemID , U.UnitID Union Select P.PartyID , I.ItemID , U.UnitID , Sum(Decode(To_Char(Db.DocDate,'MON'),'APR',Dd.PriQty)) Apr, Sum(Decode(To_Char(Db.DocDate,'MON'),'MAY',Dd.PriQty)) May , Sum(Decode(To_Char(Db.DocDate,'MON'),'JUN',Dd.PriQty)) Jun , Sum(Decode(To_Char(Db.DocDate,'MON'),'JUL',Dd.PriQty)) Jul , Sum(Decode(To_Char(Db.DocDate,'MON'),'AUG',Dd.PriQty)) Aug , Sum(Decode(To_Char(Db.DocDate,'MON'),'SEP',Dd.PriQty)) Sep, Sum(Decode(To_Char(Db.DocDate,'MON'),'OCT',Dd.PriQty)) Oct , Sum(Decode(To_Char(Db.DocDate,'MON'),'NOV',Dd.PriQty)) Nov , Sum(Decode(To_Char(Db.DocDate,'MON'),'DEC',Dd.PriQty)) Dec , Sum(Decode(To_Char(Db.DocDate,'MON'),'JAN',Dd.PriQty)) Jan, Sum(Decode(To_Char(Db.DocDate,'MON'),'FEB',Dd.PriQty)) Feb  , Sum(Decode(To_Char(Db.DocDate,'MON'),'MAR',Dd.PriQty)) Mar From grnBLbasic Db , grnBLdetail Dd , PartyMast P , ItemMaster I , UnitMast U , finyrsplit f Where Db.grnBLbasicID = Dd.grnBLbasicID And Db.PartyID = P.PartyMastID And Dd.ItemID = I.ItemMasterID And U.UnitMastID = I.PriUnit And f.sfinyr = '" + SFINYR + "'  And Db.DocDate Between f.SFinyrst And f.SFinyred Group By   P.PartyID , I.ItemID , U.UnitID Union Select P.PartyID , I.ItemID , U.UnitID , Sum(Decode(To_Char(Db.DocDate,'MON'),'APR',Dd.PriQty)) Apr , Sum(Decode(To_Char(Db.DocDate,'MON'),'MAY',Dd.PriQty)) May, Sum(Decode(To_Char(Db.DocDate,'MON'),'JUN',Dd.PriQty)) Jun , Sum(Decode(To_Char(Db.DocDate,'MON'),'JUL',Dd.PriQty)) Jul , Sum(Decode(To_Char(Db.DocDate,'MON'),'AUG',Dd.PriQty)) Aug , Sum(Decode(To_Char(Db.DocDate,'MON'),'SEP',Dd.PriQty)) Sep , Sum(Decode(To_Char(Db.DocDate,'MON'),'OCT',Dd.PriQty)) Oct , Sum(Decode(To_Char(Db.DocDate,'MON'),'NOV',Dd.PriQty)) Nov , Sum(Decode(To_Char(Db.DocDate,'MON'),'DEC',Dd.PriQty)) Dec , Sum(Decode(To_Char(Db.DocDate,'MON'),'JAN',Dd.PriQty)) Jan , Sum(Decode(To_Char(Db.DocDate,'MON'),'FEB',Dd.PriQty)) Feb , Sum(Decode(To_Char(Db.DocDate,'MON'),'MAR',Dd.PriQty)) Mar From igrnbasic Db , igrndetail Dd , PartyMast P , ItemMaster I , UnitMast U   , finyrsplit f Where Db.igrnbasicID = Dd.igrnbasicID And Db.PartyID = P.PartyMastID And Dd.ItemmasterID = I.ItemMasterID And U.UnitMastID = I.PriUnit And f.sfinyr = '" + SFINYR + "' And Db.DocDate Between f.SFinyrst And f.SFinyred Group By  P.PartyID , I.ItemID , U.UnitID Order By 2 , 1";
           
            OracleDataAdapter dat = new OracleDataAdapter(SvSql, _connectionString);

            using (DataTable dt = new DataTable()) 
            {
                dat.Fill(dt);

                DataView dv1 = dt.DefaultView;
                // dv1.RowFilter = " TotalOutstandingAmount > 0 AND  OutstandingPrinciple > 0  ";

                dtNew = dv1.ToTable();
                using (XLWorkbook wb = new XLWorkbook()) 
                {
                    wb.Worksheets.Add(dtNew, "MonthwiseReport");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=CallsProReport.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "MonthwiseReport.xlsx");
                    }
                }
            }

        }






    }

}



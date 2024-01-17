using Microsoft.AspNetCore.Mvc;
using Arasan.Interface;
using Arasan.Interface.Report;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Report;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;


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


        //public JsonResult GetItemJSON(string itemid)
        //{
        //    GRNReport model = new GRNReport();
        //    model.Itemlst = BindItemlst(itemid);
        //    return Json(BindItemlst(itemid));

        //}

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


        //public ActionResult GRNReports(string Branch, string dtFrom, string dtTo, string Customer)
        //{
        //    List<GRNReportItems> Reg = new List<GRNReportItems>();
        //    DataTable dtUsers = new DataTable();

        //    dtUsers = (DataTable)GRNReportService.GetAllReport(Branch, dtFrom, dtTo, Customer);
        //    for (int i = 0; i < dtUsers.Rows.Count; i++)
        //    {


        //        Reg.Add(new GRNReportItems
        //        {
        //            id = Convert.ToInt64(dtUsers.Rows[i]["GRNBLBASICID"].ToString()),
        //            branch = dtUsers.Rows[i]["BRANCHID"].ToString(),
        //            docNo = dtUsers.Rows[i]["DOCID"].ToString(),
        //            po = dtUsers.Rows[i]["POBASICID"].ToString(),
        //            docDate = dtUsers.Rows[i]["DOCDATE"].ToString(),
        //            currency = dtUsers.Rows[i]["MAINCURR"].ToString(),
        //            party = dtUsers.Rows[i]["PARTYNAME"].ToString(),
        //            item = dtUsers.Rows[i]["ITEMID"].ToString(),





        //        });
        //    }

        //    return Json(new
        //    {
        //        Reg
        //    });

        //}
    }
}

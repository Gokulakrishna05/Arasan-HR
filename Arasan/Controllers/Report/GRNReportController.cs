using Microsoft.AspNetCore.Mvc;
using Arasan.Interface;
using Arasan.Interface.Report;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Report;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Controllers.Report
{
    public class GRNReportController : Controller
    {
        IGRNReportService GRNReportService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public GRNReportController(IGRNReportService _GRNReportService, IConfiguration _configuratio)
        {
            GRNReportService = _GRNReportService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult GRNReport(string strfrom, string strTo)
        {
            try
            {
                GRNReport objR = new GRNReport();
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
        public IActionResult ListGRNReport()
        {
            return View();
        }
        public List<SelectListItem> BindItemlst(string id)
        {
            try
            {
                DataTable dtDesg = GRNReportService.GetItem(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["ITEMMASTERID"].ToString() });
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
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTYNAME"].ToString(), Value = dtDesg.Rows[i]["PARTYMASTID"].ToString() });
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
                DataTable dtDesg = GRNReportService.GetBranch();
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
            GRNReport model = new GRNReport();
            model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));

        }
        public ActionResult MyListGRNReportGrid(string Branch, string Customer,string Item,string dtFrom,string dtTo)
        {
            List<GRNReportItems> Reg = new List<GRNReportItems>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)GRNReportService.GetAllReport(Branch, Customer, Item,dtFrom, dtTo);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                Reg.Add(new GRNReportItems
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["GRNBLBASICID"].ToString()),
                    branch = dtUsers.Rows[i]["BRANCHID"].ToString(),
                    docNo = dtUsers.Rows[i]["DOCID"].ToString(),
                    po = dtUsers.Rows[i]["doc"].ToString(),
                    docDate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    currency = dtUsers.Rows[i]["MAINCURR"].ToString(),
                    party = dtUsers.Rows[i]["PARTYID"].ToString(),
                    item = dtUsers.Rows[i]["ITEMID"].ToString(),





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

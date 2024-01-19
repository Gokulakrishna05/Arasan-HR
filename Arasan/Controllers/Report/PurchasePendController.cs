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
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["BRANCHID"].ToString(), Value = dtDesg.Rows[i]["BRANCHMASTID"].ToString() });
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
                    id = Convert.ToInt64(dtUsers.Rows[i]["PINDBASICID"].ToString()),
                    did  = dtUsers.Rows[i]["DOCID"].ToString(),
                    dcdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    loc = dtUsers.Rows[i]["LOCID"].ToString(),
                    item = dtUsers.Rows[i]["ITEMID"].ToString(),
                    pend = dtUsers.Rows[i]["QTY"].ToString(),
                    pur = dtUsers.Rows[i]["GRNQTY"].ToString(),
                    due = dtUsers.Rows[i]["DUEDATE"].ToString(),

                });
            }

            return Json(new
            {
                Reg
            });

        }


    }
}


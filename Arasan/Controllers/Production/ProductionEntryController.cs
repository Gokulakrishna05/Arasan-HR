using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Interface.Stores_Management;
using Arasan.Models;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class ProductionEntryController : Controller
    {
        IProductionEntry IProductionEntry;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public ProductionEntryController(IProductionEntry _IProductionEntry, IConfiguration _configuratio)
        {
            IProductionEntry = _IProductionEntry;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ProductionEntry()
        {
            ProductionEntry ca = new ProductionEntry();
            ca.Brlst = BindBranch();
            ca.Loclst = BindLocation();
            ca.Location= Request.Cookies["LocationId"];
            ca.Branch= Request.Cookies["BranchId"];
            ca.Shiftlst = BindShift();
            return View(ca);
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
        public List<SelectListItem> BindShift()
        {
            try
            {
                DataTable dtDesg = IProductionEntry.ShiftDeatils();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SHIFTNO"].ToString(), Value = dtDesg.Rows[i]["SHIFTMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindLocation()
        {
            try
            {
                DataTable dtDesg = datatrans.GetLocation();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCID"].ToString(), Value = dtDesg.Rows[i]["LOCDETAILSID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetshiftDetail(string Shiftid)
        {
            try
            {
                DataTable dt = new DataTable();
                string fromtime = "";
                string totime= "";
                string tothrs = "";
                dt = datatrans.GetData("Select FROMTIME,TOTIME,SHIFTHRS from SHIFTMAST where SHIFTMASTID='"+ Shiftid + "'");
                if (dt.Rows.Count > 0)
                {

                    fromtime = dt.Rows[0]["FROMTIME"].ToString();
                    totime = dt.Rows[0]["TOTIME"].ToString();
                    tothrs = dt.Rows[0]["SHIFTHRS"].ToString();
                }
               
                var result = new { fromtime = fromtime, totime = totime, tothrs = tothrs};
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

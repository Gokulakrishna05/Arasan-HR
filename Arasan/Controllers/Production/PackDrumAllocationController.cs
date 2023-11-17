using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using Arasan.Interface;
 
using Arasan.Models;
using Arasan.Services.Sales;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Arasan.Controllers
{
    public class PackDrumAllocationController : Controller
    {
        IPackDrumAllocation Pack;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;

        public PackDrumAllocationController(IPackDrumAllocation _Pack, IConfiguration _configuratio)
        {
            Pack = _Pack;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult PackDrumAllocation()
        {
            PackDrumAllocation pa = new PackDrumAllocation();
            pa.Brlst = BindBranch();
            pa.Branch = Request.Cookies["BranchId"];
            pa.Enter = Request.Cookies["LocationId"];
            pa.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            pa.Loclst = BindLoc();
            pa.Emplst = BindEmp();
            DataTable dtv = datatrans.GetSequence("PDA");
            if (dtv.Rows.Count > 0)
            {
                pa.Docid = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            List<DrumCreate> TData = new List<DrumCreate>();
            DrumCreate tda = new DrumCreate();
            for (int i = 0; i < 3; i++)
            {
                tda = new DrumCreate();
               
             

                tda.Isvalid = "Y";
                TData.Add(tda);

            }
            pa.drumlst = TData;
            return View(pa);
        }
        public List<SelectListItem> BindLoc()
        {
            try
            {
                DataTable dtDesg = Pack.GetLoc();
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
        public List<SelectListItem> BindEmp()
        {
            try
            {
                DataTable dtDesg = datatrans.GetEmp();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPNAME"].ToString(), Value = dtDesg.Rows[i]["EMPMASTID"].ToString() });
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
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["BRANCHID"].ToString(), Value = dtDesg.Rows[i]["BRANCHMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetPrefixDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                string start = "";
                string end = "";
                string prefix = "";
                 
                dt = Pack.GetDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    end = dt.Rows[0]["LASTNO"].ToString();
                    prefix = dt.Rows[0]["SPREFIX"].ToString();
                    start = dt.Rows[0]["STARTNO"].ToString();
                    
                }

                var result = new { end = end, prefix = prefix, start = start };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

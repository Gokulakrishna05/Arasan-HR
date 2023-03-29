using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Production;
using Arasan.Interface.Sales;
using Arasan.Models;
using Arasan.Services.Production;
using Arasan.Services.Qualitycontrol;
using Arasan.Services.Sales;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Arasan.Controllers.Production
{
    public class CuringOutwardController : Controller
    {
        ICuringOutward curingoutward;
        IPackingNote Packing;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public CuringOutwardController(ICuringOutward _CuringOutward, IPackingNote _packing, IConfiguration _configuratio)
        {
            curingoutward = _CuringOutward;
            Packing= _packing;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult CuringOutward(string id)
        {
            PackingNote ca = new PackingNote();
            ca.Brlst = BindBranch();
            ca.Worklst = BindWorkCenter();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Shiftlst = BindShift();
            ca.RecList = BindEmp();
            ca.DrumLoclst = BindDrumLoc();
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            List<DrumDetail> TData = new List<DrumDetail>();
            DrumDetail tda = new DrumDetail();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new DrumDetail();
                    tda.DrumNolst = BindDrumNo("");
                    tda.Batchlst = BindBatch("");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            ca.DrumDetlst = TData;
            return View(ca);
        }

        public List<SelectListItem> BindWorkCenter()
        {
            try
            {
                DataTable dtDesg = Packing.GetWorkCenter();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["WCID"].ToString(), Value = dtDesg.Rows[i]["WCBASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindDrumLoc()
        {
            try
            {
                DataTable dtDesg = Packing.GetDrumLocation();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCID"].ToString(), Value = dtDesg.Rows[i]["TOLOCATION"].ToString() });
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
        public List<SelectListItem> BindShift()
        {
            try
            {
                DataTable dtDesg = Packing.ShiftDeatils();
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
        public List<SelectListItem> BindDrumNo(string id)
        {
            try
            {
                DataTable dtDesg = Packing.GetDrumNo(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DRUMNO"].ToString(), Value = dtDesg.Rows[i]["ODRUMNO"].ToString() });
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
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPNAME"].ToString(), Value = dtDesg.Rows[i]["EMPNAME"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindBatch(string value)
        {
            try
            {
                DataTable dtDesg = Packing.GetBatch(value);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["NBATCHNO"].ToString(), Value = dtDesg.Rows[i]["NBATCHNO"].ToString() });

                }


                return lstdesg;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

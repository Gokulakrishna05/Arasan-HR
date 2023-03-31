using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
 
using Arasan.Interface.Sales;
using Arasan.Models;
 
using Arasan.Services.Production;
using Arasan.Services.Qualitycontrol;
using Arasan.Services.Sales;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Arasan.Controllers
{
    public class CuringOutwardController : Controller
    {
        ICuringOutward curingoutward;
       
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public CuringOutwardController(ICuringOutward _CuringOutward , IConfiguration _configuratio)
        {
            curingoutward = _CuringOutward;
             
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult CuringOutward(string id)
        {
            CuringOutward ca = new CuringOutward();
            ca.Brlst = BindBranch();
            ca.FromWorklst = BindWorkCenterID();
            ca.ToWorklst = BindWorkCenter();
            ca.Notelst = BindPackingNote();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Shiftlst = BindShift("");
            ca.RecList = BindEmp();
            ca.DrumLoclst = BindDrumLoc();
            ca.Itemlst = BindItemlst("");
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            //List<CuringDrumDetail> TData = new List<CuringDrumDetail>();
            //CuringDrumDetail tda = new CuringDrumDetail();
            //if (id == null)
            //{
            //    for (int i = 0; i < 3; i++)
            //    {
            //        tda = new CuringDrumDetail();
            //        tda.DrumNolst = BindDrumNo("");
            //        tda.Batchlst = BindBatch("");
            //        tda.Isvalid = "Y";
            //        TData.Add(tda);
            //    }
            //}
            //ca.DrumDetlst = TData;
            return View(ca);
        }
        [HttpPost]
        public ActionResult CuringOutward(CuringOutward Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = curingoutward.CuringOutwardCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "CuringOutward Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "CuringOutward Updated Successfully...!";
                    }
                    return RedirectToAction("ListCuringOutward");
                }

                else
                {
                    ViewBag.PageTitle = "Edit CuringOutward";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Cy);
        }
        public List<SelectListItem> BindWorkCenter()
        {
            try
            {
                DataTable dtDesg = curingoutward.GetWorkCenter();
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
        public List<SelectListItem> BindPackingNote()
        {
            try
            {
                DataTable dtDesg = curingoutward.GetPackingNote();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["PACKNOTEBASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindWorkCenterID()
        {
            try
            {
                DataTable dtDesg = curingoutward.GetWorkCenterID();
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
                DataTable dtDesg = curingoutward.GetDrumLocation();
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
        public JsonResult GetShiftJSON(string supid)
        {
            CuringOutward model = new CuringOutward();
            model.Shiftlst = BindShift(supid);
            return Json(BindShift(supid));

        }
        public List<SelectListItem> BindShift(string id)
        {
            try
            {
                DataTable dtDesg = curingoutward.ShiftDeatils(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SHIFTNO"].ToString(), Value = dtDesg.Rows[i]["SHIFT"].ToString() });
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
                DataTable dtDesg = curingoutward.GetDrumNo(id);
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
                DataTable dtDesg = curingoutward.GetBatch(value);
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
        public JsonResult GetItemJSON(string supid)
        {
            CuringOutward model = new CuringOutward();
            model.Itemlst = BindItemlst(supid);
            return Json(BindItemlst(supid));

        }
        public List<SelectListItem> BindItemlst(string value)
        {
            try
            {
                DataTable dtDesg = curingoutward.GetItembyId(value);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["OITEMID"].ToString() });

                }


                return lstdesg;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetPackingDetails(string id)
        {
            CuringOutward model = new CuringOutward();
            DataTable dtt = new DataTable();
            List<CuringDetail> Data = new List<CuringDetail>();
            CuringDetail tda = new CuringDetail();
            dtt = curingoutward.GetPackingDetail(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new CuringDetail();
                     
                    tda.drum = dtt.Rows[i]["DRUMNO"].ToString();
                    tda.batch = dtt.Rows[i]["IBATCHNO"].ToString();
                    tda.qty = dtt.Rows[i]["IBATCHQTY"].ToString();
                    tda.comp = dtt.Rows[i]["COMBNO"].ToString();
                   
                    Data.Add(tda);
                }
            }
            model.Curinglst = Data;
            return Json(model.Curinglst);

        }
        public IActionResult ListCuringOutward()
        {
            IEnumerable<CuringOutward> cmp = curingoutward.GetAllCuringOutward();
            return View(cmp);
        }

    }
}

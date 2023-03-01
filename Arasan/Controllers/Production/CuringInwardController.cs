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
    public class CuringInwardController : Controller
    {
        ICuringInwardService CuringInwardService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;

        public CuringInwardController(ICuringInwardService _CuringInwardService, IConfiguration _configuratio)
        {
            CuringInwardService = _CuringInwardService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult CuringInward(string id)
        {
            CuringInward ca = new CuringInward();
            ca.Brlst = BindBranch();
            ca.assignList = BindEmp();
            ca.Shiftlst = BindShift();
            List<CIItem> TData = new List<CIItem>();
            CIItem tda = new CIItem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new CIItem();
                    tda.ItemGrouplst = BindItemGrplst();
                    tda.Itemlst = BindItemlst("");
                    tda.drumlst = Binddrum();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                //ca = LocationService.GetLocationsById(id);
                DataTable dt = new DataTable();

                dt = CuringInwardService.GetCuringInward(id);
                if (dt.Rows.Count > 0)
                {
                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.ID = id;
                    ca.WorkCenter = dt.Rows[0]["WCID"].ToString();
                    ca.Shift = dt.Rows[0]["SHIFT"].ToString();
                    ca.RecevedBy = dt.Rows[0]["ENTEREDBY"].ToString();
                    
                }
                DataTable dt2 = new DataTable();
                dt2 = CuringInwardService.GetCuringInwardDetail(id);
                if (dt2.Rows.Count > 0)
                {

                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {

                        tda = new CIItem();
                        tda.ItemGrouplst = BindItemGrplst();
                        DataTable dt3 = new DataTable();
                        dt3 = datatrans.GetItemSubGroup(dt2.Rows[i]["ITEMID"].ToString());
                        if (dt3.Rows.Count > 0)
                        {
                            tda.ItemGroupId = dt3.Rows[0]["SUBGROUPCODE"].ToString();
                        }
                        tda.ItemId = dt2.Rows[0]["ITEMID"].ToString();
                        tda.Itemlst = BindItemlst(tda.ItemGroupId);
                        tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();
                       
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }

                }
            }
            ca.CILst = TData;
            return View(ca);
        }
        [HttpPost]
        public ActionResult CuringInward(CuringInward Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = CuringInwardService.CuringInwardCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "CuringInward Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "CuringInward Updated Successfully...!";
                    }
                    return RedirectToAction("ListCuringInward");
                }

                else
                {
                    ViewBag.PageTitle = "Edit CuringInward";
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
        public IActionResult ListCuringInward()
        {
            IEnumerable<CuringInward> cmp = CuringInwardService.GetAllCuringInward();
            return View(cmp);
        }
        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = CuringInwardService.GetBranch();
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
        public List<SelectListItem> BindItemlst(string value)
        {
            try
            {
                DataTable dtDesg = datatrans.GetItem(value);
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
        public List<SelectListItem> Binddrum()
        {
            try
            {
                DataTable dtDesg = CuringInwardService.DrumDeatils();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DRUMNO"].ToString(), Value = dtDesg.Rows[i]["DRUMMASTID"].ToString() });
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
        public List<SelectListItem> BindShift()
        {
            try
            {
                DataTable dtDesg = CuringInwardService.ShiftDeatils();
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
        public ActionResult GetshiftDetail(string Shiftid)
        {
            try
            {
                DataTable dt = new DataTable();
                string fromtime = "";
                string totime = "";
                string tothrs = "";
                dt = datatrans.GetData("Select FROMTIME,TOTIME,SHIFTHRS from SHIFTMAST where SHIFTMASTID='" + Shiftid + "'");
                if (dt.Rows.Count > 0)
                {

                    fromtime = dt.Rows[0]["FROMTIME"].ToString();
                    totime = dt.Rows[0]["TOTIME"].ToString();
                    tothrs = dt.Rows[0]["SHIFTHRS"].ToString();
                }

                var result = new { fromtime = fromtime, totime = totime, tothrs = tothrs };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemJSON(string itemid)
        {
            CIItem model = new CIItem();
            model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));

        }
    }
}

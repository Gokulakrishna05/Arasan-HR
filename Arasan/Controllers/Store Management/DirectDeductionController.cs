using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Interface.Store_Management;
using Arasan.Models;
using Arasan.Services.Store_Management;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Store_Management
{
    public class DirectDeductionController : Controller
    {
        IDirectDeductionService DirectDeductionService;
        public DirectDeductionController(IDirectDeductionService _DirectDeductionService)
        {
            DirectDeductionService = _DirectDeductionService;
        }
        public IActionResult DirectDeduction(string id)
        {
            DirectDeduction st = new DirectDeduction();
            st.Loc = BindLocation();
            st.Brlst = BindBranch();
            List<DeductionItem> TData = new List<DeductionItem>();
            DeductionItem tda = new DeductionItem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new DeductionItem();
                    tda.Itlst = BindItem();
                    tda.ItemGrouplst = BindItemGrplst();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                //st = DirectDeductionService.GetDirectDeductionById(id);

                DataTable dt = new DataTable();
                dt = DirectDeductionService.GetDirectDeductionDetails(id);
                if (dt.Rows.Count > 0)
                {
                    st.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    st.Location = dt.Rows[0]["LOCID"].ToString();
                    st.DocId = dt.Rows[0]["DOCID"].ToString();
                    st.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    st.Dcno = dt.Rows[0]["DCNO"].ToString();
                    st.Reason = dt.Rows[0]["REASON"].ToString();
                    st.Gro = dt.Rows[0]["GROSS"].ToString();
                    st.Entered = dt.Rows[0]["ENTBY"].ToString();
                    st.Narr = dt.Rows[0]["NARRATION"].ToString();
                    st.NoDurms = dt.Rows[0]["NOOFD"].ToString();

                }

            }
            st.Itlst = TData;
            return View(st);
        }
        [HttpPost]
        public ActionResult DirectDeduction(DirectDeduction ss, string id)
        {

            try
            {
                ss.ID = id;
                string Strout = DirectDeductionService.DirectDeductionCRUD(ss);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (ss.ID == null)
                    {
                        TempData["notice"] = " DirectDeduction Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " DirectDeduction Updated Successfully...!";
                    }
                    return RedirectToAction("ListDirectDeduction");
                }

                else
                {
                    ViewBag.PageTitle = "Edit DirectDeduction";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(ss);
        } 
        public IActionResult ListDirectDeduction()
        {
            IEnumerable<DirectDeduction> sta = DirectDeductionService.GetAllDirectDeduction();
            return View(sta);
        }
        public List<SelectListItem> BindLocation()
        {
            try
            {
                DataTable dtDesg = DirectDeductionService.GetLocation();
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
        public List<SelectListItem> BindItem()
        {
            try
            {
                DataTable dtDesg = DirectDeductionService.GetItem();
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
                DataTable dtDesg = DirectDeductionService.GetItemGrp();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["GROUPCODE"].ToString(), Value = dtDesg.Rows[i]["ITEMGROUPID"].ToString() });
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
                DataTable dtDesg = DirectDeductionService.GetBranch();
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
        //    DirectDeduction model = new DirectDeduction();
            // model.Itlst = BindItem();
            return Json(BindItem());
        }
        public JsonResult GetItemGrpJSON()
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst();
            return Json(BindItemGrplst());
        }
    }

}


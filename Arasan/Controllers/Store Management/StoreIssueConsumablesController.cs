using System.Collections.Generic;
using Arasan.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Models;
using System.Xml.Linq;
using Arasan.Services;

namespace Arasan.Controllers
{
    public class StoreIssueConsumablesController : Controller
    {
        IStoreIssueConsumables StoreIssService;
        public StoreIssueConsumablesController(IStoreIssueConsumables _StoreIssService)
        {
            StoreIssService = _StoreIssService;
        }
        public IActionResult StoreIssueCons(string id)
        {

            StoreIssueConsumables ca = new StoreIssueConsumables();
            ca.Brlst = BindBranch();
            ca.Loclst = GetLoc();
            ca.EnqassignList = BindEmp();
            List<SICItem> TData = new List<SICItem>();
            SICItem tda = new SICItem();
            if (id == null)
            {

                for (int i = 0; i < 3; i++)
                {
                    tda = new SICItem();
                    tda.ItemGrouplst = BindItemGrplst();
                    tda.Itemlst = BindItemlst("");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                // ca = StoreIssService.GetLocationsById(id);
                DataTable dt = new DataTable();
                dt = StoreIssService.EditSICbyID(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    ca.DocNo = dt.Rows[0]["DOCID"].ToString();
                    ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.ReqNo = dt.Rows[0]["REQNO"].ToString();
                    ca.SIId = id;
                    ca.ReqDate = dt.Rows[0]["REQDATE"].ToString();
                    ca.Location = dt.Rows[0]["TOLOCID"].ToString();
                    ca.LocCon = dt.Rows[0]["LOCIDCONS"].ToString();
                    ca.Process = dt.Rows[0]["PROCESSID"].ToString();
                    ca.MCNo = dt.Rows[0]["MCID"].ToString();
                    ca.MCNa = dt.Rows[0]["MCNAME"].ToString();
                    ca.Narr = dt.Rows[0]["NARRATION"].ToString();
                    ca.User = dt.Rows[0]["USERID"].ToString();
                    ca.Work = dt.Rows[0]["WCID"].ToString();
                }
            }
            ca.SICLst = TData;
            return View(ca);
        }
        [HttpPost]
        public ActionResult StoreIssueConsumables(StoreIssueConsumables Cy, string id)
        {

            try
            {
                Cy.SIId = id;
                string Strout = StoreIssService.StoreIssueCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.SIId == null)
                    {
                        TempData["notice"] = "StoreIssueCons Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "StoreIssueCons Updated Successfully...!";
                    }
                    return RedirectToAction("ListStoreIssueCons");
                }

                else
                {
                    ViewBag.PageTitle = "Edit StoreIssueCons";
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
        
        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = StoreIssService.GetBranch();
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
        public List<SelectListItem> GetLoc()
        {
            try
            {
                DataTable dtDesg = StoreIssService.GetLocation();


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
                DataTable dtDesg = StoreIssService.GetEmp();
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
                DataTable dtDesg = StoreIssService.GetItem(value);
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
                DataTable dtDesg = StoreIssService.GetItemGrp();
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
        public JsonResult GetItemJSON(string itemid)
        {
            DirItem model = new DirItem();
            model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));

        }
        public JsonResult GetItemGrpJSON()
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItemGrplst());
        }

        public IActionResult ListStoreIssueCons()
        {
            IEnumerable<StoreIssueConsumables> cmp = StoreIssService.GetAllStoreIssue();
            return View(cmp);
        }
    }
}

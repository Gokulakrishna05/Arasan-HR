using System.Collections.Generic;
using Arasan.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Models;


namespace Arasan.Controllers
{
    public class StoreIssueProductionController : Controller
    {
        IStoreIssueProduction StoreIssueProt;
        public StoreIssueProductionController(IStoreIssueProduction _StoreIssueProt)
        {
            StoreIssueProt = _StoreIssueProt;
        }
        public IActionResult StoreIssuePro(string id)
        {
            StoreIssueProduction ca = new StoreIssueProduction();
            ca.Brlst = BindBranch();
            ca.Loclst = GetLoc();
            //ca.EnqassignList = BindEmp();
            List<SIPItem> TData = new List<SIPItem>();
            SIPItem tda = new SIPItem();
            if (id == null)
            {

                for (int i = 0; i < 3; i++)
                {
                    tda = new SIPItem();
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
                dt = StoreIssueProt.EditSIPbyID(id);
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
                    //ca.MCNo = dt.Rows[0]["MCID"].ToString();
                    //ca.MCNa = dt.Rows[0]["MCNAME"].ToString();
                    ca.Narr = dt.Rows[0]["NARRATION"].ToString();
                    ca.SchNo = dt.Rows[0]["PSCHNO"].ToString();
                    ca.Work = dt.Rows[0]["WCID"].ToString();
                }
            }
            ca.SIPLst = TData;
            return View(ca);
        }
        [HttpPost]
        public ActionResult StoreIssueProduction(StoreIssueProduction Cy, string id)
        {

            try
            {
                Cy.SIId = id;
                string Strout = StoreIssueProt.StoreIssueProCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.SIId == null)
                    {
                        TempData["notice"] = "StoreIssuePro Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "StoreIssuePro Updated Successfully...!";
                    }
                    return RedirectToAction("ListStoreIssuePro");
                }

                else
                {
                    ViewBag.PageTitle = "Edit StoreIssuePro";
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
                DataTable dtDesg = StoreIssueProt.GetBranch();
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
                DataTable dtDesg = StoreIssueProt.GetLocation();


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
        //public List<SelectListItem> BindEmp()
        //{
        //    try
        //    {
        //        DataTable dtDesg = StoreIssue.GetEmp();
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPNAME"].ToString(), Value = dtDesg.Rows[i]["EMPMASTID"].ToString() });
        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public List<SelectListItem> BindItemlst(string value)
        {
            try
            {
                DataTable dtDesg = StoreIssueProt.GetItem(value);
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
                DataTable dtDesg = StoreIssueProt.GetItemGrp();
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

    
    public IActionResult ListStoreIssuePro()
        {
            IEnumerable<StoreIssueProduction> cmp = StoreIssueProt.GetAllStoreIssuePro();
            return View(cmp);
        }
    }
}

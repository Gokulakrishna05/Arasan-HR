using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Interface.Master;

namespace Arasan.Controllers.Store_Management
{
    public class PurchaseIndentController : Controller
    {
        IPurchaseIndent PurIndent;
        public PurchaseIndentController(IPurchaseIndent _PurService)
        {
            PurIndent = _PurService;
        }
        public IActionResult Purchase_Indent()
        {
            PurchaseIndent ca = new PurchaseIndent();
            ca.Brlst = BindBranch();
            ca.SLoclst = GetStoreLoc();
            ca.PURLst = BindPurType();
            ca.ELst = BindErection();
            ca.EmpLst = BindEmp();
            List<PIndentItem> TData = new List<PIndentItem>();
            PIndentItem tda = new PIndentItem();
            for (int i = 0; i < 3; i++)
            {
                tda = new PIndentItem();
                tda.ItemGrouplst = BindItemGrplst();
                tda.Itemlst = BindItemlst("");
                tda.loclst = GetLoc();
                tda.Isvalid = "Y";
                TData.Add(tda);
            }
            ca.PILst = TData;

            List<PIndentTANDC> TData1 = new List<PIndentTANDC>();
            PIndentTANDC tda1 = new PIndentTANDC();
            for (int i = 0; i < 3; i++)
            {
                tda1 = new PIndentTANDC();
                tda.Isvalid = "Y";
                TData1.Add(tda1);
            }
            ca.TANDClst = TData1;

            return View(ca);
        }

        [HttpPost]
        public ActionResult Purchase_Indent(PurchaseIndent Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = PurIndent.IndentCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Indent Created Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Indent Updated Successfully...!";
                    }
                    return RedirectToAction("List_Purchase_Indent");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Indent";
                    TempData["notice"] = Strout;
                }
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
                DataTable dtDesg = PurIndent.GetBranch();
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
                DataTable dtDesg = PurIndent.GetEmp();
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
        public List<SelectListItem> BindErection()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
               lstdesg.Add(new SelectListItem() { Text = "Regular Consumption", Value = "Regular Consumption" });
                lstdesg.Add(new SelectListItem() { Text = "Erection Work", Value = "Erection Work" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindPurType()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "CONSUMABLES PURCHASE", Value = "CONSUMABLES PURCHASE" });
                lstdesg.Add(new SelectListItem() { Text = "FIXED PURCHASE", Value = "FIXED PURCHASE" });
                lstdesg.Add(new SelectListItem() { Text = "MACHINERIES PURCHASE", Value = "MACHINERIES PURCHASE" });
                lstdesg.Add(new SelectListItem() { Text = "RAW MATERIAL", Value = "RAW MATERIAL" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> GetStoreLoc()
        {
            try
            {
                DataTable dtDesg = PurIndent.GetSLocation();
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
        public JsonResult GetItemJSON(string itemid)
        {
            EnqItem model = new EnqItem();
            model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));

        }
        public JsonResult GetItemGrpJSON()
        {
            //EnqItem model = new EnqItem();
            //model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItemGrplst());
        }
        public JsonResult GetLocGSON()
        {
     
            return Json(GetLoc());
        }

        public List<SelectListItem> GetLoc()
        {
            try
            {
                DataTable dtDesg = PurIndent.GetLocation();
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
        public List<SelectListItem> BindItemlst(string value)
        {
            try
            {
                DataTable dtDesg = PurIndent.GetItem(value);
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
                DataTable dtDesg = PurIndent.GetItemSubGrp();
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
        public ActionResult GetItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                string QC = "";
                string unit = "";
                dt = PurIndent.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["QCYNTEMP"].ToString() == "" || string.IsNullOrEmpty(dt.Rows[0]["QCYNTEMP"].ToString()))
                    {
                        QC = "NO";
                    }
                    else
                    {
                        QC = dt.Rows[0]["QCYNTEMP"].ToString();
                    }
                    
                    unit = dt.Rows[0]["UNITID"].ToString();
                }

                var result = new { QC = QC, unit = unit};
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult List_Purchase_Indent()
        {
            return View();
        }
    }
}

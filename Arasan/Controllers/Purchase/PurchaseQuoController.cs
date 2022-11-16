using System.Collections.Generic;
using Arasan.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Models;

namespace Arasan.Controllers
{
    public class PurchaseQuoController : Controller
    {
        IPurchaseQuo PurquoService;
        public PurchaseQuoController(IPurchaseQuo _PurquoService)
        {
            PurquoService = _PurquoService;
        }
        public IActionResult PurchaseQuotation(string id)
        {
            PurchaseQuo ca = new PurchaseQuo();
            ca.Brlst = BindBranch();
            ca.Suplst = BindSupplier();
            ca.Curlst = BindCurrency();

            List<QoItem> Data = new List<QoItem>();
            QoItem tda = new QoItem();
            for (int i = 0; i < 3; i++)
            {
                tda = new QoItem();
                tda.ItemGrouplst = BindItemGrplst();
                tda.Ilst = BindItemlst("");
                tda.Isvalid = "Y";
                Data.Add(tda);
            }

            ca.QoLst = Data;
            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();
                dt = PurquoService.GetPurQuotationByName(id);
                if(dt.Rows.Count > 0)
                {
                    //ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    //ca.Enqdate = dt.Rows[0]["ENQDATE"].ToString();
                    //ca.Supplier = dt.Rows[0]["PARTYMASTID"].ToString();
                    //ca.EnqNo = dt.Rows[0]["ENQNO"].ToString();
                    //ca.ID = id;
                    //ca.ParNo = dt.Rows[0]["PARTYREFNO"].ToString();
                    //ca.Cur = dt.Rows[0]["CURRENCYID"].ToString();
                    //ca.ExRate = dt.Rows[0]["EXCRATERATE"].ToString();
                    //ca.RefNo = dt.Rows[0]["ENQREF"].ToString();
                }
                //ca = PurquoService.GetPurQuotationById(id);

            }
          
            return View(ca);

        }
        [HttpPost]
        public ActionResult PurchaseQuotation(PurchaseQuo Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = PurquoService.PurQuotationCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "PurchaseQuo Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "PurchaseQuo Updated Successfully...!";
                    }
                    return RedirectToAction("ListPurchaseQuo");
                }

                else
                {
                    ViewBag.PageTitle = "Edit PurchaseQuotation";
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
        public IActionResult ListPurchaseQuo()
        {
            IEnumerable<PurchaseQuo> cmp = PurquoService.GetAllPurQuotation();
            return View(cmp);
        }
        public IActionResult PurchaseQuotationFollowup()
        {
            return View();
        }
        public IActionResult ViewQuote(string id)
        {
            PurchaseQuo ca = new PurchaseQuo();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            dt = PurquoService.GetPurQuotationByName(id);
            if(dt.Rows.Count > 0)
            {
                ca.Supplier = dt.Rows[0]["PARTY"].ToString();
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.QuoId = dt.Rows[0]["DOCID"].ToString();
                ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                ca.EnqNo = dt.Rows[0]["ENQNO"].ToString();
                ca.EnqDate = dt.Rows[0]["ENQDATE"].ToString();
                ca.ID= id;
            }
            List<QoItem> Data = new List<QoItem>();
            QoItem tda = new QoItem();
            double tot = 0;
            dtt = PurquoService.GetPurQuoteItem(id);
            if(dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new QoItem();
                    tda.ItemId= dtt.Rows[i]["ITEMID"].ToString();
                    tda.Unit = dtt.Rows[i]["UNITID"].ToString();
                    tda.Quantity = Convert.ToDouble(dtt.Rows[i]["QTY"].ToString());
                    tda.rate = Convert.ToDouble(dtt.Rows[i]["Rate"].ToString());
                    tda.TotalAmount = tda.Quantity * tda.rate;
                    tot += tda.TotalAmount;
                    Data.Add(tda);
                }
            }
            ca.TotalAmount=tot;
            ca.QoLst = Data;
            return View(ca);
        }
        [HttpPost]
        public ActionResult ViewQuote(PurchaseQuo Cy, string id)
        {
            try
            {
                Cy.ID = id;
                string Strout = PurquoService.QuotetoPO(Cy.ID);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "PO Generated Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "PO Generated Successfully...!";
                    }
                    return RedirectToAction("ListPurchaseQuo");
                }

                else
                {
                    ViewBag.PageTitle = "Edit PurchaseQuotation";
                    TempData["notice"] = Strout;
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("ListPurchaseQuo");
        }
        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = PurquoService.GetBranch();
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

        public List<SelectListItem> BindSupplier()
        {
            try
            {
                DataTable dtDesg = PurquoService.GetSupplier();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTY"].ToString(), Value = dtDesg.Rows[i]["PARTYMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindCurrency()
        {
            try
            {
                DataTable dtDesg = PurquoService.GetCurency();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["Cur"].ToString(), Value = dtDesg.Rows[i]["CURRENCYID"].ToString() });
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
                DataTable dtDesg = PurquoService.GetItem(value);
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
                DataTable dtDesg = PurquoService.GetItemGrp();
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
            QoItem model = new QoItem();
            model.Ilst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));

        }
        public ActionResult GetItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                string Desc = "";
                string unit = "";
              
                dt = PurquoService.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    Desc = dt.Rows[0]["ITEMDESC"].ToString();
                    unit = dt.Rows[0]["UNITID"].ToString();
                    
                   
                }

                var result = new { Desc = Desc, unit = unit};
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemGrpJSON()
        {
            //EnqItem model = new EnqItem();
            //model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItemGrplst());
        }
    }
}

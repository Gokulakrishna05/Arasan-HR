using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Arasan.Interface;

using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Models;


namespace Arasan.Controllers
{
    public class DirectPurchaseController : Controller
    {
        IDirectPurchase directPurchase;
        DataTransactions datatrans;
        public DirectPurchaseController(IDirectPurchase _directPurchase)
        {
            directPurchase = _directPurchase;
        }
        public IActionResult DirectPurchase(string id)
        {
            DirectPurchase ca = new DirectPurchase();
            ca.Brlst = BindBranch();
            ca.Suplst = BindSupplier();
            ca.Curlst = BindCurrency();
            ca.Loclst = GetLoc();
            List<DirItem> TData = new List<DirItem>();
            DirItem tda = new DirItem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new DirItem();
                    tda.ItemGrouplst = BindItemGrplst();
                    tda.Itemlst = BindItemlst("");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {

               // ca = directPurchase.GetDirectPurById(id);




            }
            ca.DirLst = TData;
            return View(ca);

        }

        [HttpPost]
        public ActionResult DirectPurchase(DirectPurchase Cy, string id)
        {

            try
            {
                Cy.DPId = id;
                string Strout = directPurchase.DirectPurCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.DPId == null)
                    {
                        TempData["notice"] = "DirectPurchase Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "DirectPurchase Updated Successfully...!";
                    }
                    return RedirectToAction("ListDirectPurchase");
                }

                else
                {
                    ViewBag.PageTitle = "Edit DirectPurchase";
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
        public IActionResult ListDirectPurchase()
        {
            IEnumerable<DirectPurchase> cmp = directPurchase.GetAllDirectPur();
            return View(cmp);
        }
        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = directPurchase.GetBranch();
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
                DataTable dtDesg = directPurchase.GetSupplier();
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
                DataTable dtDesg = directPurchase.GetCurency();
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
        public List<SelectListItem> GetLoc()
        {
            try
            {
                DataTable dtDesg = directPurchase.GetLocation();

               
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
                DataTable dtDesg = directPurchase.GetItem(value);
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
                DataTable dtDesg = directPurchase.GetItemGrp();
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

        public ActionResult GetItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                
                string unit = "";
                string CF = "";
                string price = "";
                dt = directPurchase.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                 
                    unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();
                    dt1 = directPurchase.GetItemCF(ItemId, dt.Rows[0]["UNITMASTID"].ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        CF = dt1.Rows[0]["CF"].ToString();
                    }
                }

                var result = new { unit = unit, CF = CF, price = price };
                return Json(result);
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

        public IActionResult Index()
        {
            return View();
        }
    }
}

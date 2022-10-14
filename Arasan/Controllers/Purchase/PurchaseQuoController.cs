using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;


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
            if (id == null)
            {

            }
            else
            {
                ca = PurquoService.GetPurQuotationById(id);

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
                        TempData["notice"] = "PurchaseQuotation Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "PurchaseQuotation Updated Successfully...!";
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

      

       
       
        public IActionResult ListPurchaseQuo()
        {
            IEnumerable<PurchaseQuo> cmp = PurquoService.GetAllPurQuotation();
            return View(cmp);
        }
        public IActionResult PurchaseQuotationFollowup()
        {
            return View();
        }
    }
}

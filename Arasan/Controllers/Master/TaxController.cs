using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class TaxController : Controller
    {
        ITaxService TaxService;
        IConfiguration? _configuration;
        private string? _connectionString;
        public TaxController(ITaxService _TaxService, IConfiguration _configuration)
        {
        TaxService = _TaxService;
        }
        public IActionResult Tax(string id)
        {
            Tax ca = new Tax();
            ca.Taxtypelst = BindTaxtype();

            if (id != null)
            {
                DataTable dt = new DataTable();
                double total = 0;
                dt = TaxService.GetTax(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Taxtype = dt.Rows[0]["Tax"].ToString();
                    ca.Percentage = dt.Rows[0]["PERCENTAGE"].ToString();

                }
            }
    
            return View(ca);
        }

        public List<SelectListItem> BindTaxtype()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "CGST", Value = "CGST" });
                lstdesg.Add(new SelectListItem() { Text = "SGST", Value = "SGST" });
                lstdesg.Add(new SelectListItem() { Text = "IGST", Value = "IGST" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Tax(Tax Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = TaxService.TaxCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Tax Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Tax Updated Successfully...!";
                    }
                    return RedirectToAction("ListTax");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Tax";
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
        public IActionResult ListTax()
        {
            IEnumerable<Tax> cmp = TaxService.GetAllTax();
            return View(cmp);
        }

        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = TaxService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListTax");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListTax");
            }
        }
    }
}
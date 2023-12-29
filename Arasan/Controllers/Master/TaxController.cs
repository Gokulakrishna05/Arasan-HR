using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Master;
//using DocumentFormat.OpenXml.Bibliography;
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
            ca.createby = Request.Cookies["UserId"];
            ca.Taxtypelst = BindTaxtype();

            if (id != null)
            {
                DataTable dt = new DataTable();
                //double total = 0;
                dt = TaxService.GetTax(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Taxtype = dt.Rows[0]["TAX"].ToString();
                    ca.Percentage = dt.Rows[0]["PERCENTAGE"].ToString();  
                    ca.ID = id;
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
            return View();
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
        
        public ActionResult Remove(string tag, int id)
        {

            string flag = TaxService.RemoveChange(tag, id);
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

        public ActionResult MyListItemgrid(string strStatus)
        {
            List<Taxgrid> Reg = new List<Taxgrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = TaxService.GetAllTax(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    EditRow = "<a href=Tax?id=" + dtUsers.Rows[i]["TAXMASTID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["TAXMASTID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                }
                else
                {

                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["TAXMASTID"].ToString() + "><img src='../Images/close_icon.png' alt='Deactivate' /></a>";

                }
                
                Reg.Add(new Taxgrid
                {
                    id = dtUsers.Rows[i]["TAXMASTID"].ToString(),
                    tax = dtUsers.Rows[i]["TAX"].ToString(),
                    percentage = dtUsers.Rows[i]["PERCENTAGE"].ToString(),
                    editrow = EditRow,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }
    }
}
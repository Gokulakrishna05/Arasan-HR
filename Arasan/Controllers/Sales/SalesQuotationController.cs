using Microsoft.AspNetCore.Mvc;
using Arasan.Models;
using Arasan.Interface.Sales;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Controllers.Sales
{
    public class SalesQuotationController : Controller
    {
        ISalesQuotationService SalesQuotationService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public SalesQuotationController(ISalesQuotationService _SalesQuotationService, IConfiguration _configuratio)
        {
            SalesQuotationService = _SalesQuotationService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult SalesQuotation(string id)
        {
            SalesQuotation ca = new SalesQuotation();
            ca.Brlst = BindBranch();
            ca.assignList = BindEmp();
            ca.Curlst = BindCurrency();
            ca.Categorylst = BindCategory();
            if (id == null)
            {

            }
            else
            {
                //ca =SalesQuotationService.GetLocationsById(id);

            }
            return View(ca);
        }
        [HttpPost]
        public ActionResult SalesQuotation(SalesQuotation Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = SalesQuotationService.SalesQuotationCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "SalesQuotation Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "SalesQuotation Updated Successfully...!";
                    }
                    return RedirectToAction("ListSalesQuotation");
                }

                else
                {
                    ViewBag.PageTitle = "Edit SalesQuotation";
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
        public IActionResult ListSalesQuotation()
        {
            IEnumerable<SalesQuotation> cmp = SalesQuotationService.GetAllSalesQuotation();
            return View(cmp);
        }
        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = SalesQuotationService.GetBranch();
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
        public List<SelectListItem> BindCurrency()
        {
            try
            {
                DataTable dtDesg = datatrans.GetCurency();
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
        public List<SelectListItem> BindCategory()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "GEMAIL", Value = "GMAIL" });
                lstdesg.Add(new SelectListItem() { Text = "COURIER", Value = "COURIER" });
                lstdesg.Add(new SelectListItem() { Text = "MESSAGE", Value = "MESSAGE" });
               

                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

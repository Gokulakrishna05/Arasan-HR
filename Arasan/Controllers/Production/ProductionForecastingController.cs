using System.Collections.Generic;
using Arasan.Interface.Master;
using System.Data;
using System.Security.Cryptography.Pkcs;
using System.Xml.Linq;
using Arasan.Interface.Production;
using Arasan.Models;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Arasan.Services.Store_Management;

namespace Arasan.Controllers.Production
{
    public class ProductionForecastingController : Controller
    {
        IProductionForecastingService ProductionForecastingService;

        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public ProductionForecastingController(IProductionForecastingService _ProductionForecastingService, IConfiguration _configuratio)
        {
            ProductionForecastingService = _ProductionForecastingService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ProductionForecasting(string id)
        {
            ProductionForecasting ca = new ProductionForecasting();
            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            ca.RecList = BindEmp();
            List<PFCItem> TData = new List<PFCItem>();
            PFCItem tda = new PFCItem();
            List<PFCDGItem> TData1 = new List<PFCDGItem>();
            PFCDGItem tda1 = new PFCDGItem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new PFCItem();
                    tda.Itemlst = BindItemlst("");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
                for (int i = 0; i < 3; i++)
                {
                    tda1 = new PFCDGItem();
                    tda1.PItemlst = BindItemlst("");
                    tda1.Isvalid = "Y";
                    TData1.Add(tda1);
                }
            }
            else
            {
                DataTable dt = new DataTable();

                dt = ProductionForecastingService.GetPFDeatils(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.PType = dt.Rows[0]["PLANTYPE"].ToString();
                    ca.ForMonth = dt.Rows[0]["MONTH"].ToString();
                    ca.Ins = dt.Rows[0]["INCDECPER"].ToString();
                    ca.Hd = dt.Rows[0]["HD"].ToString();
                    ca.Fordate = dt.Rows[0]["FINYRPST"].ToString();
                    ca.Enddate = dt.Rows[0]["FINYRPED"].ToString();
                    ca.Enterd = dt.Rows[0]["ENTEREDBY"].ToString();
                    ca.ID = id;

                }
                DataTable dt2 = new DataTable();

                dt2 = ProductionForecastingService.GetProdForecastDetail(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new PFCItem();
                        tda.Itemlst = BindItemlst(tda.ItemId);
                        tda.ItemId = dt2.Rows[0]["ITEMID"].ToString();
                        tda.Unit = dt2.Rows[0]["UNIT"].ToString();
                        tda.PType = dt2.Rows[0]["PTYPE"].ToString();
                        tda.PysQty = dt2.Rows[0]["PREVYQTY"].ToString();
                        tda.PtmQty = dt2.Rows[0]["PREVMQTY"].ToString();
                        tda.Fqty = dt2.Rows[0]["PQTY"].ToString();
                        tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.ID = id;
                        TData.Add(tda);
                    }

                }
                DataTable dt3 = new DataTable();

                dt3 = ProductionForecastingService.GetProdForecastDGPasteDetail(id);
                if (dt3.Rows.Count > 0)
                {
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        tda1 = new PFCDGItem();
                        tda1.PItemlst = BindItemlst(tda1.ItemId);
                        tda1.ItemId = dt3.Rows[0]["DGITEMID"].ToString();
                        tda1.Target = dt3.Rows[0]["DGTARQTY"].ToString();
                        tda1.Min = dt3.Rows[0]["DGMIN"].ToString();
                        tda1.Stock = dt3.Rows[0]["DGSTOCK"].ToString();
                        tda1.Required = dt3.Rows[0]["REQDG"].ToString();
                        tda1.DgAdditID = dt3.Rows[0]["DGADDITID"].ToString();
                        tda1.ReqAdditive = dt3.Rows[0]["DGADDITREQ"].ToString();
                        tda1.RawMaterial = dt3.Rows[0]["DGRAWMAT"].ToString();
                        tda1.ReqPyro = dt3.Rows[0]["DGREQAP"].ToString();
                        tda1.saveItemId = dt3.Rows[i]["ITEMID"].ToString();
                        tda1.ID = id;
                        TData1.Add(tda1);
                    }

                }

            }

            ca.PFCILst = TData;
            ca.PFCDGILst = TData1;
            return View(ca);
        }
        [HttpPost]
        public ActionResult ProductionForecasting(ProductionForecasting Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = ProductionForecastingService.ProductionForecastingCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "ProductionForecasting Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "ProductionForecasting Updated Successfully...!";
                    }
                    return RedirectToAction("ListProductionForecasting");
                }

                else
                {
                    ViewBag.PageTitle = "Edit ProductionForecasting";
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
        public IActionResult ListProductionForecasting()
        {
            IEnumerable<ProductionForecasting> cmp = ProductionForecastingService.GetAllProductionForecasting();
            return View(cmp);
        }
        public ActionResult GetItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
               

                string unit = "";
                dt = datatrans.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    unit = dt.Rows[0]["UNITID"].ToString();
                    
                }

                var result = new { unit = unit };
                return Json(result);
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
                DataTable dtDesg = datatrans.GetItem(value);
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
        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = datatrans.GetBranch();
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
                DataTable dtDesg = datatrans.GetEmp();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPNAME"].ToString(), Value = dtDesg.Rows[i]["EMPNAME"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

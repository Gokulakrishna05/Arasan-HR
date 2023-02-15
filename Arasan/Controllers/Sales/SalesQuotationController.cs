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
            ca.cuntylst = BindCountry();
            List<QuoItem> Data = new List<QuoItem>();
            QuoItem tda = new QuoItem();
            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();
                double total = 0;
                dt = SalesQuotationService.GetSalesQuotation(id);
                if (dt.Rows.Count > 0)
                {
                    //ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    //ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                    //ca.Supplier = dt.Rows[0]["PARTYID"].ToString();
                    //ca.EnqNo = dt.Rows[0]["ENQNO"].ToString();
                    //ca.EnqDate = dt.Rows[0]["ENQDATE"].ToString();
                    //ca.ExRate = dt.Rows[0]["EXRATE"].ToString();
                    //ca.ID = id;
                    ////ca.ParNo = dt.Rows[0]["PARTYREFNO"].ToString();
                    //ca.Currency = dt.Rows[0]["MAINCURRENCY"].ToString();
                    ////ca.ExRate = dt.Rows[0]["EXCRATERATE"].ToString();
                    //ca.QuoId = dt.Rows[0]["DOCID"].ToString();
                }
                //ca =SalesQuotationService.GetLocationsById(id);
                DataTable dt2 = new DataTable();
                dt2 = SalesQuotationService.GetSalesQuotationItemDetails(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new QuoItem();
                        double toaamt = 0;
                        tda.ItemGrouplst = BindItemGrplst();
                        DataTable dt3 = new DataTable();
                        dt3 = datatrans.GetItemSubGroup(dt2.Rows[i]["ITEMID"].ToString());
                        if (dt3.Rows.Count > 0)
                        {
                            tda.ItemGroupId = dt3.Rows[0]["SUBGROUPCODE"].ToString();
                        }
                        tda.Ilst = BindItemlst(tda.ItemGroupId);
                        tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();
                        DataTable dt4 = new DataTable();
                        dt4 = datatrans.GetItemDetails(tda.ItemId);
                        if (dt4.Rows.Count > 0)
                        {
                            tda.Desc = dt4.Rows[0]["ITEMDESC"].ToString();
                            tda.ConsFa = dt4.Rows[0]["CF"].ToString();
                            tda.rate = Convert.ToDouble(dt4.Rows[0]["LATPURPRICE"].ToString());
                        }
                        tda.Quantity = Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                        toaamt = tda.rate * tda.Quantity;
                        total += toaamt;
                        //tda.QtyPrim= Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                        tda.TotalAmount = toaamt;
                        tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                        //tda.unitprim= dt2.Rows[i]["UNITID"].ToString();
                        tda.Isvalid = "Y";
                        Data.Add(tda);
                    }
                }
                ca.Net = Math.Round(total, 2);
                ca.QuoLst = Data;
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
        public ActionResult GetItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                string Desc = "";
                string unit = "";
                string CF = "";
                string price = "";
                dt = datatrans.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    Desc = dt.Rows[0]["ITEMDESC"].ToString();
                    unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();
                    dt1 = SalesQuotationService.GetItemCF(ItemId, dt.Rows[0]["UNITMASTID"].ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        CF = dt1.Rows[0]["CF"].ToString();
                    }
                }

                var result = new { Desc = Desc, unit = unit, CF = CF, price = price };
                return Json(result);
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
        public List<SelectListItem> BindItemGrplst()
        {
            try
            {
                DataTable dtDesg = datatrans.GetItemSubGrp();
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
        public List<SelectListItem> BindCountry()
        {
            try
            {
                DataTable dtDesg = SalesQuotationService.Getcountry();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["COUNTRYNAME"].ToString(), Value = dtDesg.Rows[i]["COUNTRYMASTID"].ToString() });
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

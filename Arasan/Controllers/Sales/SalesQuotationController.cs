using Microsoft.AspNetCore.Mvc;
using Arasan.Models;
using Arasan.Interface.Sales;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Collections.Generic;
using System.Xml.Linq;
using AspNetCore.Reporting;
using System.Reflection;
using Arasan.Interface;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using Arasan.Services.Production;

namespace Arasan.Controllers.Sales
{
    public class SalesQuotationController : Controller
    {
        ISalesQuotationService SalesQuotationService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        DataTransactions datatrans;
        public SalesQuotationController(ISalesQuotationService _SalesQuotationService, IConfiguration _configuratio, IWebHostEnvironment WebHostEnvironment)
        {
            this._WebHostEnvironment = WebHostEnvironment;
            SalesQuotationService = _SalesQuotationService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult SalesQuotation(string id)
        {
            SalesQuotation ca = new SalesQuotation();
            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Suplst = BindSupplier();
            ca.RecList = BindEmp();
            ca.assignList = BindEmp();
            ca.Curlst = BindCurrency();
            ca.Prilst = BindPriority();
            ca.Categorylst = BindCategory();
            ca.cuntylst = BindCountry();
            ca.Enqlst = BindEnqType();
            ca.Typelst = BindCusType();
            //ca.CusNamelst = BindCusName();
            ca.QuoteFormatList = BindQuoteFormat();
            ca.EnquiryList = BindEnquiry();
            List<QuoItem> TData = new List<QuoItem>();
            QuoItem tda = new QuoItem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new QuoItem();
                    tda.ItemGrouplst = BindItemGrplst();
                    tda.Itlst = BindItemlst("");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                DataTable dt = new DataTable();
                double total = 0;
                //ca.EnquiryList = BindEnquiry(ca.EnquiryId);
                dt = SalesQuotationService.GetSalesQuotation(id);

                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    ca.QuoteFormatId = dt.Rows[0]["NEW_EXISTINGQUOTE"].ToString();
                    ca.EnquiryId = dt.Rows[0]["FROM_ENQUIRY"].ToString();

                    ca.EnqType = dt.Rows[0]["QUOTETYPE"].ToString();
                    ca.QuoDate = dt.Rows[0]["QUOTE_DATE"].ToString();
                    
                    ca.Currency = dt.Rows[0]["CURRENCY_TYPE"].ToString();
                    ca.Customer = dt.Rows[0]["CUSTOMER"].ToString();
                    ca.CustomerType = dt.Rows[0]["CUSTOMER_TYPE"].ToString();
                    ca.Address = dt.Rows[0]["ADDRESS"].ToString();
                    ca.PinCode = dt.Rows[0]["PINCODE"].ToString();
                    ca.Gmail = dt.Rows[0]["CONTACT_PERSON_MAIL"].ToString();
                    ca.Mobile = dt.Rows[0]["CONTACT_PERSON_MOBILE"].ToString();
                    ca.Country = dt.Rows[0]["COUNTRYSHIPMENT"].ToString();
                    ca.Pro = dt.Rows[0]["PRIORITY"].ToString();
                    ca.narr = dt.Rows[0]["NARRATION"].ToString();
                    ca.Assign = dt.Rows[0]["ASSIGNED_TO"].ToString();
                    
                    ca.ID = id;

                    ca.gross = Convert.ToDouble(dt.Rows[0]["GROSS"].ToString() == "" ? "0" : dt.Rows[0]["GROSS"].ToString());
                    ca.net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());
                    //ViewBag.QuoId = id;

                }

                //DataTable dt2 = new DataTable();
                //dt2 = SalesQuotationService.GetSalesQuotationItemDetails(id);
                //if (dt2.Rows.Count > 0)
                //{
                //for (int i = 0; i < dt2.Rows.Count; i++)
                //{
                //    tda = new QuoItem();
                //    double toaamt = 0;
                //    tda.Itlst = BindItemlst(tda.itemid);

                //    tda.itemid = dt2.Rows[i]["ITEMID"].ToString();
                //    tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();

                //    tda.des = dt2.Rows[i]["ITEMDESC"].ToString();
                //    tda.confac = dt2.Rows[i]["CF"].ToString();
                //    tda.rate = dt2.Rows[i]["RATE"].ToString();
                //    tda.quantity = dt2.Rows[i]["QTY"].ToString();
                //    tda.unit = dt2.Rows[i]["UNIT"].ToString();
                //    tda.amount = dt2.Rows[i]["AMOUNT"].ToString();

                //    //tda.disc = dt2.Rows[i]["DISC"].ToString();
                //    //tda.discamount = dt2.Rows[i]["DISCAMOUNT"].ToString();
                //    //tda.frigcharge = dt2.Rows[i]["IFREIGHTCH"].ToString();
                //    //tda.totalamount = dt2.Rows[i]["TOTAMT"].ToString();
                //    //tda.cgstp = dt2.Rows[i]["CGSTPER"].ToString();
                //    //tda.sgstp = dt2.Rows[i]["SGSTPER"].ToString();
                //    //tda.igstp = dt2.Rows[i]["IGSTPER"].ToString();
                //    //tda.cgst = dt2.Rows[i]["CGSTAMT"].ToString();
                //    //tda.sgst = dt2.Rows[i]["SGSTAMT"].ToString();
                //    //tda.igst = dt2.Rows[i]["IGSTAMT"].ToString();

                //    tda.disc = Convert.ToDouble(dt2.Rows[0]["DISC"].ToString() == "" ? "0" : dt2.Rows[0]["DISC"].ToString());
                //    tda.discamount = Convert.ToDouble(dt2.Rows[0]["DISCAMOUNT"].ToString() == "" ? "0" : dt2.Rows[0]["DISCAMOUNT"].ToString());
                //    tda.frigcharge = Convert.ToDouble(dt2.Rows[0]["frigcharge"].ToString() == "" ? "0" : dt2.Rows[0]["frigcharge"].ToString());
                //    tda.totalamount = Convert.ToDouble(dt2.Rows[0]["TOTAMT"].ToString() == "" ? "0" : dt2.Rows[0]["TOTAMT"].ToString());
                //    tda.cgstp = Convert.ToDouble(dt2.Rows[0]["CGSTPER"].ToString() == "" ? "0" : dt2.Rows[0]["CGSTPER"].ToString());
                //    tda.sgstp = Convert.ToDouble(dt2.Rows[0]["SGSTPER"].ToString() == "" ? "0" : dt2.Rows[0]["SGSTPER"].ToString());
                //    tda.igstp = Convert.ToDouble(dt2.Rows[0]["IGSTPER"].ToString() == "" ? "0" : dt2.Rows[0]["IGSTPER"].ToString());
                //    tda.cgst = Convert.ToDouble(dt2.Rows[0]["CGSTAMT"].ToString() == "" ? "0" : dt2.Rows[0]["CGSTAMT"].ToString());
                //    tda.sgst = Convert.ToDouble(dt2.Rows[0]["SGSTAMT"].ToString() == "" ? "0" : dt2.Rows[0]["SGSTAMT"].ToString());
                //    tda.igst = Convert.ToDouble(dt2.Rows[0]["IGSTAMT"].ToString() == "" ? "0" : dt2.Rows[0]["IGSTAMT"].ToString());

                //    TData.Add(tda);
                //}


                //        }
                //    }
                //    ca.QuoLst = TData;
                //    return View(ca);

                //}


                //ca = SalesQuotationService.GetLocationsById(id);
                DataTable dt2 = new DataTable();
                dt2 = SalesQuotationService.GetSalesQuotationItemDetails(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new QuoItem();
                        double toaamt = 0;
                        //tda.ItemGrouplst = BindItemGrplst();
                        DataTable dt3 = new DataTable();
                        //dt3 = datatrans.GetItemSubGroup(dt2.Rows[i]["ITEMID"].ToString());
                        //if (dt3.Rows.Count > 0)
                        //{
                        //    tda.ItemGroupId = dt3.Rows[0]["SUBGROUPCODE"].ToString();
                        //}
                        tda.Itlst = BindItemlst(tda.itemid);
                        tda.itemid = dt2.Rows[i]["ITEMID"].ToString();
                        tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();
                        DataTable dt4 = new DataTable();
                        dt4 = datatrans.GetItemDetails(tda.itemid);
                        if (dt4.Rows.Count > 0)
                        {
                            tda.des = dt4.Rows[0]["ITEMDESC"].ToString();
                            tda.confac = dt4.Rows[0]["CF"].ToString();
                            tda.rate = Convert.ToDouble(dt4.Rows[0]["LATPURPRICE"].ToString());
                        }
                        tda.quantity = Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                        toaamt = tda.rate * tda.quantity;
                        total += toaamt;
                        //tda.QtyPrim= Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                        tda.amount = toaamt;
                        tda.unit = dt2.Rows[i]["UNIT"].ToString();
                        //tda.unitPrim = dt2.Rows[i]["UNITID"].ToString();
                        tda.disc = Convert.ToDouble(dt2.Rows[i]["DISC"].ToString() == "" ? "0" : dt2.Rows[i]["DISC"].ToString());
                        tda.discamount = Convert.ToDouble(dt2.Rows[i]["DISCAMOUNT"].ToString() == "" ? "0" : dt2.Rows[i]["DISCAMOUNT"].ToString());
                        tda.frigcharge = Convert.ToDouble(dt2.Rows[i]["IFREIGHTCH"].ToString() == "" ? "0" : dt2.Rows[i]["IFREIGHTCH"].ToString());
                        tda.totalamount = Convert.ToDouble(dt2.Rows[i]["TOTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["TOTAMT"].ToString());
                        tda.cgstp = Convert.ToDouble(dt2.Rows[i]["CGSTPER"].ToString() == "" ? "0" : dt2.Rows[i]["CGSTPER"].ToString());
                        tda.sgstp = Convert.ToDouble(dt2.Rows[i]["SGSTPER"].ToString() == "" ? "0" : dt2.Rows[i]["SGSTPER"].ToString());
                        tda.igstp = Convert.ToDouble(dt2.Rows[i]["IGSTPER"].ToString() == "" ? "0" : dt2.Rows[i]["IGSTPER"].ToString());
                        tda.cgst = Convert.ToDouble(dt2.Rows[i]["CGSTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["CGSTAMT"].ToString());
                        tda.sgst = Convert.ToDouble(dt2.Rows[i]["SGSTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["SGSTAMT"].ToString());
                        tda.igst = Convert.ToDouble(dt2.Rows[i]["IGSTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["IGSTAMT"].ToString());
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }
                //ca.Net = Math.Round(total, 2);
                //ca.QuoLst = Data;
            }
            ca.QuoLst = TData;
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
        public IActionResult ListSalesQuotation(string status)
        {

            //HttpContext.Session.SetString("SalesStatus", "Y");
            IEnumerable<SalesQuotation> cmp = SalesQuotationService.GetAllSalesQuotation(status);
            return View(cmp);
        }
        public List<SelectListItem> BindCusType()
        {
            try
            {
                DataTable dtDesg = SalesQuotationService.GetCusType();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["CUSTOMER_TYPE"].ToString(), Value = dtDesg.Rows[i]["CUSTOMERTYPEID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public List<SelectListItem> BindCusName()
        //{
        //    try
        //    {
        //        DataTable dtDesg = SalesQuotationService.GetCusName();
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["CUSTOMER_NAME"].ToString(), Value = dtDesg.Rows[i]["CUSTOMERTYPEID"].ToString() });
        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public List<SelectListItem> BindSupplier()
        {
            try
            {
                DataTable dtDesg = SalesQuotationService.GetSupplier();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTYNAME"].ToString(), Value = dtDesg.Rows[i]["PARTYMASTID"].ToString() });
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
        public List<SelectListItem> BindEnqType()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "Phone", Value = "Phone" });
                lstdesg.Add(new SelectListItem() { Text = "Email", Value = "Email" });
                lstdesg.Add(new SelectListItem() { Text = "Personal Visit", Value = "Personal Visit" });
                lstdesg.Add(new SelectListItem() { Text = "Tender", Value = "Tender" });
                lstdesg.Add(new SelectListItem() { Text = "Lead from Principals", Value = "Lead from Principals" });
                lstdesg.Add(new SelectListItem() { Text = "Expo Lead", Value = "Expo Lead" });
                lstdesg.Add(new SelectListItem() { Text = "IndiaMart", Value = "IndiaMart " });
                lstdesg.Add(new SelectListItem() { Text = "Lab Expo", Value = "Lab Expo" });
                lstdesg.Add(new SelectListItem() { Text = "Others", Value = "Others" });


                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public ActionResult GetCustomerDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string address = "";
                string contact = "";
                string city = "";
                string pin = "";
                dt = SalesQuotationService.GetCustomerDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    address = dt.Rows[0]["ADD1"].ToString();
                    contact = dt.Rows[0]["INTRODUCEDBY"].ToString();
                    city = dt.Rows[0]["CITY"].ToString();
                    pin = dt.Rows[0]["PINCODE"].ToString();

                }

                var result = new { address = address, contact = contact, city = city, pin = pin };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult GetEnqDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string quodate = "";
                string address = "";
                string city = "";
                string pincode = "";
                string mobile = "";
                
                string custid = "";
                string custype = "";
                string quotype = "";
                string currency = "";
                string pri = "";
                string gross = "";
                string net = "";


                dt = SalesQuotationService.GetEnqDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    quodate = dt.Rows[0]["ENQ_DATE"].ToString();
                    
                    address = dt.Rows[0]["ADDRESS"].ToString();
                    city = dt.Rows[0]["CITY"].ToString();
                    pincode = dt.Rows[0]["PINCODE"].ToString();
                    mobile = dt.Rows[0]["CONTACT_PERSON_MOBILE"].ToString();
                    custid = dt.Rows[0]["CUSTOMER_NAME"].ToString();
                    custype = dt.Rows[0]["CUSTOMER_TYPE"].ToString();
                    quotype = dt.Rows[0]["ENQ_TYPE"].ToString();
                    currency = dt.Rows[0]["CURRENCY_TYPE"].ToString();
                    pri = dt.Rows[0]["PRIORITY"].ToString();
                    gross = dt.Rows[0]["GROSS"].ToString();
                    net = dt.Rows[0]["NET"].ToString();
                }

                var result = new { quodate = quodate, address = address, city = city, pincode = pincode, mobile = mobile, custid = custid, custype= custype, quotype = quotype, currency = currency, pri = pri, gross = gross, net = net };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         public JsonResult GetEnqJSON(string ItemId)
        {
            SalesQuotation model = new SalesQuotation();
            model.Enqlst = BindEnqlst(ItemId);
            return Json(BindEnqlst(ItemId));


        }

        public JsonResult GetCurrencyJSON(string ItemId)
        {
            SalesQuotation model = new SalesQuotation();
            model.Currlst = BindCurrlst(ItemId);
            return Json(BindCurrlst(ItemId));

        }

        public JsonResult GetCustypeJSON(string ItemId)
        {
            SalesQuotation model = new SalesQuotation();
            model.Custypelst = BindCustypelst(ItemId);
            return Json(BindCustypelst(ItemId));

        }
        public JsonResult GetTypeJSON(string ItemId)
        {
            SalesQuotation model = new SalesQuotation();
            model.Typelst = BindTypelst(ItemId);
            return Json(BindTypelst(ItemId));

        }
        public JsonResult GetPriJSON(string ItemId)
        {
            SalesQuotation model = new SalesQuotation();
            model.Prilst = BindPrilst(ItemId);
            return Json(BindPrilst(ItemId));

        }


        public List<SelectListItem> BindEnqlst(string value)
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();

                DataTable dtDesg = SalesQuotationService.GetQuobyId(value);
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ENQ_TYPE"].ToString(), Value = dtDesg.Rows[i]["SALESENQUIRYID"].ToString() });

                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindCurrlst(string value)
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();

                DataTable dtDesg = SalesQuotationService.GetCurrbyId(value);
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["CURRENCY_TYPE"].ToString(), Value = dtDesg.Rows[i]["SALESENQUIRYID"].ToString() });

                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindCustypelst(string value)
        {
            try
            {

                List<SelectListItem> lstdesg = new List<SelectListItem>();

                DataTable dtDesg = SalesQuotationService.GetCustypebyId(value);
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["CUSTOMER_TYPE"].ToString(), Value = dtDesg.Rows[i]["SALESENQUIRYID"].ToString() });

                }
                return lstdesg;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindTypelst(string value)
        {
            try
            {

                List<SelectListItem> lstdesg = new List<SelectListItem>();

                DataTable dtDesg = SalesQuotationService.GetTypelstbyId(value);
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["CUSTOMER_NAME"].ToString(), Value = dtDesg.Rows[i]["SALESENQUIRYID"].ToString() });

                }
                return lstdesg;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindPrilst(string value)
        {
            try
            {

                List<SelectListItem> lstdesg = new List<SelectListItem>();

                DataTable dtDesg = SalesQuotationService.GetPribyId(value);
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PRIORITY"].ToString(), Value = dtDesg.Rows[i]["SALESENQUIRYID"].ToString() });

                }
                return lstdesg;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public ActionResult AssignSession(string status)
        {
            try
            {
                HttpContext.Session.SetString("SalesStatus", status);
                string result = "";
                if (!string.IsNullOrEmpty(status))
                {
                    result = status;
                    //HttpContext.Session.SetString("SalesStatus", status);
                }
                return Json(result);
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

                string Unit = "";
                string CF = "";
                string price = "";
                string Desc = "";
                //string Desc = "";
                dt = datatrans.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    Unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();
                    dt1 = SalesQuotationService.GetItemCF(ItemId, dt.Rows[0]["UNITMASTID"].ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        CF = dt1.Rows[0]["CF"].ToString();
                        Desc = dt.Rows[0]["ITEMDESC"].ToString();
                    }
                }

                var result = new { Unit = Unit, CF = CF, Desc = Desc, price = price };
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
        public List<SelectListItem> BindEnquiry()
        {
            try
            {
                DataTable dtDesg = SalesQuotationService.GetEnquiry();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ENQ_NO"].ToString(), Value = dtDesg.Rows[i]["SALESENQUIRYID"].ToString() });
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
        public List<SelectListItem> BindPriority()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "Normal", Value = "Normal" });
                lstdesg.Add(new SelectListItem() { Text = "Urgent", Value = "Urgent" });
                lstdesg.Add(new SelectListItem() { Text = "Critical", Value = "Critical" });

                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindQuoteFormat()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "New", Value = "new" });
                lstdesg.Add(new SelectListItem() { Text = "Existing", Value = "existing" });

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
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["COUNTRY"].ToString(), Value = dtDesg.Rows[i]["COUNTRYMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult SendMail(string id)
        {
            try
            {
                datatrans = new DataTransactions(_connectionString);
                MailRequest requestwer = new MailRequest();

                requestwer.ToEmail = "deepa@icand.in";
                requestwer.Subject = "Quotations";
                string Content = "";
                IEnumerable<QuoItem> cmp = SalesQuotationService.GetAllSalesQuotationItem(id);
                Content = @"<html> 
                < head >
    < style >
                table, th, td {
                border: 1px solid black;
                    border - collapse: collapse;
                }
    </ style >
</ head >
< body >
<p>Dear Sir,</p>
</br>
  <p> Kindly arrange to send your lowest price offer for the following items through our email immediately.</p>
</br>
< table style = 'border: 1px solid black;border-collapse: collapse;' > ";



                foreach (QuoItem item in cmp)
                {
                    Content += " <tr><td>" + item.des + "</td>";
                    Content += " <td>" + item.quantity + "</td>";
                    Content += " <td>" + item.unit + "</td></tr>";
                }
                Content += "</table>";

                Content += @" </br> 
<p style='padding-left:30px;font-style:italic;'>With Regards,
</br><img src='../assets/images/Arasan_Logo.png' alt='Arasan Logo'/>
</br>N Balaji Purchase Manager
</br>The Arasan Aluminium Industries (P) Ltd.
<br/102-A

</br>
</p> ";
                Content += @" </body> 
</html> ";

                requestwer.Body = Content;
                //request.Attachments = "Yes";
                datatrans.sendemail("Test mail", Content, "kesavanmoorthi81@gmail.com", "kesavanmoorthi70@gmail.com", "aqhfevhczfrnbtgz", "587", "true", "smtp.gmail.com", "Arasan");
                return Ok();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public IActionResult Followup(string id)
        {
            QuotationFollowup cmp = new QuotationFollowup();
            cmp.EnqassignList = BindEmp();
            cmp.Followdate = DateTime.Now.ToString("dd-MMM-yyyy");
            List<QuotationFollowupDetail> TData = new List<QuotationFollowupDetail>();
            if (id == null)
            {

            }
            else
            {
                if (!string.IsNullOrEmpty(id))
                {
                    cmp.Quoteid = id;
                    DataTable dt = new DataTable();
                    dt = SalesQuotationService.GetPurchaseQuotationDetails(id);
                    if (dt.Rows.Count > 0)
                    {
                        cmp.QuoId = dt.Rows[0]["QUOTE_NO"].ToString();
                        cmp.Customer = dt.Rows[0]["PARTYNAME"].ToString();
                    }
                    DataTable dtt = new DataTable();
                    string e = cmp.QuoId;
                    dtt = SalesQuotationService.GetFollowup(e);
                    QuotationFollowupDetail tda = new QuotationFollowupDetail();

                    if (dtt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtt.Rows.Count; i++)
                        {
                            tda = new QuotationFollowupDetail();
                            tda.Followby = dtt.Rows[i]["FOLLOW_BY"].ToString();
                            tda.Followdate = dtt.Rows[i]["FOLLOW_DATE"].ToString();
                            tda.Nfdate = dtt.Rows[i]["NEXT_FOLLOW_DATE"].ToString();
                            tda.Rmarks = dtt.Rows[i]["REMARKS"].ToString();
                            tda.Enquiryst = dtt.Rows[i]["FOLLOW_STATUS"].ToString();
                            TData.Add(tda);
                        }
                    }
                }
            }
            cmp.qflst = TData;

            return View(cmp);

        }

        [HttpPost]
        public ActionResult Followup(QuotationFollowup Pf, string id)
        {

            try
            {
                Pf.FolID = id;
                string Strout = SalesQuotationService.SalesQuotationFollowupCRUD(Pf);
                //IEnumerable<QuoFollowup> cmp = PurquoService.GetAllPurchaseFollowup();
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Pf.FolID == null)
                    {
                        TempData["notice"] = "Followup Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Followup Updated Successfully...!";
                    }

                }

                else
                {
                    ViewBag.PageTitle = "Edit Followup";
                    TempData["notice"] = Strout;
                    //return View();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("Followup", new { id = Pf.Quoteid });
        }
        public JsonResult GetItemJSON(string itemid)
        {
            QuoItem model = new QuoItem();
            //model.Itlst = BindItem(itemid);
            return Json(BindItemlst(itemid));
        }
        public JsonResult GetItemGrpJSON()
        {
            QuoItem model = new QuoItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItemGrplst());
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
        public ActionResult CloseQuote(string tag, int id)
        {

            string flag = SalesQuotationService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {
                return RedirectToAction("ListSalesQuotation");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListSalesQuotation");
            }
        }
        public IActionResult ViewSQ(string id)
        {
            SalesQuotation ca = new SalesQuotation();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            dt = SalesQuotationService.GetSalesQuo(id);
            if (dt.Rows.Count > 0)
            {
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.QuoId = dt.Rows[0]["QUOTE_NO"].ToString();
                ca.QuoDate = dt.Rows[0]["QUOTE_DATE"].ToString();
                ca.EnNo = dt.Rows[0]["ENQ_NO"].ToString();
                ca.EnDate = dt.Rows[0]["ENQ_DATE"].ToString();
                ca.Currency = dt.Rows[0]["CURRENCY_TYPE"].ToString();
                //ca.Customer = dt.Rows[0]["PARTY"].ToString();
                ca.CustomerType = dt.Rows[0]["CUSTOMER_TYPE"].ToString();
                ca.Address = dt.Rows[0]["ADDRESS"].ToString();
                ca.PinCode = dt.Rows[0]["PINCODE"].ToString();
                ca.Gmail = dt.Rows[0]["CONTACT_PERSON_MAIL"].ToString();
                ca.Mobile = dt.Rows[0]["CONTACT_PERSON_MOBILE"].ToString();
                ca.Pro = dt.Rows[0]["PRIORITY"].ToString();
                ca.Assign = dt.Rows[0]["ASSIGNED_TO"].ToString();
                ca.ID = id;


            }
            List<QuoItem> Data = new List<QuoItem>();
            QuoItem tda = new QuoItem();

            double total = 0;
            dtt = SalesQuotationService.GetSalesQuoItem(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new QuoItem();

                    tda.itemid = dtt.Rows[i]["ITEMID"].ToString();
                    tda.saveItemId = dtt.Rows[i]["ITEMID"].ToString();
                    //tda.Quantity = Convert.ToDouble(dtt.Rows[i]["QTY"].ToString() == "" ? "0" : dtt.Rows[i]["QTY"].ToString());
                    //tda.Amount = Convert.ToDouble(dtt.Rows[i]["AMOUNT"].ToString() == "" ? "0" : dtt.Rows[i]["AMOUNT"].ToString());
                    //tda.Rate = Convert.ToDouble(dtt.Rows[i]["RATE"].ToString() == "" ? "0" : dtt.Rows[i]["RATE"].ToString());

                    tda.unit = dtt.Rows[i]["UNIT"].ToString();
                    //tda.unitprim= dt2.Rows[i]["UNITID"].ToString();
                    //tda.Disc = Convert.ToDouble(dtt.Rows[i]["DISC"].ToString() == "" ? "0" : dtt.Rows[i]["DISC"].ToString());
                    //tda.DiscAmount = Convert.ToDouble(dtt.Rows[i]["DISCAMOUNT"].ToString() == "" ? "0" : dtt.Rows[i]["DISCAMOUNT"].ToString());
                    //tda.FrigCharge = Convert.ToDouble(dtt.Rows[i]["IFREIGHTCH"].ToString() == "" ? "0" : dtt.Rows[i]["IFREIGHTCH"].ToString());
                    //tda.TotalAmount = Convert.ToDouble(dtt.Rows[i]["TOTAMT"].ToString() == "" ? "0" : dtt.Rows[i]["TOTAMT"].ToString());
                    //tda.CGSTP = Convert.ToDouble(dtt.Rows[i]["CGSTPER"].ToString() == "" ? "0" : dtt.Rows[i]["CGSTPER"].ToString());
                    //tda.SGSTP = Convert.ToDouble(dtt.Rows[i]["SGSTPER"].ToString() == "" ? "0" : dtt.Rows[i]["SGSTPER"].ToString());
                    //tda.IGSTP = Convert.ToDouble(dtt.Rows[i]["IGSTPER"].ToString() == "" ? "0" : dtt.Rows[i]["IGSTPER"].ToString());
                    //tda.CGST = Convert.ToDouble(dtt.Rows[i]["CGSTAMT"].ToString() == "" ? "0" : dtt.Rows[i]["CGSTAMT"].ToString());
                    //tda.SGST = Convert.ToDouble(dtt.Rows[i]["SGSTAMT"].ToString() == "" ? "0" : dtt.Rows[i]["SGSTAMT"].ToString());
                    //tda.IGST = Convert.ToDouble(dtt.Rows[i]["IGSTAMT"].ToString() == "" ? "0" : dtt.Rows[i]["IGSTAMT"].ToString());
                    tda.Isvalid = "Y";
                    Data.Add(tda);
                }
            }

            ca.QuoLst = Data;
            return View(ca);
        }
        [HttpPost]
        public ActionResult ViewSQ(SalesQuotation Cy, string id)
        {
            try
            {
                Cy.ID = id;
                string Strout = "";/*SalesQuotationService.QuotetoOrder(Cy.ID);*/
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "WorkOrder Generated Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "WorkOrder Generated Successfully...!";
                    }
                    return RedirectToAction("ListSalesQuotation");
                }

                else
                {
                    ViewBag.PageTitle = "Edit SalesQuotation";
                    TempData["notice"] = Strout;
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("ListSalesEnquiry");
        }
        public async Task<IActionResult> Print(string id)
        {
            string mimtype = "";
            int extension = 1;
            DataSet ds = new DataSet();
            var path = $"{this._WebHostEnvironment.WebRootPath}\\Reports\\SQbasic.rdlc";
            Dictionary<string, string> Parameters = new Dictionary<string, string>();

            var SQuoitem = await SalesQuotationService.GetSQuoItem(id);

            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("DataSet1", SQuoitem);
           

            var result = localReport.Execute(RenderType.Pdf, extension, Parameters, mimtype);
            return File(result.MainStream, "application/Pdf");
        }

        public ActionResult GetItemgrpDetails(string id)
        {
            SalesQuotation model = new SalesQuotation();
            DataTable dtt = new DataTable();
            List<QuoItem> Data = new List<QuoItem>();
            QuoItem tda = new QuoItem();
            dtt = SalesQuotationService.GetItemgrpDetail(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)  
                {
                    tda = new QuoItem();

                    tda.itemid = dtt.Rows[0]["ITEMID"].ToString();
                    tda.unit = dtt.Rows[0]["UNIT"].ToString();
                    
                    tda.quantity = Convert.ToDouble(dtt.Rows[0]["QUANTITY"].ToString());
                    tda.des = dtt.Rows[0]["ITEM_DESCRIPTION"].ToString();
                    tda.rate = Convert.ToDouble(dtt.Rows[0]["RATE"].ToString());
                    tda.amount = Convert.ToDouble(dtt.Rows[0]["AMOUNT"].ToString());
                    tda.confac = dtt.Rows[0]["CF"].ToString();
                    

                    Data.Add(tda);
                }
            }
            model.QuoLst = Data;
            return Json(model.QuoLst);

        }
    }
}

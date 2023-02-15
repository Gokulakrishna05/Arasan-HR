using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.Pkcs;
using System.Xml.Linq;
using Arasan.Interface;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class SalesEnquiryController : Controller
    {
       
            ISalesEnq Sales;
            IConfiguration? _configuratio;
            private string? _connectionString;

            DataTransactions datatrans;
            public SalesEnquiryController(ISalesEnq _Sales, IConfiguration _configuratio)
            {
                Sales = _Sales;
                _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
                datatrans = new DataTransactions(_connectionString);
            }
            public IActionResult Sales_Enquiry(string id )
            {
                SalesEnquiry ca = new SalesEnquiry();
                ca.Brlst = BindBranch();
                ca.Suplst = BindSupplier();
                ca.Curlst = BindCurrency();
            ca.RecList = BindEmp();
            ca.assignList = BindEmp();
            ca.Prilst = BindPriority();
            ca.Enqlst = BindEnqType();
            ca.Typelst = BindCusType();
            List<SalesItem> TData = new List<SalesItem>();
            SalesItem tda = new SalesItem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new SalesItem();

                    tda.Itemlst = BindItemlst();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {

                // ca = directPurchase.GetDirectPurById(id);


                DataTable dt = new DataTable();

                dt = Sales.GetSalesEnquiry(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCH_ID"].ToString();
                    ca.EnqNo = dt.Rows[0]["ENQ_NO"].ToString();
                    ca.EnqDate = dt.Rows[0]["ENQ_DATE"].ToString();
                    ca.City = dt.Rows[0]["CITY"].ToString();
                    ca.ID = id;
                    ca.ContactPersion = dt.Rows[0]["CONTACT_PERSON"].ToString();
                    ca.Mobile = dt.Rows[0]["CONTACT_PERSON_MOBILE"].ToString();
                    ca.Recieved = dt.Rows[0]["LEADBY"].ToString();
                    ca.Assign = dt.Rows[0]["ASSIGNED_TO"].ToString();
                    ca.Customer = dt.Rows[0]["CUSTOMER_NAME"].ToString();
                    ca.CustomerType = dt.Rows[0]["CUSTOMER_TYPE"].ToString();
                    ca.EnqType = dt.Rows[0]["ENQ_TYPE"].ToString();
                    ca.ContactPersion = dt.Rows[0]["CONTACT_PERSON"].ToString();
                    ca.PinCode = dt.Rows[0]["PINCODE"].ToString();
                    ca.Priority = dt.Rows[0]["PRIORITY"].ToString();
                    ca.Address = dt.Rows[0]["ADDRESS"].ToString();
                    ca.Currency = dt.Rows[0]["CURRENCY_TYPE"].ToString();
                }
                DataTable dt2 = new DataTable();

                dt = Sales.GetSalesEnquiryItem(id);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda.ItemId = dt2.Rows[i]["ITEM_ID"].ToString();
                        tda.saveItemId = dt2.Rows[i]["ITEM_ID"].ToString();
                        DataTable dt4 = new DataTable();
                        dt4 = Sales.GetItemDetails(tda.ItemId);
                        if (dt4.Rows.Count > 0)
                        {
                            tda.Des = dt4.Rows[0]["ITEM_DESCRIPTION"].ToString();
                           
                           
                        }
                        tda.Unit = dt2.Rows[i]["UNIT"].ToString();
                        tda.Qty = dt2.Rows[i]["QUANTITY"].ToString();
                    }
                }
            }
            ca.SalesLst = TData;
            return View(ca);

            }
        [HttpPost]
        public ActionResult Sales_Enquiry(SalesEnquiry Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = Sales.SalesEnqCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Sales_Enquiry Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Sales_Enquiry Updated Successfully...!";
                    }
                    return RedirectToAction("ListSalesEnquiry");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Sales_Enquiry";
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
        public List<SelectListItem> BindItemlst()
        {
            try
            {
                DataTable dtDesg = Sales.GetItem();
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
        public JsonResult GetItemJSON()
        {
            SalesItem model = new SalesItem();
            model.Itemlst = BindItemlst();
            return Json(BindItemlst());

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
        public List<SelectListItem> BindSupplier()
        {
            try
            {
                DataTable dtDesg = Sales.GetSupplier();
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
        public List<SelectListItem> BindCusType()
        {
            try
            {
                DataTable dtDesg = Sales.GetCusType();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["CUSTOMER_TYPE"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
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
               
                string city = "";
                string pin = "";
                dt = Sales.GetCustomerDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    address = dt.Rows[0]["ADD1"].ToString();
                   
                 
                    city = dt.Rows[0]["CITY"].ToString();
                
                   
                        pin = dt.Rows[0]["PINCODE"].ToString();
                   
                }

                var result = new { address = address, city = city, pin = pin };
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
                string Desc = "";
                string unit = "";
              
                dt = Sales.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    Desc = dt.Rows[0]["ITEMDESC"].ToString();
                    unit = dt.Rows[0]["UNITID"].ToString();
                
                   
                }

                var result = new { Desc = Desc, unit = unit };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult ListSalesEnquiry()
        {
            IEnumerable<SalesEnquiry> cmp = Sales.GetAllSalesEnq();
            return View(cmp);
        }
        public IActionResult Sales_Quotation()
        {
            return View();
        }
        public IActionResult Proforma_Invoice()
        {
            return View();
        }
        public IActionResult Excise_Invoice()
        {
            return View();
        }
        public IActionResult Supplimantry_Invoice()
        {
            return View();
        }
        public IActionResult Depot_Invoice()
        {
            return View();
        }

        public IActionResult Work_Order()
        {
            return View();
        }
        public IActionResult Work_Order_Amedment()
        {
            return View();
        }
        public IActionResult Work_Orde_ShortClose()
        {
            return View();
        }
        public IActionResult Sales_Return()
        {
            return View();
        }
        public IActionResult Debit_Note_Bill()
        {
            return View();
        }
        public IActionResult Credit_Note_Bill()
        {
            return View();
        }
        public IActionResult Credit_Note_Approval()
        {
            return View();
        }
        public IActionResult Sales_Forecasting()
        {
            return View();
        }
    }
 }

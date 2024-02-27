using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.Pkcs;
using System.Xml.Linq;
using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
using AspNetCore.Reporting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class SalesEnquiryController : Controller
    {
       
            ISalesEnq Sales;
            IConfiguration? _configuratio;
            private string? _connectionString;
            //private readonly IWebHostEnvironment _WebHostEnvironment;
        DataTransactions datatrans;
            public SalesEnquiryController(ISalesEnq _Sales, IConfiguration _configuratio, IWebHostEnvironment WebHostEnvironment)
            {
                //this._WebHostEnvironment = WebHostEnvironment;
                Sales = _Sales;
                _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
                datatrans = new DataTransactions(_connectionString);
            }
            public IActionResult Sales_Enquiry(string id )
          {
            SalesEnquiry ca = new SalesEnquiry();
            //ca.CreatedBy = Request.Cookies["UserId"];
            //ca.UpdatedBy = Request.Cookies["UserId"];

            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Suplst = BindSupplier();
            ca.Curlst = BindCurrency();
            ca.RecList = BindEmp();
            ca.assignList = BindEmp();
            ca.Assign = Request.Cookies["UserId"];
            ca.Prilst = BindPriority();
            ca.Enqlst = BindEnqType();
            ca.Typelst = BindCusType();
            ca.EnqDate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.Currency = "1";
            DataTable dtv = datatrans.GetSequence("taai");
            if (dtv.Rows.Count > 0)
            {
                ca.EnqNo = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            List<SalesItem> TData = new List<SalesItem>();
            SalesItem tda = new SalesItem();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new SalesItem();

                    //tda.ItemGrouplst = BindItemGrplst();
                    tda.Itemlst = BindItemlst();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {

                // ca = directPurchase.GetDirectPurById(id);


                DataTable dt = new DataTable();
                //double total = 0;
                dt = Sales.GetSalesEnquiry(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCH_ID"].ToString();
                    ca.EnqNo = dt.Rows[0]["ENQ_NO"].ToString();
                    ca.EnqDate = dt.Rows[0]["ENQ_DATE"].ToString();
                    ca.City = dt.Rows[0]["CITY"].ToString();
                    ca.ID = id;
                    ca.ContactPerson = dt.Rows[0]["CONTACT_PERSON"].ToString();
                    ca.Mobile = dt.Rows[0]["CONTACT_PERSON_MOBILE"].ToString();
                    ca.Recieved = dt.Rows[0]["LEADBY"].ToString();
                    ca.Assign = dt.Rows[0]["ASSIGNED_TO"].ToString();
                    ca.Customer = dt.Rows[0]["CUSTOMER_NAME"].ToString();
                    ca.CustomerType = dt.Rows[0]["CUSTOMER_TYPE"].ToString();
                    ca.EnqType = dt.Rows[0]["ENQ_TYPE"].ToString();
                    ca.ContactPerson = dt.Rows[0]["CONTACT_PERSON"].ToString();
                    ca.PinCode = dt.Rows[0]["PINCODE"].ToString();
                    ca.Priority = dt.Rows[0]["PRIORITY"].ToString();
                    ca.Address = dt.Rows[0]["ADDRESS"].ToString();
                    ca.Currency = dt.Rows[0]["CURRENCY_TYPE"].ToString();

                }
                DataTable dt2 = new DataTable();

                dt2 = Sales.GetSalesEnquiryItem(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new SalesItem();
                        tda.Itemlst = BindItemlst();
                        tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.Des = dt2.Rows[0]["ITEM_DESCRIPTION"].ToString();
                        tda.Unit = dt2.Rows[i]["UNIT"].ToString();
                        tda.Qty = dt2.Rows[i]["QUANTITY"].ToString();
                        tda.Rate = dt2.Rows[i]["RATE"].ToString();
                        tda.Amount = dt2.Rows[i]["AMOUNT"].ToString();
                        TData.Add(tda);
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
        public JsonResult GetItemJSON(string itemid)
        {
            DirItem model = new DirItem();
            model.Itemlst = BindItemlst();
            return Json(BindItemlst());

        }
        public JsonResult GetItemGrpJSON()
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
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTYNAME"].ToString(), Value = dtDesg.Rows[i]["PARTYMASTID"].ToString() });
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
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["CUSTOMER_TYPE"].ToString(), Value = dtDesg.Rows[i]["CUSTOMERTYPEID"].ToString() });
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
                string contact = "";
                string city = "";
                string pin = "";
                string mob = "";


                dt = Sales.GetCustomerDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    address = dt.Rows[0]["ADD1"].ToString();

                    contact = dt.Rows[0]["INTRODUCEDBY"].ToString();
                    city = dt.Rows[0]["CITY"].ToString();
                
                   
                        pin = dt.Rows[0]["PINCODE"].ToString();
                    mob = dt.Rows[0]["MOBILE"].ToString();
                   
                }

                var result = new { address = address, contact= contact, city = city, pin = pin , mob = mob };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetItemDetails(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                string Desc = "";
                string unit = "";
                string price = "";
                dt = Sales.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    Desc = dt.Rows[0]["ITEMDESC"].ToString();
                    unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();


                }

                var result = new { Desc = Desc, unit = unit, price= price };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      public IActionResult ViewQuote(string id)
        {
            SalesEnquiry ca = new SalesEnquiry();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            
            dt = Sales.GetEnqByName(id);
            if (dt.Rows.Count > 0)
            {
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.Customer = dt.Rows[0]["PARTYNAME"].ToString();
                ca.EnqNo = dt.Rows[0]["ENQ_NO"].ToString();
                ca.EnqDate = dt.Rows[0]["ENQ_DATE"].ToString();
                ca.EnqType = dt.Rows[0]["ENQ_TYPE"].ToString();
                ca.CustomerType = dt.Rows[0]["CUSTOMER_TYPE"].ToString();
                ca.Currency = dt.Rows[0]["CURRENCY_TYPE"].ToString();
                ca.Priority = dt.Rows[0]["PRIORITY"].ToString();
                ca.PinCode = dt.Rows[0]["PINCODE"].ToString();
                ca.Address = dt.Rows[0]["ADDRESS"].ToString();
                ca.ContactPerson = dt.Rows[0]["CONTACT_PERSON"].ToString();
                ca.City = dt.Rows[0]["CITY"].ToString();
                ca.ID = id;


                List<SalesItem> Data = new List<SalesItem>();
                SalesItem tda = new SalesItem();
                //double tot = 0;
  
                dtt = Sales.GetEnqItem(id);
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda = new SalesItem();
                        tda.ItemId = dtt.Rows[i]["ITEMID"].ToString();
                        tda.Des = dtt.Rows[i]["ITEM_DESCRIPTION"].ToString();
                        tda.Unit = dtt.Rows[i]["UNIT"].ToString();
                        tda.Qty = dtt.Rows[i]["QUANTITY"].ToString();
                        tda.ID = id;
                        // tot += tda.TotalAmount;
                        Data.Add(tda);
                    }
                }
                //ca.Net = tot;
                ca.SalesLst = Data;
            }
            return View(ca);
        }
        
       

        [HttpPost]
        public ActionResult ViewQuote(SalesEnquiry Cy, string id)
        {
            try
            {
                Cy.ID = id;
                string Strout = Sales.EnquirytoQuote(Cy.ID);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "SalesQuotation Generated Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "SalesQuotation Generated Successfully...!";
                    }
                    return RedirectToAction("ListSalesEnquiry");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Sales_Enquiry";
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
        public IActionResult ListSalesEnquiry()
        {
            //IEnumerable<SalesEnquiry> cmp = Sales.GetAllSalesEnq();
            return View();
        }
        public ActionResult MyListSalesEnquiryGrid(string strStatus)
        {
            List<SalesEnquiryItems> Reg = new List<SalesEnquiryItems>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)Sales.GetAllListSalesEnquiryItem(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string SendMail = string.Empty;
                string Followup = string.Empty;
                string Moved = string.Empty;
                string EditRow = string.Empty;
                string DeleteRow = string.Empty;

                SendMail = "<a href=SendMail?id=" + dtUsers.Rows[i]["SALESENQUIRYID"].ToString() + "><img src='../Images/mail_icon.png' alt='Send Email' /></a>";
                Followup = "<a href=Followup?id=" + dtUsers.Rows[i]["SALESENQUIRYID"].ToString() + "><img src='../Images/followup.png' alt='FollowUp' /> - (1)</a>";


                if (dtUsers.Rows[i]["STATUS"].ToString() == "Generated")
                {
                    Moved = "<img src='../Images/tick.png' alt='Moved to Quote' width='20' />";
                    EditRow = "";
                }
                else
                {
                    //Moved = dtUsers.Rows[i]["STATUS"].ToString();
                    Moved = "<a href=ViewQuote?id=" + dtUsers.Rows[i]["SALESENQUIRYID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/move_quote.png' alt='View Details' width='20' /></a>";
                    EditRow = "<a href=Sales_Enquiry?id=" + dtUsers.Rows[i]["SALESENQUIRYID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";


                }
                DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["SALESENQUIRYID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                Reg.Add(new SalesEnquiryItems
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["SALESENQUIRYID"].ToString()),
                    enqno = dtUsers.Rows[i]["ENQ_NO"].ToString(),
                    date = dtUsers.Rows[i]["ENQ_DATE"].ToString(),
                    type = dtUsers.Rows[i]["ENQ_TYPE"].ToString(),
                    sendmail = SendMail,
                    followup = Followup,
                    move = Moved,
                    editrow = EditRow,
                    delrow = DeleteRow,



                });
            }

            return Json(new
            {
                Reg
            });

        }
        public IActionResult Followup(string id)
        {
            EnqFollowup cmp = new EnqFollowup();
            cmp.EnqassignList = BindEmpl();
            cmp.Followdate = DateTime.Now.ToString("dd-MMM-yyyy");
            List<SalesEnqFollowupDetails> TData = new List<SalesEnqFollowupDetails>();
            if (id == null)
            {

            }
            else
            {
                if (!string.IsNullOrEmpty(id)) 
                {
                    DataTable dt = new DataTable();
                    dt = Sales.GetEnqDetail(id);
                    if (dt.Rows.Count > 0)
                    {
                        cmp.EnqNo = dt.Rows[0]["ENQ_NO"].ToString();
                        cmp.CusName = dt.Rows[0]["PARTYNAME"].ToString();
                    }
                    DataTable dtt = new DataTable();
                    string e = cmp.EnqNo;
                    dtt = Sales.GetFollowup(e);
                   SalesEnqFollowupDetails tda = new SalesEnqFollowupDetails();

                    if (dtt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtt.Rows.Count; i++)
                        {
                            tda = new SalesEnqFollowupDetails();
                            tda.Followby = dtt.Rows[i]["FOLLOW_BY"].ToString();
                            tda.Followdate = dtt.Rows[i]["FOLLOWDATE"].ToString();
                            tda.Nfdate = dtt.Rows[i]["NEXTFOLLOWDATE"].ToString();
                            tda.Rmarks = dtt.Rows[i]["REMARK"].ToString();
                            tda.Enquiryst = dtt.Rows[i]["FOLLOW_DETAILS"].ToString();
                           
                            TData.Add(tda);
                        }
                    }
               
                }
            }
            cmp.qflst = TData;
            return View(cmp);


        }

        [HttpPost]
        public ActionResult Followup(EnqFollowup Pf, string id)
        {

            try
            {
                Pf.FolID = id;
                string Strout = Sales.PurchaseFollowupCRUD(Pf);
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

            return RedirectToAction("Followup", new { id = id });
        }
        public List<SelectListItem> BindEmpl()
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
        public ActionResult SendMail(string id)
        {
            try
            {
                datatrans = new DataTransactions(_connectionString);
                MailRequest requestwer = new MailRequest();

                requestwer.ToEmail = "deepa@icand.in";
                requestwer.Subject = "Enquiry";
                string Content = "";
                IEnumerable<SalesItem> cmp = Sales.GetAllSalesenquriyItem(id);
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



                foreach (SalesItem item in cmp)
                {
                    Content += " <tr><td>" + item.Des + "</td>";
                    Content += " <td>" + item.Qty + "</td>";
                    Content += " <td>" + item.Unit + "</td></tr>";
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
                //request.Attachments = "No";
                datatrans.sendemail("Test mail", Content, "kesavanmoorthi81@gmail.com", "kesavanmoorthi70@gmail.com", "aqhfevhczfrnbtgz", "587", "true", "smtp.gmail.com", "IcanD");
                return Ok();

            }
            catch (Exception ex)
            {
                throw ex;
            }

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

        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = Sales.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListSalesEnquiry");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListSalesEnquiry");
            }
        }

       

    }
 }

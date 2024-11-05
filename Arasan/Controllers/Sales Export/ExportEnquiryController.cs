using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.Pkcs;
using System.Xml.Linq;
using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
using AspNetCore.Reporting;
using Intuit.Ipp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Sales_Export
{
	public class ExportEnquiryController : Controller
	{
		IExportEnquiry ExportEnquiry;
		IConfiguration? _configuratio;
		private string? _connectionString;
		DataTransactions datatrans;
		public ExportEnquiryController(IExportEnquiry _ExportEnquiry, IConfiguration _configuratio)
		{
			
			ExportEnquiry = _ExportEnquiry;
			_connectionString = _configuratio.GetConnectionString("OracleDBConnection");
			datatrans = new DataTransactions(_connectionString);
		}
		public IActionResult Export_Enquiry(string id)
		{
			ExportEnquiry ca = new ExportEnquiry();
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
			DataTable dtv = datatrans.GetSequence("vchsl");
			if (dtv.Rows.Count > 0)
			{
				ca.EnqNo = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
			}
			List<ExportItem> TData = new List<ExportItem>();
            ExportItem tda = new ExportItem();

            List<TermsItem> TData1 = new List<TermsItem>();
            TermsItem tda1 = new TermsItem();

            if (id == null)
			{
				for (int i = 0; i < 1; i++)
				{
					tda = new ExportItem();

					//tda.ItemGrouplst = BindItemGrplst();
					tda.Itemlst = BindItemlst();
					tda.Isvalid = "Y";
					TData.Add(tda);
				}
                for (int i = 0; i < 1; i++)
                {
                    tda1 = new TermsItem();

                    tda1.Tandclst = BindTandclst();
                    tda1.Condlst = BindCondlst();
                    tda1.Isvalid1 = "Y";
                    TData1.Add(tda1);
                }
            }
			else
			{

				// ca = directPurchase.GetDirectPurById(id);


				DataTable dt = new DataTable();
				//double total = 0;
				dt = ExportEnquiry.GetExportEnquiry(id);
				if (dt.Rows.Count > 0)
				{
					ca.EnqNo = dt.Rows[0]["ENQNO"].ToString();
					ca.EnqDate = dt.Rows[0]["ENQDATE"].ToString();
					ca.EnqType = dt.Rows[0]["ENQREF"].ToString();
					ca.ID = id;
					ca.Currency = dt.Rows[0]["MAINCURRENCY"].ToString();
					ca.Rate = dt.Rows[0]["EXRATE"].ToString();
					ca.Customer = dt.Rows[0]["PARTYID"].ToString();
                    DataTable dt3 = new DataTable();
                    dt3 = ExportEnquiry.GetParty(ca.Customer);
					if (dt3.Rows.Count > 0)
					{
                        ca.Address = dt3.Rows[0]["ADD1"].ToString() + "@" + dt3.Rows[0]["ADD2"].ToString() + "@" + dt3.Rows[0]["ADD3"].ToString();
                        ca.Address = ca.Address.Replace("@", "\n");
                        ca.Phone = dt3.Rows[0]["PHONENO"].ToString();
						ca.Email = dt3.Rows[0]["EMAIL"].ToString();
						ca.Mobile = dt3.Rows[0]["MOBILE"].ToString();
						ca.City = dt3.Rows[0]["CITY"].ToString();
						ca.PinCode = dt3.Rows[0]["PINCODE"].ToString();

					}
					ca.Assign = dt.Rows[0]["ASSIGNTO"].ToString();
					ca.Recieved = dt.Rows[0]["ENQRECDBY"].ToString();

					ca.Time = dt.Rows[0]["FOLLOWUPTIME"].ToString();
					ca.FollowUp = dt.Rows[0]["FOLLOWDT"].ToString();
					ca.Deatails = dt.Rows[0]["REMARKS"].ToString();
					ca.Emaildate = dt.Rows[0]["SMSDATE"].ToString();
					ca.Send = dt.Rows[0]["SENDSMS"].ToString();
					

				}
				DataTable dt2 = new DataTable();

				dt2 = ExportEnquiry.GetExportEnquiryItem(id);
				if (dt2.Rows.Count > 0)
				{
					for (int i = 0; i < dt2.Rows.Count; i++)
					{
						tda = new ExportItem();
						tda.Itemlst = BindItemlst();
						tda.ItemId = dt2.Rows[i]["ITEMDESC"].ToString();
						tda.Des = dt2.Rows[0]["ITEMDETAILS"].ToString();
						tda.Unit = dt2.Rows[i]["UNIT"].ToString();
						tda.Qty = dt2.Rows[i]["QTY"].ToString();
						TData.Add(tda);
					}
				}
			}
			ca.ExportLst = TData;
			ca.TermsLst = TData1;

			return View(ca);
		}
		[HttpPost]
		public ActionResult Export_Enquiry(ExportEnquiry Cy, string id)
		{

			try
			{
				Cy.ID = id;
				Cy.Branch = Request.Cookies["BranchId"];
				string Strout = ExportEnquiry.Export_EnquiryCRUD(Cy);
				if (string.IsNullOrEmpty(Strout))
				{
					if (Cy.ID == null)
					{
						TempData["notice"] = "ExportEnquiry Inserted Successfully...!";
					}
					else
					{
						TempData["notice"] = "ExportEnquiry Updated Successfully...!";
					}
					return RedirectToAction("ListExport_Enquiry");
				}

				else
				{
					ViewBag.PageTitle = "Edit Export_Enquiry";
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
        public List<SelectListItem> BindTandclst()
        {
            try
            {
                DataTable dtDesg = ExportEnquiry.Gettemplete();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["TEMPID"].ToString(), Value = dtDesg.Rows[i]["TANDCBASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindCondlst()
        {
            try
            {
                DataTable dtDesg = ExportEnquiry.GetCondition();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["TANDC"].ToString(), Value = dtDesg.Rows[i]["TANDCDETAILID"].ToString() });
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
		public List<SelectListItem> BindSupplier()
		{
			try
			{
				DataTable dtDesg = ExportEnquiry.GetSupplier();
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
		public List<SelectListItem> BindCusType()
		{
			try
			{
				DataTable dtDesg = datatrans.GetData("SELECT COMMON_VALUE FROM COMMONMASTER WHERE COMMON_TEXT='PARTYTYPE'");
				List<SelectListItem> lstdesg = new List<SelectListItem>();
				for (int i = 0; i < dtDesg.Rows.Count; i++)
				{
					lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["COMMON_VALUE"].ToString(), Value = dtDesg.Rows[i]["COMMON_VALUE"].ToString() });
				}
				return lstdesg;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
        public ActionResult GetExRate(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string rate = "";

                dt = ExportEnquiry.GetExRateDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    rate = dt.Rows[0]["EXRATE"].ToString();

                }

                var result = new { rate = rate };
                return Json(result);
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
                string email = "";


                dt = ExportEnquiry.GetCustomerDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    address = dt.Rows[0]["ADD1"].ToString() + "@" + dt.Rows[0]["ADD2"].ToString() + "@" + dt.Rows[0]["ADD3"].ToString();
                    address = address.Replace("@", "\n");
                    city = dt.Rows[0]["CITY"].ToString();
                    pin = dt.Rows[0]["PINCODE"].ToString();
                    contact = dt.Rows[0]["PHONENO"].ToString();
                    mob = dt.Rows[0]["MOBILE"].ToString();
                    email = dt.Rows[0]["EMAIL"].ToString();

                }

                var result = new { address = address, contact = contact, city = city, pin = pin, mob = mob , email = email };
                return Json(result);
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
                DataTable dtDesg = ExportEnquiry.GetItem();
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
        public ActionResult GetItemDetails(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                string Desc = "";
                string unit = "";
                string price = "";
                dt = ExportEnquiry.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    Desc = dt.Rows[0]["ITEMDESC"].ToString();
                    unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();


                }

                var result = new { Desc = Desc, unit = unit, price = price };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult ListExport_Enquiry()
        {
            return View();
        }
        public ActionResult MyListExportEnquiryGrid(string strStatus)
        {
            List<ExportEnquiryItems> Reg = new List<ExportEnquiryItems>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)ExportEnquiry.GetAllListExportEnquiry(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string SendMail = string.Empty;
                string ViewRow = string.Empty;
                string EditRow = string.Empty;
                string DeleteRow = string.Empty;

                if (dtUsers.Rows[i]["STATUS"].ToString() == "Y")
                {
                    SendMail = "<a href=SendMail?id=" + dtUsers.Rows[i]["EENQUIRYBASICID"].ToString() + "><img src='../Images/mail_icon.png' alt='Send Email' /></a>";
                    EditRow = "<a href=Export_Enquiry?id=" + dtUsers.Rows[i]["EENQUIRYBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    ViewRow = "<a href=ViewExportEnquiry?id=" + dtUsers.Rows[i]["EENQUIRYBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View' /></a>";
                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["EENQUIRYBASICID"].ToString() + "";

                }
                else
                {
                    DeleteRow = "DeleteMR?tag=Active&id=" + dtUsers.Rows[i]["EENQUIRYBASICID"].ToString() + "";
                }
                Reg.Add(new ExportEnquiryItems
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["EENQUIRYBASICID"].ToString()),
                    enqno = dtUsers.Rows[i]["ENQNO"].ToString(),
                    date = dtUsers.Rows[i]["ENQDATE"].ToString(),
                    type = dtUsers.Rows[i]["ENQREF"].ToString(),
                    sendmail = SendMail,
                    view = ViewRow,
                    editrow = EditRow,
                    delrow = DeleteRow,



                });
            }

            return Json(new
            {
                Reg
            });

        }
        public IActionResult ViewExportEnquiry(string id)
        {
            ExportEnquiry ca = new ExportEnquiry();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();

            dt = ExportEnquiry.GetExportEnquiryView(id);
            if (dt.Rows.Count > 0)
            {
                ca.EnqNo = dt.Rows[0]["ENQNO"].ToString();
                ca.EnqDate = dt.Rows[0]["ENQDATE"].ToString();
                ca.EnqType = dt.Rows[0]["ENQREF"].ToString();
                ca.ID = id;
                ca.Currency = dt.Rows[0]["MAINCURR"].ToString();
                ca.Rate = dt.Rows[0]["EXRATE"].ToString();
                ca.Customer = dt.Rows[0]["PARTYID"].ToString();
                DataTable dt3 = new DataTable();
                string party = datatrans.GetDataString("SELECT PARTYMASTID FROM PARTYMAST WHERE PARTYNAME='" + ca.Customer + "' ");
                dt3 = ExportEnquiry.GetPartyName(party);
                if (dt3.Rows.Count > 0)
                {
                    ca.Address = dt3.Rows[0]["ADD1"].ToString() + "@" + dt3.Rows[0]["ADD2"].ToString() + "@" + dt3.Rows[0]["ADD3"].ToString();
                    ca.Address = ca.Address.Replace("@", "\n");
                    ca.Phone = dt3.Rows[0]["PHONENO"].ToString();
                    ca.Email = dt3.Rows[0]["EMAIL"].ToString();
                    ca.Mobile = dt3.Rows[0]["MOBILE"].ToString();
                    ca.City = dt3.Rows[0]["CITY"].ToString();
                    ca.PinCode = dt3.Rows[0]["PINCODE"].ToString();

                }
                ca.Assign = dt.Rows[0]["EMPNAME"].ToString();
                ca.Recieved = dt.Rows[0]["ENQRECDBY"].ToString();
                ca.ID = id;


                List<ExportItem> TData = new List<ExportItem>();
                ExportItem tda = new ExportItem();
                //double tot = 0;

                dtt = ExportEnquiry.GetExportItem(id);
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda = new ExportItem();
                        tda.Itemlst = BindItemlst();
                        tda.ItemId = dtt.Rows[i]["ITEMID"].ToString();
                        tda.Des = dtt.Rows[0]["ITEMDETAILS"].ToString();
                        tda.Unit = dtt.Rows[i]["UNIT"].ToString();
                        tda.Qty = dtt.Rows[i]["QTY"].ToString();
                        TData.Add(tda);
                    }
                }
                //ca.Net = tot;
                ca.ExportLst = TData;
            }
            return View(ca);
        }
        public ActionResult DeleteMR(string tag, string id)
        {
            string flag = "";
            if (tag == "Del")
            {
                flag = ExportEnquiry.StatusChange(tag, id);
            }
            else
            {
                flag = ExportEnquiry.ActStatusChange(tag, id);
            }

            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListExport_Enquiry");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListExport_Enquiry");
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
                IEnumerable<ExportItem> cmp = ExportEnquiry.GetAllExportEnquiryItem(id);
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



                foreach (ExportItem item in cmp)
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
        public JsonResult GetItemGrpJSON()
        {
            ExportItem model = new ExportItem();
            model.Itemlst = BindItemlst();
            return Json(BindItemlst());
        }
        public JsonResult GetTandclstJSON()
        {
            TermsItem model = new TermsItem();
            model.Tandclst = BindTandclst();
            return Json(BindTandclst());
        }
        public JsonResult GetCondlstJSON()
        {
            TermsItem model = new TermsItem();
            model.Condlst = BindCondlst();
            return Json(BindCondlst());
        }
    }
}

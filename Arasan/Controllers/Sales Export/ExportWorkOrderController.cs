using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.Pkcs;
using System.Xml.Linq;
using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
using AspNetCore.Reporting;
using Intuit.Ipp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Arasan.Controllers.Sales_Export
{
    public class ExportWorkOrderController : Controller
    {
        IExportWorkOrder ExportWorkOrder;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public ExportWorkOrderController(IExportWorkOrder _ExportWorkOrder, IConfiguration _configuratio)
        {

            ExportWorkOrder = _ExportWorkOrder;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult Export_WorkOrder(string id)
        {
            ExportWorkOrder ca = new ExportWorkOrder();
            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Suplst = BindSupplier();
            ca.Curlst = BindCurrency();
            ca.RecList = BindEmp();
            ca.assignList = BindEmp();
            ca.Assign = Request.Cookies["UserId"];
            ca.user = Request.Cookies["UserName"];
            ca.active = "1";
            ca.Prilst = BindPriority();
            ca.Officelst = BindOfficerType();
            ca.Testlst = BindTest();
            ca.Order = "ORDER";
            ca.trancelst = BindDesp();
            ca.jobDate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.Refdate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.Emaildate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("eso");
            if (dtv.Rows.Count > 0)
            {
                ca.Job = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
            }

            List<WorkOrderItem> TData = new List<WorkOrderItem>();
            WorkOrderItem tda = new WorkOrderItem();

            List<TermsDeatils> TData1 = new List<TermsDeatils>();
            TermsDeatils tda1 = new TermsDeatils();

            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new WorkOrderItem();

                    //tda.ItemGrouplst = BindItemGrplst();
                    tda.Itemlst = BindItemlst();
                    tda.itemspeclst = Bindpec();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda1 = new TermsDeatils();

                    //tda1.Tandclst = BindTandclst();
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
                dt = ExportWorkOrder.GetExportWorkOrder(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Job = dt.Rows[0]["DOCID"].ToString();
                    ca.jobDate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.active = dt.Rows[0]["ACTIVE"].ToString();
                    ca.ID = id;
                    ca.Currency = dt.Rows[0]["MAINCURRENCY"].ToString();
                    ca.Rate = dt.Rows[0]["EXRATE"].ToString();
                    ca.Order = dt.Rows[0]["ORDTYPE"].ToString();
                    ca.Customer = dt.Rows[0]["PARTYID"].ToString();
                    ca.Refno = dt.Rows[0]["CREFNO"].ToString();
                    ca.Refdate = dt.Rows[0]["CREFDATE"].ToString();
                    ca.Officer = dt.Rows[0]["TYPE"].ToString();
                    ca.Emaildate = dt.Rows[0]["SMSDATE"].ToString();
                    ca.Send = dt.Rows[0]["SENDSMS"].ToString();
                    ca.Assign = dt.Rows[0]["ASSIGNTO"].ToString();
                    ca.Recieved = dt.Rows[0]["RECDBY"].ToString();

                    ca.Time = dt.Rows[0]["FOLLOWUPTIME"].ToString();
                    ca.FollowUp = dt.Rows[0]["FOLLOWDT"].ToString();
                    ca.Deatails = dt.Rows[0]["REMARKS"].ToString();
                    ca.Transporter = dt.Rows[0]["TRANSPORTER"].ToString();
                    ca.Test = dt.Rows[0]["TEST"].ToString();


                }
                DataTable dt2 = new DataTable();

                dt2 = ExportWorkOrder.GetExportWorkItem(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new WorkOrderItem();
                        tda.Itemlst = BindItemlst();
                        tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.Des = dt2.Rows[0]["ITEMSPEC"].ToString();
                        tda.Unit = dt2.Rows[i]["UNIT"].ToString();
                        tda.Qty = dt2.Rows[i]["QTY"].ToString();
                        tda.Rate = dt2.Rows[i]["RATE"].ToString();
                        tda.Amount = dt2.Rows[i]["AMOUNT"].ToString();
                        tda.QtyDisc = dt2.Rows[i]["QDISC"].ToString();
                        tda.CashDisc = dt2.Rows[i]["CDISC"].ToString();
                        tda.Introduction = dt2.Rows[i]["IDISC"].ToString();
                        tda.Trade = dt2.Rows[i]["TDISC"].ToString();
                        tda.Addition = dt2.Rows[i]["ADISC"].ToString();
                        tda.Special = dt2.Rows[i]["SDISC"].ToString();
                        tda.Discount = dt2.Rows[i]["DISCOUNT"].ToString();
                        tda.Bed = dt2.Rows[i]["BED"].ToString();
                        tda.Supply = dt2.Rows[i]["MATSUPP"].ToString();
                        tda.Packing = dt2.Rows[i]["PACKSPEC"].ToString();
                        TData.Add(tda);
                    }
                }
            }
            ca.WorkOrderLst = TData;
            ca.TermsDeaLst = TData1;

            return View(ca);
        }
        public List<SelectListItem> BindTest()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "NOT TO BE SENT", Value = "NOT TO BE SENT" });
                lstdesg.Add(new SelectListItem() { Text = "TO BE SENT", Value = "TO BE SENT" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindTranceport()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("SELECT PARTYMASTID,PARTYNAME FROM PARTYMAST WHERE TYPE='TRANSPORTER' union select 1,'None' from dual union select 2,'Customer Vehicle' from dual union all select 3,'Own Vechicle' from dual");
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
        [HttpPost]
        public ActionResult Export_WorkOrder(ExportWorkOrder Cy, string id)
        {

            try
            {
                Cy.ID = id;
                Cy.Branch = Request.Cookies["BranchId"];
                string Strout = ExportWorkOrder.ExportWorkOrderCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Export WorkOrder Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Export WorkOrder Updated Successfully...!";
                    }
                    return RedirectToAction("ListExportWorkOrder");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Export WorkOrder";
                    TempData["notice"] = "Not Inserted";
                    return RedirectToAction("ListExportWorkOrder");
                }

                // }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "Not Inserted";
                return RedirectToAction("ListExportWorkOrder");
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
        public List<SelectListItem> BindDesp()
        {
            try
            {

                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "NONE", Value = "NONE" });
                lstdesg.Add(new SelectListItem() { Text = "BY AIR", Value = "BY AIR" });
                lstdesg.Add(new SelectListItem() { Text = "BY VAN", Value = "BY VAN" });
               
                lstdesg.Add(new SelectListItem() { Text = "SHIP", Value = "SHIP" });
                
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
                DataTable dtDesg = ExportWorkOrder.GetSupplier();
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
        public List<SelectListItem> Bindpec()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("SELECT COMMON_VALUE FROM COMMONMASTER WHERE COMMON_TEXT='JOSPEC'");
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
        public JsonResult GetItemspecJSON()
        {
            //BatchItem model = new BatchItem();
            //model.Processidlst = BindProcessid(itemid);
            return Json(Bindpec());

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
                lstdesg.Add(new SelectListItem() { Text = "Order", Value = "Order" });
                lstdesg.Add(new SelectListItem() { Text = "Sample", Value = "Sample" });
                

                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindOfficerType()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "STANDARD", Value = "STANDARD" });
                lstdesg.Add(new SelectListItem() { Text = "SPECIAL OFFER", Value = "SPECIALOFFER" });
                lstdesg.Add(new SelectListItem() { Text = "GOVERNMENT OFFER", Value = "GOVERNMENTOFFER" });
                lstdesg.Add(new SelectListItem() { Text = "CONVERSION OFFER", Value = "CONVERSIONOFFER" });


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

                dt = ExportWorkOrder.GetExRateDetails(ItemId);

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
        public List<SelectListItem> BindItemlst()
        {
            try
            {
                DataTable dtDesg = ExportWorkOrder.GetItem();
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
       
        public List<SelectListItem> BindCondlst()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("SELECT TANDC FROM TANDCDETAIL,TANDCBASIC WHERE TANDCBASIC.TANDCBASICID=TANDCDETAIL.TANDCBASICID AND TANDCBASIC.TEMPID='Sales Terms'");
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["TANDC"].ToString(), Value = dtDesg.Rows[i]["TANDC"].ToString() });
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
                dt = ExportWorkOrder.GetItemDetails(ItemId);

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
        public JsonResult GetItemGrpJSON()
        {
            WorkOrderItem model = new WorkOrderItem();
            model.Itemlst = BindItemlst();
            return Json(BindItemlst());
        }
        //public JsonResult GetTandclstJSON()
        //{
        //    TermsDeatils model = new TermsDeatils();
        //    model.Tandclst = BindTandclst();
        //    return Json(BindTandclst());
        //}
        public JsonResult GetCondlstJSON()
        {
            TermsDeatils model = new TermsDeatils();
            model.Condlst = BindCondlst();
            return Json(BindCondlst());
        }
        public IActionResult ListExportWorkOrder()
        {
            return View();
        }
        public ActionResult MyListExportWorkOrderGrid(string strStatus)
        {
            List<ExportWorkOrderItems> Reg = new List<ExportWorkOrderItems>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)ExportWorkOrder.GetAllExportWorkOrder(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string SendMail = string.Empty;
                string ViewRow = string.Empty;
                string EditRow = string.Empty;
                string DeleteRow = string.Empty;
                string Close = string.Empty;

                if (dtUsers.Rows[i]["STATUS"].ToString() == "Y")
                {
                    SendMail = "<a href=SendMail?id=" + dtUsers.Rows[i]["EJOBASICID"].ToString() + "><img src='../Images/mail_icon.png' alt='Send Email' /></a>";
                    EditRow = "<a href=Export_WorkOrder?id=" + dtUsers.Rows[i]["EJOBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    ViewRow = "<a href=ViewExportWorkOrder?id=" + dtUsers.Rows[i]["EJOBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View' /></a>";
                    Close = "<a href=/ExWorkOrderClose/ExWorkOrderClose?id=" + dtUsers.Rows[i]["EJOBASICID"].ToString() + "><img src='../Images/close_icon.png' alt='close' /></a>";

                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["EJOBASICID"].ToString() + "";

                }
                else
                {
                    DeleteRow = "DeleteMR?tag=Active&id=" + dtUsers.Rows[i]["EJOBASICID"].ToString() + "";
                }
                Reg.Add(new ExportWorkOrderItems
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["EJOBASICID"].ToString()),
                    jobno = dtUsers.Rows[i]["DOCID"].ToString(),
                    jobdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    currency = dtUsers.Rows[i]["MAINCURR"].ToString(),
                    sendmail = SendMail,
                    view = ViewRow,
                    editrow = EditRow,
                    delrow = DeleteRow,
                    close = Close,



                });
            }

            return Json(new
            {
                Reg
            });

        }
        public IActionResult ViewExportWorkOrder(string id)
        {
            ExportWorkOrder ca = new ExportWorkOrder();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();

            dt = ExportWorkOrder.GetExportWorkOrderView(id);
            if (dt.Rows.Count > 0)
            {
                ca.Job = dt.Rows[0]["DOCID"].ToString();
                ca.jobDate = dt.Rows[0]["DOCDATE"].ToString();
                ca.active = dt.Rows[0]["ACTIVE"].ToString();
                ca.ID = id;
                ca.Currency = dt.Rows[0]["MAINCURR"].ToString();
                ca.Order = dt.Rows[0]["ORDTYPE"].ToString();
                ca.Rate = dt.Rows[0]["EXRATE"].ToString();
                ca.Customer = dt.Rows[0]["PARTYID"].ToString();
                ca.Refno = dt.Rows[0]["CREFNO"].ToString();
                ca.Refdate = dt.Rows[0]["CREFDATE"].ToString();
                ca.Officer = dt.Rows[0]["TYPE"].ToString();
                ca.Emaildate = dt.Rows[0]["SMSDATE"].ToString();
                ca.Send = dt.Rows[0]["SENDSMS"].ToString();
                ca.Time = dt.Rows[0]["FOLLOWUPTIME"].ToString();
                ca.FollowUp = dt.Rows[0]["FOLLOWDT"].ToString();
                ca.Deatails = dt.Rows[0]["REMARKS"].ToString();
                ca.Transporter = dt.Rows[0]["TRANSPORTER"].ToString();
                ca.Test = dt.Rows[0]["TEST"].ToString();
                
                ca.Assign = dt.Rows[0]["EMPNAME"].ToString();
                ca.Recieved = dt.Rows[0]["RECDBY"].ToString();
                ca.ID = id;


                List<WorkOrderItem> TData = new List<WorkOrderItem>();
                WorkOrderItem tda = new WorkOrderItem();
                //double tot = 0;

                dtt = ExportWorkOrder.GetExportWorkOrderItem(id);
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda = new WorkOrderItem();
                        tda.Itemlst = BindItemlst();
                        tda.ItemId = dtt.Rows[i]["ITEMID"].ToString();
                        tda.Des = dtt.Rows[0]["ITEMSPEC"].ToString();
                        tda.Unit = dtt.Rows[i]["UNITID"].ToString();
                        tda.Qty = dtt.Rows[i]["QTY"].ToString();
                        tda.Rate = dtt.Rows[i]["RATE"].ToString();
                        tda.Amount = dtt.Rows[i]["AMOUNT"].ToString();
                        tda.QtyDisc = dtt.Rows[i]["QDISC"].ToString();
                        tda.CashDisc = dtt.Rows[i]["CDISC"].ToString();
                        tda.Introduction = dtt.Rows[i]["IDISC"].ToString();
                        tda.Trade = dtt.Rows[i]["TDISC"].ToString();
                        tda.Addition = dtt.Rows[i]["ADISC"].ToString();
                        tda.Special = dtt.Rows[i]["SDISC"].ToString();
                        tda.Discount = dtt.Rows[i]["DISCOUNT"].ToString();
                        tda.Bed = dtt.Rows[i]["BED"].ToString();
                        tda.Supply = dtt.Rows[i]["MATSUPP"].ToString();
                        tda.Packing = dtt.Rows[i]["PACKSPEC"].ToString();
                        TData.Add(tda);
                    }
                }
                //ca.Net = tot;
                ca.WorkOrderLst = TData;
            }
            return View(ca);
        }
        public ActionResult DeleteMR(string tag, string id)
        {
            string flag = "";
            if (tag == "Del")
            {
                flag = ExportWorkOrder.StatusChange(tag, id);
            }
            else
            {
                flag = ExportWorkOrder.ActStatusChange(tag, id);
            }

            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListExportWorkOrder");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListExportWorkOrder");
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
                IEnumerable<WorkOrderItem> cmp = ExportWorkOrder.GetAllExportWorkOrderItem(id);
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



                foreach (WorkOrderItem item in cmp)
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
        public ActionResult GetAdvDetails(string custid)
        {
            try
            {
                ExportWorkOrder cy = new ExportWorkOrder();
                DataTable dtParty = datatrans.GetData("select distinct P.CREDITDAYS,P.CREDITLIMIT,P.GSTNO,P.PARTYNAME,P.ACCOUNTNAME,P.PartyGroup,A.ratecode,a.limit from PARTYMAST P,PartyAdvDisc A Where P.PartyMastID =A.PartyMastID(+) and A.active = 'Yes' and P.PARTYMASTID='" + custid + "' union select distinct P.CREDITDAYS,P.CREDITLIMIT,P.GSTNO,P.PARTYNAME,P.ACCOUNTNAME,P.PartyGroup,A.Bratecode,0 from PARTYMAST P,PartymastBRCode A Where P.PartyMastID =A.PartyMastID(+) and P.PARTYMASTID='" + custid + "' and 0=(select count(A.ratecode) from PARTYMAST P,PartyAdvDisc A Where P.PartyMastID =A.PartyMastID(+) and A.active = 'Yes' and P.PARTYMASTID='" + custid + "')");
                cy.arc = dtParty.Rows[0]["ratecode"].ToString();
                 
               // cy.crd = (long)Convert.ToDouble(dtParty.Rows[0]["CREDITDAYS"].ToString());
                
                

                var result = new { arc = cy.arc };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Getdociddetails(string typeid)
        {
            try
            {
                string docid = "";
                DataTable did = new DataTable();
                if (typeid == "ORDER")
                {
                    did = datatrans.GetData("SELECT PREFIX,LASTNO as last FROM SEQUENCE WHERE PREFIX='ESO#'");
                    docid = did.Rows[0]["PREFIX"].ToString() + "" + did.Rows[0]["last"].ToString();
                }
                else
                {
                    did = datatrans.GetData("SELECT PREFIX,LASTNO as last FROM SEQUENCE WHERE PREFIX='ESJF'");
                    docid = did.Rows[0]["PREFIX"].ToString() + "" + did.Rows[0]["last"].ToString();
                }



                var result = new { docid = docid };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetItemRate(string ItemId, string custid, string ratec, string ordtype)
        {
            try
            {
                DataTable dt = new DataTable();
                string price = datatrans.GetDataString("Select nvl(sum(rate),0) rate , 1 AS SNO from (SELECT D.RATE FROM RATEBASIC B, RATEDETAIL D, ITEMMASTER I WHERE D.RCODE = '" + ratec + "' AND I.ITEMMASTERID = '" + ItemId + "' AND 'ORDER' ='" + ordtype + "' AND D.ITEMID = I.ITEMMASTERID AND B.RATEBASICID = D.RATEBASICID ANd B.VALIDFROM = (Select max(Validfrom) from Ratebasic R1 Where R1.RATECODE = '" + ratec + "' ANd R1.VALIDFROM <='" + DateTime.Now.ToString("dd-MMM-yyyy") + "') Union SELECT(-disc) FROM PARTYADVDISC WHERE PARTYMASTID ='" + custid + "' and active = 'Yes' and RATECODE = '" + ratec + "')");


                var result = new { price = price };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult ScheduleCreate(string Itemid, string qty, string rowid, string duedate)
        {
            ExportWorkOrder ca = new ExportWorkOrder();
            string itemname = datatrans.GetDataString("SELECT ITEMID FROM ITEMMASTER WHERE ITEMMASTERID='" + Itemid + "'");
            ca.item = itemname;
            ca.qty = qty;
            ca.duedate = duedate;

            List<SchItem> TData = new List<SchItem>();
            SchItem tda = new SchItem();

            for (int i = 0; i < 1; i++)
            {
                tda = new SchItem();

                tda.Isvalid = "Y";
                TData.Add(tda);
            }


            ca.schlst = TData;
            return View(ca);
        }

        public IActionResult DispatchDrumAllo(string id)
        {
            ExportWorkOrder ca = new ExportWorkOrder();
            ca.Branch = Request.Cookies["BranchId"];

            ca.JopDate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.Location = Request.Cookies["LocationId"];
            ca.Emp = Request.Cookies["UserId"];
            ca.user = Request.Cookies["UserName"];

            
            DataTable dtv = datatrans.GetSequence("EJDP");
            if (dtv.Rows.Count > 0)
            {
                ca.JopId = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
            }
            List<ExWorkItem> TData = new List<ExWorkItem>();
            ExWorkItem tda = new ExWorkItem();

            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new ExWorkItem();
                    
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }


            }


            ca.Worklst = TData;

            return View(ca);
        }
        [HttpPost]

        public ActionResult DispatchDrumAllo(ExportWorkOrder cy, string id)
        {

            try
            {
                cy.ID = id;
                string Strout = ExportWorkOrder.DispDrumCRUD(cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "DrumAllocation Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "DrumAllocation Updated Successfully...!";
                    }
                    return RedirectToAction("DispatchDrumAllo");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Schselect";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(cy);
        }
        public List<SelectListItem> BindLocation(string id, string emp)
        {
            try
            {
                DataTable dtDesg = datatrans.GetData(" select L.LOCDETAILSID,L.LOCID from LOCDETAILS L,EMPLOYEELOCATION E WHERE L.BRANCHID='"+id+ "' AND E.LOCID=L.LOCDETAILSID AND E.EMPID='"+emp+"' AND L.TRADEYN='Yes'");
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCID"].ToString(), Value = dtDesg.Rows[i]["LOCDETAILSID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetstkitemDetail(string indentid)
        {
            ExportWorkOrder ca = new ExportWorkOrder();
            List<SchItem> TDatab = new List<SchItem>();
            SchItem tdab = new SchItem();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            //string[] lotno = indentid.Split(',');
            //string[] lotqty = doctype.Split(',');


            dt = datatrans.GetData("Select JS.EJOSCHEDULEID,JS.SCHNO,JS.SCHQTY,J.DOCID,J.PARTYNAME,to_char(JS.SCHDATE,'dd-MON-yyyy')SCHDATE,to_char(J.DOCDATE,'dd-MON-yyyy')DOCDATE,JD.QTY,JS.IS_ALLOCATE,ITEMMASTER.ITEMID from EJOSCHEDULE JS LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=JS.SCHITEMID,EJOBASIC J,EJODETAIL JD    WHERE J.EJOBASICID =JS.EJOBASICID AND JS.PARENTRECORDID =JD.EJODETAILID AND JS.EJOSCHEDULEID IN (" + indentid + ") ");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tdab = new SchItem();

                    tdab.jobno = dt.Rows[i]["DOCID"].ToString();

                    tdab.suppliar = dt.Rows[i]["PARTYNAME"].ToString();
                    tdab.schno = dt.Rows[i]["SCHNO"].ToString();
                    tdab.schdate = dt.Rows[i]["SCHDATE"].ToString();
                    tdab.qty = dt.Rows[i]["SCHQTY"].ToString();
                    tdab.schid = dt.Rows[i]["EJOSCHEDULEID"].ToString();
                    tdab.itemid = dt.Rows[i]["ITEMID"].ToString();

                    TDatab.Add(tdab);
                }
            }

            ca.schlst = TDatab;
            return Json(ca.schlst);
        }
        public IActionResult Schselect()
        {
            ExportWorkOrder ca = new ExportWorkOrder();
            List<SchItem> TDatab = new List<SchItem>();
            SchItem tdab = new SchItem();
            DataTable dt = new DataTable();
            dt = datatrans.GetData(" Select JS.EJOSCHEDULEID,JS.SCHNO,JS.SCHQTY-JS.SCHSUPPQTY SCHQTY,J.DOCID,J.PARTYNAME,to_char(JS.SCHDATE,'dd-MON-yyyy')SCHDATE,to_char(J.DOCDATE,'dd-MON-yyyy')DOCDATE,JD.QTY,JS.IS_ALLOCATE from EJOSCHEDULE JS,EJOBASIC J,EJODETAIL JD    WHERE J.EJOBASICID =JS.EJOBASICID AND JS.PARENTRECORDID =JD.EJODETAILID AND   J.STATUS='Y' AND J.ACTIVE='0'  AND  JD.QTY-JD.PRECLQTY-EXCISEQTY > 0 AND JS.SCHQTY-JS.SCHSUPPQTY>0 AND JS.IS_DRUMDISP='N' ORDER BY to_date(J.DOCDATE) DESC,J.DOCID DESC");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tdab = new SchItem();
                    // tdab.item = dt.Rows[i]["ITEMID"].ToString();
                    //tdab.invid = dt.Rows[i]["EMPNAME"].ToString();
                    tdab.qty = dt.Rows[i]["SCHQTY"].ToString();
                    //tdab.item = dt.Rows[i]["EMPMASTID"].ToString();
                    tdab.schdate = dt.Rows[i]["SCHDATE"].ToString();
                    tdab.schid = dt.Rows[i]["EJOSCHEDULEID"].ToString();
                    tdab.schno = dt.Rows[i]["SCHNO"].ToString();
                    tdab.suppliar = dt.Rows[i]["PARTYNAME"].ToString();
                    tdab.jobno = dt.Rows[i]["DOCID"].ToString();


                    TDatab.Add(tdab);
                }
            }
            ca.schlst = TDatab;
            return View(ca);
        }

        public IActionResult ListWorkSchedule()
        {
            //IEnumerable<WorkOrder> cmp = WorkOrderService.GetAllWorkOrder(status);
            return View();
        }
        public ActionResult MyListWorkScheduleGrid()
        {
            List<ListEWSchItems> Reg = new List<ListEWSchItems>();
            DataTable dtUsers = new DataTable();
            //strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)ExportWorkOrder.GetAllListWorkScheduleItems();
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string View = string.Empty;
                string deactive = string.Empty;
                string Drum = string.Empty;
                if (dtUsers.Rows[i]["IS_ALLOCATE"].ToString() == "Y")
                {
                    Drum = "";
                    View = "<a href=ViewDrumAllocation?id=" + dtUsers.Rows[i]["EJOSCHEDULEID"].ToString() + " class='fancybox' data-fancybox-type='iframe' ><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";
                    deactive = "<a href=StockRelease?id=" + dtUsers.Rows[i]["EJOSCHEDULEID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                }
                else
                {
                    Drum = "<a href=WDrumAllocation?id=" + dtUsers.Rows[i]["EJOSCHEDULEID"].ToString() + "><img src='../Images/checklist.png' alt='Allocate' /></a>";
                    View = "";
                    deactive = "";
                }



                Reg.Add(new ListEWSchItems
                {

                    id = dtUsers.Rows[i]["EJOSCHEDULEID"].ToString(),
                    jobid = dtUsers.Rows[i]["DOCID"].ToString(),
                    schid = dtUsers.Rows[i]["SCHNO"].ToString(),
                    qty = dtUsers.Rows[i]["QTY"].ToString(),

                    customername = dtUsers.Rows[i]["PARTYNAME"].ToString(),
                    schdate = dtUsers.Rows[i]["SCHDATE"].ToString(),
                    schqty = dtUsers.Rows[i]["SCHQTY"].ToString(),

                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    dispdate = dtUsers.Rows[i]["dispdate"].ToString(),
                    dispid = dtUsers.Rows[i]["dipid"].ToString(),
                    view = View,
                    deactive = deactive,
                    drum = Drum,



                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult StockRelease(string id)
        {
            string shid = datatrans.GetDataString("SELECT EJODRUMALLOCATIONBASICID FROM EJODRUMALLOCATIONBASIC WHERE EJOSCHEDULEID='" + id + "' AND IS_ALLOCATE='Y'");

            DataTable lot = datatrans.GetData("SELECT PLSTOCKID FROM EJODRUMALLOCATIONDETAIL WHERE EJODRUMALLOCATIONBASICID='" + shid + "'");
            string joid = datatrans.GetDataString("SELECT JOPID FROM EJODRUMALLOCATIONBASIC WHERE EJODRUMALLOCATIONBASICID='" + shid + "'");
            string flag = "";
            if (lot.Rows.Count > 0)
            {
                for (int i = 0; i < lot.Rows.Count; i++)
                {
                    string lotno = lot.Rows[i]["PLSTOCKID"].ToString();
                    flag = ExportWorkOrder.StatusStockRelease(lotno, joid, id);
                }
            }

            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListWorkSchedule");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListWorkSchedule");
            }
        }
        public ActionResult ViewDrumAllocation(string id)
        {
            EWDrumAllocation ca = new EWDrumAllocation();
            DataTable dt = new DataTable();
            string shid = datatrans.GetDataString("SELECT EJODRUMALLOCATIONBASICID FROM EJODRUMALLOCATIONBASIC WHERE EJOSCHEDULEID='" + id + "' AND IS_ALLOCATE='Y'");
            dt = ExportWorkOrder.GetDrumAllByID(shid);
            if (dt.Rows.Count > 0)
            {
                //ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.Location = dt.Rows[0]["LOCID"].ToString();
                ca.JobId = dt.Rows[0]["jobid"].ToString();
                //ca.JobDate = dt.Rows[0]["DOCDATE"].ToString();
                ca.Customername = dt.Rows[0]["PARTYNAME"].ToString();


                //ca.JOId = dt.Rows[0]["JOBASICID"].ToString();
                ca.DOCId = dt.Rows[0]["DOCID"].ToString();
                ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
            }

            List<EWorkItem> TData = new List<EWorkItem>();
            EWorkItem tda = new EWorkItem();
            DataTable dtt = new DataTable();
            dtt = ExportWorkOrder.GetDrumAllDetails(shid);
            if (dtt.Rows.Count > 0)
            {

                tda = new EWorkItem();

                tda.items = dtt.Rows[0]["ITEMID"].ToString();
                //tda.orderqty = dtt.Rows[i]["QTY"].ToString();

                List<EDrumdetails> tlstdrum = new List<EDrumdetails>();
                EDrumdetails tdrum = new EDrumdetails();
                DataTable dt3 = new DataTable();
                dt3 = ExportWorkOrder.GetAllocationDrumDetails(shid);
                if (dt3.Rows.Count > 0)
                {
                    int sn = 1;
                    for (int j = 0; j < dt3.Rows.Count; j++)
                    {
                        tdrum = new EDrumdetails();
                        tdrum.sno = sn.ToString();
                        tdrum.lotno = dt3.Rows[j]["LOTNO"].ToString();
                        tdrum.drumno = dt3.Rows[j]["DRUMNO"].ToString();
                        tdrum.qty = dt3.Rows[j]["QTY"].ToString();
                        tdrum.rate = dt3.Rows[j]["RATE"].ToString();
                        sn++;
                        tlstdrum.Add(tdrum);
                    }
                }
                tda.drumlst = tlstdrum;
                TData.Add(tda);

            }
            ca.Worklst = TData;
            return View(ca);
        }
        public ActionResult WDrumAllocation(string id)
        {
            EWDrumAllocation ca = new EWDrumAllocation();
            DataTable dt = new DataTable();
            dt = ExportWorkOrder.GetWorkOrderByID(id);
            if (dt.Rows.Count > 0)
            {
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                //ca.Location = dt.Rows[0]["LOCID"].ToString();
                ca.JobId = dt.Rows[0]["DOCID"].ToString();
                ca.JobDate = dt.Rows[0]["DOCDATE"].ToString();
                ca.Customername = dt.Rows[0]["PARTYNAME"].ToString();
                ca.CustomerId = dt.Rows[0]["CUSTOMERID"].ToString();
               // ca.Locid = dt.Rows[0]["LOCMASTERID"].ToString();
                ca.Schno = dt.Rows[0]["SCHNO"].ToString();
                ca.Schdate = dt.Rows[0]["SCHDATE"].ToString();
                ca.JOId = dt.Rows[0]["EJOBASICID"].ToString();
            }
            ca.DocDate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("eJod");
            if (dtv.Rows.Count > 0)
            {
                ca.DOCId = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
            }
            List<EWorkItem> TData = new List<EWorkItem>();
            EWorkItem tda = new EWorkItem();
            DataTable dtt = new DataTable();
            dtt = ExportWorkOrder.GetWorkOrderDetailsss(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new EWorkItem();
                    tda.itemid = dtt.Rows[i]["item"].ToString();
                    tda.items = dtt.Rows[i]["ITEMID"].ToString();
                    tda.orderqty = dtt.Rows[i]["SCHQTY"].ToString();
                    tda.Jodetailid = dtt.Rows[i]["EJODETAILID"].ToString();

                    List<EDrumdetails> tlstdrum = new List<EDrumdetails>();
                    EDrumdetails tdrum = new EDrumdetails();
                    DataTable dt3 = new DataTable();
                    dt3 = ExportWorkOrder.GetDrumDetails(tda.itemid, "12423000000238");
                    if (dt3.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt3.Rows.Count; j++)
                        {
                            tdrum = new EDrumdetails();
                            tdrum.lotno = dt3.Rows[j]["LOTNO"].ToString();
                            tdrum.drumno = dt3.Rows[j]["DRUMNO"].ToString();
                            tdrum.qty = dt3.Rows[j]["QTY"].ToString();
                            //tdrum.rate = dt3.Rows[j]["RATE"].ToString();
                            tdrum.invid = datatrans.GetDataString("SELECT PLSTOCKVALUEID FROM PLSTOCKVALUE WHERE LOTNO='" + tdrum.lotno + "'");
                            tlstdrum.Add(tdrum);
                        }
                    }
                    tda.drumlst = tlstdrum;
                    TData.Add(tda);
                }
            }
            ca.Worklst = TData;
            return View(ca);
        }
        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]

        public ActionResult EWDrumAllocation(EWDrumAllocation cy, string id)
        {

            try
            {
                cy.ID = id;
                string Strout = ExportWorkOrder.DrumAllocationCRUD(cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "DrumAllocation Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "DrumAllocation Updated Successfully...!";
                    }
                    return RedirectToAction("ListWorkSchedule");
                }

                else
                {
                    ViewBag.PageTitle = "Edit WDrumAllocation";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(cy);
        }
    }
}

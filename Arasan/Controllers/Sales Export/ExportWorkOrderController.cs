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
            ca.trancelst = BindTranceport();
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

                if (dtUsers.Rows[i]["STATUS"].ToString() == "Y")
                {
                    SendMail = "<a href=SendMail?id=" + dtUsers.Rows[i]["EJOBASICID"].ToString() + "><img src='../Images/mail_icon.png' alt='Send Email' /></a>";
                    EditRow = "<a href=Export_WorkOrder?id=" + dtUsers.Rows[i]["EJOBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    ViewRow = "<a href=ViewExportWorkOrder?id=" + dtUsers.Rows[i]["EJOBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View' /></a>";
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
    }
}

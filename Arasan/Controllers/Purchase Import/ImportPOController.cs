using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCore.Reporting;
using NuGet.Packaging.Signing;
using System.Net.Mail;
using Arasan.Services.Qualitycontrol;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Reporting.WebForms;

namespace Arasan.Controllers 
{
    public class ImportPOController : Controller
    {
        IIPO PoService;
        private string? _connectionString;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        IConfiguration? _configuratio;
        DataTransactions datatrans;
        public ImportPOController(IIPO _PoService, IConfiguration _configuratio, IWebHostEnvironment WebHostEnvironment)
        {
            this._WebHostEnvironment = WebHostEnvironment;
            PoService = _PoService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            //object value = InitializeComponent();
        }

        public IActionResult ImportPO(string id)
        {
            ImportPO po = new ImportPO();
            po.Brlst = BindBranch();
            po.Suplst = BindSupplier();
            po.Curlst = BindCurrency();
            po.RecList = BindEmp();
            po.assignList = BindEmp();
            po.Paymenttermslst = BindPaymentterms();
            po.deltermlst = Binddeliveryterms();
            po.warrantytermslst = Bindwarrantyterms();
            po.desplst = Binddespatch();

            List<IPOItem> TData = new List<IPOItem>();
            IPOItem tda = new IPOItem();
            List<PODoc> TData1 = new List<PODoc>();
            PODoc tda1 = new PODoc();

            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new IPOItem();
                    tda.Itemlst = BindItemlst();
                    tda.PURLst = BindPurType();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda1 = new PODoc();
                    
                    tda1.Isvalid = "Y";
                    TData1.Add(tda1);
                }

            }
            else
            {
                double total = 0;
                DataTable dt = new DataTable();
                dt = PoService.EditPObyID(id);
                if (dt.Rows.Count > 0)
                {
                    po.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    po.BranchId = dt.Rows[0]["BRANCHID"].ToString();
                    po.POdate = dt.Rows[0]["DOCDATE"].ToString();
                    po.Supplier = dt.Rows[0]["PARTYID"].ToString();
              
                    po.PONo = dt.Rows[0]["DOCID"].ToString();
                    po.ID = id;
                    po.QuoteNo = dt.Rows[0]["Quotno"].ToString();
                    po.Cur = dt.Rows[0]["MAINCURRENCY"].ToString();
                    po.ExRate = dt.Rows[0]["EXRATE"].ToString();
                    po.RefNo = dt.Rows[0]["REFNO"].ToString();
                    po.RefDate = dt.Rows[0]["REFDT"].ToString();

                   
                  
                    po.Round = Convert.ToDouble(dt.Rows[0]["RNDOFF"].ToString() == "" ? "0" : dt.Rows[0]["RNDOFF"].ToString());
                  

                    po.Gross = Convert.ToDouble(dt.Rows[0]["GROSS"].ToString() == "" ? "0" : dt.Rows[0]["GROSS"].ToString());
                    po.Net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());

                }
                DataTable dt2 = new DataTable();
                dt2 = PoService.GetPOItemDetails(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new IPOItem();
                        double toaamt = 0;
                        //tda.ItemGrouplst = BindItemGrplst();
                        //DataTable dt3 = new DataTable();
                        //dt3 = datatrans.GetItemSubGroup(dt2.Rows[i]["ITEMID"].ToString());
                        //if (dt3.Rows.Count > 0)
                        //{
                        //    tda.ItemGroupId = dt3.Rows[0]["SUBGROUPCODE"].ToString();
                        //}
                        tda.Itemlst = BindItemlst();
                        tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                        tda.saveItemId = dt2.Rows[i]["ITEMID"].ToString();
                         
                             
                            tda.Conversionfactor = dt2.Rows[0]["CF"].ToString();
                            tda.rate = Convert.ToDouble(dt2.Rows[0]["RATE"].ToString());
                            tda.Amount = Convert.ToDouble(dt2.Rows[0]["AMOUNT"].ToString());
                         
                        tda.Quantity = Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                         
                        //tda.QtyPrim= Convert.ToDouble(dt2.Rows[i]["QTY"].ToString());
                        tda.Amount = toaamt;
                        tda.Unit = dt2.Rows[i]["PUNIT"].ToString();
                        tda.PURLst = BindPurType();
                        //tda.unitprim= dt2.Rows[i]["UNITID"].ToString();
                       
                        tda.Purtype = dt2.Rows[i]["PURTYPE"].ToString();
                        tda.Duedate = dt2.Rows[i]["DUEDATE"].ToString();

                        tda.Isvalid = "Y";
                        TData.Add(tda);

                    }
                }
               

            }
            po.PoItem = TData;
            po.doclst = TData1;
            return View(po);
        }
        [HttpPost]
        public ActionResult ImportPO(ImportPO Cy, string id)
        {

            try
            {
                Cy.POID = id;
                string Strout = PoService.PurOrderCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.POID == null)
                    {
                        TempData["notice"] = "PO Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "PO Updated Successfully...!";
                    }
                    return RedirectToAction("ListPO");
                }

                else
                {
                    ViewBag.PageTitle = "Edit PO";
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
        public IActionResult ListImportPO()
        {
            //IEnumerable<Models.PO> cmp = PoService.GetAllPO();
            return View();
        }
        public ActionResult MyListPOGrid(string strStatus)
        {
            List<IPOItems> Reg = new List<IPOItems>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)PoService.GetAllPoItems(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string MailRow = string.Empty;
                string GeneratePO = string.Empty;
                string MoveToGRN = string.Empty;
                //string Download = string.Empty;
                string View = string.Empty;
                string EditRow = string.Empty;
                string Doc = string.Empty;
                string DeleteRow = string.Empty;

                //MailRow = "<a href=SendMail?id=" + dtUsers.Rows[i]["IPOBASICID"].ToString() + "><img src='../Images/mail_icon.png' alt='Send Email' /></a>";
                //GeneratePO = "<a href=Print?id=" + dtUsers.Rows[i]["IPOBASICID"].ToString() + "><img src='../Images/pdficon.png' alt='View Details' width='20' /></a>";
                //if (dtUsers.Rows[i]["STATUS"].ToString() == "GRN Generated")
                //{
                //    MoveToGRN = "<img src='../Images/tick.png' alt='View Details' width='20' />";
                //    EditRow = "";

                //}
                //else
                //{
                //    MoveToGRN = "<a href=ViewPO?id=" + dtUsers.Rows[i]["POBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/move_quote.png' alt='View Details' width='20' /></a>";
                //  EditRow = "<a href=PurchaseOrder?id=" + dtUsers.Rows[i]["POBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";


                //}
                //Download = "<a href=CreatePDF?id=" + dtUsers.Rows[i]["POBASICID"].ToString() + "><img src='../Images/pdficon.png' alt='View Details' width='20' /></a>";
                Doc = "<a href=CollectDoc?id=" + dtUsers.Rows[i]["IPOBASICID"].ToString() + " ><img src='../Images/move_quote.png' alt='Edit' /></a>";

                EditRow = "<a href=importPO?id=" + dtUsers.Rows[i]["iPOBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
 
                View = "<a href=ViewPOrder?id=" + dtUsers.Rows[i]["IPOBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";
                DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["IPOBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                Reg.Add(new IPOItems
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["IPOBASICID"].ToString()),
                    branch = dtUsers.Rows[i]["BRANCHID"].ToString(),
                    pono = dtUsers.Rows[i]["DOCID"].ToString(),
                    podate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    quono = dtUsers.Rows[i]["Quotno"].ToString(),
                    supplier = dtUsers.Rows[i]["PARTYNAME"].ToString(),
                    mailrow = MailRow,
                    genpo = GeneratePO,
                    move = MoveToGRN,
                    //download = Download,
                    view = View,
                    doc = Doc,
                    editrow = EditRow,
                    delrow = DeleteRow,



                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult DeleteItem(string tag, string id)
        {

            string flag = PoService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListPO");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListPO");
            }
        }
       
        public JsonResult GetSuppJSON(string supid)
        {
            GateInward model = new GateInward();
            model.POlst = BindPOlst(supid);
            return Json(BindPOlst(supid));

        }
        //public JsonResult GetPOJSON(string poid)
        //{
        //    GateInward model = new GateInward();
        //    DataTable dtt = new DataTable();
        //    List<POGateItem> Data = new List<POGateItem>();
        //    POGateItem tda = new POGateItem();
        //    dtt = PoService.GetPOItembyID(poid);
        //    if (dtt.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dtt.Rows.Count; i++)
        //        {
        //            tda = new POGateItem();
        //            tda.itemid = dtt.Rows[i]["Itemi"].ToString();
        //            tda.itemname = dtt.Rows[i]["ITEMID"].ToString();
        //            tda.unit = dtt.Rows[i]["UNITID"].ToString();
        //            tda.quantity = Convert.ToDouble(dtt.Rows[i]["QTY"].ToString());
        //            DataTable dt4 = new DataTable();
        //            dt4 = datatrans.GetItemDetails(tda.itemid);
        //            if (dt4.Rows.Count > 0)
        //            {
        //                tda.Conversionfactor = dt4.Rows[0]["CF"].ToString();
        //                tda.qc = dt4.Rows[0]["QCCOMPFLAG"].ToString();
        //            }
        //            Data.Add(tda);
        //        }
        //    }
        //    //ca.Net = tot;
        //    model.PoItem = Data;
        //    return Json(model.PoItem);

        //}
        public ActionResult GetItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string unit = "";
                string CF = "";
                string price = "";
                dt = datatrans.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    unit = dt.Rows[0]["UNITID"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();
                    dt1 = PoService.GetItemCF(ItemId, dt.Rows[0]["UNITMASTID"].ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        CF = dt1.Rows[0]["CF"].ToString();
                    }
                }

                var result = new { unit = unit, CF = CF, price = price };
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
      
        public List<SelectListItem> BindPurType()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "CONSUMABLES PURCHASE", Value = "CONSUMABLES PURCHASE" });
                lstdesg.Add(new SelectListItem() { Text = "FIXED PURCHASE", Value = "FIXED PURCHASE" });
                lstdesg.Add(new SelectListItem() { Text = "MACHINERIES PURCHASE", Value = "MACHINERIES PURCHASE" });
                lstdesg.Add(new SelectListItem() { Text = "RAW MATERIAL", Value = "RAW MATERIAL" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindPaymentterms()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "30 Days  Credit", Value = "30 Days  Credit" });
                lstdesg.Add(new SelectListItem() { Text = "10 Days  Credit", Value = "10 Days  Credit" });
                lstdesg.Add(new SelectListItem() { Text = "Against Proforma Invoice", Value = "Against Proforma Invoice" });

                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> Binddespatch()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "ALAGAR SAMY PARCEL SERVICE", Value = "ALAGAR SAMY PARCEL SERVICE" });
                lstdesg.Add(new SelectListItem() { Text = "By Lorry", Value = "By Lorry" });
                lstdesg.Add(new SelectListItem() { Text = "R.G. Road Lines", Value = "R.G. Road Lines" });

                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> Bindwarrantyterms()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "One Year Free service", Value = "One Year Free service" });
                lstdesg.Add(new SelectListItem() { Text = "3 Years On Site Warrenty", Value = "3 Years On Site Warrenty" });

                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> Binddeliveryterms()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "immediate (Indimate despatch details by mail)", Value = "immediate (Indimate despatch details by mail)" });
                lstdesg.Add(new SelectListItem() { Text = "20 Days (Indimate despatch details by mail)", Value = "20 Days (Indimate despatch details by mail)" });
                lstdesg.Add(new SelectListItem() { Text = "7 to 10 Days (Indimate despatch details by mail)", Value = "7 to 10 Days (Indimate despatch details by mail)" });
                lstdesg.Add(new SelectListItem() { Text = "Within 14 Days only ", Value = "Within 14 Days only " });
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
        public List<SelectListItem> BindSupplier()
        {
            try
            {
                DataTable dtDesg = datatrans.GetSupplier();
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
        public List<SelectListItem> BindItemlst()
        {
            try
            {
                DataTable dtDesg = datatrans.GetItem();
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
        public List<SelectListItem> BindPOlst(string value)
        {
            try
            {
                DataTable dtDesg = PoService.GetPObySuppID(value);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["POBASICID"].ToString() });
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
        //public IActionResult PODetails(string id)
        //{
        //    IEnumerable<POItem> cmp = PoService.GetAllPOItem(id);
        //    return View(cmp);
        //}
        //public IActionResult GateInwardDetails(string id)
        //{
        //    IEnumerable<POItem> cmp = PoService.GetAllGateInwardItem(id);
        //    return View(cmp);
        //}
        public IActionResult ViewPO(string id)
        {
           ImportPO ca = new ImportPO();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            dt = PoService.GetPObyID(id)
;
            if (dt.Rows.Count > 0)
            {
                ca.Supplier = dt.Rows[0]["PARTYNAME"].ToString();
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.PONo = dt.Rows[0]["DOCID"].ToString();
                ca.POdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.QuoteNo = dt.Rows[0]["Quotno"].ToString();
                ca.QuoteDate = dt.Rows[0]["Quotedate"].ToString();
               
                ca.Round = Convert.ToDouble(dt.Rows[0]["RNDOFF"].ToString() == "" ? "0" : dt.Rows[0]["RNDOFF"].ToString());
                

                ca.Gross = Convert.ToDouble(dt.Rows[0]["GROSS"].ToString() == "" ? "0" : dt.Rows[0]["GROSS"].ToString());
                ca.Net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());
                ca.ID = id;
            }
            List<IPOItem> Data = new List<IPOItem>();
            IPOItem tda = new IPOItem();
            double tot = 0;
            dtt = PoService.GetPOItembyID(id)
;
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new IPOItem();
                    tda.ItemId = dtt.Rows[i]["ITEMID"].ToString();
                    tda.Unit = dtt.Rows[i]["UNITID"].ToString();
                    tda.Quantity = Convert.ToDouble(dtt.Rows[i]["QTY"].ToString() == "" ? "0" : dtt.Rows[i]["QTY"].ToString());
                    tda.rate = Convert.ToDouble(dtt.Rows[i]["RATE"].ToString() == "" ? "0" : dtt.Rows[i]["RATE"].ToString());
                    tda.Amount = tda.Quantity * tda.rate;
                    tot += tda.Amount;
                    
                    Data.Add(tda);
                }
            }
            //ca.Net = tot;
            ca.PoItem = Data;
            return View(ca);
        }


        public IActionResult ViewPOrder(string id)
        {
           ImportPO ca = new ImportPO();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            dt = PoService.GetPOrderID(id)
;
            if (dt.Rows.Count > 0)
            {
                ca.Supplier = dt.Rows[0]["PARTYNAME"].ToString();
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.PONo = dt.Rows[0]["DOCID"].ToString();
                ca.POdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.QuoteNo = dt.Rows[0]["Quotno"].ToString();
               
                ca.Round = Convert.ToDouble(dt.Rows[0]["RNDOFF"].ToString() == "" ? "0" : dt.Rows[0]["RNDOFF"].ToString());
               

                ca.Gross = Convert.ToDouble(dt.Rows[0]["GROSS"].ToString() == "" ? "0" : dt.Rows[0]["GROSS"].ToString());
                ca.Net = Convert.ToDouble(dt.Rows[0]["NET"].ToString() == "" ? "0" : dt.Rows[0]["NET"].ToString());
                ca.ID = id;
            }
            List<IPOItem> Data = new List<IPOItem>();
            IPOItem tda = new IPOItem();
            double tot = 0;
            dtt = PoService.GetPOItem(id)
;
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new IPOItem();
                    tda.ItemId = dtt.Rows[i]["ITEMID"].ToString();
                    tda.Unit = dtt.Rows[i]["UNITID"].ToString();
                    tda.Quantity = Convert.ToDouble(dtt.Rows[i]["QTY"].ToString() == "" ? "0" : dtt.Rows[i]["QTY"].ToString());
                    tda.rate = Convert.ToDouble(dtt.Rows[i]["RATE"].ToString() == "" ? "0" : dtt.Rows[i]["RATE"].ToString());
                    tda.Amount = tda.Quantity * tda.rate;
                    tot += tda.Amount;
                  
                    Data.Add(tda);
                }
            }
            //ca.Net = tot;
            ca.PoItem = Data;
            return View(ca);
        }


        [HttpPost]
        public ActionResult ViewPO(ImportPO Cy, string id)
        {
            try
            {
                Cy.ID = id;
                string Strout = PoService.POtoGRN(Cy.ID);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "GRN Generated Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "GRN Generated Successfully...!";
                    }
                    return RedirectToAction("ListPO");
                }

                else
                {
                    ViewBag.PageTitle = "Edit PO";
                    TempData["notice"] = Strout;
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("ListPO");
        }


       

        public ActionResult SendMail(string id)
        {


            try
            {
                datatrans = new DataTransactions(_connectionString);
                DataTable ddt1 = new DataTable();
                ddt1 = datatrans.GetEmailConfig();
                string HostAdd = ddt1.Rows[0]["SMTP_HOST"].ToString();
                string FromEmailid = ddt1.Rows[0]["EMAIL_ID"].ToString();
                string password = ddt1.Rows[0]["PASSWORD"].ToString();
                int port = Convert.ToInt32(ddt1.Rows[0]["PORT_NO"].ToString());
                Boolean ssl = ddt1.Rows[0]["SSL"].ToString() == "No" ? false : true;

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(FromEmailid); //From Email Id  
                mailMessage.Subject = "PO"; //Subject of Email  
                mailMessage.Body = "PO"; //body or message of Email  
                mailMessage.IsBodyHtml = true;
                mailMessage.To.Add(new MailAddress("deepa@icand.in")); //adding multiple TO Email Id  

                SmtpClient smtp = new SmtpClient();  // creating object of smptpclient  
                smtp.Host = HostAdd;              //host of emailaddress for example smtp.gmail.com etc  
                smtp.EnableSsl = ssl;
                NetworkCredential NetworkCred = new NetworkCredential();
                NetworkCred.UserName = mailMessage.From.Address;
                NetworkCred.Password = password;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = NetworkCred;
                smtp.Port = port;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //  mailMessage.Attachments.Add(new Attachment(DownloadFile()));


                smtp.Send(mailMessage);
                return Ok();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public IActionResult CollectDoc(string id)
        {
            ImportPO br = new ImportPO();

            br.ID = id;

            //for edit & delete
            //if (id != null)
            //{
            //    DataTable dt = new DataTable();
            //    //double total = 0;
            //    dt = BranchService.GetEditBranch(id);
            //    if (dt.Rows.Count > 0)
            //    {
            //        br.CompanyName = dt.Rows[0]["COMPANYID"].ToString();
            //        br.BranchName = dt.Rows[0]["BRANCHID"].ToString();
            //        br.Address = dt.Rows[0]["ADDRESS1"].ToString();
            //        br.StateName = dt.Rows[0]["STATE"].ToString();
            //        br.Citylst = BindCity(br.StateName);
            //        br.City = dt.Rows[0]["CITY"].ToString();
            //        br.PinCode = dt.Rows[0]["PINCODE"].ToString();
            //        br.GSTNo = dt.Rows[0]["CSTNO"].ToString();
            //        br.GSTDate = dt.Rows[0]["CSTDATE"].ToString();
            //        br.ID = id;

            //    }
            //}
            return View(br);
        }
        //public ActionResult CollectDoc(ImportPO Cy, string id)
        //{

           


        //    string Strout = PoService.CRUDPOAttachement(Cy);
        //    if (string.IsNullOrEmpty(Strout))
        //    {
        //        if (Cy.ID == null)
        //        {
        //            TempData["notice"] = "Attachement Inserted Successfully...!";
        //        }
        //        else
        //        {
        //            TempData["notice"] = "Quote Updated Successfully...!";
        //        }

        //        string Fid = (Cy.ID).ToString();

        //        return RedirectToAction("CollectDoc", "ImportPO", new { Fid });
        //    }

        //    else
        //    {
        //        ViewBag.PageTitle = "Edit Quote Attachement";
        //        TempData["notice"] = Strout;
        //    }

        //    return View(Cy);
        //}
        
 
        [HttpPost]
        public IActionResult CollectDoc(List<IFormFile> file, ImportPO cy)
        {
            string Strout = PoService.CRUDPOAttachement(file, cy);
            if (string.IsNullOrEmpty(Strout))
            {
                if (file != null && file.Count > 0)
                {
                    ViewBag.Message = "File uploaded successfully!";
                    ViewBag.FileName = file.Select(f => f.FileName).ToList(); 
                    return View("CollectDoc");
                }
                else
                {
                    ViewBag.Message = "Please select a file!";
                    return View("CollectDoc");
                }

                
            }
            return View("CollectDoc");
        }
        public ActionResult MyListAttachPOGrid(long id)
        {
            List<IPOItems> Reg = new List<IPOItems>();
            DataTable dtUsers = new DataTable();
            
            dtUsers = (DataTable)PoService.GetAllAttachment(id);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string Upload = string.Empty;
                


                Upload = "<a href='/" + dtUsers.Rows[i]["DOCNAME"].ToString() + "' target='_blank' title='Attachement'>" + dtUsers.Rows[i]["DOCNAME"].ToString() + "</a>";

                 
                Reg.Add(new IPOItems
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["IPODOCDETAILID"].ToString()),
                     
                    upload = Upload,
                     



                });
            }

            return Json(new
            {
                Reg
            });

        }
    }
}


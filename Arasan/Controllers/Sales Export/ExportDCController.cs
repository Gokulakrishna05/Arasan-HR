using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.Pkcs;
using System.Xml.Linq;
using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
using AspNetCore.Reporting;
using Intuit.Ipp.Data;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Sales_Export
{
    public class ExportDCController : Controller
    {
        IExportDC ExportDC;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public ExportDCController(IExportDC _ExportDC, IConfiguration _configuratio)
        {

            ExportDC = _ExportDC;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult Export_DC(String id)
        {
            ExportDC ca = new ExportDC();
            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Suplst = BindSupplier();
            ca.Loclst = BindWorkCenter();
            ca.joplst = BindJobno();
            ca.Location = Request.Cookies["LocationId"];
            ca.RecList = BindEmp();
            ca.Despatchlst = BindDespatch();
            ca.Inspectedlst = BindInspected();
            ca.Doclst = BindDoc();
            ca.Area = "NONE";
            ca.DCdate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.Refdate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.Emaildate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("edelc");
            if (dtv.Rows.Count > 0)
            {
                ca.DCNo = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
            }

            List<ExportDCItem> TData = new List<ExportDCItem>();
            ExportDCItem tda = new ExportDCItem();

            List<ScrapItem> TData1 = new List<ScrapItem>();
            ScrapItem tda1 = new ScrapItem();

            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new ExportDCItem();

                    //tda.ItemGrouplst = BindItemGrplst();
                    tda.Itemlst = BindItemlst();
                    tda.Material = "OWN";
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda1 = new ScrapItem();

                    //tda.ItemGrouplst = BindItemGrplst();
                    tda1.Itemlst = BindItemlst();
                    tda1.Rejected = "REJECT";
                    tda1.Isvalid1 = "Y";
                    TData1.Add(tda1);
                }

            }
            else
            {

                // ca = directPurchase.GetDirectPurById(id);


                DataTable dt = new DataTable();
                //double total = 0;
                dt = ExportDC.GetExportDC(id);
                if (dt.Rows.Count > 0)
                {
                    ca.DCNo = dt.Rows[0]["DOCID"].ToString();
                    ca.DCdate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.Location = dt.Rows[0]["LOCID"].ToString();
                    ca.Ref = dt.Rows[0]["REFNO"].ToString();
                    ca.Refdate = dt.Rows[0]["REFDATE"].ToString();
                    ca.Customer = dt.Rows[0]["PARTYID"].ToString();
                    ca.Jobid = dt.Rows[0]["JOBORDNO"].ToString();
                    ca.Recieved = dt.Rows[0]["RECDBY"].ToString();
                    ca.Despatch = dt.Rows[0]["DESPBY"].ToString();
                    ca.Inspected = dt.Rows[0]["INSPBY"].ToString();
                    ca.Doc = dt.Rows[0]["DOCTHROUGH"].ToString();
                    ca.CName = dt.Rows[0]["CNAME"].ToString();
                    ca.Emaildate = dt.Rows[0]["SMSDATE"].ToString();
                    ca.Send = dt.Rows[0]["SENDSMS"].ToString();
                    ca.Deliver = dt.Rows[0]["DELIVAT"].ToString();
                    ca.Narration = dt.Rows[0]["DELIVAT"].ToString();
                    ca.ID = id;


                }
                //DataTable dt2 = new DataTable();

                //dt2 = ExportDC.GetExportDCItem(id);
                //if (dt2.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dt2.Rows.Count; i++)
                //    {
                //        tda = new ExportDCItem();
                //        tda.Itemlst = BindItemlst();
                //        tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                //        tda.Des = dt2.Rows[0]["PACKSPEC"].ToString();
                //        tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                //        tda.qty = dt2.Rows[i]["MQTY"].ToString();
                //        tda.Rate = dt2.Rows[i]["ISSRATE"].ToString();
                //        tda.Amount = dt2.Rows[i]["ISSAMT"].ToString();
                //        tda.Lot = dt2.Rows[i]["LOTYN"].ToString();
                //        tda.Serial = dt2.Rows[i]["SERIALYN"].ToString();
                //        tda.Seal = dt2.Rows[i]["SEALNO"].ToString();
                //        tda.Order = dt2.Rows[i]["ORDQTY"].ToString();
                //        tda.Current = dt2.Rows[i]["CLSTOCK"].ToString();
                //        tda.DC = dt2.Rows[i]["DCQTY"].ToString();
                //        tda.QtyPrimary = dt2.Rows[i]["QTY"].ToString();
                //        tda.Quantity = dt2.Rows[i]["QDISC"].ToString();
                //        tda.CashDisc = dt2.Rows[i]["CDISC"].ToString();
                //        tda.Introduction = dt2.Rows[i]["IDISC"].ToString();
                //        tda.Trade = dt2.Rows[i]["TDISC"].ToString();
                //        tda.Addition = dt2.Rows[i]["ADISC"].ToString();
                //        tda.Special = dt2.Rows[i]["SDISC"].ToString();
                //        tda.Fright = dt2.Rows[i]["FREIGHT"].ToString();
                //        tda.Drum = dt2.Rows[i]["DRUMDESC"].ToString();
                //        tda.Container = dt2.Rows[i]["CNO"].ToString();
                //        tda.Tare = dt2.Rows[i]["TWT"].ToString();
                //        tda.Vechile = dt2.Rows[i]["VECHILENO"].ToString();
                //        tda.Material = dt2.Rows[i]["MATSUPP"].ToString();
                //        TData.Add(tda);
                //    }
                //}
                DataTable dt3 = new DataTable();

                dt3 = ExportDC.GetExportDCScrap(id);
                if (dt3.Rows.Count > 0)
                {
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        tda1 = new ScrapItem();
                        tda1.Rejected = dt3.Rows[i]["RLOCID"].ToString();
                        tda1.Itemlst = BindItemlst();
                        tda1.ItemId = dt3.Rows[i]["RITEMID"].ToString();
                        tda1.Unit = dt3.Rows[i]["UNITID"].ToString();
                        tda1.Stock = dt3.Rows[i]["SL1"].ToString();
                        tda1.CF = dt3.Rows[i]["RCF"].ToString();
                        tda1.Qty = dt3.Rows[i]["RQTY"].ToString();
                        tda1.Rate = dt3.Rows[i]["RCOSTRATE"].ToString();
                        tda1.Amount = dt3.Rows[i]["BILLEDQTY"].ToString();
                        TData1.Add(tda1);
                    }
                }
            }
            ca.ExportDCLst = TData;
            ca.ScrapLst = TData1;

            return View(ca);
        }
        [HttpPost]
        public ActionResult Export_DC(ExportDC Cy, string id)
        {

            try
            {
                Cy.ID = id;
                Cy.Branch = Request.Cookies["BranchId"];
                string Strout = ExportDC.ExportDCCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "ExportDC Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "ExportDC Updated Successfully...!";
                    }
                    return RedirectToAction("ListExportDC");
                }

                else
                {
                    ViewBag.PageTitle = "Edit ExportDC";
                    TempData["notice"] = "Not Inserted";
                    return RedirectToAction("ListExportDC");
                }

                // }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "Not Inserted";
                return RedirectToAction("ListExportDC");
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
                DataTable dtDesg = ExportDC.GetSupplier();
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
        public List<SelectListItem> BindWorkCenter()
        {
            try
            {
                DataTable dtDesg = ExportDC.GetWorkCenter();
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
        public List<SelectListItem> BindJobno()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("SELECT P.DOCID,J.JOPID  FROM EJODRUMALLOCATIONBASIC J,EJOBASIC P WHERE J.JOPID=P.EJOBASICID AND J.IS_ALLOCATE='Y' GROUP BY P.DOCID,J.JOPID ");
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["JOPID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindDespatch()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "ROAD", Value = "Road" });
                lstdesg.Add(new SelectListItem() { Text = "FLIGHT", Value = "Flight" });
                lstdesg.Add(new SelectListItem() { Text = "TRAIN", Value = "Train" });
                lstdesg.Add(new SelectListItem() { Text = "SEA", Value = "Sea" });

                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindInspected()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "OWN", Value = "OWN" });
                lstdesg.Add(new SelectListItem() { Text = "CUSTOMER", Value = "CUSTOMER" });
                lstdesg.Add(new SelectListItem() { Text = "THIRD PARTY", Value = "THIRDPARTY" });

                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindDoc()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "BY HAND", Value = "BYHAND" });
                lstdesg.Add(new SelectListItem() { Text = "COURIER", Value = "COURIER" });
                lstdesg.Add(new SelectListItem() { Text = "BANK", Value = "BANK" });
                lstdesg.Add(new SelectListItem() { Text = "FAX", Value = "FAX" });

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
                DataTable dtDesg = ExportDC.GetItem();
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
                string cf = "";
                string price = "";
                dt = ExportDC.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    Desc = dt.Rows[0]["ITEMDESC"].ToString();
                    unit = dt.Rows[0]["UNITID"].ToString();
                    cf = dt.Rows[0]["CF"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();


                }

                var result = new { Desc = Desc, unit = unit, cf = cf , price = price };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetItemScrap(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                string Desc = "";
                string unit = "";
                string cf = "";
                string price = "";
                dt = ExportDC.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    Desc = dt.Rows[0]["ITEMDESC"].ToString();
                    unit = dt.Rows[0]["UNITID"].ToString();
                    cf = dt.Rows[0]["CF"].ToString();
                    price = dt.Rows[0]["LATPURPRICE"].ToString();


                }

                var result = new { Desc = Desc, unit = unit, cf = cf, price = price };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemGrpJSON()
        {
            ExportDCItem model = new ExportDCItem();
            model.Itemlst = BindItemlst();
            return Json(BindItemlst());
        }
        public JsonResult GetItemJSON()
        {
            ScrapItem model = new ScrapItem();
            model.Itemlst = BindItemlst();
            return Json(BindItemlst());
        }
        public IActionResult ListExportDC()
        {
            return View();
        }
        public ActionResult MyListExportDCGrid(string strStatus)
        {
            List<ListExportDCItems> Reg = new List<ListExportDCItems>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)ExportDC.GetAllListExportDC(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string ViewRow = string.Empty;
                string EditRow = string.Empty;
                string DeleteRow = string.Empty;

                if (dtUsers.Rows[i]["STATUS"].ToString() == "Y")
                {
                    EditRow = "<a href=Export_DC?id=" + dtUsers.Rows[i]["EDCBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    ViewRow = "<a href=ViewExportDC?id=" + dtUsers.Rows[i]["EDCBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View' /></a>";
                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["EDCBASICID"].ToString() + "";

                }
                else
                {
                    DeleteRow = "DeleteMR?tag=Active&id=" + dtUsers.Rows[i]["EDCBASICID"].ToString() + "";
                }
                Reg.Add(new ListExportDCItems
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["EDCBASICID"].ToString()),
                    dcno = dtUsers.Rows[i]["DOCID"].ToString(),
                    dcdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    loc = dtUsers.Rows[i]["LOCID"].ToString(),
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
        public IActionResult ViewExportDC(string id)
        {
            ExportDC ca = new ExportDC();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();

            dt = ExportDC.GetExportDCView(id);
            if (dt.Rows.Count > 0)
            {
                ca.DCNo = dt.Rows[0]["DOCID"].ToString();
                ca.DCdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.Location = dt.Rows[0]["LOCID"].ToString();
                ca.Ref = dt.Rows[0]["REFNO"].ToString();
                ca.Refdate = dt.Rows[0]["REFDATE"].ToString();
                ca.Customer = dt.Rows[0]["PARTYID"].ToString();
                ca.Jobid = dt.Rows[0]["JOBORDNO"].ToString();
                ca.Recieved = dt.Rows[0]["EMPNAME"].ToString();
                ca.Despatch = dt.Rows[0]["DESPBY"].ToString();
                ca.Inspected = dt.Rows[0]["INSPBY"].ToString();
                ca.Doc = dt.Rows[0]["DOCTHROUGH"].ToString();
                ca.CName = dt.Rows[0]["CNAME"].ToString();
                ca.Emaildate = dt.Rows[0]["SMSDATE"].ToString();
                ca.Send = dt.Rows[0]["SENDSMS"].ToString();
                ca.Deliver = dt.Rows[0]["DELIVAT"].ToString();
                ca.Narration = dt.Rows[0]["DELIVAT"].ToString();
                ca.ID = id;
              
            }
            List<ExportDCItem> TData = new List<ExportDCItem>();
            ExportDCItem tda = new ExportDCItem();
            DataTable dt2 = new DataTable();

            dt2 = ExportDC.GetExportDCItemview(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new ExportDCItem();
                    tda.Itemlst = BindItemlst();
                    tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                    tda.des = dt2.Rows[0]["PACKSPEC"].ToString();
                    tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                    tda.qty = Convert.ToDouble(dt2.Rows[i]["MQTY"].ToString());
                    tda.rate = Convert.ToDouble(dt2.Rows[i]["ISSRATE"].ToString());
                    tda.Amount = dt2.Rows[i]["ISSAMT"].ToString();
                    tda.Lot = dt2.Rows[i]["LOTYN"].ToString();
                    tda.Serial = dt2.Rows[i]["SERIALYN"].ToString();
                    tda.Seal = dt2.Rows[i]["SEALNO"].ToString();
                    tda.Order = dt2.Rows[i]["ORDQTY"].ToString();
                    tda.Current = dt2.Rows[i]["CLSTOCK"].ToString();
                    tda.DC = dt2.Rows[i]["DCQTY"].ToString();
                    tda.QtyPrimary = dt2.Rows[i]["QTY"].ToString();
                    tda.quantity = Convert.ToDouble(dt2.Rows[i]["QDISC"].ToString());
                    tda.CashDisc = dt2.Rows[i]["CDISC"].ToString();
                    tda.Introduction = dt2.Rows[i]["IDISC"].ToString();
                    tda.Trade = dt2.Rows[i]["TDISC"].ToString();
                    tda.Addition = dt2.Rows[i]["ADISC"].ToString();
                    tda.Special = dt2.Rows[i]["SDISC"].ToString();
                    tda.Fright = dt2.Rows[i]["FREIGHT"].ToString();
                    tda.Drum = dt2.Rows[i]["DRUMDESC"].ToString();
                    tda.Container = dt2.Rows[i]["CNO"].ToString();
                    tda.Tare = dt2.Rows[i]["TWT"].ToString();
                    tda.Vechile = dt2.Rows[i]["VECHILENO"].ToString();
                    tda.Material = dt2.Rows[i]["MATSUPP"].ToString();
                    TData.Add(tda);
                }
            }
            List<ScrapItem> TData1 = new List<ScrapItem>();
            ScrapItem tda1 = new ScrapItem();
            DataTable dt3 = new DataTable();

            dt3 = ExportDC.GetExportDCScrapview(id);
            if (dt3.Rows.Count > 0)
            {
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    tda1 = new ScrapItem();
                    tda1.Rejected = dt3.Rows[i]["RLOCID"].ToString();
                    tda1.Itemlst = BindItemlst();
                    tda1.ItemId = dt3.Rows[i]["ITEMID"].ToString();
                    tda1.Unit = dt3.Rows[i]["UNITID"].ToString();
                    tda1.Stock = dt3.Rows[i]["SL1"].ToString();
                    tda1.CF = dt3.Rows[i]["RCF"].ToString();
                    tda1.Qty = dt3.Rows[i]["RQTY"].ToString();
                    tda1.Rate = dt3.Rows[i]["RCOSTRATE"].ToString();
                    tda1.Amount = dt3.Rows[i]["BILLEDQTY"].ToString();
                    TData1.Add(tda1);
                }
            }
            ca.ExportDCLst = TData;
            ca.ScrapLst = TData1;
            return View(ca);
        }
        public ActionResult DeleteMR(string tag, string id)
        {
            string flag = "";
            if (tag == "Del")
            {
                flag = ExportDC.StatusChange(tag, id);
            }
            else
            {
                flag = ExportDC.ActStatusChange(tag, id);
            }

            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListExportDC");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListExportDC");
            }
        }
        public ActionResult GetJoDetails(string custid)
        {
            try
            {
                string pono = "";
                string order = "";
                string vno = "";
                string disp = "";
                string trans = "";
                string shipdis = "";
                string cust = "";
                string partyid = "";
                string adress = "";
                string narr = "";
                DataTable did = new DataTable();
               // DataTable detid1 = datatrans.GetData("select JOPID FROM EJODRUMALLOCATIONBASIC WHERE CUSTOMERID='" + custid + "' GROUP BY JOPID");
                DataTable detid2 = datatrans.GetData("select JB.ORDTYPE,JB.CREFNO,JD.TRUCKNO,P.SDIST,JD.CUSTOMERID,P.PARTYNAME,P.PARTYMASTID,P.ADD1||','||P.ADD2||','||P.ADD2||','||P.CITY||','||P.STATE as address FROM EJODRUMALLOCATIONBASIC JD,EJOBASIC JB,PARTYMAST P WHERE P.PARTYMASTID=JB.PARTYID AND JD.JOPID=JB.EJOBASICID  AND JB.EJOBASICID='" + custid + "'");

                if (detid2.Rows.Count > 0)
                {
                    for (int i = 0; i < detid2.Rows.Count; i++)
                    {

                         pono = detid2.Rows[0]["CREFNO"].ToString();
                      //  order = detid2.Rows[0]["ORDTYPE"].ToString();
                      //  vno = detid2.Rows[0]["TRUCKNO"].ToString();
                      //  disp = detid2.Rows[0]["DESPTHROUGH"].ToString();
                      //  trans = detid2.Rows[0]["TRANSP"].ToString();
                      //  shipdis = detid2.Rows[0]["SDIST"].ToString();
                        cust = detid2.Rows[0]["PARTYNAME"].ToString();
                        partyid = detid2.Rows[0]["PARTYMASTID"].ToString();
                        adress = detid2.Rows[0]["address"].ToString();

                        narr = "Delivered to " + detid2.Rows[0]["PARTYNAME"].ToString();
                    }
                }

              
                var result = new { pono = pono, cust= cust, custid= partyid, adress= adress, narr= narr/*, order = order, vno = vno, disp = disp, trans = trans, shipdis = shipdis*/ };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Getdrumdetails(string schid)
        {
            try
            {
                string drumid = "";
                DataTable dtEnq = ExportDC.GetDrumDetails(schid);
                for (int i = 0; i < dtEnq.Rows.Count; i++)
                {
                    string dmid = dtEnq.Rows[i]["PLSTOCKID"].ToString();
                    drumid = String.Format("{0},{1}", dmid, drumid);
                }
                drumid = drumid.TrimEnd(',');

                var result = new { drumid = drumid };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetInvoiceDetails(string id)
        {
            ExportDC model = new ExportDC();

            List<ExportDCItem> Data = new List<ExportDCItem>();
            ExportDCItem tda = new ExportDCItem();
            //SalesInvoiceItem tda1 = new SalesInvoiceItem();
            DataTable detid1 = datatrans.GetData("select EJOSCHEDULE.EJOSCHEDULEID,EJOSCHEDULE.EJOBASICID FROM EJODRUMALLOCATIONBASIC,EJOSCHEDULE WHERE EJOSCHEDULE.EJOSCHEDULEID=EJODRUMALLOCATIONBASIC.EJOSCHEDULEID AND JOPID='" + id + "' AND EJODRUMALLOCATIONBASIC.IS_ALLOCATE='Y' AND EJODRUMALLOCATIONBASIC.STATUS IS Null");
            if (detid1.Rows.Count > 0)
            {
                for (int i = 0; i < detid1.Rows.Count; i++)
                {
                    string detid = datatrans.GetDataString("select PARENTRECORDID FROM EJOSCHEDULE WHERE EJOSCHEDULEID='" + detid1.Rows[i]["EJOSCHEDULEID"].ToString() + "'");
                    string sch = datatrans.GetDataString("select SCHDATE FROM EJOSCHEDULE WHERE EJOSCHEDULEID='" + detid1.Rows[i]["EJOSCHEDULEID"].ToString() + "'");
                    DataTable dt2 = datatrans.GetData("SELECT JD.EJODETAILID,JB.DOCID work,QTY,MATSUPP,JD.ITEMID item,ITEMMASTER.ITEMID,RATE,AMOUNT,UNITMAST.UNITID,ITEMSPEC,PACKSPEC,DISCOUNT,ITEMTYPE,FREIGHTAMT,QDISC,CDISC,IDISC,TDISC,ADISC,SDISC,FREIGHT,DUEDATE,DCQTY,PRECLQTY,MRPQTY,BLOCKQTY,POQTY,PEQTY,REWORKQTY,REJQTY,INVQTY,EJODETAILROW,JS.SCHNO,JS.EJOSCHEDULEID FROM EJODETAIL JD LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID =JD.UNIT LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID =JD.ITEMID ,EJOBASIC JB,EJOSCHEDULE JS WHERE JB.EJOBASICID=JD.EJOBASICID AND JD.EJODETAILID=JS.PARENTRECORDID AND  JD.EJODETAILID='" + detid + "' AND JS.IS_ALLOCATE='Y'");
                    //DataTable fri = datatrans.GetData("SELECT ITEMID,JODETAILID FROM JODETAIL WHERE JOBASICID='" + dti.Rows[0]["JOBASICID"].ToString() + "'");

                    tda = new ExportDCItem();
                    tda.work = dt2.Rows[0]["work"].ToString();
                    tda.jobschid = dt2.Rows[0]["SCHNO"].ToString();
                    tda.jodetid = dt2.Rows[0]["EJODETAILID"].ToString();
                    tda.schid = dt2.Rows[0]["EJOSCHEDULEID"].ToString();
                    tda.itemss = dt2.Rows[0]["ITEMID"].ToString();
                    tda.saveitem = dt2.Rows[0]["item"].ToString();
                    tda.unitname = dt2.Rows[0]["UNITID"].ToString();
                    tda.itemtypes = dt2.Rows[0]["ITEMTYPE"].ToString();
                    tda.itemdesc = dt2.Rows[0]["ITEMSPEC"].ToString();
                    tda.des = dt2.Rows[0]["PACKSPEC"].ToString();
                    DateTime ssd = DateTime.Parse(sch);
                    tda.shedate = ssd.ToString("dd-MMM-yyyy");
                    
                    string totqty = datatrans.GetDataString("SELECT SUM(QTY) FROM EJODRUMALLOCATIONDETAIL JD,EJODRUMALLOCATIONBASIC JB WHERE JB.EJODRUMALLOCATIONBASICID=JD.EJODRUMALLOCATIONBASICID AND JB.EJOSCHEDULEID='" + detid1.Rows[i]["EJOSCHEDULEID"].ToString() + "' AND JB.IS_ALLOCATE='Y'");

                    //string tarrif = datatrans.GetDataString("select ETARIFFMASTERID from ETARIFFMASTER WHERE TARIFFID='"+ tt + "'");

                   
                    tda.frigcharges = Convert.ToDouble(dt2.Rows[0]["FREIGHTAMT"].ToString() == "" ? "0" : dt2.Rows[0]["FREIGHTAMT"].ToString());
                    tda.discamt = Convert.ToDouble(dt2.Rows[0]["QDISC"].ToString() == "" ? "0" : dt2.Rows[0]["QDISC"].ToString());
                   
                    tda.introdisc = Convert.ToDouble(dt2.Rows[0]["IDISC"].ToString() == "" ? "0" : dt2.Rows[0]["IDISC"].ToString());
                    tda.cashdis = Convert.ToDouble(dt2.Rows[0]["CDISC"].ToString() == "" ? "0" : dt2.Rows[0]["CDISC"].ToString());
                    tda.tradedis = Convert.ToDouble(dt2.Rows[0]["TDISC"].ToString() == "" ? "0" : dt2.Rows[0]["TDISC"].ToString());
                    tda.adddis = Convert.ToDouble(dt2.Rows[0]["ADISC"].ToString() == "" ? "0" : dt2.Rows[0]["ADISC"].ToString());
                    tda.specdis = Convert.ToDouble(dt2.Rows[0]["SDISC"].ToString() == "" ? "0" : dt2.Rows[0]["SDISC"].ToString());
                    //tda.TotalAmount = Convert.ToDouble(dt2.Rows[i]["TOTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["TOTAMT"].ToString());
                    tda.rate = Convert.ToDouble(dt2.Rows[0]["RATE"].ToString() == "" ? "0" : dt2.Rows[0]["RATE"].ToString());
                    tda.amountt = Convert.ToDouble(dt2.Rows[0]["AMOUNT"].ToString() == "" ? "0" : dt2.Rows[0]["AMOUNT"].ToString());
                    tda.quantity = Convert.ToDouble(totqty);



                    Data.Add(tda);

                }
 


            }



            model.ExportDCLst = Data;
            return Json(model.ExportDCLst);

        }

        public ActionResult DrumSelection(string schid, string rowid)
        {
            EWorkItem ca = new EWorkItem();
            List<EDrumdetails> TData = new List<EDrumdetails>();
            EDrumdetails tda = new EDrumdetails();
            DataTable dtEnq = new DataTable();
            dtEnq = datatrans.GetData("select DRUMNO,QTY,RATE,LOTNO,EJODRUMALLOCATIONDETAILID,PLSTOCKID from EJODRUMALLOCATIONDETAIL D,EJODRUMALLOCATIONBASIC B where B.EJODRUMALLOCATIONBASICID=D.EJODRUMALLOCATIONBASICID AND B.EJOSCHEDULEID= '" + schid + "' AND B.IS_ALLOCATE='Y' ORDER BY DRUMNO");
            int sn = 1;
            for (int i = 0; i < dtEnq.Rows.Count; i++)
            {
                tda = new EDrumdetails();
                tda.sno = sn.ToString();
                tda.lotno = dtEnq.Rows[i]["LOTNO"].ToString();
                tda.drumno = dtEnq.Rows[i]["DRUMNO"].ToString();
                tda.qty = dtEnq.Rows[i]["QTY"].ToString();
                tda.rate = dtEnq.Rows[i]["RATE"].ToString();
                //tda.invid = dtEnq.Rows[i]["PLotmastID"].ToString();
                sn++;
                TData.Add(tda);
            }
            ca.drumlst = TData;
            return View(ca);
        }

    }
}

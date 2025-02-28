using Arasan.Interface;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Controllers
{
    public class ExportInvoiceController : Controller
    {
        IExportInvoice ExportInvoice;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public ExportInvoiceController(IExportInvoice _ExportInvoice, IConfiguration _configuratio)
        {

            ExportInvoice = _ExportInvoice;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult Export_Invoice(string id)
        {
            ExportInvoice ca = new ExportInvoice();
            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Loclst = BindWorkCenter();
            ca.Curlst = BindCurrency();
            ca.Suplst = BindSupplier();
            ca.Suplst2 = BindSupplier2();
            ca.Voclst = BindVocherType();
            ca.Templst = BindTemplete();
            ca.Orderlst = BindOrderType();
            ca.Schemelst = BindScheme();
            ca.Termslst = BindTerms();
            ca.arealist = BindArea("");
            ca.InvDate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.RefDate = DateTime.Now.ToString("dd-MMM-yyyy");

            List<ExportInvoiceItem> TData = new List<ExportInvoiceItem>();
            ExportInvoiceItem tda = new ExportInvoiceItem();

            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new ExportInvoiceItem();
                    tda.Itemlst = BindItemlst();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {

            }
            ca.InvoiceLst = TData;
            return View(ca);
        }
        [HttpPost]
        public ActionResult Export_Invoice(ExportInvoice Cy, string id)
        {

            try
            {
                Cy.ID = id;
                Cy.Branch = Request.Cookies["BranchId"];
                string Strout = ExportInvoice.slaesinvoiceCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "ExportSalesInvoice Inserted Successfully...!";
                    }
                  
                    return RedirectToAction("ListExportDC");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Export_Invoice";
                    TempData["notice"] = "Not Inserted";
                    return RedirectToAction("ListExportInvoice");
                }

                // }
            }
            catch (Exception ex)
            {
                TempData["notice"] = "Not Inserted";
                return RedirectToAction("ListExportInvoice");
            }

            return View(Cy);
        }
        public List<SelectListItem> BindItemlst()
        {
            try
            {
                DataTable dtDesg = ExportInvoice.GetItem();
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
        public List<SelectListItem> BindWorkCenter()
        {
            try
            {
                DataTable dtDesg = ExportInvoice.GetWorkCenter();
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
                DataTable dtDesg = datatrans.GetData("SELECT PARTYMASTID,PARTYNAME FROM PARTYMAST WHERE PARTYMAST.TYPE IN ('Customer','BOTH')  AND COUNTRY NOT IN ('INDIA','null')");
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
        public List<SelectListItem> BindSupplier2()
        {
            try
            {
                DataTable dtDesg = ExportInvoice.GetSupplier2();
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
        public List<SelectListItem> BindScheme()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "ADVANCELICENCESCHME", Value = "ADVANCELICENCESCHME" });
                lstdesg.Add(new SelectListItem() { Text = "DUTY DRAWBACK", Value = "DUTYDRAWBACK" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindOrderType()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "SAMPLE", Value = "SAMPLE" });
                lstdesg.Add(new SelectListItem() { Text = "ORDER", Value = "ORDER" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindVocherType()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "R", Value = "R" });
                lstdesg.Add(new SelectListItem() { Text = "I", Value = "I" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindTemplete()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "EEI-EXEN", Value = "EEI-EXEN" });
                lstdesg.Add(new SelectListItem() { Text = "EEI-IGST 18%", Value = "EEI-IGST18%" });
                lstdesg.Add(new SelectListItem() { Text = "EEI-IGST-FI 18%", Value = "EEI-IGST-FI18%" });
                lstdesg.Add(new SelectListItem() { Text = "EEI-SAMPLE", Value = "EEI-IGSTSAMPLE18%" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindTerms()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "FOB", Value = "FOB" });
                lstdesg.Add(new SelectListItem() { Text = "CPT", Value = "CPT" });
                lstdesg.Add(new SelectListItem() { Text = "C&F", Value = "C&F" });
                lstdesg.Add(new SelectListItem() { Text = "CFR", Value = "CFR" });
                lstdesg.Add(new SelectListItem() { Text = "CIF", Value = "CIF" });
                
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetPartyaddrJSON(string custid)
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindArea(custid));
        }
        public List<SelectListItem> BindArea(string custid)
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("select ADDBOOKTYPE,PARTYMASTADDRESSID from PARTYMASTADDRESS where PARTYMASTID='" + custid + "' UNION SELECT 'None',0 FROM DUAL");
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ADDBOOKTYPE"].ToString(), Value = dtDesg.Rows[i]["ADDBOOKTYPE"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetAreaDetail(string ItemId, string custid)
        {
            try
            {
                DataTable dt = new DataTable();

                string reciver = "";
                //string address = "";
                string state = "";
                string city = "";
                string pincode = "";
                string phone = "";
                string email = "";
                string fax = "";
                string add1 = "";
                string add2 = "";
                string add3 = "";
                string shd = "";
                string shipdist = "";


                dt = datatrans.GetData("Select PARTYMASTADDRESSID,ADDBOOKCOMPANY,SPHONE,SFAX,SEMAIL,SSTATE,SCITY,SPINCODE,SADD1,SADD2,SADD3,SSHIPD from PARTYMASTADDRESS  where ADDBOOKTYPE='" + ItemId + "' AND PARTYMASTID='" + custid + "'");

                if (dt.Rows.Count > 0)
                {

                    reciver = dt.Rows[0]["ADDBOOKCOMPANY"].ToString();
                    //address = dt.Rows[0]["address"].ToString();
                    state = dt.Rows[0]["SSTATE"].ToString();
                    city = dt.Rows[0]["SCITY"].ToString();
                    pincode = dt.Rows[0]["SPINCODE"].ToString();
                    phone = dt.Rows[0]["SPHONE"].ToString();
                    email = dt.Rows[0]["SEMAIL"].ToString();
                    fax = dt.Rows[0]["SFAX"].ToString();
                    add1 = dt.Rows[0]["SADD1"].ToString();
                    add2 = dt.Rows[0]["SADD2"].ToString();
                    add3 = dt.Rows[0]["SADD3"].ToString();
                    shd = dt.Rows[0]["SSHIPD"].ToString();


                }
                shipdist = datatrans.GetDataString("SELECT SDIST FROM PARTYMAST WHERE PARTYMASTID='" + custid + "'");

                var result = new { reciver = reciver, state = state, city = city, pincode = pincode, phone = phone, email = email, fax = fax, add1 = add1, add2 = add2, add3 = add3, shd = shd, shipdist = shipdist };
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
                string country = "";
                string code = "";
                dt = datatrans.GetData("SELECT PARTYMAST.COUNTRY,CONMAST.COUNTRYCODE FROM PARTYMAST,CONMAST  WHERE CONMAST.COUNTRY=PARTYMAST.COUNTRY AND  PARTYMASTID ='" + ItemId + "'");

                if (dt.Rows.Count > 0)
                {
                    country = dt.Rows[0]["COUNTRY"].ToString();
                    code = dt.Rows[0]["COUNTRYCODE"].ToString();
                }

                var result = new { country = country , code = code };
                return Json(result);
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
                string rate = "";
                dt = ExportInvoice.GetExRateDetails(ItemId);

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
        public ActionResult GetdcDetails(string ItemId)
        {
            try
            {
                string pono = "";
                string order = "";
                string vno = "";
                string disp = "";
                string trans = "";
                string shipdis = "";
                string loc = "";
                string curr = "";
                DataTable did = new DataTable();
               // DataTable detid1 = datatrans.GetData("select JOPID FROM JODRUMALLOCATIONBASIC WHERE CUSTOMERID='" + custid + "' GROUP BY JOPID");
                DataTable detid2 = datatrans.GetData("select JB.ORDTYPE,JD.REFNO, JD.LOCID,JB.MAINCURRENCY    FROM EDCBASIC JD,EJOBASIC JB  WHERE   JD.JOBORDNO=JB.EJOBASICID AND JD.PARTYID='" + ItemId + "' AND JD.INVOICENO IS NULL");

                if (detid2.Rows.Count > 0)
                {
                    for (int i = 0; i < detid2.Rows.Count; i++)
                    {

                        pono = detid2.Rows[0]["REFNO"].ToString();
                        order = detid2.Rows[0]["ORDTYPE"].ToString();
                        loc = detid2.Rows[0]["LOCID"].ToString();
                        curr = detid2.Rows[0]["MAINCURRENCY"].ToString();
                       // vno = detid2.Rows[0]["TRUCKNO"].ToString();
                       // disp = detid2.Rows[0]["DESPTHROUGH"].ToString();
                        //trans = detid2.Rows[0]["TRANSP"].ToString();
                        //shipdis = detid2.Rows[0]["SDIST"].ToString();
                    }
                }


                var result = new { pono = pono, order = order, loc= loc, curr= curr/* vno = vno, disp = disp, trans = trans, shipdis = shipdis*/ };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetInvoiceDetails(string id)
        {
            ExportInvoice model = new ExportInvoice();

            List<ExportInvoiceItem> Data = new List<ExportInvoiceItem>();
            ExportInvoiceItem tda = new ExportInvoiceItem();
            //SalesInvoiceItem tda1 = new SalesInvoiceItem();

            DataTable detid1 = datatrans.GetData("select EDCBASIC.EDCBASICID  FROM EDCBASIC WHERE PARTYID='" + id + "'  AND EDCBASIC.INVOICENO IS Null");
            if (detid1.Rows.Count > 0)
            {
                DataTable dt2 = datatrans.GetData("SELECT ITEMMASTER.ITEMID,JD.EDCDETAILID,JB.EJOBASICID,JD.ITEMID as item ,UNITMAST.UNITID,JD.QTY,JD.ISSRATE,JD.ISSAMT,JD.QDISC,JD.CDISC,JD.IDISC,JD.TDISC,JD.ADISC,JD.SDISC,JD.FREIGHT,JD.DRUMDESC,JD.PACKSPEC,JD.GWT,JD.NWT,JD.TWT,JD.TDRUM,JB.DOCID as jobno,to_char(JB.DOCDATE,'dd-MON-yyyy')jodate,JS.DOCID as dcno,to_char(JS.DOCDATE,'dd-MON-yyyy')dcdate,EJ.ITEMSPEC,EJ.ITEMTYPE,EJ.EJODETAILID FROM EDCDETAIL JD LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID =JD.UNIT LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID =JD.ITEMID ,EJOBASIC JB,EDCBASIC JS,EJODETAIL EJ  WHERE JS.EDCBASICID=JD.EDCBASICID AND JS.JOBORDNO=JB.EJOBASICID AND EJ.EJODETAILID=JD.JODETAILID   AND JD.EDCBASICID='" + detid1.Rows[0]["EDCBASICID"].ToString() + "'");

                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        //  string detid = datatrans.GetDataString("select PARENTRECORDID FROM EJOSCHEDULE WHERE EJOSCHEDULEID='" + detid1.Rows[i]["EJOSCHEDULEID"].ToString() + "'");
                        // string sch = datatrans.GetDataString("select SCHDATE FROM EJOSCHEDULE WHERE EJOSCHEDULEID='" + detid1.Rows[i]["EJOSCHEDULEID"].ToString() + "'");
                        //DataTable fri = datatrans.GetData("SELECT ITEMID,JODETAILID FROM JODETAIL WHERE JOBASICID='" + dti.Rows[0]["JOBASICID"].ToString() + "'");

                        tda = new ExportInvoiceItem();
                        tda.work = dt2.Rows[0]["jobno"].ToString();
                        tda.workid = dt2.Rows[0]["EJOBASICID"].ToString();
                        tda.dcno = dt2.Rows[0]["dcno"].ToString();
                        tda.dcid = dt2.Rows[0]["EDCDETAILID"].ToString();
                        tda.jodate = dt2.Rows[0]["jodate"].ToString();
                        tda.dcdate = dt2.Rows[0]["dcdate"].ToString();
                        tda.itemss = dt2.Rows[0]["ITEMID"].ToString();
                        tda.saveitem = dt2.Rows[0]["item"].ToString();
                        tda.unitname = dt2.Rows[0]["UNITID"].ToString();
                        tda.itemtypes = dt2.Rows[0]["ITEMTYPE"].ToString();
                        tda.itemdesc = dt2.Rows[0]["ITEMSPEC"].ToString();
                        tda.des = dt2.Rows[0]["PACKSPEC"].ToString();
                        tda.drum = dt2.Rows[0]["DRUMDESC"].ToString();
                        tda.gwt = dt2.Rows[0]["GWT"].ToString();
                        tda.nwt = dt2.Rows[0]["NWT"].ToString();
                        tda.twt = dt2.Rows[0]["TWT"].ToString();
                        tda.schid = datatrans.GetDataString("select EJOSCHEDULEID FROM EJOSCHEDULE WHERE PARENTRECORDID ='" + dt2.Rows[i]["EJODETAILID"].ToString() + "'");

                        tda.frigcharges = Convert.ToDouble(dt2.Rows[0]["FREIGHT"].ToString() == "" ? "0" : dt2.Rows[0]["FREIGHT"].ToString());
                        tda.discamt = Convert.ToDouble(dt2.Rows[0]["QDISC"].ToString() == "" ? "0" : dt2.Rows[0]["QDISC"].ToString());
                        tda.quantity = Convert.ToDouble(dt2.Rows[0]["QTY"].ToString() == "" ? "0" : dt2.Rows[0]["QTY"].ToString());

                        tda.introdisc = Convert.ToDouble(dt2.Rows[0]["IDISC"].ToString() == "" ? "0" : dt2.Rows[0]["IDISC"].ToString());
                        tda.cashdis = Convert.ToDouble(dt2.Rows[0]["CDISC"].ToString() == "" ? "0" : dt2.Rows[0]["CDISC"].ToString());
                        tda.tradedis = Convert.ToDouble(dt2.Rows[0]["TDISC"].ToString() == "" ? "0" : dt2.Rows[0]["TDISC"].ToString());
                        tda.adddis = Convert.ToDouble(dt2.Rows[0]["ADISC"].ToString() == "" ? "0" : dt2.Rows[0]["ADISC"].ToString());
                        tda.specdis = Convert.ToDouble(dt2.Rows[0]["SDISC"].ToString() == "" ? "0" : dt2.Rows[0]["SDISC"].ToString());
                        //tda.TotalAmount = Convert.ToDouble(dt2.Rows[i]["TOTAMT"].ToString() == "" ? "0" : dt2.Rows[i]["TOTAMT"].ToString());
                        tda.rate = Convert.ToDouble(dt2.Rows[0]["ISSRATE"].ToString() == "" ? "0" : dt2.Rows[0]["ISSRATE"].ToString());
                        tda.amountt = Convert.ToDouble(dt2.Rows[0]["ISSAMT"].ToString() == "" ? "0" : dt2.Rows[0]["ISSAMT"].ToString());
                      



                        Data.Add(tda);

                    }



                }

            }

            model.InvoiceLst = Data;
            return Json(model.InvoiceLst);

        }

        public ActionResult Getdrumdetails(string schid)
        {
            try
            {
                string drumid = "";
                DataTable dtEnq = datatrans.GetData("select DRUMNO,QTY,RATE,LOTNO,EJODRUMALLOCATIONDETAILID,PLSTOCKID from EJODRUMALLOCATIONDETAIL D,EJODRUMALLOCATIONBASIC B where B.EJODRUMALLOCATIONBASICID=D.EJODRUMALLOCATIONBASICID AND B.EJOSCHEDULEID= '" + schid + "' AND B.IS_ALLOCATE='Y' ORDER BY DRUMNO");
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

    }
}

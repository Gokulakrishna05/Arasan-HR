using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.Pkcs;
using System.Xml.Linq;
using Arasan.Interface;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Arasan.Controllers.Sales
{
    public class SalesReturnController : Controller
    {
        ISalesReturn SRInterface;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public SalesReturnController(ISalesReturn _SalesReturnService, IConfiguration _configuratio)
        {
            SRInterface = _SalesReturnService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult SalesReturn()
        {
            SalesReturn SR = new SalesReturn();
            SR.invoicelst = BindInvoice();
            SR.vlst = BindVtype();
            DataTable dtv = datatrans.GetSequence("vchpr");
            if (dtv.Rows.Count > 0)
            {
                SR.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["LASTNO"].ToString();
            }
            SR.Vtype = "1";
            SR.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            return View(SR);
        }
        public List<SelectListItem> BindInvoice()
        {
            try
            {
                DataTable dtDesg = SRInterface.GetInvoice();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["PINVBASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindVtype()
        {
            try
            {
                DataTable dtDesg = datatrans.GetVType();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DESCRIPTION"].ToString(), Value = dtDesg.Rows[i]["VCHTYPEID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetInvoice(string invoiceid)
        {
            try
            {
                DataTable dt = new DataTable();
               
                string gross = "";
                string net = "";
                string partyname = "";
                string invdate = "";

                dt = SRInterface.GetInvoiceDetails(invoiceid);

                if (dt.Rows.Count > 0)
                {
                    gross = dt.Rows[0]["GROSS"].ToString();
                    net = dt.Rows[0]["NET"].ToString();
                    partyname = dt.Rows[0]["PARTYNAME"].ToString();
                    invdate = dt.Rows[0]["DOCDATE"].ToString();
                }

                var result = new {  gross = gross, net = net, partyname = partyname , invdate = invdate };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetSRJSON(string invoiceid)
        {
            SalesReturn model = new SalesReturn();
            DataTable dtt = new DataTable();
            List<SalesReturnItem> Data = new List<SalesReturnItem>();
            SalesReturnItem tda = new SalesReturnItem();
            dtt = SRInterface.GetInvoiceItem(invoiceid);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new SalesReturnItem();
                    tda.itemid = dtt.Rows[i]["itemi"].ToString();
                    tda.itemname = dtt.Rows[i]["ITEMID"].ToString();
                    tda.unit = dtt.Rows[i]["UNITID"].ToString();
                    tda.quantity = dtt.Rows[i]["QTY"].ToString();
                    tda.soldqty = dtt.Rows[i]["QTY"].ToString();
                    //tda.confac = dtt.Rows[i]["CF"].ToString();
                    tda.rate = dtt.Rows[i]["RATE"].ToString();
                    tda.amount = dtt.Rows[i]["AMOUNT"].ToString();
                    tda.disc = dtt.Rows[i]["DISCPER"].ToString() == "" ? 0 : Convert.ToDouble(dtt.Rows[i]["DISCPER"].ToString());
                    tda.discamount = dtt.Rows[i]["DISCOUNT"].ToString()=="" ? 0 : Convert.ToDouble(dtt.Rows[i]["DISCOUNT"].ToString());
                    tda.frigcharge = dtt.Rows[i]["FREIGHT"].ToString() == "" ? 0 : Convert.ToDouble(dtt.Rows[i]["FREIGHT"].ToString());
                    tda.cgstamt = Convert.ToDouble(dtt.Rows[i]["CGST"].ToString());
                    tda.cgstper = Convert.ToDouble(dtt.Rows[i]["CGSTP"].ToString());
                    tda.sgstamt = Convert.ToDouble(dtt.Rows[i]["SGST"].ToString());
                    tda.sgstper = Convert.ToDouble(dtt.Rows[i]["SGSTP"].ToString());
                    tda.igstamt = Convert.ToDouble(dtt.Rows[i]["IGST"].ToString());
                    tda.igstper = Convert.ToDouble(dtt.Rows[i]["IGSTP"].ToString());
                    tda.totalamount = dtt.Rows[i]["TOTAMT"].ToString() == "" ? 0 : Convert.ToDouble(dtt.Rows[i]["TOTAMT"].ToString());
                    tda.exicetype= dtt.Rows[i]["EXCISETYPE"].ToString(); 
                    tda.traiffid= dtt.Rows[i]["TARIFFID"].ToString();
                    //tda.binid = dtt.Rows[i]["BINID"].ToString();
                    tda.unitid = dtt.Rows[i]["UNIT"].ToString();
                    DataTable dt = new DataTable();
                    //dt = PurReturn.Getstkqty(grnid, loc, branch);
                    //if (dt.Rows.Count > 0)
                    //{
                    //    tda.stkqty = dt.Rows[0]["QTY"].ToString();
                    //}
                    Data.Add(tda);
                }
            }
            model.returnlist = Data;
            return Json(model.returnlist);

        }
    }
}

using Arasan.Interface;
using Arasan.Interface.Qualitycontrol;
using Arasan.Interface.Sales;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Qualitycontrol;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Reflection;

namespace Arasan.Controllers.Sales
{
    public class DebitNoteBillController : Controller
    {
        IDebitNoteBillService DebitNoteBillService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public DebitNoteBillController(IDebitNoteBillService _DebitNoteBillService, IConfiguration _configuratio)
        {
            DebitNoteBillService = _DebitNoteBillService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult DebitNoteBill(string id, string tag)
        {
            DebitNoteBill ca = new DebitNoteBill();
            ca.Brlst = BindBranch();
            ca.Partylst = BindGParty();
            ca.Vocherlst = BindVocher();
            DataTable dtv = datatrans.GetSequence("Dbnot");
            if (dtv.Rows.Count > 0)
            {
                ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            ca.Location = Request.Cookies["LocationId"];
            ca.Branch = Request.Cookies["BranchId"];
            List<DebitNoteItem> TData = new List<DebitNoteItem>();
            DebitNoteItem tda = new DebitNoteItem();
            if (id == null)
            {
                tda = new DebitNoteItem();
                tda.Grnlst = BindGrnlst("");
                tda.Itemlst = BindItemlst("");
                tda.Invdate = DateTime.Now.ToString("dd-MMM-yyyy");
                //tda.CGSTP = "9";
                //tda.SGSTP = "9";
                //tda.IGSTP = "0";
                tda.Isvalid = "Y";
                TData.Add(tda);

            }
            else
            {


                DataTable dt = new DataTable();
                dt = DebitNoteBillService.GetDebitNoteBillDetail(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    ca.Vocher = dt.Rows[0]["VTYPE"].ToString();
                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.RefNo = dt.Rows[0]["REFNO"].ToString();
                    ca.RefDate = dt.Rows[0]["REFDT"].ToString();
                    ca.Party = dt.Rows[0]["PARTYID"].ToString();
                    ca.Gross = dt.Rows[0]["GROSS"].ToString();
                    ca.Net = dt.Rows[0]["NET"].ToString();
                    ca.Amount = dt.Rows[0]["AMTINWRD"].ToString();
                    ca.Bigst = dt.Rows[0]["BIGST"].ToString();
                    ca.Bsgst = dt.Rows[0]["BSGST"].ToString();
                    ca.Bcgst = dt.Rows[0]["BCGST"].ToString();
                    ca.Narration = dt.Rows[0]["NARRATION"].ToString();



                }
                DataTable dt2 = new DataTable();

                dt2 = DebitNoteBillService.GetDebitNoteBillItem(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new DebitNoteItem();
                        tda.Grnlst = BindGrnlst("");
                        tda.Grnlst = BindGrnlst(ca.Party);
                        tda.InvNo = dt2.Rows[i]["INVNO"].ToString();
                        tda.Invdate = dt2.Rows[i]["INVDT"].ToString();
                        tda.Itemlst = BindItemlst(tda.InvNo);
                        tda.Item = dt2.Rows[i]["ITEMID"].ToString();
                        tda.Cf = dt2.Rows[i]["CONVFACTOR"].ToString();
                        tda.Unit = dt2.Rows[i]["PRIUNIT"].ToString();
                        tda.Qty = dt2.Rows[i]["QTY"].ToString();
                        tda.Rate = dt2.Rows[i]["RATE"].ToString();
                        tda.Amount = dt2.Rows[i]["AMOUNT"].ToString();
                        tda.CGST = dt2.Rows[i]["CGST"].ToString();
                        tda.SGST = dt2.Rows[i]["SGST"].ToString();
                        tda.IGST = dt2.Rows[i]["IGST"].ToString();
                        tda.Total = dt2.Rows[i]["TOTAMT"].ToString();
                        tda.ID = id;
                        TData.Add(tda);
                    }

                }



            }
            ca.Depitlst = TData;
            return View(ca);
        }
        [HttpPost]
        public ActionResult DebitNoteBill(DebitNoteBill Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = DebitNoteBillService.DebitNoteBillCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "- Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "DebitNoteBill Updated Successfully...!";
                    }
                    return RedirectToAction("ListDebitNoteBill");
                }

                else
                {
                    ViewBag.PageTitle = "Edit DebitNoteBill";
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
        [HttpPost]
        public ActionResult Credit_Note_Approval(DebitNoteBill Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = DebitNoteBillService.CreditNoteStock(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    TempData["notice"] = "CreditNote Approved Successfully...!";
                    return RedirectToAction("ListDebitNoteBill");
                }

                else
                {
                    ViewBag.PageTitle = "Edit DebitNoteBill";
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
        public IActionResult Credit_Note_Approval(string PROID)
        {

            DebitNoteBill ca = new DebitNoteBill();
            ca.RecList = BindEmp();
            ca.Currency = "1";
            ca.Exchange = "1";
            ca.Curlst = BindCurrency();
            List<CreditItem> TData = new List<CreditItem>();
            CreditItem tda = new CreditItem();
            tda = new CreditItem();
            tda.Isvalid = "Y";
            tda.Crlst = BindCredit();
            tda.Acclst = BindLedger();
            TData.Add(tda);

            DataTable dt = DebitNoteBillService.EditProEntry(PROID);
            if (dt.Rows.Count > 0)
               {
                    ca.ID = PROID;
                    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.Bra = dt.Rows[0]["BRA"].ToString();
                    ca.Net = dt.Rows[0]["NET"].ToString();
                    ca.Location = Request.Cookies["Locationname"];
                    ca.Loc = Request.Cookies["Locationid"];

                }
            
            ca.Creditlst = TData;
            return View(ca);
        }
        public List<SelectListItem> BindItemlst(string value)
        {
            try
            {
                DataTable dtDesg = DebitNoteBillService.GetItem(value);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["item"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindLedger()
        {
            try
            {
                DataTable dtDesg = datatrans.GetLedger();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DISPLAY_NAME"].ToString(), Value = dtDesg.Rows[i]["LEDGERID"].ToString() });
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
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPNAME"].ToString(), Value = dtDesg.Rows[i]["EMPNAME"].ToString() });
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
        public List<SelectListItem> BindVocher()
        {
            try
            {
                DataTable dtDesg = DebitNoteBillService.GetVocher();
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
        public JsonResult GetPartyJSON(string supid)
        {
            //string CityID = datatrans.GetDataString("Select STATEMASTID from STATEMAST where STATE='" + supid + "' ");
            DebitNoteItem model = new DebitNoteItem();
            model.Grnlst = BindGrnlst(supid);
            return Json(BindGrnlst(supid));

        }
        public List<SelectListItem> BindCredit()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "Dr", Value = "Dr" });
                lstdesg.Add(new SelectListItem() { Text = "Cr", Value = "Cr" });

                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetAccountJSON()
        {

            return Json(BindLedger());

        }
        public ActionResult GetItemDetail(string ItemId,string grnid)
        {
            try
            {
                DataTable dt = new DataTable();


                //string invDate = "";
                //string item = "";
                //string ItemType = "";
                //string ItemSpec = "";
                string cf = "";
                string unit = "";
                string qty = "";
                string rate = "";
                string amount = "";
                string cgstp = "";
                string sgstp = "";
                string igstp = "";

                string cgst = "";
                string sgst = "";
                string igst = "";
                string total = "";

                dt = DebitNoteBillService.GetItemDetails(ItemId, grnid);

                if (dt.Rows.Count > 0)
                {


                    ////invDate = dt.Rows[0]["DOCDATE"].ToString();
                    //item = dt.Rows[0]["ITEMID"].ToString();
                    //ItemType = dt.Rows[0]["UNITID"].ToString();
                    //ItemSpec = dt.Rows[0]["UNITID"].ToString();
                    cf = dt.Rows[0]["CF"].ToString();
                    unit = dt.Rows[0]["PUNIT"].ToString();
                    qty = dt.Rows[0]["QTY"].ToString();
                    rate = dt.Rows[0]["RATE"].ToString();
                    amount = dt.Rows[0]["AMOUNT"].ToString();
                    cgstp = dt.Rows[0]["CGSTP"].ToString();
                    sgstp = dt.Rows[0]["SGSTP"].ToString();
                    igstp = dt.Rows[0]["IGSTP"].ToString();
                    cgst = dt.Rows[0]["CGST"].ToString();
                    sgst = dt.Rows[0]["SGST"].ToString();
                    igst = dt.Rows[0]["IGST"].ToString();
                    total = dt.Rows[0]["TOTAMT"].ToString();

                }

                var result = new { cf = cf, unit = unit, qty = qty, rate = rate, amount = amount, cgstp= cgstp, sgstp= sgstp, igstp= igstp, cgst = cgst, sgst = sgst, igst = igst, total = total };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public ActionResult GetInvoDate(string ItemId)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();


        //       string invDate = "";
               

        //        dt = DebitNoteBillService.GetInvoDates(ItemId);

        //        if (dt.Rows.Count > 0)
        //        {

        //            invDate = dt.Rows[0]["DOCDATE"].ToString();
        //        }

        //        var result = new { invDate = invDate };
        //        return Json(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public IActionResult ListDebitNoteBill()
        {
            IEnumerable<DebitNoteBill> sta = DebitNoteBillService.GetAllDebitNoteBill();
            return View(sta);
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
        public List<SelectListItem> BindGParty()
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
        public List<SelectListItem> BindGrnlst(string id)
        {
            try
            {
                DataTable dtDesg = DebitNoteBillService.GetGrn(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["GRNBLBASICID"].ToString() });
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
            DebitNoteItem model = new DebitNoteItem();
            model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));

        }
    }
}

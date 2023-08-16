using Arasan.Interface.Qualitycontrol;
using Arasan.Interface.Sales;
using Arasan.Models;
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
            
            ca.Branch = Request.Cookies["BranchId"];
            List<DebitNoteItem> TData = new List<DebitNoteItem>();
            DebitNoteItem tda = new DebitNoteItem();
            if (id == null)
            {
                tda = new DebitNoteItem();
                tda.Grnlst = BindGrnlst("");
                tda.CGSTP = "9";
                tda.SGSTP = "9";
                tda.IGSTP = "18";
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
                        TempData["notice"] = "DebitNoteBill Inserted Successfully...!";
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
        public JsonResult GetPartyJSON(string supid)
        {
            //string CityID = datatrans.GetDataString("Select STATEMASTID from STATEMAST where STATE='" + supid + "' ");
            DebitNoteItem model = new DebitNoteItem();
            model.Grnlst = BindGrnlst(supid);
            return Json(BindGrnlst(supid));

        }
        public JsonResult GetItemGrpJSON(string supid)
        {
            //DeductionItem model = new DeductionItem();
            //model.Grnlst = BindGrnlst(supid);
            return Json(BindGrnlst(supid));
        }
        public ActionResult GetItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();


                //string invDate = "";
                string item = "";
                //string ItemType = "";
                //string ItemSpec = "";
                string cf = "";
                string unit = "";
                string qty = "";
                string rate = "";
                string amount = "";
                string cgst = "";
                string sgst = "";
                string igst = "";
                string total = "";

                dt = DebitNoteBillService.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {


                    ////invDate = dt.Rows[0]["DOCDATE"].ToString();
                    item = dt.Rows[0]["ITEMID"].ToString();
                    //ItemType = dt.Rows[0]["UNITID"].ToString();
                    //ItemSpec = dt.Rows[0]["UNITID"].ToString();
                    cf = dt.Rows[0]["CF"].ToString();
                    unit = dt.Rows[0]["PUNIT"].ToString();
                    qty = dt.Rows[0]["QTY"].ToString();
                    rate = dt.Rows[0]["RATE"].ToString();
                    amount = dt.Rows[0]["AMOUNT"].ToString();
                    cgst = dt.Rows[0]["CGST"].ToString();
                    sgst = dt.Rows[0]["SGST"].ToString();
                    igst = dt.Rows[0]["IGST"].ToString();
                    total = dt.Rows[0]["TOTAMT"].ToString();

                }

                var result = new { item = item, cf = cf, unit = unit, qty = qty, rate = rate, amount = amount, cgst = cgst, sgst = sgst, igst = igst, total = total };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetInvoDate(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();


               string invDate = "";
               

                dt = DebitNoteBillService.GetInvoDates(ItemId);

                if (dt.Rows.Count > 0)
                {


                    invDate = dt.Rows[0]["DOCDATE"].ToString();


                }

                var result = new { invDate = invDate };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
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
                DataTable dtDesg = DebitNoteBillService.GetParty();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTYNAME"].ToString(), Value = dtDesg.Rows[i]["PARTYID"].ToString() });
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
        //public JsonResult GetDeptitJSON(string supid)
        //{
        //    DebitNote model = new DebitNote();
        //    model.Grnlst = BindGrnlst(supid);
        //    return Json(BindGrnlst(supid));

        //}
    }
}

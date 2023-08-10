using Arasan.Interface.Qualitycontrol;
using Arasan.Interface.Sales;
using Arasan.Models;
using Arasan.Services.Qualitycontrol;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

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
            List<DebitNote> TData = new List<DebitNote>();
            DebitNote tda = new DebitNote();
            if (id == null)
            {
                tda = new DebitNote();
                tda.Grnlst = BindGrnlst();
                tda.Isvalid = "Y";
                TData.Add(tda);

            }
            else
            {
               
                    //ca = QCTestValueEntryService.GetQCTestValueEntryById(id);
                    //DataTable dt = new DataTable();
                    //dt = DebitNoteBillService.GetQCTestValueEntryDetails(id);
                    //if (dt.Rows.Count > 0)
                    //{
                    //    //ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    //    //ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    //    //ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    //    //ca.Work = dt.Rows[0]["WCID"].ToString();
                    //    //ca.Shift = dt.Rows[0]["SHIFTNO"].ToString();
                    //    //ca.Process = dt.Rows[0]["PROCESSLOTNO"].ToString();
                    //    //ca.Drum = dt.Rows[0]["DRUMNO"].ToString();
                    //    //ca.Prodate = dt.Rows[0]["PRODDATE"].ToString();
                    //    //ca.Sample = dt.Rows[0]["DSAMPLE"].ToString();
                    //    //ca.Sampletime = dt.Rows[0]["DSAMPLETIME"].ToString();
                    //    //ca.Item = dt.Rows[0]["ITEMID"].ToString();
                    //    //ca.Entered = dt.Rows[0]["ENTEREDBY"].ToString();
                    //    //ca.Remarks = dt.Rows[0]["REMARKS"].ToString();



                    //}
               
               

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
        public JsonResult GetItemGrpJSON()
        {
            //DeductionItem model = new DeductionItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindGrnlst());
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
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTYNAME"].ToString(), Value = dtDesg.Rows[i]["PARTYNAME"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindGrnlst()
        {
            try
            {
                DataTable dtDesg = DebitNoteBillService.GetGrn();
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

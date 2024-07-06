using Microsoft.AspNetCore.Mvc;
using Arasan.Interface;
using Arasan.Models;
using System.Data;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Interface.Master;
using Arasan.Services.Master;

namespace Arasan.Controllers.Master
{
    public class CommissionController : Controller
    {
        ICommission CommissionService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public CommissionController(ICommission _CommissiontService, IConfiguration _configuratio)
        {
            CommissionService = _CommissiontService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public IActionResult Commission(String id)
        {

            Commission pm = new Commission();

            pm.Date = DateTime.Now.ToString("dd-MMM-yyyy");
            pm.Valid = DateTime.Now.ToString("dd-MMM-yyyy");
            //pm.Costtypelst = BindCosttype();

            comlst pr = new comlst();
            List<comlst> TData = new List<comlst>();

              
            
             
        
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    pr = new comlst();
                    pr.ilst = BindItem();
                    pr.ulst = BindUnit();
                    pr.commtypelst = BindCommtype();

                    pr.Isvalid = "Y";
                    TData.Add(pr);
                }
                
            }
            //for edit & delete
            else
                    {
                DataTable dt = new DataTable();
                //double total = 0;
                dt = CommissionService.GetEditCommission(id);
                if (dt.Rows.Count > 0)
                {
                    pm.Cid = dt.Rows[0]["DOCID"].ToString();
                    pm.Date = dt.Rows[0]["DOCDATE"].ToString();
                    pm.Code = dt.Rows[0]["COMMCODE"].ToString();
                    pm.Value = dt.Rows[0]["COMMDESC"].ToString();
                    pm.Valid = dt.Rows[0]["VALIDTO"].ToString();
                    pm.ID = id;

                }
                DataTable dt1 = new DataTable();

                        //double total = 0;
                        dt1 = CommissionService.GetEditComm(id);
                        if (dt1.Rows.Count > 0)
                        {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        pr = new comlst();
                        pr.ilst = BindItem();

                        pr.item = dt1.Rows[0]["ITEMID"].ToString();
                        pr.ulst = BindUnit();

                        pr.unit = dt1.Rows[0]["UNIT"].ToString();
                        pr.commtypelst = BindCommtype();

                        pr.type = dt1.Rows[0]["COMMTYPE"].ToString();
                        pr.val = dt1.Rows[0]["COMMVALUE"].ToString();
                        pr.Isvalid = "Y";

                        TData.Add(pr);
                    }
                    //pm.ID = id;

                }

                    }

            pm.colst = TData;
         return View(pm);
        }
        public List<SelectListItem> BindCommtype()
        {
            try
            {
                List<SelectListItem> lstprodhr = new List<SelectListItem>();
                lstprodhr.Add(new SelectListItem() { Text = "% On BasicValue", Value = "% On BasicValue" });
                lstprodhr.Add(new SelectListItem() { Text = "Rs. Per Kg", Value = "Rs. Per Kg" });

                return lstprodhr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindUnit()
        {
            try
            {
                DataTable dtDesg = CommissionService.GetUnit();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["UNITID"].ToString(), Value = dtDesg.Rows[i]["UNITID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindItem()
        {
            try
            {
                DataTable dtDesg = CommissionService.GetItem();
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

        public JsonResult GetItemJSON()
        {
            comlst model = new comlst();
            //model.Itemlst = BindItemlst(itemid);
            return Json(BindUnit());
        }
        public JsonResult GetItemJSON1()
        {
            comlst model = new comlst();
            //model.Itemlst = BindItemlst(itemid);
            return Json(BindItem());
        }
        public JsonResult GetItemJSON2()
        {
            comlst model = new comlst();
            //model.Itemlst = BindItemlst(itemid);
            return Json(BindCommtype());
        }

        [HttpPost]

        public ActionResult Commission(Commission Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = CommissionService.CommissionCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Commission Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Commission Updated Successfully...!";
                    }
                    return RedirectToAction("ListCommission");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Commission";
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
        public IActionResult ListCommission()
        {
            return View();
        }

        public ActionResult MyListCommissiongrid(string strStatus)
        {
            List<Commissionlist> Reg = new List<Commissionlist>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = CommissionService.GetAllCommissions(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    EditRow = "<a href=Commission?id=" + dtUsers.Rows[i]["COMMBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["COMMBASICID"].ToString() + "";

                }
                else
                {
                    EditRow = "";
                    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["COMMBASICID"].ToString() + "";

                }

                Reg.Add(new Commissionlist
                {
                    cid = dtUsers.Rows[i]["DOCID"].ToString(),
                    date = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    code = dtUsers.Rows[i]["COMMCODE"].ToString(),
                    editrow = EditRow,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }

        public ActionResult DeleteMR(string tag, string id)
        {

            string flag = CommissionService.StatusDelete(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListCommission");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListCommission");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = CommissionService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListCommission");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListCommission");
            }
        }
    }
}

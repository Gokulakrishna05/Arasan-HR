using Arasan.Interface;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Controllers
{
    public class OnDutyController : Controller
    {
        IOnDuty onDuty;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public OnDutyController(IOnDuty _onDuty, IConfiguration _configuratio)
        {
            onDuty = _onDuty;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public IActionResult OnDuty(string id)
        {
            OnDuty ca = new OnDuty();

            ca.EmplIdlst = BindEmplId();

            List<ODLS> TData = new List<ODLS>();
            ODLS tda = new ODLS();

            DataTable dtv = datatrans.GetSequence("SEntr");
            if (dtv.Rows.Count > 0)
            {
                ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
            }
            ca.DocDate = DateTime.Now.ToString("dd-MMM-yyyy");

            if (id == null)

            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new ODLS();
                    tda.Stslst = BindStatus(); 
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }

            else
            {
                DataTable dt = new DataTable();

                //double total = 0;
                dt = onDuty.GetEditOnDuty(id);
                if (dt.Rows.Count > 0)
                {
                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.EmplId = dt.Rows[0]["EMPLOYEEID"].ToString();
                    ca.EmplIdlst = BindEmplId();
                    ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.EmpName = dt.Rows[0]["EMPNAME"].ToString();
                    ca.EDes = dt.Rows[0]["EMPDESIGN"].ToString();
                    ca.EGen = dt.Rows[0]["EGENDER"].ToString();
                    ca.ID = id;
                }
            }
            DataTable dt2 = new DataTable();
            dt2 = onDuty.GetEditDutDet(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new ODLS();

                    tda.StartDate = dt2.Rows[i]["DUTYDATE"].ToString();
                    tda.FrTime = dt2.Rows[i]["FROMTIME"].ToString();
                    tda.ToTime = dt2.Rows[i]["TOTIME"].ToString();
                    tda.ToHR = dt2.Rows[i]["TOHRS"].ToString();
                    tda.DuSit = dt2.Rows[i]["DSCN"].ToString();
                    tda.Res = dt2.Rows[i]["REASON"].ToString();
                    tda.Sts = dt2.Rows[i]["STATUS"].ToString();
                    tda.Stslst = BindStatus();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            ca.OdLst = TData;
            return View(ca);
        }

        [HttpPost]

        public ActionResult OnDuty(OnDuty Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = onDuty.OnDutyCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "On Duty Application Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "On Duty Application Updated Successfully...!";
                    }
                    return RedirectToAction("ListOnDuty");
                }

                else
                {
                    ViewBag.PageTitle = "Edit On Duty Application";
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
        public List<SelectListItem> BindStatus()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "APPROVED", Value = "APPROVED" });
                lstdesg.Add(new SelectListItem() { Text = "REJECTED", Value = "REJECTED" });
                lstdesg.Add(new SelectListItem() { Text = "APPLIED", Value = "APPLIED" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindEmplId()
        {
            try
            {
                DataTable dtDesg = onDuty.GetEmplId();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPID"].ToString(), Value = dtDesg.Rows[i]["EMPMASTID"].ToString() });


                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetEMPDetails(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string name = "";
                string des = "";
                string gen = "";

                dt = datatrans.GetData("SELECT EMPNAME,EMPSEX,EMPDESIGN FROM EMPMAST WHERE EMPMASTID='" + ItemId + "'");

                if (dt.Rows.Count > 0)
                {

                    name = dt.Rows[0]["EMPNAME"].ToString();
                    des = dt.Rows[0]["EMPDESIGN"].ToString();
                    gen = dt.Rows[0]["EMPSEX"].ToString();

                }

                var result = new { name = name ,des = des ,gen = gen };
                
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public JsonResult GetODDUTYJSON()
        {
            ODLS model = new ODLS();
            return Json(BindStatus());

        }

        public IActionResult ListOnDuty()
        {
            return View();
        }

        public ActionResult MyListListOnDutygrid(string strStatus)
        {
            List<ListOnDuty> Reg = new List<ListOnDuty>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;

            dtUsers = onDuty.GetAllOnDuty(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=OnDuty?id=" + dtUsers.Rows[i]["ODBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["ODBASICID"].ToString() + "";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["ODBASICID"].ToString() + "";
                }

                Reg.Add(new ListOnDuty
                {
                    did = dtUsers.Rows[i]["DOCID"].ToString(),
                    eid = dtUsers.Rows[i]["EMPID"].ToString(),
                    ddat = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    ename = dtUsers.Rows[i]["EMPNAME"].ToString(),
                    edes = dtUsers.Rows[i]["EMPDESIGN"].ToString(),
                    egen = dtUsers.Rows[i]["EGENDER"].ToString(),
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

            string flag = onDuty.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListOnDuty");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListOnDuty");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = onDuty.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListOnDuty");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListOnDuty");
            }
        }
    }
}

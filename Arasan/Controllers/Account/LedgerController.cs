using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Oracle.ManagedDataAccess.Client;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

using Newtonsoft.Json.Linq;
using Arasan.Services.Master;

namespace Arasan.Controllers
{
    public class LedgerController : Controller
    {
        ILedger ledger;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public LedgerController(ILedger _ledger, IConfiguration _configuratio)
        {
            ledger = _ledger;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult Ledger(string id)
        {
            Ledger ca = new Ledger();
            ca.Typelst = BindAccType();
            ca.AccGrouplst = BindAccGroup("");
            //ca.Date = DateTime.Now.ToString("dd-MMM-yyyy");
            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();

                dt = ledger.GetLedger(id);
                if (dt.Rows.Count > 0)
                {
                    ca.AType = dt.Rows[0]["ACCOUNTTYPE"].ToString();
                    ca.AccGrouplst = BindAccGroup(dt.Rows[0]["ACCOUNTTYPEID"].ToString());
                    ca.AccGroup = dt.Rows[0]["ACCOUNTGROUP"].ToString();
                    ca.LedName = dt.Rows[0]["LEDNAME"].ToString();
                    ca.OpStock = dt.Rows[0]["OPSTOCK"].ToString();
                    ca.ClStock = dt.Rows[0]["CLSTOCK"].ToString();
                    ca.DisplayName = dt.Rows[0]["DISPLAY_NAME"].ToString();
                    ca.Groupcode = dt.Rows[0]["GROUPCODE"].ToString();
                    ca.Ledgercode = dt.Rows[0]["LEDGERCODE"].ToString();
                    ca.ID = id;

                }
            }
            return View(ca);
        }
        [HttpPost]
        public ActionResult Ledger(Ledger Cy, string id)
        {

            try
            {
                Cy.ID = id;
                int legcode = Convert.ToInt32(Cy.Ledgercode);
                string code = GetNumberwithPrefix(legcode, 6);
                Cy.LegCode = code;

                //int grocode = Convert.ToInt32(Cy.Groupcode);
                //string code1 = GetNumberwithPrefix1(grocode, 6);
                //Cy.Grocode = code1;

                string Strout = ledger.LedgerCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Ledger Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Ledger Updated Successfully...!";
                    }
                    return RedirectToAction("ListLedger");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Ledger";
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
        public static string GetNumberwithPrefix(int Ledgercode, int totalchar)
        {
            string tempnumber = Ledgercode.ToString();
            while (tempnumber.Length < 6)
            tempnumber = "0" + tempnumber;
            return tempnumber;
        }
        public ActionResult MyListLedgergrid(string strStatus)
        {
            List<LedgerItems> Reg = new List<LedgerItems>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)ledger.GetAllLedgers(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                EditRow = "<a href=Ledger?id=" + dtUsers.Rows[i]["LEDGERID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["LEDGERID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                Reg.Add(new LedgerItems
                {
                    id = dtUsers.Rows[i]["LEDGERID"].ToString(),
                    atype = dtUsers.Rows[i]["ACCOUNTTYPE"].ToString(),
                    accgroup = dtUsers.Rows[i]["ACCOUNTGROUP"].ToString(),
                    ledname = dtUsers.Rows[i]["LEDNAME"].ToString(),
                    displayname = dtUsers.Rows[i]["DISPLAY_NAME"].ToString(),
                    legcode = dtUsers.Rows[i]["LEDGERCODE"].ToString(),
                    editrow = EditRow,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }
        //public static string GetNumberwithPrefix1(int Groupcode, int totalchar)
        //{
        //    string tempnumber = Groupcode.ToString();
        //    while (tempnumber.Length < 6)
        //        tempnumber = "0" + tempnumber;
        //    return tempnumber;
        //}
        public IActionResult ListLedger()
        {
            //IEnumerable<Ledger> cmp = ledger.GetAllLedger();
            return View();
        }
        public JsonResult GetItemJSON(string itemid)
        {
            Ledger model = new Ledger();
            model.AccGrouplst = BindAccGroup(itemid);
            return Json(BindAccGroup(itemid));
        }
        public ActionResult DeleteItem(string tag, string id)
        {

            string flag = ledger.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListLedger");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListLedger");
            }
        }

        public List<SelectListItem> BindAccGroup(string value)
        {
            try
            {
                DataTable dtDesg = ledger.GetAccGroup(value);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ACCOUNTGROUP"].ToString(), Value = dtDesg.Rows[i]["ACCGROUPID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetGroupCodeDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
               
                string code = "";

                dt = ledger.GetGroupCodeDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    code = dt.Rows[0]["GROUPCODE"].ToString();
                   
                }

                var result = new { code = code };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindAccType()
        {
            try
            {
                DataTable dtDesg = ledger.GetAccType();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ACCOUNTTYPE"].ToString(), Value = dtDesg.Rows[i]["ACCOUNTTYPEID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult ListDayBook()
        {
            //IEnumerable<Ledger> cmp = ledger.GetAllLedger();
            return View();
        }
        public ActionResult MyListDayBookgrid(string strfrom, string strTo)
        {
            List<ListDayItems> Reg = new List<ListDayItems>();
            DataTable dtUsers = new DataTable();
            dtUsers = (DataTable)ledger.GetAllListDayBookItem(strfrom, strTo);
            DataTable dt = new DataTable();
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
               // dt = (DataTable)ledger.GetAllListDayBookItems(dtUsers.Rows[i]["MID"].ToString());

                Reg.Add(new ListDayItems
                {
                    id = dtUsers.Rows[i]["TRANS2ID"].ToString(),
                    vocherno = dtUsers.Rows[i]["T1VCHNO"].ToString(),
                    vocherdate = dtUsers.Rows[i]["T1VCHDT"].ToString(),
                    tratype = dtUsers.Rows[i]["T1TYPE"].ToString(),
                    vocmemo = dtUsers.Rows[i]["T1NARR"].ToString(),
                    vtype = dtUsers.Rows[i]["DBCR"].ToString(),
                    ledgercode = dtUsers.Rows[i]["MID"].ToString(),
                    debitamount = dtUsers.Rows[i]["DBAMOUNT"].ToString(),
                    creditamount = dtUsers.Rows[i]["CRAMOUNT"].ToString(),
                });
            }

            return Json(new
            {
                Reg
            });

        }
    }
}


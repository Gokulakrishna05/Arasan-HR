using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Controllers
{
    public class AccountGroupController : Controller
    {
        IAccountGroup accountGroup;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public AccountGroupController(IAccountGroup _accountGroup, IConfiguration _configuratio)
        {
            accountGroup = _accountGroup;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult AccountGroup(string id)
        {
            AccountGroup sq = new AccountGroup();
            sq.Brlst = BindBranch();
            sq.Typelst = BindAccType();
            //sq.Statuslst = BindStatus();
            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();

                dt = accountGroup.GetAccountGroup(id);
                if (dt.Rows.Count > 0)
                {
                    sq.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    sq.AccGroup = dt.Rows[0]["ACCOUNTGROUP"].ToString();
                    sq.AType = dt.Rows[0]["ACCOUNTTYPE"].ToString();
                    sq.GCode = dt.Rows[0]["GROUPCODE"].ToString();
                    sq.Display = dt.Rows[0]["DISPLAY_NAME"].ToString();
                    sq.ID = id;

                }
            }
            return View(sq);
        }
        [HttpPost]
        public ActionResult AccountGroup(AccountGroup Cy, string id)
        {

            try
            {
                Cy.ID = id;
                int grpcode = Convert.ToInt32(Cy.GCode);
                string code = GetNumberwithPrefix(grpcode, 3);
                Cy.GrpCode = code;
                string Strout = accountGroup.AccountGroupCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "AccountGroup Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "AccountGroup Updated Successfully...!";
                    }
                    return RedirectToAction("ListAccountGroup");
                }

                else
                {
                    ViewBag.PageTitle = "Edit AccountGroup";
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
        public static string GetNumberwithPrefix(int AccountCode, int totalchar)
        {
            string tempnumber = AccountCode.ToString();
            while (tempnumber.Length < 3)
                tempnumber = "0" + tempnumber;
            return tempnumber;
        }
        //public List<SelectListItem> BindStatus()
        //{
        //    try
        //    {
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        lstdesg.Add(new SelectListItem() { Text = "Active", Value = "Active" });
        //        lstdesg.Add(new SelectListItem() { Text = "Active", Value = "Active" });
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = accountGroup.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListAccountGroup");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListAccountGroup");
            }
        }
        public IActionResult ListAccountGroup()
        {
            IEnumerable<AccountGroup> cmp = accountGroup.GetAllAccountGroup();
            return View(cmp);
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
        public List<SelectListItem> BindAccType()
        {
            try
            {
                DataTable dtDesg = accountGroup.GetAccType();
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

        public ActionResult GetItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();

                string acccocde = "";
                string clid = "";
                string clacode = "";
                string acc = "";

                dt = accountGroup.Gettypecode(ItemId);

                if (dt.Rows.Count > 0)
                {

                    acccocde = dt.Rows[0]["ACCOUNTCODE"].ToString();
                    clid = dt.Rows[0]["ACCOUNTCLASS"].ToString();
                    dt2 = accountGroup.Getclasscode(clid);
                    if (dt2.Rows.Count > 0)
                    {
                        clacode = dt2.Rows[0]["ACCCLASS_CODE"].ToString();
                    }

                }

                acc = clacode + "" + acccocde;


                var result = new { acc = acc };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public ActionResult GetItemDetail(string ItemId)
        //{
        //    try
        //    {
        //        DataTable dt2 = new DataTable();
        //        DataTable dt = new DataTable();

        //        string code = "";
        //        string type = "";
        //        string grp = "";
        //        string acc = "";

        //        dt = accountGroup.Getaccgrpcode(ItemId);

        //        if (dt2.Rows.Count > 0)
        //        {
        //            grp = dt2.Rows[0]["ACCCLASS_CODE"].ToString();

        //            dt2 = accountGroup.Getgrpcode(grp);
        //            if (dt.Rows.Count > 0)
        //            {

        //                code = dt.Rows[0]["ACCOUNTCODE"].ToString();
        //                type = dt.Rows[0]["ACCOUNTCLASS"].ToString();
        //            }

        //        }

        //        acc = grp + "" + code;


        //        var result = new { acc = acc };
        //        return Json(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}

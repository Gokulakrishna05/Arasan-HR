using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class AccountTypeController : Controller
    {

        IAccountType AccountTypeService;

        IConfiguration? _configuration;
        private string? _connectionString;

        public AccountTypeController(IAccountType _AccountTypeService, IConfiguration _configuration)
        {
            AccountTypeService = _AccountTypeService;
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");

        }

        public IActionResult AccountType(string id)
        {
            AccountType AG = new AccountType();
            AG.CreatedBy = Request.Cookies["UserId"];
            AG.ATypelst = BindATypelst();

            //for edit & delete
            if (id != null)
            {
                DataTable dt = new DataTable();
                double total = 0;
                dt = AccountTypeService.GetAccountType(id);
                if (dt.Rows.Count > 0)
                {
                    AG.Accountclass = dt.Rows[0]["ACCOUNTCLASS"].ToString();
                    AG.AccountCode = dt.Rows[0]["ACCOUNTCODE"].ToString();
                    AG.Accounttype = dt.Rows[0]["ACCOUNTTYPE"].ToString();
                    //AG.Status = dt.Rows[0]["IS_ACTIVE"].ToString();
                    AG.ID = id;


                }
            }
            return View(AG);
        }


        [HttpPost]
        public IActionResult AccountType(AccountType AG, string id)
        {

            try
            {
                AG.ID = id;
                int acccode = Convert.ToInt32(AG.AccountCode);
                string code = GetNumberwithPrefix(acccode, 3);
                AG.AccCode = code;
                string Strout = AccountTypeService.AccountTypeCRUD(AG);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (AG.ID == null)
                    {
                        TempData["notice"] = "AccountType Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "AccountType Updated Successfully...!";
                    }
                    return RedirectToAction("ListAccountType");
                }

                else
                {
                    ViewBag.PageTitle = "Edit AccountType";
                    TempData["notice"] = Strout;
                    //return View();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(AG);
        }

        public IActionResult ListAccountType(string status)
        {
            IEnumerable<AccountType> cmp = AccountTypeService.GetAllAccountType(status);
            return View(cmp);
        }

        public static string GetNumberwithPrefix(int AccountCode, int totalchar)
        {
            string tempnumber = AccountCode.ToString();
            while (tempnumber.Length < 3)
                tempnumber = "0" + tempnumber;
            return tempnumber;
        }

        public List<SelectListItem> BindATypelst()
        {
            try
            {
                DataTable dtDesg = AccountTypeService.GetType();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ACCOUNT_CLASS"].ToString(), Value = dtDesg.Rows[i]["ACCCLASSID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = AccountTypeService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListAccountType");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListAccountType");
            }
        }
        public ActionResult Remove(string tag, int id)
        {

            string flag = AccountTypeService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListAccountType");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListAccountType");
            }
        }

    }
}

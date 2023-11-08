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


            //for edit & delete
            if (id != null)
            {
                DataTable dt = new DataTable();
                double total = 0;
                dt = AccountTypeService.GetAccountType(id);
                if (dt.Rows.Count > 0)
                {
                    AG.AccountCode = dt.Rows[0]["ACCOUNTCODE"].ToString();
                    AG.Accounttype = dt.Rows[0]["ACCOUNTTYPE"].ToString();
                    AG.Status = dt.Rows[0]["STATUS"].ToString();
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

﻿using System.Collections.Generic;
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
                    sq.AType = dt.Rows[0]["ACCTYPE"].ToString();
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
    }
}
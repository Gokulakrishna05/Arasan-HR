using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class AccClassController : Controller
    {
        IAccClass AccClassService;

        IConfiguration? _configuration;
        private string? _connectionString;

        public AccClassController(IAccClass _AccClassService, IConfiguration _configuration)
        {
            AccClassService = _AccClassService;
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");

        }

        public IActionResult AccClass(string id)
        {
            AccClass AC = new AccClass();
            AC.CreatedBy = Request.Cookies["UserId"];
            //AC.ATypelst = BindATypelst();

            //for edit & delete
            if (id != null)
            {
                DataTable dt = new DataTable();
                double total = 0;
                dt = AccClassService.GetAccClass(id);
                if (dt.Rows.Count > 0)
                {
                    AC.AccountCode = dt.Rows[0]["ACCCLASS_CODE"].ToString();
                    AC.Accounttype = dt.Rows[0]["ACCOUNT_CLASS"].ToString();
                    //AC.status = dt.Rows[0]["STATUS"].ToString();
                    AC.ID = id;


                }
            }
            return View(AC);
        }

        [HttpPost]
        public IActionResult AccClass(AccClass AC, string id)
        {

            try
            {
                AC.ID = id;
                string Strout = AccClassService.AccClassCRUD(AC);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (AC.ID == null)
                    {
                        TempData["notice"] = "AccClass Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "AccClass Updated Successfully...!";
                    }
                    return RedirectToAction("ListAccClass");
                }

                else
                {
                    ViewBag.PageTitle = "Edit AccClass";
                    TempData["notice"] = Strout;
                    //return View();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(AC);
        }
       
        public IActionResult ListAccClass(string active)
        {
            IEnumerable<AccClass> cmp = AccClassService.GetAllAccClass(active);
            return View(cmp);
        }

        //public List<SelectListItem> BindATypelst()
        //{
        //    try
        //    {
        //        DataTable dtDesg = AccClassService.GetType();
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ACCOUNTTYPE"].ToString(), Value = dtDesg.Rows[i]["ACCOUNTTYPE"].ToString() });
        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = AccClassService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListAccClass");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListAccClass");
            }
        }
        public ActionResult Remove(string tag, int id)
        {

            string flag = AccClassService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListAccClass");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListAccClass");
            }
        }


    }
}

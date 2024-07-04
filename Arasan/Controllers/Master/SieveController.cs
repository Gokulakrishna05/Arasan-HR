using System.Collections.Generic;
using System.Data;
using System.Runtime.Intrinsics.Arm;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class SieveController : Controller
    {
        ISieve SieveService;
        public SieveController(ISieve _SieveService)
        {
            SieveService = _SieveService;
        }
        public IActionResult Sieve(string id)
        {
            Sieve Dp = new Sieve();
            Dp.createby = Request.Cookies["UserId"];


            if (id == null)
            {


            }
            else
            {
                DataTable dt = new DataTable();
                double total = 0;
                dt = SieveService.GetSieve(id);
                if (dt.Rows.Count > 0)
                {
                    Dp.Svalue = dt.Rows[0]["STARTVALUE"].ToString();
                    Dp.Evalue = dt.Rows[0]["ENDVALUE"].ToString();
                    Dp.SID = dt.Rows[0]["SIEVE"].ToString();

                }

            }

            return View(Dp);
        }

        [HttpPost]
        public IActionResult Sieve(Sieve Dp, string id)
        {
            try
            {
                Dp.ID = id;
                string Strout = SieveService.SieveCRUD(Dp);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Dp.ID == null)
                    {
                        TempData["notice"] = "Sieve Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Sieve Updated Successfully...!";
                    }
                    return RedirectToAction("ListSieve");

                }

                else
                {
                    ViewBag.PageTitle = "Edit Stop";
                    TempData["notice"] = Strout;
                    //return View();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }



            return View(Dp);
        }

        public IActionResult ListSieve()
        {

            return View();
        }


        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = SieveService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListSieve");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListSieve");
            }
        }
        public ActionResult Remove(string tag, int id)
        {

            string flag = SieveService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListSieve");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListSieve");
            }
        }


        public IActionResult ViewSieve(string id)
        {
            Sieve Dp = new Sieve();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            dt = SieveService.GetviewSieve(id);
            if (dt.Rows.Count > 0)
            {
                Dp.Svalue = dt.Rows[0]["STARTVALUE"].ToString();
                Dp.Evalue = dt.Rows[0]["ENDVALUE"].ToString();
                Dp.SID = dt.Rows[0]["SIEVE"].ToString();

                Dp.ID = id;
            }

            // ca.QuoLst = Data;
            return View(Dp);
        }


        public ActionResult MyListItemgrid(string strStatus)
        {
            List<Sievegrid> Reg = new List<Sievegrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = SieveService.GetAllSieve(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string ViewRow = string.Empty;
                string EditRow = string.Empty;
                string DeleteRow = string.Empty;


                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                   ViewRow = "<a href=ViewSieve?id=" + dtUsers.Rows[i]["SIEVEMASTID"].ToString() + "><img src='../Images/view_icon.png' alt='View Details' /></a>";
                    EditRow = "<a href=Sieve?id=" + dtUsers.Rows[i]["SIEVEMASTID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["SIEVEMASTID"].ToString() + "";
                }
                else
                {

                    ViewRow = "";
                    EditRow = "";
                    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["SIEVEMASTID"].ToString() + "";
                }

                Reg.Add(new Sievegrid
                {
                    id = dtUsers.Rows[i]["SIEVEMASTID"].ToString(),
                    svalue = dtUsers.Rows[i]["STARTVALUE"].ToString(),
                    evalue = dtUsers.Rows[i]["ENDVALUE"].ToString(),
                    sid = dtUsers.Rows[i]["SIEVE"].ToString(),

                   viewrow = ViewRow,
                    editrow = EditRow,
                    delrow = DeleteRow,


                });
            }

            return Json(new
            {
                Reg
            });

            }

        }
}


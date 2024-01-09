using System.Collections.Generic;
using System.Data;
using System.Runtime.Intrinsics.Arm;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class StopController : Controller
    {
        IStop StopService;
        public StopController(IStop _StopService)
        {
            StopService = _StopService;
        }
        public IActionResult Stop(string id)
        {
            Stop Dp = new Stop();

          

            if (id == null)
            {


            }
            else
            {
                DataTable dt = new DataTable();
                double total = 0;
                dt = StopService.GetStop(id);
                if (dt.Rows.Count > 0)
                {
                    Dp.SID = dt.Rows[0]["STOPID"].ToString();
                    Dp.SDESC = dt.Rows[0]["STOPDESC"].ToString();

                }

            }

            return View(Dp); 
        }

        [HttpPost]
        public IActionResult Stop(Stop Dp, string id)
        {
            try
            {
                Dp.ID = id;
                string Strout = StopService.StopCRUD(Dp);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Dp.ID == null)
                    {
                        TempData["notice"] = "Stop Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Stop Updated Successfully...!";
                    }
                    return RedirectToAction("ListStop");
                
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

        public IActionResult ListStop()
        {

            return View();
        }


        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = StopService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListStop");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListStop");
            }
        }
        public ActionResult Remove(string tag, int id)
        {

            string flag = StopService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListStop");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListStop");
            }
        }


        public IActionResult ViewStop(string id)
        {
            Stop ca = new Stop();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            dt = StopService.GetviewStop(id);
            if (dt.Rows.Count > 0)
            {
                ca.SID = dt.Rows[0]["STOPID"].ToString();
                ca.SDESC = dt.Rows[0]["STOPDESC"].ToString();
               
                ca.ID = id;


            }
           
           // ca.QuoLst = Data;
            return View(ca);
        }
        public ActionResult MyListItemgrid(string strStatus)
        {
            List<Stopgrid> Reg = new List<Stopgrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = StopService.GetAllStop(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string ViewRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    ViewRow = "<a href=viewStop?id=" + dtUsers.Rows[i]["STOPMASTID"].ToString() + "><img src='../Images/view_icon.png' alt='View Details' /></a>";
                    EditRow = "<a href=Stop?id=" + dtUsers.Rows[i]["STOPMASTID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["STOPMASTID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                }
                else
                {

                    ViewRow = "";
                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["STOPMASTID"].ToString() + "><img src='../Images/close_icon.png' alt='Deactivate' /></a>";

                }

                Reg.Add(new Stopgrid
                {
                    id = dtUsers.Rows[i]["STOPMASTID"].ToString(),
                    sid = dtUsers.Rows[i]["STOPID"].ToString(),
                    sdesc = dtUsers.Rows[i]["STOPDESC"].ToString(),
                    //description = dtUsers.Rows[i]["DESCRIPTION"].ToString(),
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


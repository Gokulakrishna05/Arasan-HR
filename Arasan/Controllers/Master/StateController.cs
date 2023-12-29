using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services.Master;
//using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Master
{
    public class StateController : Controller
    {
        IStateService StateService;
        public StateController(IStateService _StateService)
        {
            StateService = _StateService;
        }
        public IActionResult State(string id)
        {
            State st = new State();
            st.createby = Request.Cookies["UserId"];
            st.cuntylst = BindCountry();
            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();

                dt = StateService.GetEditState(id);
                if (dt.Rows.Count > 0)
                {
                    st.StateName = dt.Rows[0]["STATE"].ToString();
                    st.StateCode = dt.Rows[0]["STCODE"].ToString();
                    st.countryid = dt.Rows[0]["COUNTRYMASTID"].ToString();
                    st.ID = id;


                }

            }
            return View(st);
        }
        [HttpPost]
        public ActionResult State(State ss, string id)
        {

            try
            {
                ss.ID = id;
                string Strout = StateService.StateCRUD(ss);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (ss.ID == null)
                    {
                        TempData["notice"] = " State Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " State Updated Successfully...!";
                    }
                    return RedirectToAction("ListState");
                }

                else
                {
                    ViewBag.PageTitle = "Edit State";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(ss);
        }
        public IActionResult ListState()
        {
            return View();
        }

        public List<SelectListItem> BindCountry()
        {
            try
            {
                DataTable dtDesg = StateService.Getcountry();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["COUNTRY"].ToString(), Value = dtDesg.Rows[i]["COUNTRYMASTID"].ToString() });
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

            string flag = StateService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListState");
            }
            else
            {
               TempData["notice"] = flag;
               return RedirectToAction("ListState");
            }
        }
        public ActionResult Remove(string tag, int id)
        {

            string flag = StateService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListState");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListState");
            }
        }

        public ActionResult MyListItemgrid(string strStatus)
        {
            List<StateGrid> Reg = new List<StateGrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = StateService.GetAllState(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;


                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    EditRow = "<a href=State?id=" + dtUsers.Rows[i]["STATEMASTID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["STATEMASTID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                }
                else
                {

                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["STATEMASTID"].ToString() + "><img src='../Images/close_icon.png' alt='Deactivate' /></a>";

                }

              
                Reg.Add(new StateGrid
                {
                    id = dtUsers.Rows[i]["STATEMASTID"].ToString(),
                    statename = dtUsers.Rows[i]["STATE"].ToString(),
                    //statecode = dtUsers.Rows[i]["STCODE"].ToString(),
                    countryid = dtUsers.Rows[i]["COUNTRY"].ToString(),
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

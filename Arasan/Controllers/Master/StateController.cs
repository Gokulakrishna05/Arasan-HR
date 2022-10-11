using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services.Master;
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
            st.cuntylst = BindCountry();
            if (id == null)
            {

            }
            else
            {
                st = StateService.GetStateById(id);

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
            IEnumerable<State> sta = StateService.GetAllState();
            return View(sta);
        }

        public List<SelectListItem> BindCountry()
        {
            try
            {
                DataTable dtDesg = StateService.Getcountry();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["COUNTRYNAME"].ToString(), Value = dtDesg.Rows[i]["COUNTRYMASTID"].ToString() });
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

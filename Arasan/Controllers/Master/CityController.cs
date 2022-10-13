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
    public class CityController : Controller
    {
        ICityService CityService;
        public CityController(ICityService _CityService)
        {
            CityService = _CityService;
        }
        public IActionResult City(string id)
        {
            City st = new City();
            st.sta = BindState();
            st.cuntylst = BindCountry();

            if (id == null)
            {

            }
            else
            {
                st = CityService.GetCityById(id);

            }
            return View(st);
        }
        [HttpPost]
        public ActionResult City(City ss, string id)
        {

            try
            {
                ss.ID = id;
                string Strout = CityService.CityCRUD(ss);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (ss.ID == null)
                    {
                        TempData["notice"] = " City Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " City Updated Successfully...!";
                    }
                    return RedirectToAction("ListCity");
                }

                else
                {
                    ViewBag.PageTitle = "Edit City";
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
        public IActionResult ListCity()
        {
            IEnumerable<City> sta = CityService.GetAllCity();
            return View(sta);
        }
        public List<SelectListItem> BindState()
        {
            try
            {
                DataTable dtDesg = CityService.GetState();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["STATE"].ToString(), Value = dtDesg.Rows[i]["STATEMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindCountry()
        {
            try
            {
                DataTable dtDesg = CityService.Getcountry();
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

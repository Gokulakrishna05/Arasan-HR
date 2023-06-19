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
        ICityService city;
        public CityController(ICityService _city)
        {
            city = _city;
        }
        public IActionResult City(string id)
        {
            City st = new City();
          
            st.cuntylst = BindCountry();
            st.sta = BindState("");

            if (id == null)
            {

            }
            else
            {

                DataTable dt = new DataTable();

                dt = city.GetCity(id);
                if (dt.Rows.Count > 0)
                {
                    st.Cit = dt.Rows[0]["CITYNAME"].ToString();
                    st.countryid = dt.Rows[0]["COUNTRYID"].ToString();
                    st.sta = BindState(st.countryid);
                    st.State = dt.Rows[0]["STATEID"].ToString();
                    
                    st.ID = id;

                }

            }
            return View(st);
        }
        [HttpPost]
        public ActionResult City(City ss, string id)
        {

            try
            {
                ss.ID = id;
                string Strout = city.CityCRUD(ss);
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
        public IActionResult ListCity(string status)
        {
            IEnumerable<City> sta = city.GetAllCity(status);
            return View(sta);
        }
       
        public List<SelectListItem> BindCountry()
        {
            try
            {
                DataTable dtDesg = city.Getcountry();
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
        public JsonResult GetStateJSON(string supid)
        {
            City model = new City();
            model.sta = BindState(supid);
            return Json(BindState(supid));

        }
        public List<SelectListItem> BindState(string id)
        {
            try
            {
                DataTable dtDesg = city.GetState(id);
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

        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = city.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListCity");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListCity");
            }
        }
    }
}

using System.Collections.Generic;
using System.Data;
using Arasan.Interface;

using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class LocationController : Controller

    {
        ILocationService LocationService;
        public LocationController(ILocationService _locationService)
        {
            LocationService = _locationService;
        }
        public IActionResult Location(string id)
        {
            Location ca = new Location();
            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            if (id == null)
            {

            }
            else
            {
                //ca = LocationService.GetLocationsById(id);
                DataTable dt = new DataTable();

                dt = LocationService.GetEditLocation(id);
                if (dt.Rows.Count > 0)
                {
                    ca.LocationId = dt.Rows[0]["LOCID"].ToString();
                    ca.LocType = dt.Rows[0]["LOCATIONTYPE"].ToString();
                    ca.ContactPer = dt.Rows[0]["CPNAME"].ToString();
                    ca.PhoneNo = dt.Rows[0]["PHNO"].ToString();
                    ca.EmailId = dt.Rows[0]["EMAIL"].ToString();
                    ca.Address = dt.Rows[0]["ADD1"].ToString();
                    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    ca.ID = id;

                }

            }
            return View(ca);
        }
        [HttpPost]
        public ActionResult Location(Location Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = LocationService.LocationsCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Location Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Location Updated Successfully...!";
                    }
                    return RedirectToAction("ListLocation");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Location";
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
        public IActionResult ListLocation()
        {
            return View();
        }
        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = LocationService.GetBranch();
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

        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = LocationService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListLocation");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListLocation");
            }
        } 
        public ActionResult Remove(string tag, int id)
        {

            string flag = LocationService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListLocation");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListLocation");
            }
        }
        public ActionResult MyListItemgrid(string strStatus)
        {
            List<Locationgrid> Reg = new List<Locationgrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = LocationService.GetAllLocation(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                EditRow = "<a href=city?id=" + dtUsers.Rows[i]["LOCDETAILSID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["LOCDETAILSID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                Reg.Add(new Locationgrid
                {
                    id = dtUsers.Rows[i]["LOCDETAILSID"].ToString(),
                    locationid = dtUsers.Rows[i]["LOCID"].ToString(),
                    loctype = dtUsers.Rows[i]["LOCATIONTYPE"].ToString(),
                    contactper = dtUsers.Rows[i]["CPNAME"].ToString(),
                    phoneno = dtUsers.Rows[i]["PHNO"].ToString(),
                    emailid = dtUsers.Rows[i]["EMAIL"].ToString(),
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




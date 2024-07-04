using System.Collections.Generic;
using System.Data;
using Arasan.Interface;

using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Master;
using Arasan.Services.Store_Management;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class LocationController : Controller

    {
        ILocationService LocationService;
        private string? _connectionString;
        IConfiguration? _configuratio;
        DataTransactions datatrans;
        public LocationController(ILocationService _locationService, IConfiguration _configuratio)
        {
            LocationService = _locationService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult Location(string id)
        {
            Location ca = new Location();
            ca.Brlst = BindBranch();
            ca.Suplst = BindSupplier();
            ca.Statelst = BindState();
            ca.Citylst = BindCity();
            ca.Branch = Request.Cookies["BranchId"];
            //ca.Loclst = GetLoc();
            ca.createby = Request.Cookies["UserId"];

            ca.Trader = "No";
            ca.Requried = "NO";
            List<LocationItem> TData = new List<LocationItem>();
            LocationItem tda = new LocationItem();
            List<LocItem> TData1 = new List<LocItem>();
            LocItem tda1 = new LocItem();

            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new LocationItem();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda1 = new LocItem();
                    tda1.Isslst = BindIssuseType();
                    tda1.Loclst = GetLoc();
                    tda1.Isvalid1 = "Y";
                    TData1.Add(tda1);
                }
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
                    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    ca.Trader = dt.Rows[0]["TRADEYN"].ToString();
                    ca.Suplst = BindSupplier();
                    ca.Party = dt.Rows[0]["PARTYID"].ToString();
                    ca.Requried = dt.Rows[0]["BINYN"].ToString();
                    ca.ContactPer = dt.Rows[0]["CPNAME"].ToString();
                    ca.PhoneNo = dt.Rows[0]["PHNO"].ToString();
                    ca.Address = dt.Rows[0]["ADD1"].ToString();
                    ca.Fax = dt.Rows[0]["FAXNO"].ToString();
                    ca.Add2 = dt.Rows[0]["ADD2"].ToString();
                    ca.Mail = dt.Rows[0]["EMAIL"].ToString();
                    ca.Add3 = dt.Rows[0]["ADD3"].ToString();
                    ca.FlowOrd = dt.Rows[0]["FLWORD"].ToString();
                    ca.City = dt.Rows[0]["CITY"].ToString();
                    ca.State = dt.Rows[0]["STATE"].ToString();
                    ca.PinCode = dt.Rows[0]["PINCODE"].ToString();
                    ca.ID = id;

                }
                DataTable dt2 = new DataTable();
                dt2 = LocationService.GetEditBinDeatils(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new LocationItem();

                        tda.BinId = dt2.Rows[i]["BINID"].ToString();
                        tda.BinDesc = dt2.Rows[i]["BINDESC"].ToString();
                        tda.Capacity = dt2.Rows[i]["CAPACITY"].ToString();
                        TData.Add(tda);
                    }
                }
                DataTable dt3 = new DataTable();
                dt3 = LocationService.GetEditLocDeatils(id);
                if (dt3.Rows.Count > 0)
                {
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        tda1 = new LocItem();
                        tda1.Isslst = BindIssuseType();
                        tda1.Issuse = dt3.Rows[i]["ISSUETYPE"].ToString();
                        tda1.Loclst = GetLoc();
                        tda1.Location = dt3.Rows[i]["TOLOCID"].ToString();
                        TData1.Add(tda1);
                    }
                }

            }
            ca.Locationlst = TData;
            ca.Loclst = TData1;
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
        public IActionResult ViewLocation(string id)
        {
            Location ca = new Location();
            DataTable dt = new DataTable();

            dt = LocationService.GetViewLocationDeatils(id);
            if (dt.Rows.Count > 0)
            {
                ca.LocationId = dt.Rows[0]["LOCID"].ToString();
                ca.LocType = dt.Rows[0]["LOCATIONTYPE"].ToString();
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.Trader = dt.Rows[0]["TRADEYN"].ToString();
                ca.Suplst = BindSupplier();
                ca.Party = dt.Rows[0]["PARTYNAME"].ToString();
                ca.Requried = dt.Rows[0]["BINYN"].ToString();
                ca.ContactPer = dt.Rows[0]["CPNAME"].ToString();
                ca.PhoneNo = dt.Rows[0]["PHNO"].ToString();
                ca.Address = dt.Rows[0]["ADD1"].ToString();
                ca.Fax = dt.Rows[0]["FAXNO"].ToString();
                ca.Add2 = dt.Rows[0]["ADD2"].ToString();
                ca.Mail = dt.Rows[0]["EMAIL"].ToString();
                ca.Add3 = dt.Rows[0]["ADD3"].ToString();
                ca.FlowOrd = dt.Rows[0]["FLWORD"].ToString();
                ca.City = dt.Rows[0]["CITY"].ToString();
                ca.State = dt.Rows[0]["STATE"].ToString();
                ca.PinCode = dt.Rows[0]["PINCODE"].ToString();
                ca.ID = id;

            }

            DataTable dt2 = new DataTable();
            List<LocationItem> TData = new List<LocationItem>();
            LocationItem tda = new LocationItem();

            dt2 = LocationService.GetEditBinDeatils(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new LocationItem();

                    tda.BinId = dt2.Rows[i]["BINID"].ToString();
                    tda.BinDesc = dt2.Rows[i]["BINDESC"].ToString();
                    tda.Capacity = dt2.Rows[i]["CAPACITY"].ToString();
                    TData.Add(tda);
                }
            }

            DataTable dt3 = new DataTable();
            List<LocItem> TData1 = new List<LocItem>();
            LocItem tda1 = new LocItem();

            dt3 = LocationService.GetViewEditLocDeatils(id);
            if (dt3.Rows.Count > 0)
            {
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    tda1 = new LocItem();
                    tda1.Isslst = BindIssuseType();
                    tda1.Issuse = dt3.Rows[i]["ISSUETYPE"].ToString();
                    tda1.Loclst = GetLoc();
                    tda1.Location = dt3.Rows[i]["LOCID"].ToString();
                    TData1.Add(tda1);
                }
            }
            ca.Locationlst = TData;
            ca.Loclst = TData1;
            return View(ca);
        }
        public ActionResult GetPartyDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string add1 = "";
                string add2 = "";
                string add3 = "";
                string city = "";
                string state = "";
                string pincode = "";
                string email = "";
                string mobile = "";
                string fax = "";
                string phone = "";

                dt = LocationService.GetPartyDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    add1 = dt.Rows[0]["ADD1"].ToString();
                    add2 = dt.Rows[0]["ADD2"].ToString();
                    add3 = dt.Rows[0]["ADD3"].ToString();
                    city = dt.Rows[0]["CITY"].ToString();
                    state = dt.Rows[0]["STATE"].ToString();
                    pincode = dt.Rows[0]["PINCODE"].ToString();
                    email = dt.Rows[0]["EMAIL"].ToString();
                    mobile = dt.Rows[0]["MOBILE"].ToString();
                    fax = dt.Rows[0]["FAX"].ToString();
                    phone = dt.Rows[0]["PHONENO"].ToString();
                }

                var result = new { add1 = add1, add2 = add2, add3 = add3, city = city, state = state, pincode = pincode, email = email, mobile = mobile, fax = fax, phone = phone };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindState()
        {
            try
            {
                DataTable dtDesg = LocationService.GetState();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["STATE"].ToString(), Value = dtDesg.Rows[i]["STATE"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindCity()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("SELECT COMMON_VALUE FROM COMMONMASTER WHERE COMMON_TEXT='CITY' ");
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["COMMON_VALUE"].ToString(), Value = dtDesg.Rows[i]["COMMON_VALUE"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindSupplier()
        {
            try
            {
                DataTable dtDesg = LocationService.GetSupplier();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTYNAME"].ToString(), Value = dtDesg.Rows[i]["PARTYMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindIssuseType()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "DRUM ISSUE", Value = "DRUM ISSUE" });
                lstdesg.Add(new SelectListItem() { Text = "PACK ISSUE", Value = "PACK ISSUE" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> GetLoc()
        {
            try
            {
                DataTable dtDesg = datatrans.GetLocation();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCID"].ToString(), Value = dtDesg.Rows[i]["LOCDETAILSID"].ToString() });
                }
                return lstdesg;

            }
            catch (Exception ex)
            {
                throw ex;
            }
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
        public JsonResult GetItemJSON()
        {
            Location model = new Location();
            //model.Itemlst = BindItemlst(itemid);
            return Json(model);

        }
        public JsonResult GetIssuseJSON()
        {
            LocItem model = new LocItem();
            model.Isslst = BindIssuseType();
            return Json(BindIssuseType());

        }
        public JsonResult GetLocationJSON()
        {
            LocItem model = new LocItem();
            model.Loclst = GetLoc();
            return Json(GetLoc());

        }
        public ActionResult DeleteMR(string tag, string id)
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

        public ActionResult Remove(string tag, string id)
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
                string View = string.Empty;
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    View = "<a href=ViewLocation?id=" + dtUsers.Rows[i]["LOCDETAILSID"].ToString() + "><img src='../Images/view_icon.png' alt='Edit' /></a>";

                    EditRow = "<a href=Location?id=" + dtUsers.Rows[i]["LOCDETAILSID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["LOCDETAILSID"].ToString() + "";
                }
                else
                {

                    EditRow = "";
                    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["LOCDETAILSID"].ToString() + "";

                }


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
                    view = View,

                });
            }

            return Json(new
            {
                Reg
            });

        }
        public IActionResult LocationDetail(string id)
        {
            Location ca = new Location();
            ca.Brlst = BindBranch();
            ca.emplst = BindEmp();
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
        public ActionResult LocationDetail(Location Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = LocationService.LocCRUD(Cy);
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
        public List<SelectListItem> BindEmp()
        {
            try
            {
                DataTable dtDesg = datatrans.GetEmp();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPNAME"].ToString(), Value = dtDesg.Rows[i]["EMPNAME"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult AddCity(string id)
        {
            Location ca = new Location();
            // ca.Brlst = BindBranch();

            return View(ca);
        }
        public JsonResult SaveCity(string category)
        {
            string Strout = LocationService.CityCRUD(category);
            var result = new { msg = Strout };
            return Json(result);
        }
        public JsonResult GetCityJSON()
        {
            return Json(BindCity());
        }
    }
}




using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Interface.Stores_Management;
using Arasan.Models;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Stores_Management
{
    public class StoresReturnController : Controller
    {
        IStoresReturnService StoresReturnService;
        public StoresReturnController(IStoresReturnService _StoresReturnService)
        {
            StoresReturnService = _StoresReturnService;
        }
        public IActionResult StoresReturn(string id)
        {
            StoresReturn ca = new StoresReturn();
            ca.Brlst = BindBranch();
            ca.Loc = BindLocation();
            if (id == null)
            {

            }
            else
            {
                //ca = LocationService.GetLocationsById(id);

            }
            return View(ca);
        }
        [HttpPost]
        public ActionResult StoresReturn(StoresReturn Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = StoresReturnService.StoresReturnCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "StoresReturn Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "StoresReturn Updated Successfully...!";
                    }
                    return RedirectToAction("ListStoresReturn");
                }

                else
                {
                    ViewBag.PageTitle = "Edit StoresReturn";
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
        public IActionResult ListStoresReturn()
        {
            IEnumerable<StoresReturn> cmp = StoresReturnService.GetAllStoresReturn();
            return View();
        }
        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = StoresReturnService.GetBranch();
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
        public List<SelectListItem> BindLocation()
        {
            try
            {
                DataTable dtDesg = StoresReturnService.GetLocation();
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


    }
}



//public IActionResult Returnable_NonReturnable_Dc()
//{
//    return View();
//}
//public IActionResult Stores_Acceptance()
//{
//    return View();
//}
//public IActionResult Receipt_Against_Returnable_DC()
//{
//    return View();
//}
//public IActionResult Stores_Issuse_Consumbables()
//{
//    return View();
//}
////public IActionResult Purchase_Indent()
////{
////    return View();
////}
////public IActionResult List_Purchase_Indent()
////{
////    return View();
////}
//public IActionResult Stores_Issuse_Production()
//{
//    return View();
//}
//public IActionResult Material_Requisition_Short_Close()
//{
//    return View();
//}


//public IActionResult Receipt_for_SubContract()
//{
//    return View();
//}
////public IActionResult Direct_Deducation()
////{
////    return View();
////}
////public IActionResult Direct_Addition()
////{
////    return View();
////}
//public IActionResult Sub_Contracting_DC()
//{
//    return View();
//}
//public IActionResult Item_Transfer()
//{
//    return View();
//}
//public IActionResult Sub_Contracting_Material_Receipt()
//{
//    return View();
//}



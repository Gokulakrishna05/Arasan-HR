using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Controllers.Store_Management
{
    public class StoreAccController : Controller
    {
        IStoreAccService StoreAccService;
        public StoreAccController(IStoreAccService _StoreAccService)
        {
            StoreAccService = _StoreAccService;
        }
        public IActionResult StoreAcc(string id)
        {
            StoreAcc st = new StoreAcc();
            st.Loc = BindLocation();
            st.Brlst = BindBranch();
            if (id == null)
            {

            }
            else
            {
                st = StoreAccService.GetStoreAccById(id);

            }
            return View(st);
        }
        [HttpPost]
        public ActionResult StoreAcc(StoreAcc ss, string id)
        {

            try
            {
                ss.ID = id;
                string Strout = StoreAccService.StoreAccCRUD(ss);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (ss.ID == null)
                    {
                        TempData["notice"] = " StoreAccepatence Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " StoreAccepatence Updated Successfully...!";
                    }
                    return RedirectToAction("List_StoreAcc");
                }

                else
                {
                    ViewBag.PageTitle = "Edit HSNcode";
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
        public IActionResult List_StoreAcc()
        {
            IEnumerable<StoreAcc> sta = StoreAccService.GetAllStoreAcc();
            return View(sta);
        }
        public List<SelectListItem> BindLocation()
        {
            try
            {
                DataTable dtDesg = StoreAccService.GetLocation();
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
                DataTable dtDesg = StoreAccService.GetBranch();
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
    }
}

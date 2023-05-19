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
    public class HSNcodeController : Controller
    {
        IHSNcodeService HSNcodeService;
        public HSNcodeController(IHSNcodeService _HSNcodeService)
        {
            HSNcodeService = _HSNcodeService;
        }
        public IActionResult HSNcode(string id)
        {
            HSNcode st = new HSNcode();
            st.GSTlst = BindGST();

            if (id == null)
            {
               
            }
            else
            {
                DataTable dt = new DataTable();
                double total = 0;
                dt = HSNcodeService.GetHSNcode(id);
                if (dt.Rows.Count > 0)
                {
                    st.HCode = dt.Rows[0]["HSNCODE"].ToString();
                    st.Dec = dt.Rows[0]["DESCRIPTION"].ToString();
                    st.Gt = dt.Rows[0]["GST"].ToString();

                }

            }
            return View(st);
        }

        public List<SelectListItem> BindGST()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "CGST", Value = "CGST" });
                lstdesg.Add(new SelectListItem() { Text = "SGST", Value = "SGST" });
                lstdesg.Add(new SelectListItem() { Text = "IGST", Value = "IGST" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public ActionResult HSNcode(HSNcode ss, string id)
        {

            try
            {
                ss.ID = id;
                string Strout = HSNcodeService.HSNcodeCRUD(ss);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (ss.ID == null)
                    {
                        TempData["notice"] = " HSNcode Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " HSNcode Updated Successfully...!";
                    }
                    return RedirectToAction("ListHSNcode");
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
        public IActionResult ListHSNcode()
        {
            IEnumerable<HSNcode> sta = HSNcodeService.GetAllHSNcode();
            return View(sta);
        }

       

    }
}

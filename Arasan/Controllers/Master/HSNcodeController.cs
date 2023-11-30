using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services;
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
            st.createby = Request.Cookies["UserId"];
            //st.CGstlst = BindCGst();
            //st.SGstlst = BindSGst();
            //st.IGstlst = BindIGst();


            List<HSNItem> TData = new List<HSNItem>();
            HSNItem tda = new HSNItem();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new HSNItem();

                    
                    tda.tarifflst = Bindtarifflst();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }

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

                }
               
                DataTable dt2 = new DataTable();

                dt2 = HSNcodeService.GettariffItem(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new HSNItem();
                        //double toaamt = 0;
                        tda.tarifflst = Bindtarifflst();
                        tda.tariff = dt2.Rows[i]["TARIFFID"].ToString();
                        
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }
            }
            st.hsnlst = TData;
            return View(st);
        }

       
        public JsonResult GettariffJSON()
        {
            //DeductionItem model = new DeductionItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(Bindtarifflst());
        }

        public List<SelectListItem> Bindtarifflst()
        {
            try
            {
                DataTable dtDesg = HSNcodeService.Gettariff();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["TARIFFID"].ToString(), Value = dtDesg.Rows[i]["TARIFFMASTERID"].ToString() });
                }
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

        //public List<SelectListItem> BindCGst()
        //{
        //    try
        //    {
        //        DataTable dtDesg = HSNcodeService.GetCGst();
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PERCENTAGE"].ToString(), Value = dtDesg.Rows[i]["PERCENTAGE"].ToString() });
        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public List<SelectListItem> BindSGst()
        //{
        //    try
        //    {
        //        DataTable dtDesg = HSNcodeService.GetSGst();
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PERCENTAGE"].ToString(), Value = dtDesg.Rows[i]["PERCENTAGE"].ToString() });
        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public List<SelectListItem> BindIGst()
        //{
        //    try
        //    {
        //        DataTable dtDesg = HSNcodeService.GetIGst();
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PERCENTAGE"].ToString(), Value = dtDesg.Rows[i]["PERCENTAGE"].ToString() });
        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public IActionResult ListHSNcode(/*string status*/)
        {
            return View();
        }

        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = HSNcodeService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListHSNcode");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListHSNcode");
            }
        }public ActionResult Remove(string tag, int id)
        {

            string flag = HSNcodeService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListHSNcode");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListHSNcode");
            }
        }

        public ActionResult Myhsncodegrid(string strStatus)
        {
            List<HsnList> Reg = new List<HsnList>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;

            dtUsers = HSNcodeService.GetAllhsncode(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                EditRow = "<a href=HSNcode?id=" + dtUsers.Rows[i]["HSNCODEID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["HSNCODEID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";


                Reg.Add(new HsnList
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["HSNCODEID"].ToString()),
                    hcode = dtUsers.Rows[i]["HSNCODE"].ToString(),
                    dec = dtUsers.Rows[i]["DESCRIPTION"].ToString(),
                    editrow = EditRow,
                    delrow = DeleteRow,


                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult ListMyhsncodegrid(string PRID, string strStatus)
        {
            List<HsnRowList> EnqChkItem = new List<HsnRowList>();
            DataTable dtEnq = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;

            dtEnq = HSNcodeService.Gethsnitem(PRID, strStatus);
            for (int i = 0; i < dtEnq.Rows.Count; i++)
            {
                EnqChkItem.Add(new HsnRowList
                {
                    id = Convert.ToInt64(dtEnq.Rows[i]["HSNCODEID"].ToString()),
                    tariff = dtEnq.Rows[i]["TARIFFID"].ToString(),

                });
            }

            return Json(new
            {
                EnqChkItem
            });
        }

        //public ActionResult Myhsncodegrid()
        //{
        //    List<HsnList> Reg = new List<HsnList>();
        //    DataTable dtUsers = new DataTable();

        //    dtUsers = HSNcodeService.GetAllhsncode();
        //    for (int i = 0; i < dtUsers.Rows.Count; i++)
        //    {

        //        string DeleteRow = string.Empty;
        //        string EditRow = string.Empty;

        //        EditRow = "<a href=HSNcode?id=" + dtUsers.Rows[i]["HSNCODEID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
        //        DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["HSNCODEID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";


        //        Reg.Add(new HsnList
        //        {
        //            id = Convert.ToInt64(dtUsers.Rows[i]["HSNCODEID"].ToString()),
        //            hcode = dtUsers.Rows[i]["HSNCODE"].ToString(),
        //            dec = dtUsers.Rows[i]["DESCRIPTION"].ToString(),
        //            editrow = EditRow,
        //            delrow = DeleteRow,


        //        });
        //    }

        //    return Json(new
        //    {
        //        Reg
        //    });

        //}
        //public ActionResult ListMyhsncodegrid(string PRID)
        //{
        //    List<HsnRowList> EnqChkItem = new List<HsnRowList>();
        //    DataTable dtEnq = new DataTable();
        //    dtEnq = HSNcodeService.Gethsnitem(PRID);
        //    for (int i = 0; i < dtEnq.Rows.Count; i++)
        //    {
        //        EnqChkItem.Add(new HsnRowList
        //        {
        //            id = Convert.ToInt64(dtEnq.Rows[i]["HSNCODEID"].ToString()),
        //            tariff = dtEnq.Rows[i]["TARIFFID"].ToString(),

        //        });
        //    }

        //    return Json(new
        //    {
        //        EnqChkItem
        //    });
        //}
    }
}

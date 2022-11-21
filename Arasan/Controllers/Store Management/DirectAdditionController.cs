using Arasan.Interface.Master;
using Arasan.Interface.Store_Management;
using Arasan.Models;
using Arasan.Services.Store_Management;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Controllers.Store_Management
{
    public class DirectAdditionController : Controller
    {
        IDirectAddition DirectAdditionService;
        public DirectAdditionController(IDirectAddition _DirectAdditionService)
        {
            DirectAdditionService = _DirectAdditionService;
        }
        public IActionResult DirectAddition(string id)
        {
            DirectAddition st = new DirectAddition();
            st.Loc = BindLocation();
            st.Brlst = BindBranch();
            List<DirectItem> TData = new List<DirectItem>();
            DirectItem tda = new DirectItem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new DirectItem();
                    tda.Itlst = BindItem();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                //st = StoreAccService.GetStoreAccById(id);

                DataTable dt = new DataTable();
                dt = DirectAdditionService.GetDirectAdditionDetails(id);
                if (dt.Rows.Count > 0)
                {
                    st.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    st.Location = dt.Rows[0]["LOCID"].ToString();
                    st.DocId = dt.Rows[0]["DOCID"].ToString();
                    st.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    st.ChellanNo = dt.Rows[0]["DCNO"].ToString();
                    st.Reason = dt.Rows[0]["REASON"].ToString();
                    st.Gro = dt.Rows[0]["GROSS"].ToString();
                    st.Entered = dt.Rows[0]["ENTBY"].ToString();
                    st.Narr = dt.Rows[0]["NARRATION"].ToString();

                }

            }
            st.Itlst = TData;
            return View(st);
        }
        [HttpPost]
        public ActionResult DirectAddition(DirectAddition ss, string id)
        {

            try
            {
                ss.ID = id;
                string Strout = DirectAdditionService.DirectAdditionCRUD(ss);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (ss.ID == null)
                    {
                        TempData["notice"] = " DirectAddition Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " DirectAddition Updated Successfully...!";
                    }
                    return RedirectToAction("ListDirectAddition");
                }

                else
                {
                    ViewBag.PageTitle = "Edit DirectAddition";
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
        public IActionResult ListDirectAddition()
        {
            IEnumerable<DirectAddition> sta = DirectAdditionService.GetAllDirectAddition();
            return View(sta);
        }
        public List<SelectListItem> BindLocation()
        {
            try
            {
                DataTable dtDesg = DirectAdditionService.GetLocation();
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
        public List<SelectListItem> BindItem()
        {
            try
            {
                DataTable dtDesg = DirectAdditionService.GetItem();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["ITEMMASTERID"].ToString() });
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
                DataTable dtDesg = DirectAdditionService.GetBranch();
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
        public JsonResult GetItemJSON(string itemid)
        {
            StoreAcc model = new StoreAcc();
            // model.Itlst = BindItem();
            return Json(BindItem());
        }
    }
    
}

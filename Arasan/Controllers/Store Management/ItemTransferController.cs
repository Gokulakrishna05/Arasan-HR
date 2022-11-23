using Arasan.Interface.Master;
using Arasan.Interface.Store_Management;
using Arasan.Models;
using Arasan.Services.Store_Management;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Controllers.Store_Management
{
    public class ItemTransferController : Controller
    {
        IItemTransferService ItemTransferService;
        public ItemTransferController(IItemTransferService _ItemTransferService)
        {
            ItemTransferService = _ItemTransferService;
        }
        public IActionResult ItemTransfer(string id)
        {
            ItemTransfer st = new ItemTransfer();
            st.Loc = BindLocation();
            st.Brlst = BindBranch();
            List<Itemtran> TData = new List<Itemtran>();
            Itemtran tda = new Itemtran();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new Itemtran();
                    tda.Itlst = BindItem();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                //st = ItemTransferService.GetItemTransferById(id);

                DataTable dt = new DataTable();
                dt = ItemTransferService.GetItemTransferDetails(id);
                if (dt.Rows.Count > 0)
                {
                    st.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    st.Location = dt.Rows[0]["FROMLOC"].ToString();
                    st.Toloc = dt.Rows[0]["DESTLOC"].ToString();
                    st.Docid = dt.Rows[0]["DOCID"].ToString();
                    st.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    st.Reason = dt.Rows[0]["REASONCODE"].ToString();
                    st.Gro = dt.Rows[0]["GROSS"].ToString();
                    st.Net = dt.Rows[0]["NET"].ToString();
                    st.Narr = dt.Rows[0]["NARRATION"].ToString();
                 


                }

            }
            st.Itlst = TData;
            return View(st);
        }
        [HttpPost]
        public ActionResult ItemTransfer(ItemTransfer ss, string id)
        {

            try
            {
                ss.ID = id;
                string Strout = ItemTransferService.ItemTransferCRUD(ss);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (ss.ID == null)
                    {
                        TempData["notice"] = " ItemTransfer Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " ItemTransfer Updated Successfully...!";
                    }
                    return RedirectToAction("ListItemTransfer");
                }

                else
                {
                    ViewBag.PageTitle = "Edit ItemTransfer";
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
        public IActionResult ListItemTransfer()
        {
            IEnumerable<ItemTransfer> sta = ItemTransferService.GetAllItemTransfer();
            return View(sta);
        }
        public List<SelectListItem> BindLocation()
        {
            try
            {
                DataTable dtDesg = ItemTransferService.GetLocation();
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
                DataTable dtDesg = ItemTransferService.GetItem();
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
                DataTable dtDesg = ItemTransferService.GetBranch();
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


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
    public class ItemNameController : Controller
    {

        IItemNameService ItemNameService;
        public ItemNameController(IItemNameService _ItemNameService)
        {
            ItemNameService = _ItemNameService;
        }
        public IActionResult ItemName(string id)
        {
            ItemName ca = new ItemName();
            ca.IgLst = BindItemGroup();
            ca.Iclst = BindItemCategory();
            ca.Isglst = BindItemSubGroup ();
            ca.Hsn = BindHSNcode();
            List<SupItem> TData = new List<SupItem>();
            SupItem tda = new SupItem();
            if (id == null)
           
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new SupItem();
                    tda.Suplst = BindSupplier();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                ca = ItemNameService.GetSupplierDetailById(id);


            }
            ca.Suplst = TData;
            return View(ca);
        }
        public List<SelectListItem> BindItemGroup()
        {
            try
            {
                DataTable dtDesg = ItemNameService.GetItemGroup();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["GROUPCODE"].ToString(), Value = dtDesg.Rows[i]["ITEMGROUPID"].ToString() });
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
                DataTable dtDesg = ItemNameService.GetSupplier();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTY"].ToString(), Value = dtDesg.Rows[i]["PARTYMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindItemCategory()
        {
            try
            {
                DataTable dtDesg = ItemNameService.GetItemCategory();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["CATEGORY"].ToString(), Value = dtDesg.Rows[i]["ITEMCATEGORYID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindItemSubGroup()
        {
            try
            {
                DataTable dtDesg = ItemNameService.GetItemSubGroup();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SGCODE"].ToString(), Value = dtDesg.Rows[i]["ITEMSUBGROUPID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult ItemName(ItemName ss, string id)
        {

            try
            {
                ss.ID = id;
                string Strout = ItemNameService.ItemNameCRUD(ss);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (ss.ID == null)
                    {
                        TempData["notice"] = " ItemName Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " ItemName Updated Successfully...!";
                    }
                    return RedirectToAction("ItemList");
                }

                else
                {
                    ViewBag.PageTitle = "Edit ItemName";
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
        public List<SelectListItem> BindHSNcode()
        {
            try
            {
                DataTable dtDesg = ItemNameService.GetHSNcode();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["HSNCODE"].ToString(), Value = dtDesg.Rows[i]["HSNCODEID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public IActionResult ItemList()
        {

            IEnumerable<ItemName> sta = ItemNameService.GetAllItemName();
            return View(sta);
        }
        public JsonResult GetItemGrpJSON()
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindSupplier());
        }
        public IActionResult SupplierDetail(String id)
        {
            ItemName ca = new ItemName();
            if (id == null)
            {

            }
            else
            {
                ca = ItemNameService.GetSupplierById(id);

               
            }

            return View(ca);
        }
        public IActionResult Supplier(string id)
        {
            ItemName cmp = new ItemName();
            if (!string.IsNullOrEmpty(id))
            {
                DataTable dtt = new DataTable();
                dtt = ItemNameService.GetAllSupplier(id);
                ItemName tda = new ItemName();
                List<ItemName> TData = new List<ItemName>();
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda = new ItemName();
                        tda.SupName = dtt.Rows[i]["SUPPLIERID"].ToString();
                        tda.SupPartNo = dtt.Rows[i]["SUPPLIERPARTNO"].ToString();
                        tda.Price = dtt.Rows[i]["SPURPRICE"].ToString();
                        tda.Dy = dtt.Rows[i]["DELDAYS"].ToString();
                        TData.Add(tda);
                    }
                }
                cmp.pflst = TData;

            }
            //IEnumerable<PurchaseFollowup> cmp = PurenqService.GetAllPurchaseFollowup();
            return View(cmp);
        }
        public ActionResult Supplier(ItemName Pf, string id)
        {

            try
            {
                Pf.ID = id;
                string Strout = ItemNameService.SupplierCRUD(Pf);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Pf.ID == null)
                    {
                        TempData["notice"] = "ItemName Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "ItemName Updated Successfully...!";
                    }
                    return RedirectToAction("ItemName");
                }

                else
                {
                    ViewBag.PageTitle = "Edit ItemName";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Pf);
        }

    }
}

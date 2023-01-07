using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services.Master;
using Arasan.Services.Store_Management;
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
            ca.Bin = BindBinID();
            List<SupItem> TData = new List<SupItem>();
            SupItem tda = new SupItem();

            List<BinItem> TDatab = new List<BinItem>();
            BinItem tdaB = new BinItem();

            if (id == null)
           
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new SupItem();
                    tda.Suplst = BindSupplier();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }

                for (int i = 0; i < 3; i++)
                {
                    tdaB = new BinItem();
                    tdaB.Isvalid = "Y";
                    TDatab.Add(tdaB);
                }
            }
            else
            {
                // ca = ItemNameService.GetSupplierDetailById(id);
                
                    DataTable dt = new DataTable();
                    dt = ItemNameService.GetItemNameDetails(id);
                    if (dt.Rows.Count > 0)
                    {
                        ca.ItemG = dt.Rows[0]["IGROUP"].ToString();
                        ca.ItemSub = dt.Rows[0]["ISUBGROUP"].ToString();
                        ca.SubCat = dt.Rows[0]["SUBCATEGORY"].ToString();
                        ca.ItemCode = dt.Rows[0]["ITEMCODE"].ToString();
                        ca.Item = dt.Rows[0]["ITEMID"].ToString();
                        ca.ItemDes = dt.Rows[0]["ITEMDESC"].ToString();
                        ca.Reorderqu = dt.Rows[0]["REORDERQTY"].ToString();
                        ca.Reorderlvl = dt.Rows[0]["REORDERLVL"].ToString();
                        ca.Maxlvl = dt.Rows[0]["MAXSTOCKLVL"].ToString();
                        ca.Minlvl = dt.Rows[0]["MINSTOCKLVL"].ToString();
                        ca.Con = dt.Rows[0]["CONVERAT"].ToString();
                        ca.Uom = dt.Rows[0]["UOM"].ToString();
                        ca.Hcode = dt.Rows[0]["HSN"].ToString();
                        ca.Selling = dt.Rows[0]["SELLINGPRI"].ToString();
                       
                    }
                DataTable dt2 = new DataTable();
                dt2 = ItemNameService.GetBinDeatils(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tdaB = new BinItem();
                        tdaB.BinID = dt2.Rows[0]["BINID"].ToString();
                        tdaB.BinYN = dt2.Rows[0]["BINYN"].ToString();
                        tdaB.Isvalid = "Y";
                        TDatab.Add(tdaB);
                    }
                }

                DataTable dtt = new DataTable();
                dtt = ItemNameService.GetAllSupplier(id);
             
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda = new SupItem();
                        tda.Suplst = BindSupplier();
                        tda.SupName = dtt.Rows[i]["SUPPLIERID"].ToString();
                        tda.SupplierPart = dtt.Rows[i]["SUPPLIERPARTNO"].ToString();
                        tda.PurchasePrice = dtt.Rows[i]["SPURPRICE"].ToString();
                        tda.Delivery = dtt.Rows[i]["DELDAYS"].ToString();
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }
                //ca.Suplst = TData;

            }
            ca.Binlst = TDatab;
            ca.Suplst = TData;
            return View(ca);
        }
        public List<SelectListItem> BindBinID()
        {

            
                try
                {
                    DataTable dtDesg = ItemNameService.BindBinID();
                    List<SelectListItem> lstdesg = new List<SelectListItem>();
                    for (int i = 0; i < dtDesg.Rows.Count; i++)
                    {
                        lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["BINID"].ToString(), Value = dtDesg.Rows[i]["BINBASICID"].ToString() });
                    }
                    return lstdesg;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            
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

        public IActionResult ListItem()
        {
            return View();
        }
        public ActionResult MyListItemgrid()
        {
            List<ItemList> Reg = new List<ItemList>();
            DataTable dtUsers = new DataTable();

            dtUsers = ItemNameService.GetAllItems();
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                EditRow = "<a href=ItemName?id=" + dtUsers.Rows[i]["ITEMMASTERID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                DeleteRow = "<a href=ItemName?tag=Del&id=" + dtUsers.Rows[i]["ITEMMASTERID"].ToString() + ")'><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                Reg.Add(new ItemList
                {
                    id = dtUsers.Rows[i]["ITEMMASTERID"].ToString(),
                    itemgroup = dtUsers.Rows[i]["IGROUP"].ToString(),
                    itemsubgroup = dtUsers.Rows[i]["ISUBGROUP"].ToString(),
                    itemcode = dtUsers.Rows[i]["ITEMCODE"].ToString(),
                    itemname = dtUsers.Rows[i]["ITEMID"].ToString(),
                    //Reorderqu = dtUsers.Rows[i]["REORDERQTY"].ToString(),
                    //Reorderlvl = dtUsers.Rows[i]["REORDERLVL"].ToString(),
                    //Maxlvl = dtUsers.Rows[i]["MAXSTOCKLVL"].ToString(),
                    //Minlvl = dtUsers.Rows[i]["MINSTOCKLVL"].ToString(),
                    cf = dtUsers.Rows[i]["CONVERAT"].ToString(),
                    uom = dtUsers.Rows[i]["UOM"].ToString(),
                    hsncode = dtUsers.Rows[i]["HSN"].ToString(),
                    //sellingprice = dtUsers.Rows[i]["SELLINGPRI"].ToString(),
                    editrow = EditRow,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }
        public JsonResult GetItemGrpJSON()
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindSupplier());
        }
        //public IActionResult SupplierDetail(String id)
        //{
        //    ItemName ca = new ItemName();
        //    if (id == null)
        //    {

        //    }
        //    else
        //    {
               
        //        //ca = ItemNameService.GetSupplierById(id);

               
        //    }

        //    return View(ca);
        //}
        public IActionResult GetSupplier(string id)
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

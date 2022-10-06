using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
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
        public IActionResult ItemName()
        {
            ItemName ca = new ItemName();
            ca.IgLst = BindItemGroup();
            ca.Iclst = BindItemCategory();
            ca.Isglst = BindItemSubGroup ();
         
           
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
      
        public IActionResult ItemList()
        {
            return View();
        }
       
    }
}

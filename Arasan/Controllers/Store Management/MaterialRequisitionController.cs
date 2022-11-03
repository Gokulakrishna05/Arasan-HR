using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Oracle.ManagedDataAccess.Client;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Interface.Master;

using Arasan.Models.Store_Management;
using Newtonsoft.Json.Linq;

namespace Arasan.Controllers.Store_Management
{
    public class MaterialRequisitionController:Controller
    {
        IMaterialRequisition materialReq;
       // IPurchaseIndent PurIndent;
        private string? _connectionString;
        IConfiguration? _configuratio;
        DataTransactions datatrans;
        public MaterialRequisitionController(IMaterialRequisition _MatreqService, IConfiguration _configuratio)
        {
            materialReq = _MatreqService;
           
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }

        public IActionResult MaterialRequisition(string id)
        {
            MaterialRequisition MR = new MaterialRequisition();
             MR.Brlst = BindBranch();
            MR.Loclst = GetLoc();

            List<MaterialRequistionItem> Data = new List<MaterialRequistionItem>();
            MaterialRequistionItem tda = new MaterialRequistionItem();
            for (int i = 0; i < 3; i++)
            {
                tda = new MaterialRequistionItem();
               
                tda.Itemlst = BindItemlst("");
                tda.Isvalid = "Y";
                Data.Add(tda);
            }

            MR.MRlst = Data;
            if (id == null)
            {

            }
            else
            {
               MR = materialReq.GetMaterialById(id);

            }
            return View(MR);
        }

        public List<SelectListItem> BindItemlst(string value)
        {
            try
            {
                DataTable dtDesg = materialReq.GetItem(value);
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
        public ActionResult GetItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                string Desc = "";
                string unit = "";

                dt = materialReq.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    Desc = dt.Rows[0]["ITEMDESC"].ToString();
                    unit = dt.Rows[0]["UNITID"].ToString();


                }

                var result = new { Desc = Desc, unit = unit };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       [HttpPost]
        public ActionResult MaterialRequisition(MaterialRequisition Cy, string id)
        {

            try
           {
                Cy.ID = id;
               string Strout = materialReq.MaterialCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
               {
                   if (Cy.ID == null)
                   {
                    TempData["notice"] = "MaterialRequisition Inserted Successfully...!";
                   }
                    else
                    {
                        TempData["notice"] = "MaterialRequisition Updated Successfully...!";
                    }
                   return RedirectToAction("ListMaterialRequisition");
               }

                else
                {
                    ViewBag.PageTitle = "Edit ListMaterialRequisition";
                    TempData["notice"] = Strout;
                   //return View();
              }

                 
           }
           catch (Exception ex)
           {
               throw ex;
            }

            return View(Cy);
        }
        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dt = new DataTable();
                datatrans = new DataTransactions(_connectionString);
                dt = datatrans.GetBranch();
               // DataTable dtDesg = datatrans.GetBranch();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dt.Rows[i]["BRANCHID"].ToString(), Value = dt.Rows[i]["BRANCHMASTID"].ToString() });
                }
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
                DataTable dt = new DataTable();
                datatrans = new DataTransactions(_connectionString);
                dt = datatrans.GetLocation();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dt.Rows[i]["LOCID"].ToString(), Value = dt.Rows[i]["LOCDETAILSID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //private List<SelectListItem> PopulateDropDown(String query, string textcolumn, string valuecolumn)
        //{
        //   // DataTable dt = new DataTable();
        //   // dt = datatrans.GetBranch();
        //    List<SelectListItem> items = new List<SelectListItem>();
        //   // string constr = ConfigurationManager._connectionString.["IGFSCON"].ConnectionString;
        //    using (OracleConnection con = new OracleConnection(_connectionString))
        //    {
        //        using (OracleCommand cmd = new OracleCommand(query))
        //        {
        //            cmd.Connection = con;
        //            con.Open();
        //            using (OracleDataReader sdr = cmd.ExecuteReader())
        //            {

        //                while (sdr.Read())
        //                {
        //                    items.Add(new SelectListItem
        //                    {
        //                        Text = sdr[textcolumn].ToString(),
        //                        Value = sdr[valuecolumn].ToString(),
        //                    });
        //                }

        //            }
        //            con.Close();

        //        }
        //    }
        //    return items;

        //}
        public IActionResult ListMaterialRequisition()
        {
            IEnumerable<MaterialRequisition> cmp = materialReq.GetAllMaterial();
            return View(cmp);
        }
    

    public JsonResult GetItemJSON(string itemid)
    {
        EnqItem model = new EnqItem();
        model.Itemlst = BindItemlst(itemid);
        return Json(BindItemlst(itemid));
    }
    }
}

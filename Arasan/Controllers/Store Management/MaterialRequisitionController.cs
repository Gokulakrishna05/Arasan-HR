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

        public IActionResult MaterialRequisition()
        {
            MaterialRequisition MR = new MaterialRequisition();
             MR.Brlst = BindBranch();
            MR.Loclst = GetLoc();
            return View(MR);
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
    }
}

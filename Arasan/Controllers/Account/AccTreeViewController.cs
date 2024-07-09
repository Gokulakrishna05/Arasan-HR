using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nest;
using Newtonsoft.Json;
using static Nest.JoinField;

namespace Arasan.Controllers
{
    public class AccTreeViewController : Controller
    {
        IAccTreeView Accgroup;
        IConfiguration? _configuratio; 
        private string? _connectionString;

        DataTransactions datatrans;
        public AccTreeViewController(IAccTreeView _Accgroup, IConfiguration _configuratio)
        {
            Accgroup = _Accgroup;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public IActionResult AccTreeView(string id)
        {
            List<TreeViewNode> nodes = new List<TreeViewNode>();
            DataTable df = new DataTable();
            df = Accgroup.GetParent();
            string parentid = "";
            for (int n = 0; n < df.Rows.Count; n++)
            {

                if (df.Rows[n]["MPARENT"].ToString() == "0")
                {
                    parentid = "#";
                }
                else
                {
                    parentid = df.Rows[n]["MPARENT"].ToString();
                }
                nodes.Add(new TreeViewNode { id = df.Rows[n]["MASTERID"].ToString(), parent = parentid, text = df.Rows[n]["MNAME"].ToString() });
                FillChild(nodes, df.Rows[n]["MASTERID"].ToString(), df.Rows[n]["MPARENT"].ToString());
            }
            ViewBag.Json = JsonConvert.SerializeObject(nodes);
            return View();
        }
        public IActionResult Addnode(string id)
        {
            Accounttree A=new Accounttree();
            A.parent = datatrans.GetDataString("select MNAME from Master where MASTERID='"+ id + "'");
            A.catelst = BindCate();
            A.group = "Account";
            A.cate = "None";
            A.id = id;
            return View(A);
        }

        [HttpPost]
        public ActionResult Addnode(Accounttree Cy, string id)
        {

            try
            {
                string Strout = Accgroup.NodeCreation(Cy);
                if (!string.IsNullOrEmpty(Strout))
                {
                    TempData["notice"] = Strout;

                }
                return View(Cy);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Cy);
        }
        public ActionResult delnode(string id)
        {
            int cunt = datatrans.GetDataId("SELECT COUNT(MASTERID) cunt FROM MASTER where MPARENT='"+ id + "'");
            if (cunt == 0)
            {
                string Strout = Accgroup.NodeDelete(id);
                if (!string.IsNullOrEmpty(Strout))
                {
                    TempData["notice"] = Strout;

                }
            }
            else
            {
                TempData["notice"] = "Please Delete the Child records First.";
            }
        return RedirectToAction("AccTreeView");
            
        }
        public List<SelectListItem> BindCate()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text ="None", Value = "None" });
                lstdesg.Add(new SelectListItem() { Text = "Billwise adjustment - Receivable", Value = "Billwise adjustment - Receivable" });
                lstdesg.Add(new SelectListItem() { Text = "Billwise Adjustment - Payable", Value = "Billwise Adjustment - Payable" });
                lstdesg.Add(new SelectListItem() { Text = "GST required", Value = "GST required" });
                lstdesg.Add(new SelectListItem() { Text = "Bank book required", Value = "Bank book required" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public IActionResult AccTreeView(string id)
        //{
        //    List<TreeViewNode> nodes = new List<TreeViewNode>();
        //    DataTable df = new DataTable();
        //    df = Accgroup.GetAccClass();

        //    DataTable dt = new DataTable();

        //    DataTable dt1 = new DataTable();
        //    DataTable dt2 = new DataTable();
        //    for (int n = 0; n < df.Rows.Count; n++)
        //    {
        //        nodes.Add(new TreeViewNode { id = df.Rows[n]["ACCCLASSID"].ToString(), parent = "#", text = df.Rows[n]["ACCOUNT_CLASS"].ToString() });
        //        dt = Accgroup.GetAccType(df.Rows[n]["ACCCLASSID"].ToString());
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            nodes.Add(new TreeViewNode { id = dt.Rows[i]["ACCOUNTTYPEID"].ToString(), parent = df.Rows[n]["ACCCLASSID"].ToString(), text = dt.Rows[i]["ACCOUNTTYPE"].ToString() });
        //            dt1 = Accgroup.GetAccGroup(dt.Rows[i]["ACCOUNTTYPEID"].ToString());
        //            if (dt1.Rows.Count > 0)
        //            {
        //                for (int j = 0; j < dt1.Rows.Count; j++)
        //                {
        //                    nodes.Add(new TreeViewNode { id = dt1.Rows[j]["ACCGROUPID"].ToString(), parent = dt.Rows[i]["ACCOUNTTYPEID"].ToString(), text = dt1.Rows[j]["ACCOUNTGROUP"].ToString() });

        //                    dt2 = Accgroup.GetAccLedger(dt1.Rows[j]["ACCGROUPID"].ToString());
        //                    if (dt2.Rows.Count > 0)
        //                    {
        //                        for (int k = 0; k < dt2.Rows.Count; k++)
        //                        {
        //                            nodes.Add(new TreeViewNode { id = dt2.Rows[k]["LEDGERID"].ToString(), parent = dt1.Rows[j]["ACCGROUPID"].ToString(), text = dt2.Rows[k]["LEDNAME"].ToString() });
        //                        }
        //                    }
        //                }
        //            }

        //        }
        //    }
        //    ViewBag.Json = JsonConvert.SerializeObject(nodes);
        //    return View();
        //}
        public int FillChild(List<TreeViewNode> nodes,string id,string parent)
        {
            DataTable df = new DataTable();
            df = Accgroup.Getchild(id);
            string parentid = "";
            for (int n = 0; n < df.Rows.Count; n++)
            {
                if (df.Rows[n]["MPARENT"].ToString() == "0")
                {
                    parentid = "#";
                }
                else
                {
                    parentid = df.Rows[n]["MPARENT"].ToString();
                }
                nodes.Add(new TreeViewNode { id = df.Rows[n]["MASTERID"].ToString(), parent = df.Rows[n]["MPARENT"].ToString(), text = df.Rows[n]["MNAME"].ToString() });
                FillChild(nodes,df.Rows[n]["MASTERID"].ToString(), df.Rows[n]["MPARENT"].ToString());
            }

                return 0;
        }

       
       

        public ActionResult AcctreeViewResult(string id,string text,string parentid)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = datatrans.GetData("Select M.MNAME,M.GROUPORACCOUNT,M.CATEGORY,C.MAINCURR,M.MPARENT,M.MASTERID from MASTER M,CURRENCY C where C.CURRENCYID=M.NATIVECURRENCY  AND M.MASTERID='" + id + "'");
                string accname=string.Empty;
                string group=string.Empty;
                string category = string.Empty;
                string cur=string.Empty;
                string masterid = string.Empty;
                string parent=string.Empty;
                if(dt.Rows.Count > 0)
                {
                    accname = dt.Rows[0]["MNAME"].ToString();
                    group= dt.Rows[0]["GROUPORACCOUNT"].ToString();
                    category= dt.Rows[0]["CATEGORY"].ToString();
                    cur= dt.Rows[0]["MAINCURR"].ToString();
                    masterid= dt.Rows[0]["MASTERID"].ToString();
                    parent = datatrans.GetDataString("SELECT MNAME FROM MASTER WHERE MASTERID='" + dt.Rows[0]["MPARENT"].ToString() + "'"); 
                }
              
                var result = new { accname = accname, group = group, category = category, cur = cur , parent = parent, masterid= masterid };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
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
                DataTable dt1 = new DataTable();
                DataTable dt2=new DataTable();
                string acctype = "";
                string acccode = "";
                string accgrp = "";
                string accgrpcode = "";
                string ledgername = "";
                string displayname = "";
                string ledgercode = "";
                if (string.IsNullOrEmpty(parentid) || parentid=="#")
                {
                    dt = datatrans.GetAccType(id);
                    if (dt.Rows.Count > 0)
                    {
                        acctype = dt.Rows[0]["ACCOUNTTYPE"].ToString();
                        acccode = dt.Rows[0]["ACCOUNTCODE"].ToString();
                    }
                }
                else
                {
                    if(Convert.ToInt32(parentid) < 10)
                    {
                        dt1 = datatrans.GetAccGroup(id);
                        if (dt1.Rows.Count > 0)
                        {
                            acctype = dt1.Rows[0]["ACCOUNTTYPE"].ToString();
                            acccode = dt1.Rows[0]["ACCOUNTCODE"].ToString();
                            accgrp = dt1.Rows[0]["ACCOUNTGROUP"].ToString();
                            accgrpcode = dt1.Rows[0]["GROUPCODE"].ToString();
                        }
                    }
                    else
                    {
                        dt2 = datatrans.GetAccLedger(id);
                        if (dt2.Rows.Count > 0)
                        {
                            acctype = dt2.Rows[0]["ACCOUNTTYPE"].ToString();
                            acccode = dt2.Rows[0]["ACCOUNTCODE"].ToString();
                            accgrp = dt2.Rows[0]["ACCOUNTGROUP"].ToString();
                            accgrpcode = dt2.Rows[0]["GROUPCODE"].ToString();
                            ledgername= dt2.Rows[0]["LEDNAME"].ToString();
                            displayname = dt2.Rows[0]["DISPLAY_NAME"].ToString();
                        }
                    }
                }
                var result = new { acctype = acctype, acccode = acccode , accgrp = accgrp , accgrpcode = accgrpcode , ledgername = ledgername , displayname = displayname };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

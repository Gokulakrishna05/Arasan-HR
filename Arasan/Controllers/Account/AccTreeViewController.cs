using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

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
            DataTable dt = new DataTable();
            dt = Accgroup.GetAccType();
            DataTable dt1 = new DataTable();
            DataTable dt2= new DataTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                nodes.Add(new TreeViewNode { id = dt.Rows[i]["ACCOUNTTYPEID"].ToString(), parent = "#", text = dt.Rows[i]["ACCOUNTTYPE"].ToString() });
                dt1 = Accgroup.GetAccGroup(dt.Rows[i]["ACCOUNTTYPEID"].ToString());
                if(dt1.Rows.Count > 0)
                {
                    for (int j = 0; j < dt1.Rows.Count; j++)
                    {
                        nodes.Add(new TreeViewNode { id = dt1.Rows[j]["ACCGROUPID"].ToString(), parent = dt.Rows[i]["ACCOUNTTYPEID"].ToString(), text = dt1.Rows[j]["ACCOUNTGROUP"].ToString() });

                        //dt2 = Accgroup.GetAccLedger(dt1.Rows[j]["ACCGROUPID"].ToString());
                        //if (dt2.Rows.Count > 0)
                        //{
                        //    for (int k = 0; k < dt2.Rows.Count; k++)
                        //    {
                        //        nodes.Add(new TreeViewNode { id = dt2.Rows[k]["ACCGROUPID"].ToString(), parent = dt1.Rows[j]["ACCGROUPID"].ToString(), text = dt2.Rows[k]["ACCOUNTGROUP"].ToString() });
                        //    }
                        //}
                    }
                }
                
            }
            
            ViewBag.Json = JsonConvert.SerializeObject(nodes);
            return View();
        }

        }
}

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
    public class PrivilegesController : Controller
    {
        IPrivileges privileges;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public PrivilegesController(IPrivileges _privileges, IConfiguration _configuratio)
        {
            privileges = _privileges;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult Menutree(string id)
        {
            //List<TreeViewNode> nodes = new List<TreeViewNode>();
            //DataTable df = new DataTable();
            //df = Accgroup.GetParent();
            //string parentid = "";
            //for (int n = 0; n < df.Rows.Count; n++)
            //{

            //    if (df.Rows[n]["MPARENT"].ToString() == "0")
            //    {
            //        parentid = "#";
            //    }
            //    else
            //    {
            //        parentid = df.Rows[n]["MPARENT"].ToString();
            //    }
            //    nodes.Add(new TreeViewNode { id = df.Rows[n]["MASTERID"].ToString(), parent = parentid, text = df.Rows[n]["MNAME"].ToString() });
            //    FillChild(nodes, df.Rows[n]["MASTERID"].ToString(), df.Rows[n]["MPARENT"].ToString());
            //}
            //ViewBag.Json = JsonConvert.SerializeObject(nodes);
            return View();
        }
    }
}

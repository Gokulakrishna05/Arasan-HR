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
        IPrivilegesService privileges;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public PrivilegesController(IPrivilegesService _privileges, IConfiguration _configuratio)
        {
            privileges = _privileges;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult Menutree(string id)
        {
            List<TreeViewNode> nodes = new List<TreeViewNode>();
            DataTable df = new DataTable();
            df = privileges.GetParent();
            string parentid = "";
            for (int n = 0; n < df.Rows.Count; n++)
            {

                if (df.Rows[n]["PARENT"].ToString() == "0")
                {
                    parentid = "#";
                }
                else
                {
                    parentid = df.Rows[n]["PARENT"].ToString();
                }
                nodes.Add(new TreeViewNode { id = df.Rows[n]["SITEMAPID"].ToString(), parent = parentid, text = df.Rows[n]["TITLE"].ToString() });
                FillChild(nodes, df.Rows[n]["SITEMAPID"].ToString(), df.Rows[n]["PARENT"].ToString());
            }
            ViewBag.Json = JsonConvert.SerializeObject(nodes);
            return View();
        }
        public int FillChild(List<TreeViewNode> nodes, string id, string parent)
        {
            DataTable df = new DataTable();
            df = privileges.Getchild(id);
            string parentid = "";
            for (int n = 0; n < df.Rows.Count; n++)
            {
                if (df.Rows[n]["PARENT"].ToString() == "0")
                {
                    parentid = "#";
                }
                else
                {
                    parentid = df.Rows[n]["PARENT"].ToString();
                }
                nodes.Add(new TreeViewNode { id = df.Rows[n]["SITEMAPID"].ToString(), parent = df.Rows[n]["PARENT"].ToString(), text = df.Rows[n]["TITLE"].ToString() });
                FillChild(nodes, df.Rows[n]["SITEMAPID"].ToString(), df.Rows[n]["PARENT"].ToString());
            }

            return 0;
        }
    }
}

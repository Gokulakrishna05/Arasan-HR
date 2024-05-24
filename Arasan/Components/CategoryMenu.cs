using Microsoft.AspNetCore.Mvc;
using Arasan.Models;
using Arasan.Interface.Sales;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Collections.Generic;
using System.Xml.Linq;
using AspNetCore.Reporting;
using System.Reflection;
using Arasan.Interface;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using Arasan.Services;

namespace Arasan.Components
{
    public class CategoryMenu : ViewComponent
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public CategoryMenu(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IViewComponentResult Invoke()
        {
            string userid = Request.Cookies["UserId"];
            MenuList tda = new MenuList();
            List<MenuList> Tdata = new List<MenuList>();
            DataTable dt6 = new DataTable();
            string SvSql = "select S.TITLE,S.PARENT,S.GROUP_ID,S.IS_HEAD,P.IS_DISABLE from user_privileges U,USERPRIVDETAIL P,SITEMAP S WHERE U.PRIVILEGESID=P.PRIVILEGESID AND P.SITEMAPID=S.SITEMAPID AND EMPID='" + userid + "'";
            dt6 = datatrans.GetData(SvSql);
            if (dt6.Rows.Count > 0)
            {
                for (int i = 0; i < dt6.Rows.Count; i++)
                {
                    tda = new MenuList();
                    tda.Title = dt6.Rows[i]["TITLE"].ToString();
                    tda.Parent = dt6.Rows[i]["PARENT"].ToString();
                    tda.Groupid = dt6.Rows[i]["GROUP_ID"].ToString();
                    tda.IsHead = dt6.Rows[i]["IS_HEAD"].ToString();
                    tda.IsDisable= dt6.Rows[i]["IS_DISABLE"].ToString();
                    Tdata.Add(tda);
                }
            }
            return View(Tdata);
        }
    }
}

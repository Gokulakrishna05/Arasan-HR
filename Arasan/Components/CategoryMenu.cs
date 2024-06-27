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
using Org.BouncyCastle.Ocsp;
using System.Drawing;

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
            MenuItem M = new MenuItem();
            M.Isdashborad = false;
            M.IsMaster = false;
            M.IsPurchse = false;
            M.IsStore = false;
            string userid = Request.Cookies["UserId"];
            MenuList tda = new MenuList();
            List<MenuList> Tdata = new List<MenuList>();
            List<MenuList> Tdata1 = new List<MenuList>();
            List<MenuList> Tdata2 = new List<MenuList>();
            DataTable dt6 = new DataTable();
            string SvSql = "";
            //string SvSql = "select S.TITLE,S.PARENT,S.GROUP_ID,S.IS_HEAD,P.IS_DISABLE from user_privileges U,USERPRIVDETAIL P,SITEMAP S WHERE U.PRIVILEGESID=P.PRIVILEGESID AND P.SITEMAPID=S.SITEMAPID AND EMPID='" + userid + "' AND IS_HEAD='N'";
            //dt6 = datatrans.GetData(SvSql);
            //if (dt6.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dt6.Rows.Count; i++)
            //    {
            //        tda = new MenuList();
            //        tda.Title = dt6.Rows[i]["TITLE"].ToString();
            //        tda.Parent = dt6.Rows[i]["PARENT"].ToString();
            //        tda.Groupid = dt6.Rows[i]["GROUP_ID"].ToString();
            //        tda.IsHead = dt6.Rows[i]["IS_HEAD"].ToString();
            //        tda.IsDisable= dt6.Rows[i]["IS_DISABLE"].ToString();
            //        Tdata.Add(tda);
            //    }
            //}
            //M.MmenuLists = Tdata;
            DataTable dt7 = new DataTable();
            dt7= datatrans.GetData("select S.SITEMAPID,S.TITLE, S.PARENT, S.GROUP_ID, S.IS_HEAD, P.IS_DISABLE from user_privileges U, USERPRIVDETAIL P, SITEMAP S WHERE U.PRIVILEGESID = P.PRIVILEGESID AND P.SITEMAPID = S.SITEMAPID AND EMPID = '" + userid + "' AND IS_HEAD='Y'");
            if(dt7.Rows.Count > 0)
            {
                for (int i = 0; i < dt7.Rows.Count; i++)
                {
                    if(dt7.Rows[i]["IS_DISABLE"].ToString()=="N" && dt7.Rows[i]["TITLE"].ToString() == "Dashboard")
                    {
                        M.Isdashborad = true;
                        M.dashparent = dt7.Rows[i]["SITEMAPID"].ToString();
                    }
                    if (dt7.Rows[i]["IS_DISABLE"].ToString() == "N" && dt7.Rows[i]["TITLE"].ToString() == "Masters")
                    {
                        M.IsMaster = true;
                        M.masterparent= dt7.Rows[i]["SITEMAPID"].ToString();
                    }
                    if (dt7.Rows[i]["IS_DISABLE"].ToString() == "N" && dt7.Rows[i]["TITLE"].ToString() == "Purchase")
                    {
                        M.IsPurchse = true;
                        M.purchaseparent = dt7.Rows[i]["SITEMAPID"].ToString();
                    }
                    if (dt7.Rows[i]["IS_DISABLE"].ToString() == "N" && dt7.Rows[i]["TITLE"].ToString() == "StoreManagement")
                    {
                        M.IsStore = true;
                        M.storeparent = dt7.Rows[i]["SITEMAPID"].ToString();
                    }
                    if (dt7.Rows[i]["IS_DISABLE"].ToString() == "N" && dt7.Rows[i]["TITLE"].ToString() == "Sub Contract")
                    {
                        M.Issubcontract = true;
                        M.subcontractparent = dt7.Rows[i]["SITEMAPID"].ToString();
                    }
                    if (dt7.Rows[i]["IS_DISABLE"].ToString() == "N" && dt7.Rows[i]["TITLE"].ToString() == "Account")
                    {
                        M.Isaccounts = true;
                        M.accountsparent = dt7.Rows[i]["SITEMAPID"].ToString();
                    }
                    if (dt7.Rows[i]["IS_DISABLE"].ToString() == "N" && dt7.Rows[i]["TITLE"].ToString() == "Sales")
                    {
                        M.Issales = true;
                    }
                    if (dt7.Rows[i]["IS_DISABLE"].ToString() == "N" && dt7.Rows[i]["TITLE"].ToString() == "Production")
                    {
                        M.Isproduction = true;
                    }
                }
            }
            if (M.Isdashborad == true)
            {
               SvSql = "select S.TITLE,S.PARENT,S.GROUP_ID,S.IS_HEAD,P.IS_DISABLE,P.IS_VIEW  from user_privileges U,USERPRIVDETAIL P,SITEMAP S WHERE U.PRIVILEGESID=P.PRIVILEGESID AND P.SITEMAPID=S.SITEMAPID AND EMPID='" + userid + "' AND IS_HEAD='N' AND GROUP_ID='"+ M.dashparent + "'";
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
                        tda.IsDisable = dt6.Rows[i]["IS_DISABLE"].ToString();
                        tda.IsView= dt6.Rows[i]["IS_VIEW"].ToString();
                        Tdata1.Add(tda);
                    }
                }
                M.DmenuLists = Tdata1;
            }
            if (M.IsMaster == true)
            {
                SvSql = "select S.TITLE,S.PARENT,S.GROUP_ID,S.IS_HEAD,P.IS_DISABLE,P.IS_VIEW  from user_privileges U,USERPRIVDETAIL P,SITEMAP S WHERE U.PRIVILEGESID=P.PRIVILEGESID AND P.SITEMAPID=S.SITEMAPID AND EMPID='" + userid + "' AND IS_HEAD='N' AND GROUP_ID='" + M.masterparent + "'";
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
                        tda.IsDisable = dt6.Rows[i]["IS_DISABLE"].ToString();
                        tda.IsView = dt6.Rows[i]["IS_VIEW"].ToString();
                        Tdata.Add(tda);
                    }
                }
                M.MmenuLists = Tdata;
            }
            if (M.IsPurchse == true)
            {
                SvSql = "select S.TITLE,S.PARENT,S.GROUP_ID,S.IS_HEAD,P.IS_DISABLE,P.IS_VIEW  from user_privileges U,USERPRIVDETAIL P,SITEMAP S WHERE U.PRIVILEGESID=P.PRIVILEGESID AND P.SITEMAPID=S.SITEMAPID AND EMPID='" + userid + "' AND IS_HEAD='N' AND GROUP_ID='" + M.purchaseparent + "'";
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
                        tda.IsDisable = dt6.Rows[i]["IS_DISABLE"].ToString();
                        tda.IsView = dt6.Rows[i]["IS_VIEW"].ToString();
                        Tdata2.Add(tda);
                    }
                }
                M.PmenuLists = Tdata2;
            }
            return View(M);
        }
    }
}

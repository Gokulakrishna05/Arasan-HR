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

namespace Arasan.Models
{
    public class PriCom
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PriCom(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable getmenu(string userId)
        {
            string SvSql = string.Empty;
            SvSql = "select S.TITLE,S.PARENT,S.GROUP_ID,S.IS_HEAD from user_privileges U,USERPRIVDETAIL P,SITEMAP S WHERE U.PRIVILEGESID=P.PRIVILEGESID AND P.SITEMAPID=P.SITEMAPID AND EMPID='"+ userId + "'";
            DataTable dtt = new DataTable();
            dtt = datatrans.GetData(SvSql);
            return dtt;
        }
    }
}

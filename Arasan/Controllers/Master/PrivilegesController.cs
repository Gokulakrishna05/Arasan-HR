using Microsoft.AspNetCore.Mvc;
using Arasan.Interface;
using Arasan.Interface.Report;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Report;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using ClosedXML.Excel;
using Oracle.ManagedDataAccess.Client;
using Newtonsoft.Json;

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
           DataTable df = new  DataTable();
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
        public ActionResult ListPrivileges()
        {
            return View();
        }
        public ActionResult MyListPrivgrid(string strStatus)
        {
            List<Privlist> Reg = new List<Privlist>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers=datatrans.GetData("select E.EMPNAME,P.PDESG_ID DESIGNATION,D.DEPTNAME DEPARTMENT,P.PRIVILEGESID,P.IS_ACTIVE from USER_PRIVILEGES P,EMPMAST E,DDBASIC D WHERE P.EMPID=E.EMPMASTID  AND D.DDBASICID=E.EMPDEPT AND P.IS_ACTIVE='Y'");
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string View = string.Empty;
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=Privileges?id=" + dtUsers.Rows[i]["PRIVILEGESID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "Deletepriv?tag=Del&id=" + dtUsers.Rows[i]["PRIVILEGESID"].ToString() + "";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "Deletepriv?tag=Active&id=" + dtUsers.Rows[i]["PRIVILEGESID"].ToString() + "";
                }
                View = "<a href=ViewPriv?id=" + dtUsers.Rows[i]["PRIVILEGESID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View Details' width='20' /></a>";

                Reg.Add(new Privlist
                {
                    id = dtUsers.Rows[i]["PRIVILEGESID"].ToString(),
                    empname = dtUsers.Rows[i]["EMPNAME"].ToString(),
                    desg = dtUsers.Rows[i]["DESIGNATION"].ToString(),
                    dept = dtUsers.Rows[i]["DEPARTMENT"].ToString(),
                    editrow = EditRow,
                    delrow = DeleteRow,
                    viewrow = View
                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult ViewPriv(string id)
        {
            PrivilegesModel P = new PrivilegesModel();
            DataTable dtm = new DataTable();
            dtm = datatrans.GetData("select E.EMPNAME,P.PDESG_ID DESIGNATION,D.DEPARTMENT,P.PRIVILEGESID,P.IS_ACTIVE from USER_PRIVILEGES P,EMPMAST E,PDEPT D WHERE P.EMPID=E.EMPMASTID AND P.PDEPT_ID=D.PDEPTID AND P.PRIVILEGESID='" + id +"'");
            if(dtm.Rows.Count > 0)
            {
                P.emp = dtm.Rows[0]["EMPNAME"].ToString();
                P.desg= dtm.Rows[0]["DESIGNATION"].ToString();
                P.dept= dtm.Rows[0]["DEPARTMENT"].ToString();
            }
            List<PMenuList> TData = new List<PMenuList>();
            PMenuList tda = new PMenuList();
            DataTable dt = new DataTable();
            dt= datatrans.GetData("Select S.TITLE,U.SITEMAPID,U.IS_DISABLE from USERPRIVDETAIL U,SITEMAP S WHERE U.SITEMAPID=S.SITEMAPID AND U.PRIVILEGESID='"+ id +"' AND S.IS_HEAD='Y' AND S.SM_LEVEL='1' AND U.IS_DISABLE='N'");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tda = new PMenuList();
                    tda.menuname = dt.Rows[i]["TITLE"].ToString();
                    tda.menuid = dt.Rows[i]["SITEMAPID"].ToString();
                    List<menudetails> TData1 = new List<menudetails>();
                    menudetails tda1 = new menudetails();
                    DataTable dtt = new DataTable();
                    dtt = datatrans.GetData("Select S.TITLE,U.SITEMAPID,U.IS_VIEW,U.IS_ADD,U.IS_EDIT,U.IS_DELETE from USERPRIVDETAIL U,SITEMAP S WHERE U.SITEMAPID=S.SITEMAPID AND U.PRIVILEGESID='"+ id +"' AND S.IS_HEAD='N' AND S.GROUP_ID='"+ tda.menuid +"' AND U.IS_VIEW='Y' ");
                    if (dtt.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtt.Rows.Count; j++)
                        {
                            tda1 = new menudetails();
                            tda1.mapid = dtt.Rows[j]["SITEMAPID"].ToString();
                            tda1.urlname = dtt.Rows[j]["TITLE"].ToString();
                            tda1.add = dtt.Rows[j]["IS_ADD"].ToString() == "Y" ?true : false;
                            tda1.edit = dtt.Rows[j]["IS_EDIT"].ToString() == "Y" ? true : false;
                            tda1.delete = dtt.Rows[j]["IS_DELETE"].ToString() == "Y" ? true : false;
                            TData1.Add(tda1);
                        }
                        tda.menudlst = TData1;
                    }
                    else
                    {
                        tda1 = new menudetails();
                        TData1.Add(tda1);
                        tda.menudlst = TData1;
                    }
                    TData.Add(tda);
                }
            }
            P.menulst = TData;
            return View(P);
        }
        public ActionResult Privileges(string id)
        {
            PrivilegesModel P = new PrivilegesModel();
            DataTable dt=new DataTable();
            List<PMenuList> TData = new List<PMenuList>();
            PMenuList tda = new PMenuList();
            dt = datatrans.GetData("select TITLE,SITEMAPID from SITEMAP where IS_HEAD='Y' AND IS_ACTIVE='Y' AND SM_LEVEL='1'");
            if (id == null)
            {
                P.deptlst = BindDept();
                P.desglst = BindEmpty();
                P.emplst = BindEmpty();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        tda = new PMenuList();
                        tda.sectiondisable = true;
                        tda.menuname = dt.Rows[i]["TITLE"].ToString();
                        tda.menuid = dt.Rows[i]["SITEMAPID"].ToString();
                        List<menudetails> TData1 = new List<menudetails>();
                        menudetails tda1 = new menudetails();
                        DataTable dtt = new DataTable();
                        dtt = datatrans.GetData("select TITLE,SITEMAPID from SITEMAP where IS_HEAD='N' AND IS_ACTIVE='Y' AND GROUP_ID='" + tda.menuid + "'");
                        if (dtt.Rows.Count > 0)
                        {
                            for (int j = 0; j < dtt.Rows.Count; j++)
                            {
                                tda1 = new menudetails();
                                tda1.mapid = dtt.Rows[j]["SITEMAPID"].ToString();
                                tda1.urlname = dtt.Rows[j]["TITLE"].ToString();
                               
                                TData1.Add(tda1);
                            }
                            tda.menudlst = TData1;
                        }
                        else
                        {
                            for (int j = 0; j < 1; j++)
                            {
                                tda1 = new menudetails();
                                TData1.Add(tda1);
                            }
                            tda.menudlst = TData1;
                        }
                        TData.Add(tda);
                    }
                }
            }
            else
            {
                P.deptlst = BindDept();
                P.desglst = BindEmpty();
                P.emplst = BindEmpty();
                DataTable dtr=new DataTable();
                dtr = datatrans.GetData("select EMPID,PDESG_ID,PDEPT_ID from USER_PRIVILEGES WHERE PRIVILEGESID='"+ id + "'");
                if( dtr.Rows.Count > 0)
                {
                    P.dept = dtr.Rows[0]["PDEPT_ID"].ToString();
                    P.desglst = Binddesignation(P.dept);
                    P.desg= dtr.Rows[0]["PDESG_ID"].ToString();
                    P.emplst = Bindemp(P.desg, P.dept);
                    P.emp= dtr.Rows[0]["EMPID"].ToString();
                }
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        tda = new PMenuList();
                        tda.menuname = dt.Rows[i]["TITLE"].ToString();
                        tda.menuid = dt.Rows[i]["SITEMAPID"].ToString();
                        DataTable dtm=new DataTable();
                        dtm = datatrans.GetData("select IS_DISABLE from USERPRIVDETAIL Where SITEMAPID='"+ tda.menuid + "' AND PRIVILEGESID='"+ id + "'");
                        if( dtm.Rows.Count > 0 )
                        {
                            tda.sectiondisable = dtm.Rows[0]["IS_DISABLE"].ToString() =="Y" ? true : false;
                        }
                        List<menudetails> TData1 = new List<menudetails>();
                        menudetails tda1 = new menudetails();
                        DataTable dtt = new DataTable();
                        dtt = datatrans.GetData("select TITLE,SITEMAPID from SITEMAP where IS_HEAD='N' AND IS_ACTIVE='Y' AND GROUP_ID='" + tda.menuid + "'");
                        if (dtt.Rows.Count > 0)
                        {
                            for (int j = 0; j < dtt.Rows.Count; j++)
                            {
                                tda1 = new menudetails();
                                tda1.mapid = dtt.Rows[j]["SITEMAPID"].ToString();
                                tda1.urlname = dtt.Rows[j]["TITLE"].ToString();
                                DataTable dtf=new DataTable();
                                dtf = datatrans.GetData("select IS_VIEW,IS_ADD,IS_EDIT,IS_DELETE from USERPRIVDETAIL Where SITEMAPID='" + tda1.mapid + "' AND PRIVILEGESID='"+ id + "'");
                                if(dtf.Rows.Count > 0)
                                {
                                    tda1.add = dtf.Rows[0]["IS_ADD"].ToString() == "Y" ? true :false;
                                    tda1.View = dtf.Rows[0]["IS_VIEW"].ToString() == "Y" ? true : false;
                                    tda1.edit = dtf.Rows[0]["IS_EDIT"].ToString() == "Y" ? true : false;
                                    tda1.delete = dtf.Rows[0]["IS_DELETE"].ToString() == "Y" ? true : false;
                                }
                                TData1.Add(tda1);
                            }
                            tda.menudlst = TData1;
                        }
                        else
                        {
                            for (int j = 0; j < 1; j++)
                            {
                                tda1 = new menudetails();
                                TData1.Add(tda1);
                            }
                            tda.menudlst = TData1;
                        }
                        TData.Add(tda);
                    }
                }
            }
           
            P.menulst = TData;
            return View(P);
        }
        [HttpPost]
        public ActionResult Privileges(PrivilegesModel Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = privileges.privilegesCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Privileges Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Privileges Updated Successfully...!";
                    }
                    return RedirectToAction("ListPrivileges");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Privileges";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Cy);
        }

        public List<SelectListItem> BindEmpty()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindDept()
        {
            try
            {
                DataTable dtDesg = new DataTable ();
                dtDesg = datatrans.GetData("Select ID,DEPTNAME,count(empname) c from (Select D.DDBASICID ID,D.DEPTNAME,E.EMPNAME from ddbasic d,empmast e where D.DDBASICID=E.EMPDEPT and e.username is not null and E.EACTIVE='Yes' ) group by ID,DEPTNAME Order by c desc");
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DEPTNAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetdesgJSON(string deptid)
        {
            return Json(Binddesignation(deptid));
        }
        public JsonResult GetempJSON(string desgid,string dept)
        {
            return Json(Bindemp(desgid, dept));
        }
        public List<SelectListItem> Binddesignation(string deptid)
        {
            try
            {

                DataTable dtDesg = new DataTable();
                dtDesg = datatrans.GetData("Select DESIGNATION,count(empname) c from (Select DD.DESIGNATION, E.EMPNAME from ddbasic d, empmast e, dddetail dd where D.DDBASICID = E.EMPDEPT and e.username is not null and D.DDBASICID ='"+ deptid + "' and D.DDBASICID = DD.DDBASICID and E.EACTIVE = 'Yes' and DD.DESIGNATION = E.EMPDESIGN ) group by DESIGNATION Order by c desc");
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DESIGNATION"].ToString(), Value = dtDesg.Rows[i]["DESIGNATION"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> Bindemp(string desgid, string dept)
        {
            try
            {
                DataTable dtDesg = new DataTable();
                dtDesg = datatrans.GetData("Select DD.DESIGNATION,E.EMPNAME,E.USERNAME,E.EMPMASTID from ddbasic d,empmast e,dddetail dd where D.DDBASICID = E.EMPDEPT and e.username is not null and D.DDBASICID ='"+ dept + "' and D.DDBASICID = DD.DDBASICID and E.EACTIVE = 'Yes' and DD.DESIGNATION = E.EMPDESIGN and DD.DESIGNATION ='"+ desgid + "' order by 2");
                List <SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPNAME"].ToString(), Value = dtDesg.Rows[i]["EMPMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult Deletepriv(string tag, string id)
        {

            string flag = privileges.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListPrivileges");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListPrivileges");
            }
        }
    }
}

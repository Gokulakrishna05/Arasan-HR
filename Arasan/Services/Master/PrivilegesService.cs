using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services
{
    public class PrivilegesService : IPrivilegesService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PrivilegesService(IConfiguration _configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetParent()
        {
            string SvSql = string.Empty;
            SvSql = "select TITLE,SITEMAPID,PARENT from SITEMAP Where PARENT=0 ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable Getchild(string parentid)
        {
            string SvSql = string.Empty;
            SvSql = "select TITLE,SITEMAPID,PARENT from SITEMAP Where PARENT='" + parentid + "' AND IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string privilegesCRUD(PrivilegesModel cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);
                string Pid = "";

                if (cy.ID == null)
                {
                    svSQL = " SELECT Count(EMPID) as cnt FROM USER_PRIVILEGES WHERE EMPID = LTRIM(RTRIM('" + cy.emp + "')) ";

                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "Employee Already Existed";
                        return msg;
                    }
                }
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {

                    objConn.Open();
                    using (OracleCommand command = objConn.CreateCommand())
                    {
                        using (OracleTransaction transaction = objConn.BeginTransaction(IsolationLevel.ReadCommitted))
                        {
                            command.Transaction = transaction;

                            try
                            {

                                //string desigid = datatrans.GetDataString("SELECT PDESGID FROM PDESG WHERE DESIGNATION='" + cy.desg + "'");
                                if (cy.ID == null)
                                {
                                    command.CommandText = "INSERT INTO USER_PRIVILEGES (EMPID,PDESG_ID,PDEPT_ID) VALUES ('" + cy.emp + "','" + cy.desg + "','" + cy.dept + "') RETURNING PRIVILEGESID INTO :LASTCID";
                                    command.Parameters.Add("LASTCID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                    command.ExecuteNonQuery();
                                    Pid = command.Parameters["LASTCID"].Value.ToString();
                                    foreach (PMenuList cp in cy.menulst)
                                    {
                                        if (cp.sectiondisable == false)
                                        {

                                            foreach (menudetails ca in cp.menudlst)
                                            {
                                                string view = ca.View == true ? "Y" : "N";
                                                string add = ca.add == true ? "Y" : "N";
                                                string edit = ca.edit == true ? "Y" : "N";
                                                string delete = ca.delete == true ? "Y" : "N";


                                                command.CommandText = "INSERT INTO USERPRIVDETAIL (PRIVILEGESID,SITEMAPID,IS_VIEW,IS_ADD,IS_EDIT,IS_DELETE) VALUES ('" + Pid + "','" + ca.mapid + "','" + view + "','" + add + "','" + edit + "','" + delete + "')";
                                                command.ExecuteNonQuery();
                                                if (view == "Y" || add == "Y" || edit == "Y" || delete == "Y")
                                                {
                                                    DataTable parent = datatrans.GetData(" select TITLE, SITEMAPID, PARENT from SITEMAP where IS_HEAD = 'N' AND IS_ACTIVE = 'Y' AND SITEMAPID = '" + ca.mapid + "'");
                                                    if (parent.Rows.Count > 0)
                                                    {
                                                        DataTable head = datatrans.GetData("select TITLE, SITEMAPID, PARENT from SITEMAP where IS_HEAD = 'Y' AND IS_ACTIVE = 'Y' AND SITEMAPID = '" + parent.Rows[0]["PARENT"].ToString() + "'");
                                                        DataTable pri = datatrans.GetData("select PRIVILEGESID from USERPRIVDETAIL where  SITEMAPID = '" + parent.Rows[0]["PARENT"].ToString() + "'  AND PRIVILEGESID='" + Pid + "'");
                                                        if (pri.Rows.Count <= 0)
                                                        {
                                                            svSQL = "INSERT INTO USERPRIVDETAIL (PRIVILEGESID,SITEMAPID,IS_VIEW,IS_ADD,IS_EDIT,IS_DELETE) VALUES ('" + Pid + "','" + head.Rows[0]["SITEMAPID"].ToString() + "','" + view + "','" + add + "','" + edit + "','" + delete + "')";
                                                            OracleCommand objCmds = new OracleCommand(svSQL, objConn);

                                                            objCmds.ExecuteNonQuery();
                                                        }
                                                    }
                                                }


                                            }
                                            command.CommandText = "INSERT INTO USERPRIVDETAIL (PRIVILEGESID,SITEMAPID,IS_DISABLE) VALUES ('" + Pid + "','" + cp.menuid + "','N')";
                                            command.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            command.CommandText = "INSERT INTO USERPRIVDETAIL (PRIVILEGESID,SITEMAPID,IS_DISABLE) VALUES ('" + Pid + "','" + cp.menuid + "','Y')";
                                            command.ExecuteNonQuery();
                                        }

                                    }


                                }
                                else
                                {
                                    command.CommandText = "UPDATE USER_PRIVILEGES SET EMPID='" + cy.emp + "',PDESG_ID='" + cy.desg + "',PDEPT_ID='" + cy.dept + "' WHERE PRIVILEGESID='" + cy.ID + "'";
                                    command.ExecuteNonQuery();
                                    Pid = cy.ID;
                                    foreach (PMenuList cp in cy.menulst)
                                    {
                                        if (cp.sectiondisable == false)
                                        {
                                            foreach (menudetails ca in cp.menudlst)
                                            {
                                                string view = ca.View == true ? "Y" : "N";
                                                string add = ca.add == true ? "Y" : "N";
                                                string edit = ca.edit == true ? "Y" : "N";
                                                string delete = ca.delete == true ? "Y" : "N";

                                                int cunt = datatrans.GetDataId("select COUNT(*) as cunt from USERPRIVDETAIL WHERE PRIVILEGESID='" + cy.ID + "' AND SITEMAPID='" + ca.mapid + "'");
                                                if (cunt > 0)
                                                {
                                                    command.CommandText = "UPDATE USERPRIVDETAIL SET IS_VIEW='" + view + "',IS_ADD='" + add + "',IS_EDIT='" + edit + "',IS_DELETE='" + delete + "' WHERE PRIVILEGESID='" + cy.ID + "' AND SITEMAPID='" + ca.mapid + "'";
                                                    command.ExecuteNonQuery();
                                                    if (view == "Y" || add == "Y" || edit == "Y" || delete == "Y")
                                                    {
                                                        DataTable parent = datatrans.GetData(" select TITLE, SITEMAPID, PARENT from SITEMAP where IS_HEAD = 'N' AND IS_ACTIVE = 'Y' AND SITEMAPID = '" + ca.mapid + "'");
                                                        if (parent.Rows.Count > 0)
                                                        {
                                                            DataTable head = datatrans.GetData("select TITLE, SITEMAPID, PARENT from SITEMAP where IS_HEAD = 'Y' AND IS_ACTIVE = 'Y' AND SITEMAPID = '" + parent.Rows[0]["PARENT"].ToString() + "'");
                                                            DataTable pri = datatrans.GetData("select PRIVILEGESID from USERPRIVDETAIL where  SITEMAPID = '" + parent.Rows[0]["PARENT"].ToString() + "' AND PRIVILEGESID='" + Pid + "'");
                                                            if (pri.Rows.Count <= 0)
                                                            {
                                                                svSQL = "INSERT INTO USERPRIVDETAIL (PRIVILEGESID,SITEMAPID,IS_VIEW,IS_ADD,IS_EDIT,IS_DELETE) VALUES ('" + Pid + "','" + head.Rows[0]["SITEMAPID"].ToString() + "','" + view + "','" + add + "','" + edit + "','" + delete + "')";
                                                                OracleCommand objCmds = new OracleCommand(svSQL, objConn);

                                                                objCmds.ExecuteNonQuery();
                                                            }
                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    command.CommandText = "INSERT INTO USERPRIVDETAIL (PRIVILEGESID,SITEMAPID,IS_VIEW,IS_ADD,IS_EDIT,IS_DELETE) VALUES ('" + Pid + "','" + ca.mapid + "','" + view + "','" + add + "','" + edit + "','" + delete + "')";
                                                    command.ExecuteNonQuery();
                                                    if (view == "Y" || add == "Y" || edit == "Y" || delete == "Y")
                                                    {
                                                        DataTable parent = datatrans.GetData(" select TITLE, SITEMAPID, PARENT from SITEMAP where IS_HEAD = 'N' AND IS_ACTIVE = 'Y' AND SITEMAPID = '" + ca.mapid + "'");
                                                        if (parent.Rows.Count > 0)
                                                        {
                                                            DataTable head = datatrans.GetData("select TITLE, SITEMAPID, PARENT from SITEMAP where IS_HEAD = 'Y' AND IS_ACTIVE = 'Y' AND SITEMAPID = '" + parent.Rows[0]["PARENT"].ToString() + "'");
                                                            DataTable pri = datatrans.GetData("select PRIVILEGESID from USERPRIVDETAIL where  SITEMAPID = '" + parent.Rows[0]["PARENT"].ToString() + "'  AND PRIVILEGESID='" + Pid + "'");
                                                            if (pri.Rows.Count <= 0)
                                                            {
                                                                svSQL = "INSERT INTO USERPRIVDETAIL (PRIVILEGESID,SITEMAPID,IS_VIEW,IS_ADD,IS_EDIT,IS_DELETE) VALUES ('" + Pid + "','" + head.Rows[0]["SITEMAPID"].ToString() + "','" + view + "','" + add + "','" + edit + "','" + delete + "')";
                                                                OracleCommand objCmds = new OracleCommand(svSQL, objConn);

                                                                objCmds.ExecuteNonQuery();
                                                            }
                                                        }
                                                    }

                                                }

                                            }
                                            int cunts = datatrans.GetDataId("select COUNT(*) as cunt from USERPRIVDETAIL WHERE PRIVILEGESID='" + cy.ID + "' AND SITEMAPID='" + cp.menuid + "'");
                                            if (cunts > 0)
                                            {
                                                command.CommandText = "UPDATE USERPRIVDETAIL SET IS_DISABLE='N' WHERE PRIVILEGESID='" + cy.ID + "' AND SITEMAPID='" + cp.menuid + "'";
                                                command.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                command.CommandText = "INSERT INTO USERPRIVDETAIL (PRIVILEGESID,SITEMAPID,IS_DISABLE) VALUES ('" + Pid + "','" + cp.menuid + "','N')";
                                                command.ExecuteNonQuery();

                                            }
                                        }
                                        else
                                        {
                                            int cunts = datatrans.GetDataId("select COUNT(*) as cunt from USERPRIVDETAIL WHERE PRIVILEGESID='" + cy.ID + "' AND SITEMAPID='" + cp.menuid + "'");
                                            if (cunts > 0)
                                            {
                                                command.CommandText = "INSERT INTO USERPRIVDETAIL (PRIVILEGESID,SITEMAPID,IS_DISABLE) VALUES ('" + Pid + "','" + cp.menuid + "','Y')";
                                                command.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                command.CommandText = "UPDATE USERPRIVDETAIL SET IS_DISABLE='Y' WHERE PRIVILEGESID='" + cy.ID + "' AND SITEMAPID='" + cp.menuid + "'";
                                                command.ExecuteNonQuery();
                                            }

                                        }

                                    }
                                }
                                transaction.Commit();
                            }

                            catch (DataException e)
                            {
                                transaction.Rollback();
                                Console.WriteLine(e.ToString());
                                Console.WriteLine("Neither record was written to database.");
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }

        public string StatusChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {

                    if(tag=="Del")
                    {
                        svSQL = "UPDATE USER_PRIVILEGES SET IS_ACTIVE ='N' WHERE PRIVILEGESID='" + id + "'";

                    }
                    else
                    {
                        svSQL = "UPDATE USER_PRIVILEGES SET IS_ACTIVE ='Y' WHERE PRIVILEGESID='" + id + "'";

                    }
                    OracleCommand objCmds = new OracleCommand(svSQL, objConnT);
                    objConnT.Open();
                    objCmds.ExecuteNonQuery();
                    objConnT.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "";

        }
    }
}

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
            SvSql = "select TITLE,SITEMAPID,PARENT from SITEMAP Where PARENT=0";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable Getchild(string parentid)
        {
            string SvSql = string.Empty;
            SvSql = "select TITLE,SITEMAPID,PARENT from SITEMAP Where PARENT='" + parentid + "'";
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


                                                command.CommandText = "INSERT INTO USERPRIVDETAIL (PRIVILEGESID,SITEMAPID,IS_VIEW,IS_ADD,IS_EDIT,IS_DELETE) VALUES ('" + Pid + "','" + ca.mapid + "','"+ view + "','"+ add +"','"+ edit +"','"+ delete +"')";
                                                command.ExecuteNonQuery();
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
                                    command.CommandText = "UPDATE USER_PRIVILEGES SET EMPID='" + cy.emp + "',PDESG_ID='" + cy.desg + "',PDEPT_ID='" + cy.dept + "' WHERE PRIVILEGESID='"+ cy.ID + "'";
                                    command.ExecuteNonQuery();
                                    Pid =cy.ID;
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
                                                if(cunt > 0)
                                                {
                                                    command.CommandText = "INSERT INTO USERPRIVDETAIL (PRIVILEGESID,SITEMAPID,IS_VIEW,IS_ADD,IS_EDIT,IS_DELETE) VALUES ('" + Pid + "','" + ca.mapid + "','" + view + "','" + add + "','" + edit + "','" + delete + "')";
                                                    command.ExecuteNonQuery();
                                                }
                                                else
                                                {
                                                    command.CommandText = "UPDATE USERPRIVDETAIL IS_VIEW='" + view + "',IS_ADD='" + add + "',IS_EDIT='" + edit + "',IS_DELETE='" + delete + "' WHERE PRIVILEGESID='" + cy.ID + "' AND SITEMAPID='" + ca.mapid + "'";
                                                    command.ExecuteNonQuery();
                                                }
                                               
                                            }
                                            int cunts = datatrans.GetDataId("select COUNT(*) as cunt from USERPRIVDETAIL WHERE PRIVILEGESID='" + cy.ID + "' AND SITEMAPID='" + cp.menuid + "'");
                                            if (cunts > 0)
                                            {
                                                command.CommandText = "INSERT INTO USERPRIVDETAIL (PRIVILEGESID,SITEMAPID,IS_DISABLE) VALUES ('" + Pid + "','" + cp.menuid + "','N')";
                                                command.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                command.CommandText = "UPDATE USERPRIVDETAIL IS_DISABLE='N' WHERE PRIVILEGESID='" + cy.ID + "' AND SITEMAPID='" + cp.menuid + "'";
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
                                                command.CommandText = "UPDATE USERPRIVDETAIL IS_DISABLE='Y' WHERE PRIVILEGESID='" + cy.ID + "' AND SITEMAPID='" + cp.menuid + "'";
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
    }
}

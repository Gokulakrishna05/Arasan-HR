using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{
    public class DepartmentService : IDepartment
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public DepartmentService(IConfiguration _configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        //public IEnumerable<Department> GetAllDepartment(string status)
        //{
        //    if (string.IsNullOrEmpty(status))
        //    {
        //        status = "ACTIVE";
        //    }
        //    List<Department> cmpList = new List<Department>();
        //    using (OracleConnection con = new OracleConnection(_connectionString))
        //    {

        //        using (OracleCommand cmd = con.CreateCommand())
        //        {
        //            con.Open();
        //            cmd.CommandText = "Select DEPARTMENTMASTID,DEPARTMENT_CODE,DEPARTMENT_NAME,DESCRIPTION, STATUS from DEPARTMENTMAST WHERE DEPARTMENTMAST.STATUS= '" + status + "' order by DEPARTMENTMAST.DEPARTMENTMASTID DESC";
        //            OracleDataReader rdr = cmd.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                Department cmp = new Department
        //                {
        //                    ID = rdr["DEPARTMENTMASTID"].ToString(),
        //                    Departmentcode = rdr["DEPARTMENT_CODE"].ToString(),
        //                    DepartmentName = rdr["DEPARTMENT_NAME"].ToString(),
        //                    Description = rdr["DESCRIPTION"].ToString(),
        //                    status = rdr["STATUS"].ToString()

        //                };
        //                cmpList.Add(cmp);
        //            }
        //        }
        //    }
        //    return cmpList;
        //}
        //public DataTable GetDepartmentDetail(string id)
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "SELECT DESIGNATIONMASTID,DESIGNATION FROM DESIGNATIONMAST where DEPT_ID = '" + id + "'";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}

        public string DepartmentCRUD(Department ss)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";
                if (ss.ID == null)
                {

                    svSQL = " SELECT Count(DEPARTMENT_CODE) as cnt FROM DEPARTMENTMAST WHERE DEPARTMENT_CODE = LTRIM(RTRIM('" + ss.Departmentcode + "')) ";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "Departmentcode Already Existed";
                        return msg;
                    }
                   
                }
              
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("DEPARTMENTPROC", objConn);

                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (ss.ID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = ss.ID;
                    }
                    objCmd.Parameters.Add("DEPARTMENT_CODE", OracleDbType.NVarchar2).Value = ss.Departmentcode;
                    objCmd.Parameters.Add("DEPARTMENT_NAME", OracleDbType.NVarchar2).Value = ss.DepartmentName;
                    objCmd.Parameters.Add("DESCRIPTION", OracleDbType.NVarchar2).Value = ss.Descrip;
                    objCmd.Parameters.Add("IS_ACTIVE", OracleDbType.NVarchar2).Value = "Y";

                    if (ss.ID == null)
                    {
                        objCmd.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = ss.createby;
                        objCmd.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    }
                    else
                    {
                        objCmd.Parameters.Add("UPDATED_BY", OracleDbType.NVarchar2).Value = ss.createby;
                        objCmd.Parameters.Add("UPDATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    }

                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;

                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {

                    }
                    objConn.Close();
                }
            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }
        public string PDepartmentCRUD(Department ss)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    objConn.Open();
                    if (ss.ID == null)
                    {
                        svSQL = "Insert into PDEPT (DEPARTMENT,POSITION,CREATEDBY,CREATEDON) VALUES ('" + ss.DepartmentName + "','" + ss.Pos + "','" + ss.createby + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "')";
                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        objCmds.ExecuteNonQuery();
                    }
                    else
                    {
                        svSQL = "Update PDEPT SET DEPARTMENT='" + ss.DepartmentName + "',POSITION='" + ss.Pos + "' WHERE  PDEPTID='"+ ss.ID + "'  ";
                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        objCmds.ExecuteNonQuery();
                    }
                    objConn.Close();
                }
               

                   
                   
            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }

        //try
        //{
        //    objConn.Open();
        //    objCmd.ExecuteNonQuery();
        //Object Pid = objCmd.Parameters["OUTID"].Value;
        ////string Pid = "0";
        //if (ss.ID != null)
        //{
        //    Pid = ss.ID;
        //}
        //foreach (Designation ca in ss.Designationlst)
        //{
        //    if (ca.Isvalid == "Y" && ca.Design != "0")
        //    {

        //        using (OracleConnection objConns = new OracleConnection(_connectionString))
        //        {
        //            OracleCommand objCmds = new OracleCommand("DESIGNATIONPROC", objConns);
        //            if (ss.ID == null)
        //            {
        //                StatementType = "Insert";
        //                objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
        //            }
        //            else
        //            {
        //                StatementType = "Update";
        //                objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = ss.ID;
        //            }
        //            objCmds.CommandType = CommandType.StoredProcedure;

        //            objCmds.Parameters.Add("DEPT_ID", OracleDbType.NVarchar2).Value = Pid;
        //            objCmds.Parameters.Add("DESIGNATION", OracleDbType.NVarchar2).Value = ca.Design;
        //            objCmds.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "ACTIVE";
        //            objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
        //            objConns.Open();
        //            objCmds.ExecuteNonQuery();
        //            objConns.Close();
        //        }
        //    }
        //}
        //            }
        //           catch (Exception ex)
        //            {
        //                msg = "Error Occurs, While inserting / updating Data";
        //                throw ex;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        msg = "Error Occurs, While inserting / updating Data";
        //        throw ex;
        //    }
        //    return msg;
        //}        
        public DataTable GetDepartment(string id)
                 {
                    string SvSql = string.Empty;
                    SvSql = "Select DEPARTMENTMASTID,DEPARTMENT_CODE,DEPARTMENT_NAME,DESCRIPTION from DEPARTMENTMAST where DEPARTMENTMASTID = '" + id + "' ";
                    DataTable dtt = new DataTable();
                    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
                    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
                    adapter.Fill(dtt);
                    return dtt;
        }
        public DataTable GetPDepartment(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select DEPARTMENT,PDEPTID,POSITION,IS_ACTIVE from PDEPT where PDEPTID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string StatusChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE DEPARTMENTMAST SET IS_ACTIVE ='N' WHERE DEPARTMENTMASTID='" + id + "'";
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
        
        public string RemoveChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE DEPARTMENTMAST SET IS_ACTIVE ='Y' WHERE DEPARTMENTMASTID='" + id + "'";
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

        public DataTable GetAllDEPARTMENT(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select DEPARTMENTMAST.IS_ACTIVE,DEPARTMENTMASTID,DEPARTMENT_CODE,DEPARTMENT_NAME from DEPARTMENTMAST  WHERE DEPARTMENTMAST.IS_ACTIVE = 'Y' ORDER BY DEPARTMENTMASTID DESC";
            }
            else
            {
                SvSql = "Select DEPARTMENTMAST.IS_ACTIVE,DEPARTMENTMASTID,DEPARTMENT_CODE,DEPARTMENT_NAME from DEPARTMENTMAST  WHERE DEPARTMENTMAST.IS_ACTIVE = 'N' ORDER BY DEPARTMENTMASTID DESC";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAllPDEPARTMENT(string strStatus)
        {
            if (strStatus == null)
            {
                strStatus = "Y";
            }
            string SvSql = string.Empty;
            SvSql = "Select DEPARTMENT,PDEPTID,POSITION,IS_ACTIVE from PDEPT where IS_ACTIVE='"+ strStatus + "' ORDER BY PDEPTID DESC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

    }

        
}


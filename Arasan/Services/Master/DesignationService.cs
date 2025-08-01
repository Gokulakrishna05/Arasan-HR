﻿using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services.Master
{
    public class DesignationService : IDesignation
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public DesignationService(IConfiguration _configuration)
        {
            _connectionString = _configuration.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        //public IEnumerable<Designation> GetAllDesignation(string status)
        //{
        //    if (string.IsNullOrEmpty(status))
        //    {
        //        status = "ACTIVE";
        //    }
        //    List<Designation> cmpList = new List<Designation>();
        //    using (OracleConnection con = new OracleConnection(_connectionString))
        //    {

        //        using (OracleCommand cmd = con.CreateCommand())
        //        {
        //            con.Open();
        //            cmd.CommandText = "Select DESIGNATIONMASTID,DESIGNATION,DEPARTMENT_NAME from DESIGNATIONMAST WHERE STATUS= '" + status + "' order by DESIGNATIONMAST.DESIGNATIONMASTID DESC";
        //            OracleDataReader rdr = cmd.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                Designation cmp = new Designation
        //                {
        //                    ID = rdr["DESIGNATIONMASTID"].ToString(),
        //                    Design = rdr["DESIGNATION"].ToString(),
        //                    DeptName = rdr["DEPARTMENT_NAME"].ToString()
        //                    //status = rdr["STATUS"].ToString()


        //                };
        //                cmpList.Add(cmp);
        //            }
        //        }
        //    }
        //    return cmpList;
        //}

        public string DesignationCRUD(Designation ss)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";
                if (ss.ID == null)
                {

                    svSQL = " SELECT Count(DESIGNATION) as cnt FROM DESIGNATIONMAST WHERE DESIGNATION = LTRIM(RTRIM('" + ss.Design + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "Designation Already Existed";
                        return msg;
                    }
                }
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("DESIGNATIONPROC", objConn);

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
                    
                    objCmd.Parameters.Add("DESIGNATION", OracleDbType.NVarchar2).Value = ss.Design;
                    objCmd.Parameters.Add("DEPARTMENT_NAME", OracleDbType.NVarchar2).Value = ss.DeptName;

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
                    //objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;

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
        public string PDesignationCRUD(Designation ss)
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
                        svSQL = "Insert into PDESG (PDEPTID,POSITION,CREATEDBY,CREATEDON,DESIGNATION) VALUES ('" + ss.DeptName + "','" + ss.Pos + "','" + ss.createby + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','"+ ss.Design +"')";
                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        objCmds.ExecuteNonQuery();
                    }
                    else
                    {
                        svSQL = "Update PDESG SET DESIGNATION='" + ss.Design + "',POSITION='" + ss.Pos + "',PDEPTID='" + ss.DeptName + "' WHERE  PDESGID='" + ss.ID + "'  ";
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

        public DataTable GetDeptName()
        {
            string SvSql = string.Empty;
            SvSql = "select DEPARTMENTMASTID,DEPARTMENT_NAME from DEPARTMENTMAST where IS_ACTIVE= 'Y'  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPDeptName()
        {
            string SvSql = string.Empty;
            SvSql = "select PDEPTID,DEPARTMENT from PDEPT where IS_ACTIVE= 'Y'  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetDesignation(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select DESIGNATIONMASTID,DESIGNATION,DEPARTMENT_NAME from DESIGNATIONMAST where DESIGNATIONMASTID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetPDesignation(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PDESGID,DESIGNATION,POSITION,PDEPTID FROM PDESG where PDESGID= '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string StatusChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE DESIGNATIONMAST SET IS_ACTIVE ='N' WHERE DESIGNATIONMASTID ='" + id + "'";
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

        } public string RemoveChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE DESIGNATIONMAST SET IS_ACTIVE ='Y' WHERE DESIGNATIONMASTID ='" + id + "'";
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

        public DataTable GetAllDESIGNATION(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select DESIGNATIONMAST.IS_ACTIVE,DESIGNATIONMASTID,DESIGNATION,DEPARTMENTMAST.DEPARTMENT_NAME from DESIGNATIONMAST LEFT OUTER JOIN DEPARTMENTMAST ON DEPARTMENTMAST.DEPARTMENTMASTID = DESIGNATIONMAST.DEPARTMENT_NAME  WHERE DESIGNATIONMAST.IS_ACTIVE = 'Y' ORDER BY DESIGNATIONMASTID DESC";
            }
            else
            {
                SvSql = "Select DESIGNATIONMAST.IS_ACTIVE,DESIGNATIONMASTID,DESIGNATION,DEPARTMENTMAST.DEPARTMENT_NAME from DESIGNATIONMAST LEFT OUTER JOIN DEPARTMENTMAST ON DEPARTMENTMAST.DEPARTMENTMASTID = DESIGNATIONMAST.DEPARTMENT_NAME  WHERE DESIGNATIONMAST.IS_ACTIVE = 'N' ORDER BY DESIGNATIONMASTID DESC";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAllPDESIGNATION(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == null)
            {
                strStatus = "Y";
            }
           SvSql = "Select PDESGID,P.DESIGNATION,P.POSITION,P.PDEPTID,P.IS_ACTIVE,D.DEPARTMENT FROM pdesg P,PDEPT D WHERE D.PDEPTID=P.PDEPTID AND P.IS_ACTIVE='Y' ORDER BY PDESGID DESC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}

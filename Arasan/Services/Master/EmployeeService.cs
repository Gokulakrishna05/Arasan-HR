using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services.Master
{
    public class EmployeeService : IEmployee
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public EmployeeService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);

        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            List<Employee> cmpList = new List<Employee>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select  EMPMAST. EMPNAME, EMPMAST.EMPID, EMPMAST.EMPSEX,to_char( EMPMAST.EMPDOB,'dd-MON-yyyy')EMPDOB,ECADD1, ECCITY,ECSTATE,ECMAILID,ECPHNO,FATHERNAME,MOTHERNAME,EMPPAYCAT,EMPBASIC,PFNO,ESINO,EMPCOST,to_char( EMPMAST.PFDT,'dd-MON-yyyy')PFDT,to_char( EMPMAST.ESIDT,'dd-MON-yyyy')ESIDT,USERNAME,PASSWORD,EMPDEPT,EMPDESIGN,EMPDEPTCODE,to_char( EMPMAST.JOINDATE,'dd-MON-yyyy')JOINDATE,to_char( EMPMAST.RESIGNDATE,'dd-MON-yyyy')RESIGNDATE,EMPMASTID,EMPMAST.STATUS from EMPMAST where EMPMAST.STATUS='ACTIVE'  ";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Employee cmp = new Employee
                        {

                            ID = rdr["EMPMASTID"].ToString(),
                            EmpName = rdr["EMPNAME"].ToString(),
                            EmpNo = rdr["EMPID"].ToString(),
                            Gender = rdr["EMPSEX"].ToString(),
                            DOB = rdr["EMPDOB"].ToString(),
                            Address = rdr["ECADD1"].ToString(),
                            CityId = rdr["ECCITY"].ToString(),
                            StateId = rdr["ECSTATE"].ToString(),
                            EmailId = rdr["ECMAILID"].ToString(),
                            PhoneNo = rdr["ECPHNO"].ToString(),
                            FatherName = rdr["FATHERNAME"].ToString(),
                            MotherName = rdr["MOTHERNAME"].ToString()
                            
                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }

        public long GetMregion(string regionid, string id)
        {
            string SvSql = "SELECT LOCID from EMPLOYEELOCATION where LOCID=" + regionid + " and EMPID=" + id + "";
            DataTable dtCity = new DataTable();
            long user_id = datatrans.GetDataIdlong(SvSql);
            return user_id;
        }
        public string EmployeeCRUD(Employee cy)
        {
            string msg = ""; 
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                if (cy.ID == null)
                {

 
                    svSQL = " SELECT Count(EMPNAME) as cnt FROM EMPMAST WHERE EMPNAME = LTRIM(RTRIM('" + cy.EmpNo + "')) ";
 
                     if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "Employee Name Already Existed";
                        return msg;
                    }
                }

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("EMPLOYEEPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "DIRECTPURCHASEPROC";*/

                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (cy.ID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;

                    }
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("EMPNAME", OracleDbType.NVarchar2).Value = cy.EmpName;
                    objCmd.Parameters.Add("EMPID", OracleDbType.NVarchar2).Value = cy.EmpNo;
                    objCmd.Parameters.Add("EMPSEX", OracleDbType.NVarchar2).Value = cy.Gender;
                    objCmd.Parameters.Add("EMPDOB", OracleDbType.NVarchar2).Value =cy.DOB;
                    objCmd.Parameters.Add("ECADD1", OracleDbType.NVarchar2).Value = cy.Address;
                    objCmd.Parameters.Add("ECCITY", OracleDbType.NVarchar2).Value = cy.CityId;
                    objCmd.Parameters.Add("ECSTATE", OracleDbType.NVarchar2).Value = cy.StateId;
                    objCmd.Parameters.Add("ECMAILID", OracleDbType.NVarchar2).Value = cy.EmailId;
                    objCmd.Parameters.Add("ECPHNO", OracleDbType.NVarchar2).Value = cy.PhoneNo;
                    objCmd.Parameters.Add("FATHERNAME", OracleDbType.NVarchar2).Value = cy.FatherName;
                    objCmd.Parameters.Add("MOTHERNAME", OracleDbType.NVarchar2).Value = cy.MotherName;
                    objCmd.Parameters.Add("EMPPAYCAT", OracleDbType.NVarchar2).Value = cy.EMPPayCategory;
                    objCmd.Parameters.Add("EMPBASIC", OracleDbType.NVarchar2).Value = cy.EMPBasic;
                    objCmd.Parameters.Add("PFNO", OracleDbType.NVarchar2).Value = cy.PFNo;
                    objCmd.Parameters.Add("ESINO", OracleDbType.NVarchar2).Value = cy.ESINo;
                    objCmd.Parameters.Add("EMPCOST", OracleDbType.NVarchar2).Value = cy.EMPCost;
                    objCmd.Parameters.Add("PFDT", OracleDbType.NVarchar2).Value = cy.PFdate;
                    objCmd.Parameters.Add("ESIDT", OracleDbType.NVarchar2).Value = cy.ESIDate;
                    objCmd.Parameters.Add("USERNAME", OracleDbType.NVarchar2).Value = cy.UserName;
                    objCmd.Parameters.Add("PASSWORD", OracleDbType.NVarchar2).Value = cy.Password;
                    objCmd.Parameters.Add("EMPDEPT", OracleDbType.NVarchar2).Value = cy.EMPDeptment;
                    objCmd.Parameters.Add("EMPDESIGN", OracleDbType.NVarchar2).Value = cy.EMPDesign;
                    objCmd.Parameters.Add("EMPDEPTCODE", OracleDbType.NVarchar2).Value = cy.EMPDeptCode;
                    objCmd.Parameters.Add("JOINDATE", OracleDbType.NVarchar2).Value = cy.JoinDate;
                    objCmd.Parameters.Add("RESIGNDATE", OracleDbType.NVarchar2).Value = cy.ResignDate;
                    objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "ACTIVE";

                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        Object Pid = objCmd.Parameters["OUTID"].Value;
                        //string Pid = "0";
                        if (cy.ID != null)
                        {
                            Pid = cy.ID;
                        }



                        //foreach (EduDeatils cp in cy.EduLst)
                        //{

                        using (OracleConnection objConns = new OracleConnection(_connectionString))
                        {
                            OracleCommand objCmds = new OracleCommand("EMPEDUCATIONPROC", objConns);
                            if (cy.ID == null)
                            {
                                StatementType = "Insert";
                                objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;

                            }
                            else
                            {
                                StatementType = "Update";
                                objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;

                            }
                            objCmds.CommandType = CommandType.StoredProcedure;
                            objCmds.Parameters.Add("EMPMASTID", OracleDbType.NVarchar2).Value = Pid;
                            objCmds.Parameters.Add("EDUCATION", OracleDbType.NVarchar2).Value = cy.Education;
                            objCmds.Parameters.Add("UC", OracleDbType.NVarchar2).Value = cy.College;
                            objCmds.Parameters.Add("ECPLACE", OracleDbType.NVarchar2).Value = cy.EcPlace;
                            objCmds.Parameters.Add("MPER", OracleDbType.NVarchar2).Value = cy.MPercentage;
                            objCmds.Parameters.Add("YRPASSING", OracleDbType.NVarchar2).Value = cy.YearPassing;

                            objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                            objConns.Open();
                            objCmds.ExecuteNonQuery();
                            objConns.Close();
                        }
                        using (OracleConnection objConns = new OracleConnection(_connectionString))
                        {
                            OracleCommand objCmds = new OracleCommand("EMPOTHERINFOPROC", objConns);
                            if (cy.ID == null)
                            {
                                StatementType = "Insert";
                                objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;

                            }
                            else
                            {
                                StatementType = "Update";
                                objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;

                            }
                            objCmds.CommandType = CommandType.StoredProcedure;
                            objCmds.Parameters.Add("EMPMASTID", OracleDbType.NVarchar2).Value = Pid;
                            objCmds.Parameters.Add("MARITALSTATUS", OracleDbType.NVarchar2).Value = cy.MaterialStatus;
                            objCmds.Parameters.Add("BLOODGROUP", OracleDbType.NVarchar2).Value = cy.BloodGroup;
                            objCmds.Parameters.Add("COMMUNITY", OracleDbType.NVarchar2).Value = cy.Community;
                            objCmds.Parameters.Add("PAYTYPE", OracleDbType.NVarchar2).Value = cy.PayType;
                            objCmds.Parameters.Add("EMPTYPE", OracleDbType.NVarchar2).Value = cy.EmpType;
                            objCmds.Parameters.Add("DISP", OracleDbType.NVarchar2).Value = cy.Disp;
                            objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;

                            objConns.Open();
                            objCmds.ExecuteNonQuery();
                            objConns.Close();
                        }
                        using (OracleConnection objConns = new OracleConnection(_connectionString))
                        {

                            OracleCommand objCmds = new OracleCommand("EMPSKILLPROC", objConns);
                            if (cy.ID == null)
                            {
                                StatementType = "Insert";
                                objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;

                            }
                            else
                            {
                                StatementType = "Update";
                                objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;

                            }
                            objCmds.CommandType = CommandType.StoredProcedure;
                            objCmds.Parameters.Add("EMPMASTID", OracleDbType.NVarchar2).Value = Pid;
                            objCmds.Parameters.Add("SKILL", OracleDbType.NVarchar2).Value = cy.SkillSet;
                            objConns.Open();
                            objCmds.ExecuteNonQuery();
                            objConns.Close();
                        }



                    }
                    catch (Exception ex)
                    {
                        //System.Console.WriteLine("Exception: {0}", ex.ToString());
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





        public DataTable GetState()
        {
            string SvSql = string.Empty;
            SvSql = "select STATE,STATEMASTID from STATEMAST  where STATUS='ACTIVE'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;

        }
        public DataTable GetCity(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select CITYNAME,CITYID from CITYMASTER where STATEID ='"  + id +"'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEmployee(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPMAST.EMPNAME,EMPMAST.EMPID,EMPMAST.EMPSEX,to_char(EMPMAST.EMPDOB,'dd-MON-yyyy')EMPDOB,EMPMAST.ECADD1,EMPMAST.ECCITY,EMPMAST.ECSTATE,EMPMAST.ECMAILID,EMPMAST.ECPHNO,EMPMAST.FATHERNAME,EMPMAST.MOTHERNAME,EMPMAST.EMPPAYCAT,EMPMAST.EMPBASIC,EMPMAST.PFNO,EMPMAST.ESINO,EMPMAST.EMPCOST,to_char(EMPMAST.PFDT,'dd-MON-yyyy')PFDT,to_char(EMPMAST.ESIDT,'dd-MON-yyyy')ESIDT,USERNAME,PASSWORD,EMPDEPT,EMPDESIGN,EMPDEPTCODE,to_char(EMPMAST.JOINDATE,'dd-MON-yyyy')JOINDATE,to_char(EMPMAST.RESIGNDATE,'dd-MON-yyyy')RESIGNDATE,EMPMASTID  from EMPMAST where EMPMAST.EMPMASTID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEmpEduDeatils(string data)
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPMEDU.EDUCATION,UC,EMPMEDU.ECPLACE,to_char(EMPMEDU.YRPASSING,'dd-MON-yyyy')YRPASSING,EMPMEDU.MPER,EMPMEDUID  from EMPMEDU where EMPMEDU.EMPMASTID=" + data + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEmpPersonalDeatils(string  data)
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPMOI.MARITALSTATUS,EMPMOI.BLOODGROUP,EMPMOI.COMMUNITY,EMPMOI.PAYTYPE,EMPMOI.EMPTYPE,EMPMOI.DISP,EMPMOIID  from EMPMOI where EMPMOI.EMPMASTID=" + data + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEmpSkillDeatils(string data)
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPMSS.SKILL,EMPMSSID  from EMPMSS where EMPMSS.EMPMASTID=" + data + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;

        }

        public DataTable GetCurrentUser(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPNAME  from EMPMAST where  EMPMASTID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string GetMultipleLocation(MultipleLocation mp)
        {
                string msg = "";
            try
            {
                string StatementType = string.Empty;
                string svSQL = "";
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    if (mp.Location != null)
                {
                    string EmpID = datatrans.GetDataString("Select EMPMASTID from EMPMAST where EMPNAME='" + mp.EmpName + "' ");
                        string dt = datatrans.GetDataString("Select EMPID from EMPLOYEELOCATION WHERE EMPID='" + EmpID + "'");
                    //string loc = dt.Rows[0]["LOCID"].ToString();
                        if (EmpID==dt)
                        {
                            string Sql = string.Empty;
                            Sql="DELETE FROM employeelocation WHERE empid = '" + EmpID +"'";
                            OracleCommand objCmds = new OracleCommand(Sql, objConn);
                            objConn.Open();
                            objCmds.ExecuteNonQuery();
                            objConn.Close();
                        }
                        for (int i = 0; i < mp.Location.Length; i++)
                            
                        {               
                                OracleCommand objCmd = new OracleCommand("EMPLOCATIONPROC", objConn);
                                /*objCmd.Connection = objConn;
                                objCmd.CommandText = "MULTIPLELOCATIONPROC";*/

                                objCmd.CommandType = CommandType.StoredProcedure;

                                StatementType = "Insert";
                                objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                objCmd.Parameters.Add("EMPID", OracleDbType.NVarchar2).Value = EmpID;
                                objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = mp.Location[i];
                                objCmd.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                objCmd.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = mp.CreadtedBy;
                               

                                objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;


                                try
                                {
                                    objConn.Open();
                                    objCmd.ExecuteNonQuery();
                                    //System.Console.WriteLine("Number of employees in department 20 is {0}", objCmd.Parameters["pout_count"].Value);
                                }
                                catch (Exception ex)
                                {
                                    //System.Console.WriteLine("Exception: {0}", ex.ToString());
                                }
                                objConn.Close();
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

        public string StatusChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE EMPMAST SET STATUS ='ISACTIVE' WHERE EMPMASTID='" + id + "'";
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

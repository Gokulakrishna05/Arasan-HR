using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services.Master
{
    public class EmployeeService : IEmployee
    {
        private readonly string _connectionString;

        public EmployeeService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<Employee> GetAllEmployee()
        {
            List<Employee> cmpList = new List<Employee>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select   EMPNAME,EMPID,EMPSEX,to_char(EMPDOB,'dd-MON-yyyy')EMPDOB,ECADD1, ECCITY,ECSTATE,ECMAILID,ECPHNO,FATHERNAME,MOTHERNAME,EMPMASTID from EMPMAST";
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
                            MotherName = rdr["MOTHERNAME"].ToString(),
                            // Frig = rdr["FREIGHT"].ToString(),
                            // Other = rdr["OTHERCH"].ToString(),
                            // Round = rdr["RNDOFF"].ToString(),
                            // SpDisc = rdr["OTHERDISC"].ToString(),
                            // LRCha = rdr["LRCH"].ToString(),
                            // DelCh = rdr["DELCH"].ToString()



                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public string EmployeeCRUD(Employee cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

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
                    objCmd.Parameters.Add("EMPNAME", OracleDbType.NVarchar2).Value = cy.EmpName;
                    objCmd.Parameters.Add("EMPID", OracleDbType.NVarchar2).Value = cy.EmpNo;
                    objCmd.Parameters.Add("EMPSEX", OracleDbType.NVarchar2).Value = cy.Gender;
                    objCmd.Parameters.Add("EMPDOB", OracleDbType.Date).Value = DateTime.Parse(cy.DOB);
                    objCmd.Parameters.Add("ECADD1", OracleDbType.NVarchar2).Value = cy.Address;
                    //objCmd.Parameters.Add("REFDT", OracleDbType.Date).Value = DateTime.Parse(cy.RefDate);
                    objCmd.Parameters.Add("ECCITY", OracleDbType.NVarchar2).Value = cy.CityId;
                    objCmd.Parameters.Add("ECSTATE", OracleDbType.NVarchar2).Value = cy.StateId;
                    objCmd.Parameters.Add("ECMAILID", OracleDbType.NVarchar2).Value = cy.EmailId;
                    objCmd.Parameters.Add("ECPHNO", OracleDbType.NVarchar2).Value = cy.PhoneNo;
                    objCmd.Parameters.Add("FATHERNAME", OracleDbType.NVarchar2).Value = cy.FatherName;
                    objCmd.Parameters.Add("MOTHERNAME", OracleDbType.NVarchar2).Value = cy.MotherName;
                    //objCmd.Parameters.Add("RNDOFF", OracleDbType.NVarchar2).Value = cy.Round;
                    //objCmd.Parameters.Add("OTHERDISC", OracleDbType.NVarchar2).Value = cy.SpDisc;
                    //objCmd.Parameters.Add("LRCH", OracleDbType.NVarchar2).Value = cy.LRCha;
                    //objCmd.Parameters.Add("DELCH", OracleDbType.NVarchar2).Value = cy.DelCh;
                    //objCmd.Parameters.Add("NARR", OracleDbType.NVarchar2).Value = cy.Narration;
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
                            objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                            objConns.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConns.Close();
                                }



                            
                        
                        //foreach (DirItem cp in cy.DirLst)
                        //{
                        //    if (cp.Isvalid == "Y" && cp.saveItemId != "0")
                        //    {
                        //        using (OracleConnection objConnT = new OracleConnection(_connectionString))
                        //        {
                        //            string Sql = string.Empty;
                        //            if (StatementType == "Update")
                        //            {
                        //                Sql = "Update DPDETAIL SET  QTY= '" + cp.Quantity + "',RATE= '" + cp.rate + "',CF='" + cp.ConFac + "',AMOUNT='" + cp.Amount + "',DISCAMOUNT='" + cp.DiscAmount + "',PURTYPE='" + cp.PurType + "',IFREIGHTCH='" + cp.FrigCharge + "',TOTAMT='" + cp.TotalAmount + "'  where DPBASICID='" + cy.ID + "'  AND ITEMID='" + cp.saveItemId + "' ";
                        //            }
                        //            else
                        //            {
                        //                Sql = "";
                        //            }
                        //            OracleCommand objCmds = new OracleCommand(Sql, objConnT);
                        //            objConnT.Open();
                        //            objCmds.ExecuteNonQuery();
                        //            objConnT.Close();
                        //        }
                        //    }


                        //}
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
            SvSql = "select STATE,STATEMASTID from STATEMAST order by STATEMASTID asc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;

        }
        public DataTable GetCity()
        {
            string SvSql = string.Empty;
            SvSql = "select CITYNAME,CITYID from CITYMASTER ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEmployee(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPMAST.EMPNAME,EMPMAST.EMPID,EMPMAST.EMPSEX,to_char(EMPMAST.EMPDOB,'dd-MON-yyyy')EMPDOB,EMPMAST.ECADD1,EMPMAST.ECCITY,EMPMAST.ECSTATE,EMPMAST.ECMAILID,EMPMAST.ECPHNO,EMPMAST.FATHERNAME,EMPMAST.MOTHERNAME,EMPMASTID  from EMPMAST where EMPMAST.EMPMASTID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        //public DataTable GetEmpEduDeatils(string id)
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "Select EMPMEDU.EDUCATION,EMPMEDU.UC,EMPMEDU.ECPLACE,to_char(EMPMEDU.YRPASSING,'dd-MON-yyyy')YRPASSING,EMPMEDU.MPER,EMPMEDUID  from EMPMEDU where EMPMEDU.EMPMASTID=" + id + "";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
    }
}

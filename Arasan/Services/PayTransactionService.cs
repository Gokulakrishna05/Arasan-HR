using Arasan.Interface;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;
namespace Arasan.Services
{
    public class PayTransactionService : IPayTransaction
    {
        private readonly string _connectionString;
        DataTransactions datatrans;

        public PayTransactionService(IConfiguration _configuratio)


        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetPayCategory()
        {
            string SvSql = string.Empty;
            SvSql = "Select PCBASICID,PAYCATEGORY from PCBASIC  WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetPayCode()
        {
            string SvSql = string.Empty;
            SvSql = "select PARAMETERDETAILID,PAYCODE from PARAMETERBASIC P,PARAMETERDETAIL D where P.PARAMETERBASICID=D.PARAMETERBASICID AND P.IS_ACTIVE='Y' AND D.PAYCODEVALUE='FROM TRANSACTION'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetBranch()
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHMASTID,BRANCHID,IS_ACTIVE from BRANCHMAST  WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable getPayTra(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select EMPID,EMPNAME,EMPMASTID from EMPMAST WHERE IS_ACTIVE='Y' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable getPayTraId(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select EMPID,EMPNAME,EMPMASTID from EMPMAST where EMPMASTID IN (" + id+")";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditPayTra(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select DOCID, BRANCHID, PAYCATEGORY, PAYPERIOD,PAYCODE from APBASIC where APBASICID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditEPayTra(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select EMPMAST.EMPID,APDETAIL.EMPNAME,APDETAIL.AMOUNT from APDETAIL left outer join EMPMAST ON EMPMASTID=APDETAIL.EMPID  where APDETAIL.APBASICID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string PayTransactionCRUD(PayTransaction cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("PAYTRANPROC", objConn);


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

                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Brc;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocID;
                    objCmd.Parameters.Add("PAYCODE", OracleDbType.NVarchar2).Value = cy.PayCod;
                    objCmd.Parameters.Add("PAYPERIOD", OracleDbType.NVarchar2).Value = cy.PayPer;
                    objCmd.Parameters.Add("PAYCATEGORY", OracleDbType.NVarchar2).Value = cy.PayCat;
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
                        if (cy.ptrlst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (PayTraVList cp in cy.ptrlst)
                                {
                                    

                                        svSQL = "Insert into APDETAIL (APBASICID,EMPID,EMPNAME,AMOUNT) VALUES ('" + Pid + "','" + cp.dtid + "','" + cp.empname + "','" + cp.amo + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    
                                }

                            }
                            else
                            {
                                svSQL = "Delete APDETAIL WHERE APBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (PayTraVList cp in cy.ptrlst)
                                {
                                    
                                        svSQL = "Insert into APDETAIL (APBASICID,EMPID,EMPNAME,AMOUNT) VALUES ('" + Pid + "','" + cp.dtid + "','" + cp.empname + "','" + cp.amo + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    
                                }
                            }
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

        public DataTable GetAllPayTransaction(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT APBASIC.APBASICID,APBASIC.DOCID, BRANCHMAST.BRANCHID, PARAMETERDETAIL.PAYCODE, APBASIC.PAYPERIOD,PCBASIC.PAYCATEGORY,APBASIC.IS_ACTIVE FROM APBASIC left outer join BRANCHMAST ON BRANCHMASTID=APBASIC.BRANCHID left outer join PARAMETERDETAIL ON PARAMETERDETAILID=APBASIC.PAYCODE  left outer join PCBASIC ON PCBASICID=APBASIC.PAYCATEGORY WHERE APBASIC.IS_ACTIVE = 'Y' ";

            }
            else
            {
                SvSql = "SELECT APBASIC.APBASICID,APBASIC.DOCID, BRANCHMAST.BRANCHID, PARAMETERDETAIL.PAYCODE, APBASIC.PAYPERIOD,PCBASIC.PAYCATEGORY,APBASIC.IS_ACTIVE FROM APBASIC left outer join BRANCHMAST ON BRANCHMASTID=APBASIC.BRANCHID left outer join PARAMETERDETAIL ON PARAMETERDETAILID=APBASIC.PAYCODE  left outer join PCBASIC ON PCBASICID=APBASIC.PAYCATEGORY WHERE APBASIC.IS_ACTIVE = 'N' ";

            }

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
                    svSQL = "UPDATE APBASIC SET IS_ACTIVE ='N' WHERE APBASICID='" + id + "'";
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
                    svSQL = "UPDATE APBASIC SET IS_ACTIVE ='Y' WHERE APBASICID='" + id + "'";
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

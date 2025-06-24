using Oracle.ManagedDataAccess.Client;
using System.Data;
using Arasan.Interface;
using Arasan.Models;
using Dapper;
using Arasan.Interface.Master;

namespace Arasan.Services.Master
{
    public class PayCategoryService : IPayCategory
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PayCategoryService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public DataTable getPayCat()
        {
            string SvSql = string.Empty;
            SvSql = "select PAYCODE,PRINT,PRINTAS,ADDORLESS,PAYCODEVALUE,FORMULA,PARAMETERDETAILID from PARAMETERDETAIL";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable getPayCatId(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PAYCODE,PRINT,PRINTAS,ADDORLESS,PAYCODEVALUE,FORMULA,PARAMETERDETAILID from PARAMETERDETAIL where PARAMETERDETAILID IN("+id+")";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string PayCategoryCRUD(PayCategory cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("PAYCATEGORYPROC", objConn);


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

                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocID;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.DocDate;
                    objCmd.Parameters.Add("PAYCATEGORY", OracleDbType.NVarchar2).Value = cy.PayCat;
                    objCmd.Parameters.Add("PAYPERIODTYPE", OracleDbType.NVarchar2).Value = "10127001112466";
                    objCmd.Parameters.Add("BASICCAT", OracleDbType.NVarchar2).Value = cy.BasCat;
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
                        if (cy.PcLst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (PayCat cp in cy.PcLst)
                                {
                                    if (cp.Isvalid == "Y")
                                    {

                                        svSQL = "Insert into PCDETAIL (PCBASICID,PAYCODE,PRINT,PRINTAS,ADDORLESS,PAYCODEVALUE,FORMULA) VALUES ('" + Pid + "','" + cp.pc + "','" + cp.pr + "','" + cp.prs + "','" + cp.aod + "','" + cp.pcv + "','" + cp.fo + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }
                                }

                            }
                            else
                            {
                                svSQL = "Delete PCDETAIL WHERE PCBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (PayCat cp in cy.PcLst)
                                {
                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into PCDETAIL (PCBASICID,PAYCODE,PRINT,PRINTAS,ADDORLESS,PAYCODEVALUE,FORMULA) VALUES ('" + Pid + "','" + cp.pc + "','" + cp.pr + "','" + cp.prs + "','" + cp.aod + "','" + cp.pcv + "','" + cp.fo + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }
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
        public DataTable GetAllPayCategory(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT PCBASICID,DOCID, DOCDATE, PAYCATEGORY, PAYPERIODTYPE,BASICCAT,IS_ACTIVE FROM PCBASIC WHERE PCBASIC.IS_ACTIVE = 'Y' ";

            }
            else
            {
                SvSql = "SELECT PCBASICID,DOCID, DOCDATE, PAYCATEGORY, PAYPERIODTYPE,BASICCAT,IS_ACTIVE FROM PCBASIC WHERE PCBASIC.IS_ACTIVE = 'N'  ";

            }

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditPayCat(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select DOCID, DOCDATE, PAYCATEGORY, PAYPERIODTYPE,BASICCAT from PCBASIC where PCBASICID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditPayCode(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PAYCODE,PRINT,PRINTAS,ADDORLESS,PAYCODEVALUE,FORMULA from PCDETAIL where PCBASICID = '" + id + "' ";
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
                    svSQL = "UPDATE PCBASIC SET IS_ACTIVE ='N' WHERE PCBASICID='" + id + "'";
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
                    svSQL = "UPDATE PCBASIC SET IS_ACTIVE ='Y' WHERE PCBASICID='" + id + "'";
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
        //public DataTable GetEditPayCatDetail(string id)
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "select PROCESSMASTID,PARAMETERS,UNIT,PARAMVALUE from PROCESSDETAIL where PROCESSMASTID = '" + id + "' ";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
    }
}

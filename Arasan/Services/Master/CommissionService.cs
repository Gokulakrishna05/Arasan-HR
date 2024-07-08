using Arasan.Interface.Master;
using Arasan.Models;
using Oracle.ManagedDataAccess.Client;
using Org.BouncyCastle.Security.Certificates;
using System.Data;

namespace Arasan.Services.Master
{
    public class CommissionService : ICommission
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public CommissionService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetUnit()
        {
            string SvSql = string.Empty;
            SvSql = "Select UNITID,UNITMASTID from UNITMAST";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetItem()
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMID,ITEMMASTERID from ITEMMASTER";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditComm(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID, ITEMDESC,UNIT,COMMTYPE,COMMVALUE from COMMDETAIL where COMMBASICID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string CommissionCRUD(Commission cp)
        {
            string msg = "";
            string bsid = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                if (cp.ID == null)
                {
                    datatrans = new DataTransactions(_connectionString);


                    int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'Com#' AND ACTIVESEQUENCE = 'T'");
                    string docid = string.Format("{0}{1}", "Com#", (idc + 1).ToString());

                    string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='Com#' AND ACTIVESEQUENCE ='T'";
                    try
                    {
                        datatrans.UpdateStatus(updateCMd);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    cp.Cid = docid;
                }
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    objConn.Open();
                    if (cp.ID == null)
                    {


                        svSQL = "Insert into COMMBASIC (DOCID,DOCDATE,COMMCODE,COMMDESC,VALIDTO) VALUES ('" + cp.Cid + "','" + cp.Date + "','" + cp.Code + "','" + cp.Code + "','" + cp.Valid + "') RETURNING COMMBASICID INTO : LASTID";
                                OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        objCmds.Parameters.Add("LASTID",OracleDbType.Int64,ParameterDirection.ReturnValue);
                                objCmds.ExecuteNonQuery();
                        bsid = objCmds.Parameters["LASTID"].Value.ToString();

                    }
                    else
                    {
                        svSQL = "UPDATE  COMMBASIC set DOCID='" + cp.Cid + "',DOCDATE='" + cp.Date + "',COMMCODE='" + cp.Code + "',COMMDESC='" + cp.Code + "',VALIDTO='" + cp.Valid + "' Where COMMBASICID='"+cp.ID+"'";
                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                        objCmds.ExecuteNonQuery();
                    }

                    try
                    {
                        
                        if (cp.colst != null)
                        {
                            if (cp.ID == null)
                            {
                                foreach (comlst co in cp.colst)
                                {
                                    if (co.Isvalid == "Y")
                                    {
                                        string des = datatrans.GetDataString("select ITEMDESC from ITEMMASTER WHERE ITEMMASTERID='" + co.item + "'");

                                        svSQL = "Insert into COMMDETAIL (COMMBASICID,ITEMID,ITEMDESC,UNIT,COMMTYPE,COMMVALUE) VALUES ('" + bsid + "','" + co.item + "','" + des + "','" + co.unit + "','" + co.type + "','" + co.val + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }
                                }

                            }
                            else
                            {
                                svSQL = "Delete COMMDETAIL WHERE COMMBASICID='" + cp.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach(comlst co in cp.colst)
                                {
                                    if (co.Isvalid == "Y")
                                    {
                                        string des = datatrans.GetDataString("select ITEMDESC from ITEMMASTER WHERE ITEMMASTERID='" + co.item + "'");

                                        svSQL = "Insert into COMMDETAIL (COMMBASICID,ITEMID,ITEMDESC,UNIT,COMMTYPE,COMMVALUE) VALUES ('" + cp.ID + "','" + co.item + "','" + des + "','" + co.unit + "','" + co.type + "','" + co.val + "')";
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


       
        public DataTable GetAllCommissions(string strStatus)
        {
            string SvSql = string.Empty;
            //if (strStatus == "ACTIVE" || strStatus == null)
            //{
            SvSql = "Select DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,COMMCODE,to_char(VALIDTO,'dd-MON-yyyy')VALIDTO,COMMBASICID,IS_ACTIVE FROM COMMBASIC WHERE IS_ACTIVE='Y'";
            //}
            //else
            //{
            //    SvSql = "Select CONTTYPE,SALPD,DAYORKGS FROM CONTRMAST";

            //}
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditCommission(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,COMMCODE,COMMDESC,to_char(VALIDTO,'dd-MON-yyyy')VALIDTO,COMMBASICID,IS_ACTIVE FROM COMMBASIC  where COMMBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string StatusDelete(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE COMMBASIC SET IS_ACTIVE ='N' WHERE COMMBASICID='" + id + "'";
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
                    svSQL = "UPDATE COMMBASIC SET IS_ACTIVE = 'Y' WHERE COMMBASICID='" + id + "'";
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

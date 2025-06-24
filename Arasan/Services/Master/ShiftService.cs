using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
namespace Arasan.Services
{
    public class ShiftService : IShift
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ShiftService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAlLshift(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = " Select SHIFTNO ,FROMTIME ,TOTIME,SHIFTHRS,SHIFTOTHRS,SHIFTMASTID from SHIFTMAST ";

            }
            else
            {
                SvSql = " Select DOCID ,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE ,PAYPERIODTYPE,to_char(STARTINGDATE,'dd-MON-yyyy')STARTINGDATE,to_char(ENDINGDATE,'dd-MON-yyyy')ENDINGDATE,to_char(SALDATE,'dd-MON-yyyy')SALDATE, PPBASICID,IS_ACTIVE from PPBASIC  WHERE IS_ACTIVE='N' ";
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string ShiftCRUD(Shift pp)
        {
            string msg = "";
            string svSQL = "";
            string updateCMd = "";
            try
            {
                if (pp.ID == null)
                {

                    svSQL = " SELECT Count(SHIFTNO) as cnt FROM SHIFTMAST WHERE SHIFTNO =LTRIM(RTRIM('" + pp.shiftn + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "Shift Already Existed";
                        return msg;
                    }
                }
                using (OracleConnection objconn = new OracleConnection(_connectionString))
                {
                    objconn.Open();
                    if (pp.ID == null)
                    {
                        svSQL = "Insert into SHIFTMAST (APPROVAL, MAXAPPROVED, CANCEL, T1SOURCEID, LATEMPLATEID, SHIFTNO, FROMTIME, TOTIME, SHIFTHRS, NSFLAG, SHIFTOTHRS) values ('0','0','F','0','0','" + pp.shiftn + "','" + pp.ftime + "','" + pp.ttime + "','" + pp.shifthrs + "','','" + pp.othrs + "')";
                    }

                    else
                    {
                        svSQL = " UPDATE SHIFTMAST SET   SHIFTNO = '" + pp.shiftn + "',  FROMTIME =  '" + pp.ftime + "',TOTIME = '" + pp.ttime + "',SHIFTHRS = '" + pp.shifthrs + "',SHIFTOTHRS = '" + pp.othrs + "'  Where SHIFTMASTID = '" + pp.ID + "'";

                    }
                    OracleCommand oracleCommand = new OracleCommand(svSQL, objconn);
                    oracleCommand.Parameters.Add("OUTID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                    oracleCommand.ExecuteNonQuery();
                    objconn.Close();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw ex;
            }
            return msg;
        }
    }
}

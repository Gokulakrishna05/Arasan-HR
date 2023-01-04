using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using Arasan.Interface.Master;
using Arasan.Interface.Qualitycontrol;
using Arasan.Models;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.VisualBasic;
using Oracle.ManagedDataAccess.Client;

namespace Arasan.Services.Qualitycontrol
{
    public class QCResultService : IQCResultService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public QCResultService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
       
        public DataTable GetQCResult(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select DOCID,GRNNO,GRNDATE,DOCDATE,PARTYID,TESTEDBY,LOCATION QCRESULTBASICID from QCRESULTBASIC where QCRESULTBASICID=" + id + ""; 
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
           adapter.Fill(dtt);
            return dtt;
        }
       
        public DataTable GetLocation()
        {
            string SvSql = string.Empty;
            SvSql = "Select LOCID,LOCDETAILSID from LOCDETAILS ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public IEnumerable<QCResult> GetAllQCResult()
        {
            List<QCResult> cmpList = new List<QCResult>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select DOCID,GRNNO,GRNDATE,DOCDATE,PARTYID,TESTEDBY,LOCATION,QCRESULTBASICID from QCRESULTBASIC";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        QCResult cmp = new QCResult
                        {

                            ID = rdr["QCRESULTBASICID"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                            GRNNo = rdr["GRNNO"].ToString(),
                            GRNDate = rdr["GRNDATE"].ToString(),
                            DocDate = rdr["DOCDATE"].ToString(),
                            Party = rdr["PARTYID"].ToString(),
                            TestedBy = rdr["TESTEDBY"].ToString(),
                            Location = rdr["LOCATION"].ToString(),


                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public QCResult GetQCResultById(string eid)
        {
            QCResult QCResult = new QCResult();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select DOCID,GRNNO,GRNDATE,DOCDATE,PARTYID,TESTEDBY,LOCATION QCRESULTBASICID from QCRESULTBASI where QCRESULTBASICID=" + eid + "";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        QCResult sta = new QCResult
                        {
                            ID = rdr["QCRESULTBASICID"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                            GRNNo = rdr["GRNNO"].ToString(),
                            GRNDate = rdr["GRNDATE"].ToString(),
                            DocDate = rdr["DOCDATE"].ToString(),
                            Party = rdr["PARTYID"].ToString(),
                            TestedBy = rdr["TESTEDBY"].ToString(),
                            Location = rdr["LOCATION"].ToString(),
                        };
                        QCResult = sta;
                    }
                }
            }
            return QCResult;
        }
        public string QCResultCRUD(QCResult cy)
        {
            string msg = "";
            
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("QCRESULTPROC", objConn);

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
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                    objCmd.Parameters.Add("GRNNO", OracleDbType.NVarchar2).Value = cy.GRNNo;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.Date).Value = cy.DocDate;
                    objCmd.Parameters.Add("GRNDATE", OracleDbType.Date).Value = cy.GRNDate;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Party;
                    objCmd.Parameters.Add("TESTEDBY", OracleDbType.NVarchar2).Value = cy.TestedBy;
                    objCmd.Parameters.Add("LOCATION", OracleDbType.NVarchar2).Value = cy.Location;
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
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
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }

        
    }
}

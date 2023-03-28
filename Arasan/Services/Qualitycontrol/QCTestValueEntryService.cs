using System.Collections.Generic;
using System.Data;
using Arasan.Interface.Master;
using Arasan.Interface.Qualitycontrol;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;

namespace Arasan.Services.Qualitycontrol
{
    public class QCTestValueEntryService : IQCTestValueEntryService
    {
        private readonly string _connectionString;
        public QCTestValueEntryService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<QCTestValueEntry> GetAllQCTestValueEntry()
        {
            List<QCTestValueEntry> cmpList = new List<QCTestValueEntry>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select BRANCH,DOCID,DOCDATE,WCID,SHIFTNO,PROCESSLOTNO,DRUMNO,PRODDATE,DSAMPLE,DSAMPLETIME,ITEMID,ENTEREDBY,REMARKS,QTVEBASICID from QTVEBASIC";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        QCTestValueEntry cmp = new QCTestValueEntry
                        {
                            ID = rdr["QTVEBASICID"].ToString(),
                            DocId = rdr["DOCID"].ToString(),
                            Docdate = rdr["DOCDATE"].ToString(),
                            Work = rdr["WCID"].ToString(),
                            Shift = rdr["SHIFTNO"].ToString(),
                            Process = rdr["PROCESSLOTNO"].ToString(),
                            Prodate = rdr["PRODDATE"].ToString(),
                            Sample = rdr["DSAMPLE"].ToString(),
                            Sampletime = rdr["DSAMPLETIME"].ToString(),
                            Item = rdr["ITEMID"].ToString(),
                            Entered = rdr["ENTEREDBY"].ToString(),
                            Remarks = rdr["REMARKS"].ToString(),
                        };

                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public string QCTestValueEntryCRUD(QCTestValueEntry cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("STATEPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "STATEPROC";*/

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

                    objCmd.Parameters.Add("BRANCH", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.DocId;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                    objCmd.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.Work;
                    objCmd.Parameters.Add("SHIFTNO", OracleDbType.NVarchar2).Value = cy.Shift;
                    objCmd.Parameters.Add("PROCESSLOTNO", OracleDbType.NVarchar2).Value = cy.Process;
                    objCmd.Parameters.Add("DRUMNO", OracleDbType.NVarchar2).Value = cy.Drum;
                    objCmd.Parameters.Add("PRODDATE", OracleDbType.NVarchar2).Value = cy.Prodate;
                    objCmd.Parameters.Add("DSAMPLE", OracleDbType.NVarchar2).Value = cy.Sample;
                    objCmd.Parameters.Add("DSAMPLETIME", OracleDbType.NVarchar2).Value = cy.Sampletime;
                    objCmd.Parameters.Add("ITEMID", OracleDbType.NVarchar2).Value = cy.Item;
                    objCmd.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = cy.Entered;
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Remarks;
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
        public DataTable GetBranch()
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCHMASTID,BRANCHID from BRANCHMAST order by BRANCHMASTID asc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetQCTestValueEntryDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select COUNTRYNAME,COUNTRYMASTID from CONMAST order by COUNTRYMASTID asc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }


    }
}

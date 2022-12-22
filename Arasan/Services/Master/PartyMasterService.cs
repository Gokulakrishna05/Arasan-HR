using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using Arasan.Interface.Master;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Services.Master
{
    public class PartyMasterService :IPartyMasterService
    {
        private readonly string _connectionString;

        public PartyMasterService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<PartyMaster> GetAllParty()
        {
            List<PartyMaster> cmpList = new List<PartyMaster>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select  PARTYID,PARTYNAME,PARTYCAT,ACCOUNTNAME,PARTYGROUP,COMMCODE,REGULARYN,LUTNO,to_char(PARTYMAST.LUTDT,'dd-MON-yyyy')LUTDT,to_char(PARTYMAST.PJOINDATE,'dd-MON-yyyy')PJOINDATE,PARTYTYPE,CREDITLIMIT,CREDITDAYS,SECTIONID,CSGNPARTYID,TRANSLMT,GSTNO,ACTIVE,RATECODE,PARTYMASTID from PARTYMAST";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PartyMaster cmp = new PartyMaster
                        {

                            ID = rdr["PARTYMASTID"].ToString(),
                            PartyCode = rdr["PARTYID"].ToString(),
                            PartyName = rdr["PARTYNAME"].ToString(),
                            PartyCategory = rdr["PARTYCAT"].ToString(),
                            AccName = rdr["ACCOUNTNAME"].ToString(),
                            PartyGroup = rdr["PARTYGROUP"].ToString(),
                            Comm = rdr["COMMCODE"].ToString(),
                            Regular = rdr["REGULARYN"].ToString(),
                            LUTNumber = rdr["LUTNO"].ToString(),
                            LUTDate = rdr["LUTDT"].ToString(),
                             JoinDate = rdr["PJOINDATE"].ToString(),
                            PartyType = rdr["PARTYTYPE"].ToString(),
                            CreditLimit = rdr["CREDITLIMIT"].ToString(),
                            CreditDate = rdr["CREDITDAYS"].ToString(),
                            TransationLimit = rdr["TRANSLMT"].ToString(),
                            GST = rdr["GSTNO"].ToString(),
                            RateCode = rdr["RATECODE"].ToString(),
                            // DelCh = rdr["DELCH"].ToString()



                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
        public string PartyCRUD(PartyMaster cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("PARTYPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "LOCATIONPROC";*/

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

                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.PartyCode;
                    objCmd.Parameters.Add("PARTYNAME", OracleDbType.NVarchar2).Value = cy.PartyName;
                    objCmd.Parameters.Add("PARTYCAT", OracleDbType.NVarchar2).Value = cy.PartyCategory;
                    objCmd.Parameters.Add("ACCOUNTNAME", OracleDbType.NVarchar2).Value = cy.AccName;
                    objCmd.Parameters.Add("PARTYGROUP", OracleDbType.NVarchar2).Value = cy.PartyGroup;
                    objCmd.Parameters.Add("COMMCODE", OracleDbType.NVarchar2).Value = cy.Comm;
                    objCmd.Parameters.Add("REGULARYN", OracleDbType.NVarchar2).Value = cy.Regular;
                    objCmd.Parameters.Add("LUTNO", OracleDbType.NVarchar2).Value = cy.LUTNumber;
                    objCmd.Parameters.Add("LUTDT", OracleDbType.Date).Value = DateTime.Parse(cy.LUTDate);
                    objCmd.Parameters.Add("PJOINDATE", OracleDbType.Date).Value = DateTime.Parse(cy.JoinDate);
                    objCmd.Parameters.Add("PARTYTYPE", OracleDbType.NVarchar2).Value = cy.PartyType;
                    objCmd.Parameters.Add("CREDITLIMIT", OracleDbType.NVarchar2).Value = cy.CreditLimit;
                    objCmd.Parameters.Add("CREDITDAYS", OracleDbType.NVarchar2).Value = cy.CreditDate;
                    objCmd.Parameters.Add("SECTIONID", OracleDbType.NVarchar2).Value = cy.SectionID;
                    objCmd.Parameters.Add("CSGNPARTYID", OracleDbType.NVarchar2).Value = cy.ConPartyID;
                    objCmd.Parameters.Add("TRANSLMT", OracleDbType.NVarchar2).Value = cy.TransationLimit;
                    objCmd.Parameters.Add("GSTNO", OracleDbType.NVarchar2).Value = cy.GST;
                    objCmd.Parameters.Add("ACTIVE", OracleDbType.NVarchar2).Value = cy.Active;
                    objCmd.Parameters.Add("RATECODE", OracleDbType.NVarchar2).Value = cy.RateCode;
                   
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
        public DataTable GetParty(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PARTYMAST.PARTYID,PARTYMAST.PARTYNAME,PARTYMAST.PARTYCAT,to_char(PARTYMAST.LUTDT,'dd-MON-yyyy')LUTDT,PARTYMAST.ACCOUNTNAME,PARTYMAST.PARTYGROUP,PARTYMAST.COMMCODE,PARTYMAST.REGULARYN,PARTYMAST.LUTNO,PARTYMAST.PARTYTYPE,PARTYMAST.CREDITLIMIT,PARTYMAST.CREDITDAYS,PARTYMAST.SECTIONID,PARTYMAST.CSGNPARTYID,PARTYMAST.TRANSLMT,PARTYMAST.GSTNO,to_char(PARTYMAST.PJOINDATE,'dd-MON-yyyy')PJOINDATE,ACTIVE,RATECODE,PARTYMASTID  from PARTYMAST where PARTYMAST.PARTYMASTID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetState()
        {
            string SvSql = string.Empty;
            SvSql = "select STATE,STATEMASTID from STATEMAST";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;

        }
        public DataTable GetCity()
        {
            string SvSql = string.Empty;
            SvSql = "select CITYNAME,CITYID from CITYMASTER  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
            public DataTable GetCountry()
            {
                string SvSql = string.Empty;
                SvSql = "select COUNTRYNAME,COUNTRYMASTID from CONMAST  ";
                DataTable dtt = new DataTable();
                OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
                OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
                adapter.Fill(dtt);
                return dtt;
            }

    }
}

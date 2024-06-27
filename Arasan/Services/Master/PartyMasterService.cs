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
        DataTransactions datatrans;

        public PartyMasterService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        
        public string PartyCRUD(PartyMaster cy)  
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                if (cy.ID == null)

                {

                    svSQL = " SELECT Count(PARTYID) as cnt FROM PARTYMAST WHERE PARTYID =LTRIM(RTRIM('" + cy.PartyCode + "')) ";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "Party Already Existed";
                        return msg;
                    }
                }
                

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

                     objCmd.Parameters.Add("PARTYNAME", OracleDbType.NVarchar2).Value = cy.PartyName;
                    objCmd.Parameters.Add("PARTYCAT", OracleDbType.NVarchar2).Value = cy.PartyCategory;
                    
                    objCmd.Parameters.Add("PARTYGROUP", OracleDbType.NVarchar2).Value = cy.PartyGroup;
                    objCmd.Parameters.Add("COMMCODE", OracleDbType.NVarchar2).Value = cy.Comm;
                    objCmd.Parameters.Add("REGULARYN", OracleDbType.NVarchar2).Value = cy.Regular;
                    objCmd.Parameters.Add("LUTNO", OracleDbType.NVarchar2).Value = cy.LUTNumber;
                    objCmd.Parameters.Add("LUTDT", OracleDbType.NVarchar2).Value = cy.LUTDate;
                    objCmd.Parameters.Add("PJOINDATE", OracleDbType.NVarchar2).Value = cy.JoinDate;
                    //objCmd.Parameters.Add("TYPE", OracleDbType.NVarchar2).Value = cy.PartyType;
                    objCmd.Parameters.Add("CREDITLIMIT", OracleDbType.NVarchar2).Value = cy.CreditLimit;
                    objCmd.Parameters.Add("CREDITDAYS", OracleDbType.NVarchar2).Value = cy.CreditDate;
                    objCmd.Parameters.Add("SECTIONID", OracleDbType.NVarchar2).Value = cy.SectionID;
                    objCmd.Parameters.Add("CSGNPARTYID", OracleDbType.NVarchar2).Value = cy.ConPartyID;
                    objCmd.Parameters.Add("TRANSLMT", OracleDbType.NVarchar2).Value = cy.TransationLimit;
                    objCmd.Parameters.Add("GSTNO", OracleDbType.NVarchar2).Value = cy.GST;
                    //objCmd.Parameters.Add("ACTIVE", OracleDbType.NVarchar2).Value = cy.Active;
                    objCmd.Parameters.Add("RATECODE", OracleDbType.NVarchar2).Value = cy.RateCode;
                    objCmd.Parameters.Add("MOBILE", OracleDbType.NVarchar2).Value = cy.Mobile;
                    objCmd.Parameters.Add("PHONENO", OracleDbType.NVarchar2).Value = cy.Phone;
                    objCmd.Parameters.Add("PANNO", OracleDbType.NVarchar2).Value = cy.PanNumber;
                    objCmd.Parameters.Add("CITY", OracleDbType.NVarchar2).Value = cy.City;
                    objCmd.Parameters.Add("STATE", OracleDbType.NVarchar2).Value = cy.State;
                    objCmd.Parameters.Add("COUNTRY", OracleDbType.NVarchar2).Value = cy.Country;
                    objCmd.Parameters.Add("PINCODE", OracleDbType.NVarchar2).Value = cy.Pincode;
                    objCmd.Parameters.Add("COUNTRYCODE", OracleDbType.NVarchar2).Value = cy.CountryCode;
                    objCmd.Parameters.Add("EMAIL", OracleDbType.NVarchar2).Value = cy.Email;
                    objCmd.Parameters.Add("FAX", OracleDbType.NVarchar2).Value = cy.Fax;
                    objCmd.Parameters.Add("COMMISIONERATE", OracleDbType.NVarchar2).Value = cy.Commisionerate;
                    objCmd.Parameters.Add("RANGEDIVISION", OracleDbType.NVarchar2).Value = cy.Range;
                    objCmd.Parameters.Add("ECCNO", OracleDbType.NVarchar2).Value = cy.EccID;
                    objCmd.Parameters.Add("EXCISEAPPLICABLE", OracleDbType.NVarchar2).Value = cy.Excise;
                    objCmd.Parameters.Add("HTTP", OracleDbType.NVarchar2).Value = cy.Http;
                    objCmd.Parameters.Add("OVERDUEINTEREST", OracleDbType.NVarchar2).Value = cy.OverDueInterest;
                    objCmd.Parameters.Add("ADD1", OracleDbType.NVarchar2).Value = cy.Address;
                    objCmd.Parameters.Add("ADD2", OracleDbType.NVarchar2).Value = cy.Address2;
                    objCmd.Parameters.Add("ADD3", OracleDbType.NVarchar2).Value = cy.Address3;
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Remark;
                    objCmd.Parameters.Add("INTRODUCEDBY", OracleDbType.NVarchar2).Value = cy.Intred;
                    objCmd.Parameters.Add("ACCOUNTNAME", OracleDbType.NVarchar2).Value = cy.Ledger;
                     if (string.IsNullOrEmpty(cy.ID))
                    {
                        objCmd.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = cy.createby;
                        objCmd.Parameters.Add("CREATED_ON", OracleDbType.NVarchar2).Value = DateTime.Now.ToString("dd\\/MM\\/yyyy hh:mm:ss tt");
                    }
                    else
                    {
                        objCmd.Parameters.Add("MODBY", OracleDbType.NVarchar2).Value = cy.createby;
                        objCmd.Parameters.Add("UPDATED_ON", OracleDbType.NVarchar2).Value = DateTime.Now.ToString("dd\\/MM\\/yyyy hh:mm:ss tt");
                    }
                    objCmd.Parameters.Add("SALPERNAME", OracleDbType.NVarchar2).Value = cy.branch;

                    if (string.IsNullOrEmpty(cy.salper))
                    {
                        objCmd.Parameters.Add("SALPERNAME", OracleDbType.NVarchar2).Value = cy.salper;
                    }
                    else
                    {
                        objCmd.Parameters.Add("SALPERNAME", OracleDbType.NVarchar2).Value = "0";
                    }
                    objCmd.Parameters.Add("SALLOC", OracleDbType.NVarchar2).Value = cy.salloc;
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
                        if (cy.PartyLst != null)
                        {
                            if (cy.ID == null)
                            {

                                foreach (PartyItem cp in cy.PartyLst)
                                {
                                    if (cp.Isvalid == "Y" && cp.Purpose != "0")
                                    {
                                        
                                            OracleCommand objCmds = new OracleCommand("PARTYCONTACTPROC", objConn);

                                            StatementType = "Insert";
                                            objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;

                                            objCmds.CommandType = CommandType.StoredProcedure;
                                            objCmds.Parameters.Add("PARTYMASTID", OracleDbType.NVarchar2).Value = Pid;
                                            objCmds.Parameters.Add("CONTACTPURPOSE", OracleDbType.NVarchar2).Value = cp.Purpose;
                                            objCmds.Parameters.Add("CONTACTNAME", OracleDbType.NVarchar2).Value = cp.ContactPerson;
                                            objCmds.Parameters.Add("CONTACTDESIG", OracleDbType.NVarchar2).Value = cp.Designation;
                                            objCmds.Parameters.Add("CONTACTPHONE", OracleDbType.NVarchar2).Value = cp.CPhone;
                                            objCmds.Parameters.Add("CONTACTEMAIL", OracleDbType.NVarchar2).Value = cp.CEmail;
                                            objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                             objCmds.ExecuteNonQuery();

                                         
                                    }
                                }
                            }

                            else
                            {
                                svSQL = "Delete PARTYMASTCONTACT WHERE PARTYMASTID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();

                                foreach (PartyItem cp in cy.PartyLst)
                                {
                                    if (cp.Isvalid == "Y" && cp.Purpose != "0")
                                    {

                                        OracleCommand objCmds = new OracleCommand("PARTYCONTACTPROC", objConn);

                                        StatementType = "Insert";
                                        objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;

                                        objCmds.CommandType = CommandType.StoredProcedure;
                                        objCmds.Parameters.Add("PARTYMASTID", OracleDbType.NVarchar2).Value = Pid;
                                        objCmds.Parameters.Add("CONTACTPURPOSE", OracleDbType.NVarchar2).Value = cp.Purpose;
                                        objCmds.Parameters.Add("CONTACTNAME", OracleDbType.NVarchar2).Value = cp.ContactPerson;
                                        objCmds.Parameters.Add("CONTACTDESIG", OracleDbType.NVarchar2).Value = cp.Designation;
                                        objCmds.Parameters.Add("CONTACTPHONE", OracleDbType.NVarchar2).Value = cp.CPhone;
                                        objCmds.Parameters.Add("CONTACTEMAIL", OracleDbType.NVarchar2).Value = cp.CEmail;
                                        objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                        objCmds.ExecuteNonQuery();


                                    }
                                }
                            }
                        }
                        if (cy.PartyLst != null)
                        {
                            if (cy.ID == null)
                            {
                                int r = 1;
                                foreach (ratedet cp in cy.rateLst)
                                {
                                    if (cp.Isvalid == "Y" && cp.ratetype != "")
                                    {

                                        svSQL = "Insert into PARTYMASTBRCODE (PARTYMASTID,PARTYMASTBRCODEROW,BRATECODE,BRATETYPE,BRATEDESC,ACCNAME,ACOUNTRY) VALUES ('" + Pid + "','" + r + "','" + cp.ratecode + "','" + cp.ratetype + "','" + cp.ratecode + "','0','"+cp.acco+"')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();



                                    }
                                    r++;
                                }
                            }

                            else
                            {
                                svSQL = "Delete PARTYMASTBRCODE WHERE PARTYMASTID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                int r = 1;
                                foreach (ratedet cp in cy.rateLst)
                                {
                                    if (cp.Isvalid == "Y" && cp.ratetype != "0")
                                    {

                                         svSQL = "Insert into PARTYMASTBRCODE (PARTYMASTID,PARTYMASTBRCODEROW,BRATECODE,BRATETYPE,BRATEDESC,ACCNAME,ACOUNTRY) VALUES ('" + Pid + "','" + r + "','" + cp.ratecode + "','" + cp.ratetype + "','" + cp.ratecode + "','0','" + cp.acco + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();




                                    }
                                    r++;
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

        public DataTable GetParty(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PARTYMAST.PARTYID,PARTYMAST.PARTYNAME,PARTYMAST.PARTYCAT,SALPERNAME,SALLOC,to_char(PARTYMAST.LUTDT,'dd-MON-yyyy')LUTDT,PARTYMAST.ACCOUNTNAME,PARTYMAST.PARTYGROUP,PARTYMAST.COMMCODE,PARTYMAST.REGULARYN,PARTYMAST.LUTNO,PARTYMAST.TYPE,PARTYMAST.CREDITLIMIT,PARTYMAST.CREDITDAYS,PARTYMAST.SECTIONID,PARTYMAST.CSGNPARTYID,PARTYMAST.TRANSLMT,PARTYMAST.GSTNO,to_char(PARTYMAST.PJOINDATE,'dd-MON-yyyy')PJOINDATE,RATECODE,MOBILE,PHONENO,PANNO,CITY,STATE,COUNTRY,PINCODE,CONCODE COUNTRYCODE,EMAIL,FAX,COMMISIONERATE,RANGEDIVISION,ECCNO,EXCISEAPPLICABLE,HTTP,OVERDUEINTEREST,ADD1,ADD2,ADD3,REMARKS,INTRODUCEDBY,ACTIVE,PARTYMASTID  from PARTYMAST where PARTYMAST.PARTYMASTID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetLedger()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT MNAME,MASTERID FROM MASTER";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable Getratecode()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT RATECODE FROM RATEBASIC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPartyContact(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PARTYMASTCONTACT.CONTACTPURPOSE,PARTYMASTCONTACT.CONTACTNAME,PARTYMASTCONTACT.CONTACTDESIG,PARTYMASTCONTACT.CONTACTPHONE,PARTYMASTCONTACT.CONTACTEMAIL  from PARTYMASTCONTACT where PARTYMASTCONTACT.PARTYMASTID=" + id + "";
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
                SvSql = "select COUNTRY  from CONMAST  ";
                DataTable dtt = new DataTable();
                OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
                OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
                adapter.Fill(dtt);
                return dtt;
            }
        public DataTable GetCountryDetails(string CID)
        {
            string SvSql = string.Empty;
            SvSql = "select CONCODE,COUNTRY ,CONMASTID from CONMAST where COUNTRY='" + CID + "'";
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
                    svSQL = "UPDATE PARTYMAST SET IS_ACTIVE ='N' WHERE PARTYMASTID='" + id + "'";
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
                    svSQL = "UPDATE PARTYMAST SET IS_ACTIVE ='Y' WHERE PARTYMASTID='" + id + "'";
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

        public DataTable GetAllParty(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select PARTYMAST.IS_ACTIVE,PARTYMASTID,PARTYMAST.PARTYNAME,PARTYMAST.PARTYCAT,PARTYMAST.PARTYGROUP,RATECODE,to_char(PARTYMAST.PJOINDATE,'dd-MON-yyyy')PJOINDATE from PARTYMAST WHERE PARTYMAST.IS_ACTIVE = 'Y' ORDER BY PARTYMASTID ASC";
            }
            else
            {
                SvSql = "Select PARTYMAST.IS_ACTIVE,PARTYMASTID,PARTYMAST.PARTYNAME,PARTYMAST.PARTYCAT,PARTYMAST.PARTYGROUP,RATECODE,to_char(PARTYMAST.PJOINDATE,'dd-MON-yyyy')PJOINDATE from PARTYMAST WHERE PARTYMAST.IS_ACTIVE = 'N' ORDER BY PARTYMASTID ASC";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}

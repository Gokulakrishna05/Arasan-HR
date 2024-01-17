using Arasan.Interface;

using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{
    public class LocationService : ILocationService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public LocationService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
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
        public DataTable GetSupplier()
        {
            string SvSql = string.Empty;
            SvSql = "Select PARTYMAST.PARTYMASTID,PARTYMAST.PARTYNAME from PARTYMAST  Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND PARTYMAST.PARTYNAME IS NOT NULL";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPartyDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "select ADD1,ADD2,ADD3,CITY,STATE,PINCODE,EMAIL,MOBILE,FAX,PHONENO from PARTYMAST WHERE partymast.partymastid=" + ItemId + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        //public IEnumerable<Location> GetAllLocations(string status)
        //{
        //    List<Location> cmpList = new List<Location>();
        //    using (OracleConnection con = new OracleConnection(_connectionString))
        //    {

        //        using (OracleCommand cmd = con.CreateCommand())
        //        {
        //            con.Open();
        //            cmd.CommandText = "Select LOCID,LOCATIONTYPE,CPNAME,PHNO,EMAIL,ADD1,BRANCHID,LOCDETAILSID,IS_ACTIVE from LOCDETAILS WHERE IS_ACTIVE= '" + status + "' order by LOCDETAILS.LOCID DESC";
        //            OracleDataReader rdr = cmd.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                Location cmp = new Location
        //                {
        //                    ID = rdr["LOCDETAILSID"].ToString(),
        //                    LocationId = rdr["LOCID"].ToString(),
        //                    LocType = rdr["LOCATIONTYPE"].ToString(),
        //                    ContactPer = rdr["CPNAME"].ToString(),
        //                    PhoneNo = rdr["PHNO"].ToString(),
        //                    EmailId = rdr["EMAIL"].ToString(),
        //                    Address = rdr["ADD1"].ToString(),
        //                    Branch = rdr["BRANCHID"].ToString()

        //                };
        //                cmpList.Add(cmp);
        //            }
        //        }
        //    }
        //    return cmpList;
        //}


        public Location GetLocationsById(string eid)
        {
            Location location = new Location();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select LOCID,LOCATIONTYPE,CPNAME,PHNO,EMAIL,ADD1,BRANCHID,LOCDETAILSID from LOCDETAILS where LOCDETAILSID = " + eid + "";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Location cmp = new Location
                        {
                            ID = rdr["LOCDETAILSID"].ToString(),
                            LocationId = rdr["LOCID"].ToString(),
                            LocType = rdr["LOCATIONTYPE"].ToString(),
                            ContactPer = rdr["CPNAME"].ToString(),
                            PhoneNo = rdr["PHNO"].ToString(),
                           
                            EmailId = rdr["EMAIL"].ToString(),
                            Address = rdr["ADD1"].ToString(),
                            Branch = rdr["BRANCHID"].ToString()
                           
                        };
                    
                        location = cmp;
                    }
                }
            }
            return location ;
        }

        public string LocationsCRUD(Location cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                if (cy.ID == null)
                {

                    svSQL = " SELECT Count(LOCID) as cnt FROM LOCDETAILS WHERE LOCID = LTRIM(RTRIM('" + cy.LocationId + "')) and LOCATIONTYPE = LTRIM(RTRIM('" + cy.LocType + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "Location Already Existed";
                        return msg;
                    }
                }
                
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("LOCATIONPROC", objConn);
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

                    objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.LocationId;
                    objCmd.Parameters.Add("LOCATIONTYPE", OracleDbType.NVarchar2).Value = cy.LocType;
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("TRADEYN", OracleDbType.NVarchar2).Value = cy.Trader;
                    objCmd.Parameters.Add("BINYN", OracleDbType.NVarchar2).Value = cy.Requried;
                    objCmd.Parameters.Add("PARTYID", OracleDbType.NVarchar2).Value = cy.Party;
                    objCmd.Parameters.Add("CPNAME", OracleDbType.NVarchar2).Value = cy.ContactPer;
                    objCmd.Parameters.Add("PHNO", OracleDbType.NVarchar2).Value = cy.PhoneNo;
                    objCmd.Parameters.Add("ADD1", OracleDbType.NVarchar2).Value = cy.Address;
                    objCmd.Parameters.Add("FAXNO", OracleDbType.NVarchar2).Value = cy.Fax;
                    objCmd.Parameters.Add("ADD2", OracleDbType.NVarchar2).Value = cy.Add2;
                    objCmd.Parameters.Add("EMAIL", OracleDbType.NVarchar2).Value = cy.Mail;
                    objCmd.Parameters.Add("ADD3", OracleDbType.NVarchar2).Value = cy.Add3;
                    objCmd.Parameters.Add("FLWORD", OracleDbType.NVarchar2).Value = cy.FlowOrd;
                    objCmd.Parameters.Add("CITY", OracleDbType.NVarchar2).Value = cy.City;
                    objCmd.Parameters.Add("STATE", OracleDbType.NVarchar2).Value = cy.State;
                    objCmd.Parameters.Add("PINCODE", OracleDbType.NVarchar2).Value = cy.PinCode;
                    objCmd.Parameters.Add("IS_ACTIVE", OracleDbType.NVarchar2).Value = "Y";

                    if (cy.ID == null)
                    {
                        objCmd.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = cy.createby;
                        objCmd.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    }
                    else
                    {
                        objCmd.Parameters.Add("UPDATED_BY", OracleDbType.NVarchar2).Value = cy.createby;
                        objCmd.Parameters.Add("UPDATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    }

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
                        if (cy.Locationlst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (LocationItem cp in cy.Locationlst)
                                {
                                    if (cp.Isvalid == "Y")
                                    {

                                        svSQL = "Insert into BINBASIC (LOCDETAILSID,BINID,BINDESC,CAPACITY) VALUES ('" + Pid + "','" + cp.BinId + "','" + cp.BinDesc + "','" + cp.Capacity + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }
                                }

                            }
                            else
                            {
                                svSQL = "Delete BINBASIC WHERE LOCDETAILSID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (LocationItem cp in cy.Locationlst)
                                {
                                    if (cp.Isvalid == "Y")
                                    {
                                        svSQL = "Insert into BINBASIC (LOCDETAILSID,BINID,BINDESC,CAPACITY) VALUES ('" + Pid + "','" + cp.BinId + "','" + cp.BinDesc + "','" + cp.Capacity + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }
                                }
                            }
                        }
                        if (cy.Loclst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (LocItem cp in cy.Loclst)
                                {
                                    if (cp.Isvalid1 == "Y" && cp.Issuse != "0")
                                    {
                                        svSQL = "Insert into LOCDESTDETAIL (LOCDETAILSID,ISSUETYPE,TOLOCID) VALUES ('" + Pid + "','" + cp.Issuse + "','" + cp.Location + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                    }


                                }
                            }
                            else
                            {
                                svSQL = "Delete LOCDESTDETAIL WHERE LOCDETAILSID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (LocItem cp in cy.Loclst)
                                {
                                    if (cp.Isvalid1 == "Y" && cp.Issuse != "0")
                                    {
                                        svSQL = "Insert into LOCDESTDETAIL (LOCDETAILSID,ISSUETYPE,TOLOCID) VALUES ('" + Pid + "','" + cp.Issuse + "','" + cp.Location + "')";
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
        public string LocCRUD(Location cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                if (cy.ID == null)
                {

                    svSQL = " SELECT Count(LOCID) as cnt FROM LOCATIONDETAILS WHERE LOCID = LTRIM(RTRIM('" + cy.LocationId + "')) and LOCATIONTYPE = LTRIM(RTRIM('" + cy.LocType + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "Location Already Existed";
                        return msg;
                    }
                }

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("LOCATIONDETAILPROC", objConn);
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

                    objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.LocationId;
                    objCmd.Parameters.Add("LOCATIONTYPE", OracleDbType.NVarchar2).Value = cy.LocType;
                    objCmd.Parameters.Add("CPNAME", OracleDbType.NVarchar2).Value = cy.ContactPer;
                    objCmd.Parameters.Add("PHNO", OracleDbType.NVarchar2).Value = cy.PhoneNo;
                    //  objCmd.Parameters.Add("FaxNo", OracleDbType.NVarchar2).Value = cy.FaxNo;
                    objCmd.Parameters.Add("EMAIL", OracleDbType.NVarchar2).Value = cy.EmailId;
                  
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.Int64).Value = cy.Branch;
                    // objCmd.Parameters.Add("Bin", OracleDbType.NVarchar2).Value = cy.Bin;
                    // objCmd.Parameters.Add("Trade", OracleDbType.NVarchar2).Value = cy.Trade;
                    // objCmd.Parameters.Add("FlowOrd", OracleDbType.Int64).Value = cy.FlowOrd;
                    objCmd.Parameters.Add("IS_ACTIVE", OracleDbType.NVarchar2).Value = "Y";
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
                    svSQL = "UPDATE LOCDETAILS SET IS_ACTIVE ='N' WHERE LOCDETAILSID='" + id + "'";
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
        public string RemoveChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE LOCDETAILS SET IS_ACTIVE ='Y' WHERE LOCDETAILSID='" + id + "'";
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

        public DataTable GetEditLocation(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT LOCID,LOCATIONTYPE,BRANCHID,TRADEYN,BINYN,PARTYID,CPNAME,PHNO,ADD1,FAXNO,ADD2,EMAIL,ADD3,FLWORD,CITY,STATE,PINCODE FROM LOCDETAILS where LOCDETAILSID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditBinDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT BINID,BINDESC,CAPACITY FROM BINBASIC where LOCDETAILSID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditLocDeatils(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ISSUETYPE,TOLOCID FROM LOCDESTDETAIL where LOCDETAILSID= '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAllLocation(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "select LOCDETAILS.IS_ACTIVE,LOCID,LOCATIONTYPE,CPNAME,PHNO,EMAIL,LOCDETAILSID from LOCDETAILS  WHERE LOCDETAILS.IS_ACTIVE = 'Y' ORDER BY LOCDETAILSID DESC";
            }
            else
            {
                SvSql = "select LOCDETAILS.IS_ACTIVE,LOCID,LOCATIONTYPE,CPNAME,PHNO,EMAIL,LOCDETAILSID from LOCDETAILS  WHERE LOCDETAILS.IS_ACTIVE = 'N' ORDER BY LOCDETAILSID DESC";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }

}


﻿using Arasan.Interface;

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
        public LocationService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
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
        public IEnumerable<Location> GetAllLocations()
        {
            List<Location> cmpList = new List<Location>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select LOCID,LOCATIONTYPE,CPNAME,PHNO,EMAIL,ADD1,BRANCHID,LOCDETAILSID from LOCDETAILS";
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
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }


        public Location GetLocationsById(string eid)
        {
            Location location = new Location();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select LOCID,LOCATIONTYPE,CPNAME,PHNO,EMAIL,ADD1,BRANCHID,LOCDETAILSID from LOCDETAILS where LOCDETAILSID=" + eid + "";
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
                    objCmd.Parameters.Add("CPNAME", OracleDbType.NVarchar2).Value = cy.ContactPer;
                    objCmd.Parameters.Add("PHNO", OracleDbType.NVarchar2).Value = cy.PhoneNo;
                  //  objCmd.Parameters.Add("FaxNo", OracleDbType.NVarchar2).Value = cy.FaxNo;
                    objCmd.Parameters.Add("EMAIL", OracleDbType.NVarchar2).Value = cy.EmailId;
                    objCmd.Parameters.Add("ADD1", OracleDbType.NVarchar2).Value = cy.Address;
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.Int64).Value = cy.Branch;
                   // objCmd.Parameters.Add("Bin", OracleDbType.NVarchar2).Value = cy.Bin;
                   // objCmd.Parameters.Add("Trade", OracleDbType.NVarchar2).Value = cy.Trade;
                   // objCmd.Parameters.Add("FlowOrd", OracleDbType.Int64).Value = cy.FlowOrd;
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


    }
}
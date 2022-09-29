using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services.Master
{
    public class ItemGroupService : IItemGroupService
    {
        private readonly string _connectionString;
        public ItemGroupService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<ItemGroup> GetAllItemGroup()
        {
            List<ItemGroup> itgList = new List<ItemGroup>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select ITEMGROUPID,GROUPCODE,GROUPDESC,ITEMGROUPID from ITEMGROUP";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ItemGroup itg = new ItemGroup
                        {
                            ID = rdr["ITEMGROUPID"].ToString(),
                            ItemGroupid = rdr["ITEMGROUPID"].ToString(),
                            ItemGroups = rdr["GROUPCODE"].ToString(),
                            ItemGroupDescription = rdr["GROUPDESC"].ToString()
                        };
                        itgList.Add(itg);
                    }
                }
            }
            return itgList;
        }


        public ItemGroup GetItemGroupById(string eid)
        {
            ItemGroup ItemGroup = new ItemGroup();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select ITEMGROUPID,GROUPCODE,GROUPDESC,ITEMGROUPID from ITEMGROUP where ITEMGROUPID=" + eid + "";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ItemGroup itg = new ItemGroup
                        {
                            ID = rdr["ITEMGROUPID"].ToString(),
                            ItemGroupid = rdr["ITEMGROUPID"].ToString(),
                            ItemGroups = rdr["GROUPCODE"].ToString(),
                            ItemGroupDescription = rdr["GROUPDESC"].ToString()
                        };
                        ItemGroup = itg;
                    }
                }
            }
            return ItemGroup;
        }

        public string ItemGroupCRUD(ItemGroup by)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                if (by.ID == null)
                {
                    StatementType = "Insert";
                }
                else
                {
                    StatementType = "Update";
                }
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand();
                    objCmd.Connection = objConn;
                    objCmd.CommandText = "ITEMGROUPPROC";
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.Add("ID", OracleDbType.Long).Value = by.ID;
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add(" ItemGroupid ", OracleDbType.NVarchar2).Value = by.ItemGroupid;
                    objCmd.Parameters.Add("ItemGroups", OracleDbType.NVarchar2).Value = by.ItemGroups;
                    objCmd.Parameters.Add("ItemGroupDescription", OracleDbType.NVarchar2).Value = by.ItemGroupDescription;
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
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
        DataTransactions datatrans;
        public ItemGroupService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        //public IEnumerable<ItemGroup> GetAllItemGroup(string status)
        //{
        //    if (string.IsNullOrEmpty(status))
        //    {
        //        status = "Y";
        //    }
        //    List<ItemGroup> itgList = new List<ItemGroup>();
        //    using (OracleConnection con = new OracleConnection(_connectionString))
        //    {

        //        using (OracleCommand cmd = con.CreateCommand())
        //        {
        //            con.Open();
        //            cmd.CommandText = "Select GROUPCODE,GROUPDESC,IS_ACTIVE, ITEMGROUPID from ITEMGROUP WHERE IS_ACTIVE='" + status + "' order by ITEMGROUP.ITEMGROUPID DESC ";
        //            OracleDataReader rdr = cmd.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                ItemGroup itg = new ItemGroup
        //                {
        //                    ID = rdr["ITEMGROUPID"].ToString(),
        //                    itemGroup = rdr["GROUPCODE"].ToString(),
        //                    ItemGroupDescription = rdr["GROUPDESC"].ToString()
        //                };
        //                itgList.Add(itg);
        //            }
        //        }
        //    }
        //    return itgList;
        //}

        public DataTable GetGroup(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select GROUPCODE,GROUPDESC,CATEGORY,GROUPTYPE,ITEMGROUPID from ITEMGROUP where ITEMGROUPID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetCategory()
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMCATEGORYID,CATEGORY from ITEMCATEGORY  ORDER BY ITEMCATEGORYID DESC";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        //public ItemGroup GetItemGroupById(string eid)
        //{
        //    ItemGroup ItemGroup = new ItemGroup();
        //    using (OracleConnection con = new OracleConnection(_connectionString))
        //    {
        //        using (OracleCommand cmd = con.CreateCommand())
        //        {
        //            con.Open();
        //            cmd.CommandText = "Select GROUPCODE,GROUPDESC,CATEGORY,ITEMGROUPID from ITEMGROUP where ITEMGROUPID=" + eid + "";
        //            OracleDataReader rdr = cmd.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                ItemGroup itg = new ItemGroup
        //                {
        //                    ID = rdr["ITEMGROUPID"].ToString(),
        //                    ItemCat = rdr["CATEGORY"].ToString(),
        //                    ItemGroups = rdr["GROUPCODE"].ToString(),
        //                    ItemGroupDescription = rdr["GROUPDESC"].ToString()
        //                };
        //                ItemGroup = itg;
        //            }
        //        }
        //    }
        //    return ItemGroup;
        //}

        public string ItemGroupCRUD(ItemGroup by)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                if (by.ID == null)
                {

                    svSQL = " SELECT Count(GROUPCODE) as cnt FROM ITEMGROUP WHERE GROUPCODE = LTRIM(RTRIM('" + by.ItemGroups + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "ItemGroup Already Existed";
                        return msg;
                    }
                }
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("ITEMGROUPPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "ITEMGROUPPROC";*/

                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (by.ID == null)
                     {
                    StatementType = "Insert";
                    objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value; 
                    }
                else
                {
                    StatementType = "Update";
                    objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = by.ID;
                    }
                                                                      
                   
                    objCmd.Parameters.Add("GROUPCODE", OracleDbType.NVarchar2).Value = by.ItemGroups;
                    objCmd.Parameters.Add("GROUPDESC", OracleDbType.NVarchar2).Value = by.ItemGroups;
                      objCmd.Parameters.Add("GROUPTYPE", OracleDbType.NVarchar2).Value = by.Type;
                    if (by.ID == null)
                    {
                        objCmd.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = by.createby;
                        objCmd.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    }
                    else 
                    {
                        objCmd.Parameters.Add("UPDATED_BY", OracleDbType.NVarchar2).Value = by.createby;
                        objCmd.Parameters.Add("UPDATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    }
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;

                    objConn.Open();
                        objCmd.ExecuteNonQuery();
                    Object Pid = objCmd.Parameters["OUTID"].Value;
                    if (by.ID != null)
                    {
                        Pid = by.ID;
                    }
                    if (by.ID == null)
                    {
                        if (by.Sublst != null)
                        {
                            foreach (subgrp cp in by.Sublst)
                            {
                                if (cp.Isvalid == "Y" && cp.subgrpname != null)
                                {

                                    OracleCommand objCmds = new OracleCommand("ITEMSUBGROUPPROC", objConn);

                                    objCmds.CommandType = CommandType.StoredProcedure;
                                    StatementType = "Insert";
                                    objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;

                                    objCmds.Parameters.Add("ITEMGROUPID", OracleDbType.NVarchar2).Value = Pid;
                                    objCmds.Parameters.Add("SGCODE", OracleDbType.NVarchar2).Value = cp.subgrpname;
                                    objCmds.Parameters.Add("SGDESC", OracleDbType.NVarchar2).Value = cp.subgrpname;
                                    objCmds.Parameters.Add("SCONSYN", OracleDbType.NVarchar2).Value = cp.consyn;


                                    objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;


                                    objCmds.ExecuteNonQuery();



                                }


                            }

                        }
                    }
                    else
                    {
                        svSQL = "Delete ITEMSUBGROUP WHERE ITEMMASTERID='" + by.ID + "'";
                        OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                        objCmdd.ExecuteNonQuery();

                        foreach (subgrp cp in by.Sublst)
                        {
                            if (cp.Isvalid == "Y" && cp.subgrpname != null)
                            {

                                OracleCommand objCmds = new OracleCommand("ITEMSUBGROUPPROC", objConn);

                                objCmds.CommandType = CommandType.StoredProcedure;
                                StatementType = "Insert";
                                objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;

                                objCmds.Parameters.Add("ITEMGROUPID", OracleDbType.NVarchar2).Value = Pid;
                                objCmds.Parameters.Add("SGCODE", OracleDbType.NVarchar2).Value = cp.subgrpname;
                                objCmds.Parameters.Add("SGDESC", OracleDbType.NVarchar2).Value = cp.subgrpdecs;
                                objCmds.Parameters.Add("SCONSYN", OracleDbType.NVarchar2).Value = cp.consyn;


                                objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;


                                objCmds.ExecuteNonQuery();



                            }


                        }
                    }
                    //System.Console.WriteLine("Number of employees in department 20 is {0}", objCmd.Parameters["pout_count"].Value);

                    objConn.Close();
                }
            }
            catch (Exception it)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw it;
            }

            return msg;
        }

        public string StatusChange(string tag, int id)
        {

            try
            {
                string svSQL = string.Empty;
                //string status = tag == "Del" ? "N" : "Y";
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE ITEMGROUP SET IS_ACTIVE ='N' WHERE ITEMGROUPID='" + id + "'";
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
                    svSQL = "UPDATE ITEMGROUP SET IS_ACTIVE ='Y' WHERE ITEMGROUPID='" + id + "'";
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

        public DataTable GetAllItemGroup(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = " Select ITEMGROUP.GROUPCODE,ITEMGROUP.GROUPDESC,GROUPTYPE , ITEMGROUP.ITEMGROUPID,ITEMGROUP.IS_ACTIVE ,ITEMCATEGORY.CATEGORY from ITEMGROUP left outer join ITEMCATEGORY on ITEMCATEGORY.ITEMCATEGORYID = ITEMGROUP.CATEGORY WHERE ITEMGROUP.IS_ACTIVE='Y' order by ITEMGROUP.ITEMGROUPID DESC ";

            }
            else
            {
                SvSql = " Select ITEMGROUP.GROUPCODE,ITEMGROUP.GROUPDESC,GROUPTYPE , ITEMGROUP.ITEMGROUPID,ITEMGROUP.IS_ACTIVE ,ITEMCATEGORY.CATEGORY from ITEMGROUP left outer join ITEMCATEGORY on ITEMCATEGORY.ITEMCATEGORYID = ITEMGROUP.CATEGORY WHERE ITEMGROUP.IS_ACTIVE='N' order by ITEMGROUP.ITEMGROUPID DESC ";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

    }
}
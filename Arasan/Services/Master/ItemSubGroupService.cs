using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Arasan.Services.Master
{
    public class ItemSubGroupService : IItemSubGroupService 
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ItemSubGroupService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        //public IEnumerable<ItemSubGroup> GetAllItemSubGroup(string status)
        //{
        //    if (string.IsNullOrEmpty(status))
        //    {
        //        status = "ACTIVE";
        //    }
        //    List<ItemSubGroup> staList = new List<ItemSubGroup>();
        //    using (OracleConnection con = new OracleConnection(_connectionString))
        //     {

        //        using (OracleCommand cmd = con.CreateCommand())
        //        {
        //            con.Open();
        //            cmd.CommandText = "Select SGCODE,SGDESC,STATUS,ITEMSUBGROUPID from ITEMSUBGROUP WHERE STATUS='" + status + "' order by ITEMSUBGROUP.ITEMSUBGROUPID DESC ";
        //            OracleDataReader rdr = cmd.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                ItemSubGroup sta = new ItemSubGroup
        //                {
        //                    ID = rdr["ITEMSUBGROUPID"].ToString(),
        //                    itemSubGroup = rdr["SGCODE"].ToString(),
        //                    Descreption = rdr["SGDESC"].ToString(),

        //                };
        //                staList.Add(sta);
        //            }
        //        }
        //    }
        //    return staList;
        //}
        
        public ItemSubGroup GetItemSubGroupById(string eid)
        {
            ItemSubGroup ItemSubGroup = new ItemSubGroup();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select SGCODE,SGDESC,ITEMSUBGROUPID from ITEMSUBGROUP where ITEMSUBGROUPID=" + eid + "";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ItemSubGroup gro = new ItemSubGroup
                        {
                            ID = rdr["ITEMSUBGROUPID"].ToString(),
                            itemSubGroup = rdr["SGCODE"].ToString(),
                            Descreption = rdr["SGDESC"].ToString(),
                        };
                        ItemSubGroup = gro;
                    }
                }
            }
            return ItemSubGroup;
        }

        public string ItemSubGroupCRUD(ItemSubGroup sg)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                if (sg.ID == null)
                {

                    svSQL = " SELECT Count(SGCODE) as cnt FROM ITEMSUBGROUP WHERE SGCODE = LTRIM(RTRIM('" + sg.itemSubGroup + "'))";
                    if (datatrans.GetDataId(svSQL) > 0)
                    {
                        msg = "ItemSubGroup Already Existed";
                        return msg;
                    }
                }
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("ITEMSUBGROUPPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "ITEMSUBGROUPPROC";*/

                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (sg.ID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = sg.ID;
                    }

                    objCmd.Parameters.Add("SGCODE", OracleDbType.NVarchar2).Value = sg.itemSubGroup;
                    objCmd.Parameters.Add("SGDESC", OracleDbType.NVarchar2).Value = sg.Descreption;
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
                    svSQL = "UPDATE ITEMSUBGROUP SET IS_ACTIVE ='N' WHERE ITEMSUBGROUPID='" + id + "'";
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
                    svSQL = "UPDATE ITEMSUBGROUP SET IS_ACTIVE ='Y' WHERE ITEMSUBGROUPID='" + id + "'";
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

        public DataTable GetAllItemSubGroup(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = " Select SGCODE,SGDESC,IS_ACTIVE,ITEMSUBGROUPID from ITEMSUBGROUP WHERE IS_ACTIVE='Y' order by ITEMSUBGROUP.ITEMSUBGROUPID DESC ";

            }
            else
            {
                SvSql = " Select SGCODE,SGDESC,IS_ACTIVE,ITEMSUBGROUPID from ITEMSUBGROUP WHERE IS_ACTIVE='N' order by ITEMSUBGROUP.ITEMSUBGROUPID DESC ";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }


    }

}

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
    public class ItemCategoryService  : IItemCategoryService
    {

        private readonly string _connectionString;
        public ItemCategoryService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public IEnumerable<ItemCategory> GetAllItemCategory()
        {
            List<ItemCategory> icyList = new List<ItemCategory>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select CATEGORY,STATUS,ITEMCATEGORYID from ITEMCATEGORY WHERE STATUS= 'ACTIVE'";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ItemCategory ic = new ItemCategory
                        {
                            ID = rdr["ITEMCATEGORYID"].ToString(),
                           
                            Category = rdr["CATEGORY"].ToString()
                        };
                        icyList.Add(ic);
                    }
                }
            }
            return icyList;
        }


        public ItemCategory GetCategoryById(string cid)
        {
            ItemCategory ItemCategory = new ItemCategory();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select CATEGORY,ITEMCATEGORYID from ITEMCATEGORY where ITEMCATEGORYID=" + cid + "";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ItemCategory ict = new ItemCategory
                        {
                            ID = rdr["ITEMCATEGORYID"].ToString(),
                          
                            Category = rdr["CATEGORY"].ToString()
                            
                        };
                        ItemCategory = ict;
                    }
                }
            }
            return ItemCategory;
        }

        public string CategoryCRUD(ItemCategory iy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("CATEGORYPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "CATEGORYPROC";*/

                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (iy.ID == null)
                {
                    StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    }
                else
                {
                    StatementType = "Update";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = iy.ID;
                    }


                    objCmd.Parameters.Add("ItemCategory", OracleDbType.NVarchar2).Value = iy.Category;
                    objCmd.Parameters.Add("status", OracleDbType.NVarchar2).Value = "ACTIVE";
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        //System.Console.WriteLine("Number of employees in department 20 is {0}", objCmd.Parameters["pout_count"].Value);
                    }
                    catch (Exception exs)
                    {
                        //System.Console.WriteLine("Exception: {0}", ex.ToString());
                    }
                    objConn.Close();
                }
            }
            catch (Exception exs)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw exs;
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
                    svSQL = "UPDATE ITEMCATEGORY SET STATUS ='INACTIVE' WHERE ITEMCATEGORYID='" + id + "'";
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
    }
}

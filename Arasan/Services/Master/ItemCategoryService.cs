using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services.Master;
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
                    cmd.CommandText = "Select CATEGORY,ITEMCATEGORYID from ITEMCATEGORY";
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


        public ItemCategory GetCategoryById(string eid)
        {
            ItemCategory category = new ItemCategory();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select CATEGORY,ITEMCATEGORYID from ITEMCATEGORY where ITEMCATEGORYID=" + eid + "";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ItemCategory ict = new ItemCategory
                        {
                            ID = rdr["ITEMCATEGORYID"].ToString(),
                          
                            Category = rdr["CATEGORY"].ToString()
                        };
                        category = ict;
                    }
                }
            }
            return category;
        }

        public string CategoryCRUD(ItemCategory iy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                if (iy.ID == null)
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
                    objCmd.CommandText = "CATEGORYPROC";
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = iy.ID;
                   
                   
                    objCmd.Parameters.Add("Category", OracleDbType.NVarchar2).Value = iy.Category;
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

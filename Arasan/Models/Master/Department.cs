﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;

namespace Arasan.Models
{
    public class Department
    {
        public string ID { get; set; }
       // public string DepartmentCode { get; set; }
        public string Departmentcode { get; set; }
        public string DepartmentName { get; set; }
        public string status { get; set; }
        public string Descrip { get; set; }
        public string Pos { get; set; }
        public string IsActive { get; set; }
        public string createby { get; set; }
        public string ddlStatus { get; set; }


        public List<Designationdet> Designationlst { get; set; }
    }
   public class Departmentgrid
    {
        public string id { get; set; }
        public string departmentcode { get; set; }
        public string departmentname { get; set; }
        public string description { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }

        public string pos { get; set; }
    }
    public class Designationdet
    {
        public string id { get; set; }
        public List<SelectListItem> deslst { get; set; }

        public string designation { get; set; }
        public string Isvalid { get; set; }
      
    }

}


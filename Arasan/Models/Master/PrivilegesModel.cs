using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;
namespace Arasan.Models
{
    public class PrivilegesModel
    {
        public PrivilegesModel()
        {
            this.deptlst = new List<SelectListItem>();
            this.desglst = new List<SelectListItem>();
            this.emplst = new List<SelectListItem>();
        }
        public List<PMenuList> menulst { get;set; }
        public string dept { get; set; }
        public string desg { get; set; }
        public string emp { get; set; }
        public List<SelectListItem> deptlst { get; set; }
        public List<SelectListItem> desglst { get; set; }   
        public List<SelectListItem> emplst { get; set; }
        public string ID { get; set; }
    }
    public class PMenuList
    {
        public string menuname { get;set; }
        public string menuid { get; set; }
        public List<menudetails> menudlst { get; set; }
        public bool sectiondisable { get; set; }
        public bool selectViewall { get; set; }
        public bool selectaddall { get; set; }
        public bool selecteditall { get; set; }
        public bool selectdeleteall { get; set; }
    }
    public class menudetails
    {
        public string urlname { get; set;}
        public string mapid { get; set;}
        public bool View { get; set; }
        public bool add { get; set; }
        public bool edit { get; set; }
        public bool delete { get; set; }
    }
    public class Privlist
    {
        public int id { get; set; }
        public string empname { get; set; }
        public string desg { get; set; }
        public string dept { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string viewrow { get; set; }
        public string ddlStatus { get; set; }
    }
}
